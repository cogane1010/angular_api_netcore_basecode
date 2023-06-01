using App.Core;
using App.BookingOnline.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using App.Core.Repositories;
using Microsoft.IdentityModel.Logging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Repositories.Common;
using App.BookingOnline.Service.Service.Common;
using App.BookingOnline.Service.IService.Common;
using App.BookingOnline.Data.MailService;
using App.BookingOnline.Data.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using Serilog.Events;

namespace App.BookingOnline.MobileApi
{
    public class Startup
    {
        private readonly string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<BookingOnlineDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("BookingOnlineDbContext")
            , x => x.MigrationsAssembly("App.BookingOnline.Data"));
            options.EnableSensitiveDataLogging();
            });

            services.AddScoped<DbContext, BookingOnlineDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            var lockoutTimeSpan = Configuration.GetSection("loginTime").GetValue<int>("DefaultLockoutTimeSpan");
            var failedAccessAttempts = Configuration.GetSection("loginTime").GetValue<int>("MaxFailedAccessAttempts");
            services.AddIdentity<AppUser, AspRole>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(lockoutTimeSpan);
                options.Lockout.MaxFailedAccessAttempts = failedAccessAttempts;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<BookingOnlineDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();

            services.AddScoped<ISequenceRepository, SequenceRepository>();
            services.AddScoped<ISequenceLineRepository, SequenceLineRepository>();
            services.AddScoped<IUploadFileRepository, UploadFileRepository>();

            services.AddScoped<IOrganizationTypeRepository, OrganizationTypeRepository>();
            services.AddScoped<IOrganizationTypeService, OrganizationTypeService>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IOrganizationInfoRepository, OrganizationInfoRepository>();
            services.AddScoped<IOrganizationInfoService, OrganizationInfoService>();
            services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<ILockReasonRepository, LockReasonRepository>();
            services.AddScoped<ILockReasonService, LockReasonService>();
            services.AddScoped<ICancelReasonRepository, CancelReasonRepository>();
            services.AddScoped<ICancelReasonService, CancelReasonService>();
            services.AddScoped<IBookingOtherTypeRepository, BookingOtherTypeRepository>();
            services.AddScoped<IBookingOtherTypeService, BookingOtherTypeService>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IHolidayRepository, HolidayRepository>();
            services.AddScoped<IHolidayService, HolidayService>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<ICustomerGroupRepository, CustomerGroupRepository>();
            services.AddScoped<ICustomerGroupService, CustomerGroupService>();
            services.AddScoped<ICustomerGroupMappingRepository, CustomerGroupMappingRepository>();
            services.AddScoped<ICustomerGroupMappingService, CustomerGroupMappingService>();

            services.AddScoped<IMemberCardRepository, MemberCardRepository>();
            services.AddScoped<IMemberCardService, MemberCardService>();
            services.AddScoped<IMemberCardCourseRepository, MemberCardCourseRepository>();
            services.AddScoped<IMemberCardCourseService, MemberCardCourseService>();

            services.AddScoped<IMemberRequestRepository, MemberRequestRepository>();
            services.AddScoped<IMemberRequestService, MemberRequestService>();

            services.AddScoped<ICourseTemplateRepository, CourseTemplateRepository>();
            services.AddScoped<ICourseTemplateService, CourseTemplateService>();
            services.AddScoped<ISmsHistoryService, SmsHistoryService>();
            services.AddScoped<ISmsHistoryRepository, SmsHistoryRepository>();
            services.AddScoped<ITeeSheetLockRepository, TeeSheetLockRepository>();
            services.AddScoped<ITeeSheetLockService, TeeSheetLockService>();
            services.AddScoped<ITeeSheetLockLineRepository, TeeSheetLockLineRepository>();
            services.AddScoped<ITeeSheetLockLineService, TeeSheetLockLineService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingLineRepository, BookingLineRepository>();
            services.AddScoped<IBookingLineService, BookingLineService>();

            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();

            services.AddScoped<ISequenceRepository, SequenceRepository>();
            services.AddScoped<ISequenceLineRepository, SequenceLineRepository>();
            services.AddScoped<IUploadFileRepository, UploadFileRepository>();

            services.AddScoped<IOrganizationTypeRepository, OrganizationTypeRepository>();
            services.AddScoped<IOrganizationTypeService, OrganizationTypeService>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IOrganizationInfoRepository, OrganizationInfoRepository>();
            services.AddScoped<IOrganizationInfoService, OrganizationInfoService>();
            services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<ILockReasonRepository, LockReasonRepository>();
            services.AddScoped<ILockReasonService, LockReasonService>();
            services.AddScoped<ICancelReasonRepository, CancelReasonRepository>();
            services.AddScoped<ICancelReasonService, CancelReasonService>();
            services.AddScoped<IBookingOtherTypeRepository, BookingOtherTypeRepository>();
            services.AddScoped<IBookingOtherTypeService, BookingOtherTypeService>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IHolidayRepository, HolidayRepository>();
            services.AddScoped<IHolidayService, HolidayService>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<ICustomerGroupRepository, CustomerGroupRepository>();
            services.AddScoped<ICustomerGroupService, CustomerGroupService>();
            services.AddScoped<ICustomerGroupMappingRepository, CustomerGroupMappingRepository>();
            services.AddScoped<ICustomerGroupMappingService, CustomerGroupMappingService>();

            services.AddScoped<IMemberCardRepository, MemberCardRepository>();
            services.AddScoped<IMemberCardService, MemberCardService>();
            services.AddScoped<IMemberCardCourseRepository, MemberCardCourseRepository>();
            services.AddScoped<IMemberCardCourseService, MemberCardCourseService>();

            services.AddScoped<IMemberRequestRepository, MemberRequestRepository>();
            services.AddScoped<IMemberRequestService, MemberRequestService>();

            services.AddScoped<ICourseTemplateRepository, CourseTemplateRepository>();
            services.AddScoped<ICourseTemplateService, CourseTemplateService>();


            services.AddScoped<ITeeSheetLockRepository, TeeSheetLockRepository>();
            services.AddScoped<ITeeSheetLockService, TeeSheetLockService>();
            services.AddScoped<ITeeSheetLockLineRepository, TeeSheetLockLineRepository>();
            services.AddScoped<ITeeSheetLockLineService, TeeSheetLockLineService>();

            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingLineRepository, BookingLineRepository>();
            services.AddScoped<IBookingLineService, BookingLineService>();


            services.AddScoped<IHotlineRepository, HotlineRepository>();
            services.AddScoped<IHotlineService, HotlineService>();

            services.AddScoped<IContactSupportRepository, ContactSupportRepository>();
            services.AddScoped<IContactSupportService, ContactSupportService>();

            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<IHomeService, HomeService>();



            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<BookingRepository>>();
            services.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger), logger);


            string authUrl = Configuration.GetSection("urlData").GetValue<string>("AuthUrl");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                authUrl = Configuration.GetSection("urlData").GetValue<string>("AuthUrlPro");
            }
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = OpenIdConnectDefaults.AuthenticationScheme; 
            //    options.DefaultChallengeScheme = "oidc";
            //}).AddOpenIdConnect("oidc", options =>
            //{
            //    options.Authority = authUrl;
            //    options.ClientId = "clientApp";
            //    options.ResponseType = "code token";
            //    options.SaveTokens = true;
            //    options.Scope.Add("api1");
            //    options.Scope.Add("offline_access");
            //    //options.Events.OnRedirectToIdentityProvider = async n =>
            //    //{
            //    //    n.ProtocolMessage.RedirectUri = "https://localhost:44337/signin-oidc";
            //    //    await Task.FromResult(0);
            //    //};
            //});
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Authority = authUrl;
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        //var userId = int.Parse(context.Principal.Identity.Name);
                        //var user = userService.GetById(userId);
                        //if (user == null)
                        //{
                        //    // return unauthorized if user no longer exists
                        //    context.Fail("Unauthorized");
                        //}
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiReader", policy => policy.RequireClaim("scope", "api.read"));
                options.AddPolicy("admin", policy => policy.RequireClaim(ClaimTypes.Role, Constants.Admin));
                options.AddPolicy("employee", policy => policy.RequireClaim(ClaimTypes.Role, Constants.Employee));
                options.AddPolicy("adminOrEmployee", policy => policy.RequireClaim(ClaimTypes.Role, Constants.Admin, Constants.Employee));
                options.AddPolicy("customer", policy => policy.RequireClaim(ClaimTypes.Role, Constants.Customer));
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Booking Mobile",
                    Version = "v1",
                    Description = $"API",
                });
                // using System.Reflection;
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{authUrl}/connect/authorize"),
                            TokenUrl = new Uri($"{authUrl}/connect/token"),
                            Scopes = new Dictionary<string, string>
                                {
                                    {"api1", "Demo API - full access"}
                                }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            //app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();

            app.UseStaticFiles(); // For the wwwroot folder
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Assets")),
                RequestPath = new PathString("/Assets")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"logs")),
                RequestPath = new PathString("/logs22222")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Assets")),
                RequestPath = new PathString("/Assets")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"logs")),
                RequestPath = new PathString("/logs22222")
            });


            // comment khi swagger khi len server that
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking Mobile");
                c.OAuthClientId("demo_api_swagger");
                c.OAuthAppName("Demo API - Swagger");
                c.OAuthUsePkce();
            });

            //var serilog = new LoggerConfiguration()
            //    .MinimumLevel.Verbose()
            //    .Enrich.FromLogContext()
            //    .WriteTo.File(@"ApiMobileServer_log.txt");

            var serilog = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File($"logs/log-{DateTime.Now:yyyy-MM-dd}.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}.{MethodName}) {Message}{NewLine}{Exception}");
            loggerFactory.AddSerilog(serilog.CreateLogger());
        }

        public class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var hasAuthorize =
                  context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                  || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

                if (hasAuthorize)
                {
                    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                    operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                    operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme {Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"}
                        }
                    ] = new[] {"api1"}
                }
            };

                }
            }
        }
    }
}
