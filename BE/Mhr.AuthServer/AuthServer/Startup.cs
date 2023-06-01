using AuthServer.Extensions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using Mhr.Data;
using Mhr.Data.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;
using System.Net;

namespace Mhr.AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MhrDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("MhrDBContext"),
            x => x.MigrationsAssembly("Mhr.Data")));

            services.AddDbContext<PersistedGrantDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("MhrDBContext"),
            x => x.MigrationsAssembly("Mhr.Data")));

            services.AddDbContext<ConfigurationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("MhrDBContext"),
            x => x.MigrationsAssembly("Mhr.Data")));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<MhrDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.UserInteraction.LoginUrl = "/Account/Login";
                    options.UserInteraction.LogoutUrl = "/Account/Logout";
                    options.Authentication = new IdentityServer4.Configuration.AuthenticationOptions()
                    {
                        CookieLifetime = TimeSpan.FromHours(10), // ID server cookie timeout set to 10 hours
                        CookieSlidingExpiration = true
                    };
                })
                .AddDeveloperSigningCredential()
                // this adds the operational data from DB (codes, tokens, consents)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("MhrDBContext"));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(Configuration.GetConnectionString("MhrDBContext"));
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30; // interval in seconds
                })
                //.AddInMemoryPersistedGrants()
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddInMemoryApiResources(Config.GetApiResources())
                //.AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<AppUser>();

            /* We'll play with this down the road... 
                services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "<insert here>";
                    options.ClientSecret = "<insert here>";
                });*/
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = "Cookies";
            //    options.DefaultChallengeScheme = "oidc";
            //})
            //.AddCookie("Cookies")
            //.AddOpenIdConnect("oidc", options =>
            //{
            //    options.SignInScheme = "Cookies";

            //    options.Authority = "https://localhost:44343";
            //    options.RequireHttpsMetadata = false;

            //    options.ClientId = "mvc";
            //    options.ClientSecret = "secret";
            //    options.ResponseType = "code id_token";

            //    options.SaveTokens = true;
            //    options.GetClaimsFromUserInfoEndpoint = true;

            //    options.Scope.Add("api1");
            //    options.Scope.Add("offline_access");

            //    options.ClaimActions.MapJsonKey("website", "website");
            //});
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // this will do the initial DB population
            bool seed = Configuration.GetSection("Data").GetValue<bool>("Seed");
            if (seed)
            {
                InitializeDatabase(app);
                throw new Exception("Seeding completed. Disable the seed flag in appsettings");
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                    }
                });
            });

            var serilog = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(@"authserver_log.txt");

            loggerFactory.WithFilter(new FilterLoggerSettings
                {
                    { "IdentityServer4", LogLevel.Debug },
                    { "Microsoft", LogLevel.Warning },
                    { "System", LogLevel.Warning },
                }).AddSerilog(serilog.CreateLogger());

            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
