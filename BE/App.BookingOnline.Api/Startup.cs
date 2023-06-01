using AutoMapper;
using App.BookingOnline.Data;
using App.BookingOnline.Service;
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
using App.BookingOnline.Data.Repositories;
using Microsoft.IdentityModel.Logging;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Repositories.Common;
using App.BookingOnline.Service.IService.Common;
using App.BookingOnline.Service.Service.Common;
using App.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using App.BookingOnline.Data.MailService;
using App.BookingOnline.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Hangfire;
using Hangfire.SqlServer;
using App.BookingOnline.WebApi.Jobs;

namespace App.BookingOnline.Api
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
            services.AddDbContext<BookingOnlineDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("BookingOnlineDbContext"), x => x.MigrationsAssembly("App.BookingOnline.Data"));
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

            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString("BookingOnlineDbContext")));
            services.AddHangfireServer();
            JobStorage.Current = new SqlServerStorage(Configuration.GetConnectionString("BookingOnlineDbContext"));
            RecurringJob.AddOrUpdate<LogAccountJobs>("LogAccountJobs", (myJob) => myJob.logAccountCustomer(), Cron.Daily(), TimeZoneInfo.Local);

            var cronOut = Cron.Daily(16, 0);
            RecurringJob.AddOrUpdate<SendFileOutJobs>("SendFileOut", (myJob) => myJob.SendFileOutToSbAsync(), cronOut, TimeZoneInfo.Local);
            var cronIn = Cron.Daily(9, 10);
            RecurringJob.AddOrUpdate<GetFileInSeabankJobs>("GetFileInSeabankJobs", (myJob) => myJob.GetFileInFromSbAsync(), cronIn, TimeZoneInfo.Local);

            services.AddControllers();

            #region register service
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
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IAppUserService, AppUserService>();
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
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<ICancelBookingService, CancelBookingService>();
            services.AddScoped<ICancelBookingRepository, CancelBookingRepository>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ILogDateFileOutRepository, LogDateFileOutRepository>();
            services.AddScoped<ITransactionDetailReportService, TransactionDetailReportService>();
            services.AddScoped<ITransactionDetailReportRepository, TransactionDetailReportRepository>();
            services.AddScoped<ITransactionMonthlyReportRepository, TransactionMonthlyReportRepository>();
            services.AddScoped<ITransactionMonthlyReportService, TransactionMonthlyReportService>();
            services.AddScoped<ITransactionSummaryReportRepository, TransactionSummaryReportRepository>();
            services.AddScoped<ITransactionSummaryReportService, TransactionSummaryReportService>();
            services.AddScoped<IHistoryBookingService, HistoryBookingService>();
            services.AddScoped<IHistoryBookingRepository, HistoryBookingRepository>();
            services.AddScoped<ISftpService, SftpService>();
            #endregion

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<BookingRepository>>();
            services.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger), logger);

            string authUrl = Configuration.GetSection("urlData").GetValue<string>("AuthUrl");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                authUrl = Configuration.GetSection("urlData").GetValue<string>("AuthUrlPro");
            }
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(o =>
                {
                    o.Authority = authUrl;
                    o.Audience = "api1";
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new
                        TokenValidationParameters()
                    {
                        ValidateAudience = false
                    };
                    o.SaveToken = true;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiReader", policy => policy.RequireClaim("scope", "api.read"));
                options.AddPolicy("admin", policy => policy.RequireClaim(ClaimTypes.Role, Core.Constants.Admin));
                options.AddPolicy("employee", policy => policy.RequireClaim(ClaimTypes.Role, Core.Constants.Employee));
                options.AddPolicy("adminOrEmployee", policy => policy.RequireClaim(ClaimTypes.Role, Core.Constants.Admin, Core.Constants.Employee));
                options.AddPolicy("customer", policy => policy.RequireClaim(ClaimTypes.Role, Core.Constants.Customer));
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Content-Disposition")));

            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                }).SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking web", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup));

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

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
            // comment khi swagger khi len server that
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking web");
            });

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseHangfireDashboard("/mydashboard");

            app.UseAuthentication();
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

            var serilog = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(@"ApiServer_log.txt");

            loggerFactory.AddSerilog(serilog.CreateLogger());
        }
    }
}
