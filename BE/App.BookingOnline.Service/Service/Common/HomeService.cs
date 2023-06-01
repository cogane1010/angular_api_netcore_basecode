using App.BookingOnline.Data;
using App.BookingOnline.Data.MailService;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.Core;
using App.Core.Configs;
using App.Core.Domain;
using App.Core.Helper;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static App.Core.Enums;

namespace App.BookingOnline.Service.Service.Common
{
    public class HomeService : BaseGridDataService<HomeDTO, Customer, UserPagingModel, IHomeRepository>, IHomeService
    {
        private readonly IMailService _mailService;
        private readonly ISequenceRepository seqRepo;
        private readonly ICourseRepository _course;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ILogger _log;
        private readonly IConfiguration _config;
        private string backendUrl;
        private string backendMobileUrl;

        public HomeService(IHomeRepository gridRepository, IAppUserRepository appUserRepository, IConfiguration config,
            ISequenceRepository seqRepo, ICourseRepository course, IMailService mailService, ILogger<HomeService> logger) : base(gridRepository)
        {
            _mailService = mailService;
            this.seqRepo = seqRepo;
            _course = course;
            _appUserRepository = appUserRepository;
            _log = logger;
            _config = config;

            backendUrl = _config.GetSection("urlData").GetValue<string>("BackendUrl") + "/";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                backendUrl = _config.GetSection("urlData").GetValue<string>("BackendUrlPro") + "/";
            }
            backendMobileUrl = _config.GetSection("urlData").GetValue<string>("SwaggerUrl");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                backendMobileUrl = _config.GetSection("urlData").GetValue<string>("SwaggerUrlPro");
            }
        }

        public async Task<RespondData> GetContactAllCourse()
        {
            return new RespondData { IsSuccess = true, Data = await _course.GetAlls() };
        }

        private string ConvertImageToBase64(string path)
        {
            var url = path.Replace("\\", "//");
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    byte[] data = webClient.DownloadData(url);
                    if (data != null)
                    {
                        string base64String = Convert.ToBase64String(data);
                        return base64String;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }


        public async Task<RespondData> GetHomeData(string userId, string lang, string userName)
        {

            var data = new RespondData();
            var homeData = new HomeDTO();
            try
            {
                var customer = await _gridRepository.GetCustomer(userId);
                var idCourse = new List<Guid>();
                if (customer != null)
                {
                    homeData.Customer = AutoMapperHelper.Map<Customer, CustomerDTO>(customer);
                    homeData.Customer.UserName = userName;
                    homeData.Customer.Full_Image_Url = backendMobileUrl + AppConfigs.UPLOAD_PATH + customer.Img_Url;
                    homeData.Customer.Full_Image_Url = homeData.Customer.Full_Image_Url.Replace("\\", "/");
                    //homeData.Customer.ImageData = ConvertImageToBase64(homeData.Customer.Full_Image_Url);
                    var memberCards = _appUserRepository.GetMemberCardByCustId(customer.Id);
                    if (memberCards.Any())
                    {
                        homeData.Customer.MemberCards = new List<MemberCardDTO>();
                        var config = new MapperConfiguration(c =>
                        {
                            c.CreateMap<MemberCard, MemberCardDTO>()
                            .ForMember(d => d.CoursesMemberCard, o => o.MapFrom(s => s.MemberCardCourses))
                            .AfterMap((s, d) => d.OrgMemberName = d.Golf_CardType + " " + d.Organization.Name + " " + d.Golf_CardNo);
                            c.CreateMap<MemberCardCourse, MemberCardCourseDTO>();
                            c.CreateMap<Organization, OrganizationDTO>();
                            c.CreateMap<Course, CourseDTO>();
                            c.CreateMap<CustomerGroup, CustomerGroupDTO>();
                        });
                        var mapper = config.CreateMapper();
                        var memberCardDto = mapper.Map<IEnumerable<MemberCard>, IEnumerable<MemberCardDTO>>(memberCards);
                        idCourse.AddRange(memberCardDto.SelectMany(a => a.CoursesMemberCard.Select(s => s.C_Course_Id.Value)));
                        homeData.Customer.MemberCards = memberCardDto.ToList();
                    }
                }
                var promotion = await _gridRepository.GetPromotion(userId);

                if (promotion.Any())
                {
                    var normalPromotion = promotion.Where(x => x.Promotion_Type == PromotionType.normal.ToString());
                    var seaGolfPromotion = promotion.Where(x => x.Promotion_Type == PromotionType.seagolf.ToString());
                    var config = new MapperConfiguration(c =>
                    {
                        c.CreateMap<M_Promotion, PromotionDTO>()
                        .AfterMap((s, d) => d.CustomerGroups = d.PromotionCustomerGroup.Select(s => s.CustomerGroup).ToList())
                        .AfterMap((s, d) => d.Full_Image_Url = (backendUrl + d.Img_Url).Replace("\\", "/"))
                        .AfterMap((s, d) => d.Full_Image_Url_En = (backendUrl + d.Img_Url_En).Replace("\\", "/"))
                        .AfterMap((s, d) => d.Img_Url_mobile = (backendUrl + (lang == Constants.LangVn ? d.Img_Url : d.Img_Url_En)).Replace("\\", "/"))
                        .AfterMap((s, d) => d.Full_Image_Url_mobile = (backendUrl + (lang == Constants.LangVn ? d.Img_Url : d.Img_Url_En)).Replace("\\", "/"))
                        .AfterMap((s, d) => d.PromotionCourses = s.PromotionCourse.Select(p => new PromotionCourseDTO
                        {
                            M_Promotion_Id = p.M_Promotion_Id,
                            C_Course_Id = p.C_Course_Id,
                            CourseName = p.Course.Name,
                            CourseCode = p.Course.Code
                        }).ToList());
                        c.CreateMap<M_Promotion_Course, PromotionCourseDTO>();
                        c.CreateMap<Course, CourseDTO>();
                        c.CreateMap<M_Promotion_CustomerGroup, PromotionCustomerGroupDTO>();
                        c.CreateMap<CustomerGroup, CustomerGroupDTO>();
                        c.CreateMap<Booking, BookingDTO>();
                        c.CreateMap<Organization, OrganizationDTO>();
                    });
                    var mapper = config.CreateMapper();

                    if (normalPromotion.Any())
                    {
                        homeData.Promotions = mapper.Map<IEnumerable<M_Promotion>, IEnumerable<PromotionDTO>>(normalPromotion);
                    }
                    if (seaGolfPromotion.Any())
                    {
                        homeData.SeaGofl = mapper.Map<IEnumerable<M_Promotion>, IEnumerable<PromotionDTO>>(seaGolfPromotion);

                        foreach (var prom in homeData.SeaGofl)
                        {
                            var isVisitor = prom.CustomerGroups.Any(a => a.Code == ((int)MemberCardType.visitor).ToString());
                            if (!isVisitor)
                            {
                                prom.PromotionCourses = prom.PromotionCourses.Where(s => idCourse.Contains(s.C_Course_Id.Value)).ToList();
                            }
                            else
                            {
                                prom.PromotionCourses = prom.PromotionCourses.Where(s => !idCourse.Contains(s.C_Course_Id.Value)).ToList();
                            }
                            prom.CustomerGroups = null;
                        }
                    }
                }
                var course = await _gridRepository.GetCourses(userId);
                if (course.Any())
                {
                    homeData.Courses = AutoMapperHelper.Map<Course, CourseDTO, IEnumerable<Course>, IEnumerable<CourseDTO>>(course);
                    foreach (var co in homeData.Courses)
                    {
                        co.Full_Image_Url = backendUrl + co.ImageUrl;
                        co.Full_IconUrl = backendUrl + co.IconUrl;
                    }
                }
                data.IsSuccess = true;
                homeData.IsConnectSdk = _config.GetValue<bool>("IsConnectSdk");
                data.Data = homeData;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                data.Message = e.Message;
            }
            return data;
        }

        public async void SettingLanguage(string lang, string userId)
        {
            var customer = await _gridRepository.GetCustomer(userId);
            customer.Languague = lang;
            _gridRepository.SettingLanguage(customer);
        }

        public async Task<RespondData> GetMemberCard(string userId)
        {
            return _gridRepository.GetMemberCard(userId);
        }

        public async void InsertFcmTokenDevice(string fcmToken, string lang, string userId)
        {
            var customer = await _gridRepository.GetCustomer(userId);
            if (!string.IsNullOrEmpty(fcmToken))
            {
                customer.FcmTokenDevice = fcmToken;
            }
            if (!string.IsNullOrEmpty(lang))
            {
                customer.Languague = lang;
            }
            customer.UpdatedUser = userId;
            customer.UpdatedDate = DateTime.Now;
            _gridRepository.InsertFcmTokenDevice(customer);
        }

        public RespondData GetContactAllOrg()
        {
            var data = _gridRepository.GetContactAllOrg();
            if (data.Any())
            {
                return new RespondData { IsSuccess = true, Data = data };
            }
            else
            {
                return new RespondData { IsSuccess = false, MsgCode = "53" };
            }
        }

        public bool GetStatusMessageVnByUser(string userId)
        {
            return _gridRepository.GetStatusMessageVnByUser(userId);
        }

        public bool UpdateStatusMessageVnByUser(string userId, bool v)
        {
            return _gridRepository.UpdateStatusMessageVnByUser(userId, v);
        }

        public RespondData UpdateAppUserVersion(string version, string userId)
        {
            RespondData RespondData = _gridRepository.UpdateAppUserVersion(version, userId);
            return RespondData;
        }

        public string GetAppVersion(bool isAndroid)
        {
            if (isAndroid)
            {
                return _config.GetSection("versionApp").GetValue<string>("AndroidVersion");
            }
            else
            {
                return _config.GetSection("versionApp").GetValue<string>("IosVersion");
            }
        }

        public RespondData CheckVersionApp(int platform, string currVer, string userId)
        {
            _gridRepository.UpdateAppUserVersion(currVer, userId);
            if (platform == 1)
            {
                var android = _config.GetSection("versionApp").GetValue<string>("AndroidVersion");
                var andPlit = android.Split('.');
                var curVerPlit = currVer.Split('.');
                if (Convert.ToInt32(andPlit[0]) == Convert.ToInt32(curVerPlit[0])
                    && Convert.ToInt32(andPlit[1]) == Convert.ToInt32(curVerPlit[1])
                    && Convert.ToInt32(andPlit[2]) > Convert.ToInt32(curVerPlit[2]))
                {
                    return new RespondData { IsSuccess = true, Data = 1 };
                }
                if (Convert.ToInt32(andPlit[0]) > Convert.ToInt32(curVerPlit[0])
                    || Convert.ToInt32(andPlit[1]) > Convert.ToInt32(curVerPlit[1]))
                {
                    return new RespondData { IsSuccess = true, Data = 2 };
                }
                return new RespondData { IsSuccess = true, Data = 0 };
            }
            if (platform == 0)
            {
                var ios = _config.GetSection("versionApp").GetValue<string>("IosVersion");
                var iosPlit = ios.Split('.');
                var curVerPlit = currVer.Split('.');
                if (iosPlit[0] == curVerPlit[0] && iosPlit[1] == curVerPlit[1]
                    && Convert.ToInt32(iosPlit[2]) > Convert.ToInt32(curVerPlit[2]))
                {
                    return new RespondData { IsSuccess = true, Data = 1 };
                }
                if (Convert.ToInt32(iosPlit[0]) > Convert.ToInt32(curVerPlit[0])
                    || Convert.ToInt32(iosPlit[1]) > Convert.ToInt32(curVerPlit[1]))
                {
                    return new RespondData { IsSuccess = true, Data = 2 };
                }
                return new RespondData { IsSuccess = true, Data = 0 };
            }
            return new RespondData { IsSuccess = true, Data = 0 };
        }
    }
}
