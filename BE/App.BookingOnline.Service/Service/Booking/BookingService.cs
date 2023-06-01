using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.MailService;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Data.Models.Golf;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Booking;
using App.BookingOnline.Service.DTO.Common;
using App.BookingOnline.Service.IService.Common;
using App.BookingOnline.Service.Service.Common;
using App.Core;
using App.Core.Domain;
using App.Core.Helper;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NPOI.POIFS.Crypt.Dsig;
using RestSharp;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using static App.Core.Enums;

namespace App.BookingOnline.Service
{
    public class BookingService : BaseGridDataService<BookingDTO, Booking, BookingFilterModel, IBookingRepository>
        , IBookingService
    {
        private ICourseRepository _courseRepo;
        private IBookingLineRepository _bookingLineRepo;
        private ISmsHistoryRepository _smsHistoryRepository;
        private ISmsHistoryService _smsHistoryService;
        private readonly ILogger _log;
        public IConfiguration Configuration { get; }
        private const string keyCryp = "f3a54a33f297427183d7bb4d4530982c";
        private string backendUrl;
        private readonly IMailService _mailService;
        private readonly IHolidayRepository _holidayRepository;
        private readonly ISettingRepository _settingRepository;
        public BookingService(IBookingRepository gridRepository, ICourseRepository courseRepo, ISmsHistoryRepository smsHistoryRepository, IConfiguration configuration,
            IBookingLineRepository bookingLineRepo, ILogger<BookingRepository> logger, IMailService mailService
            , IHolidayRepository holidayRepository, ISmsHistoryService smsHistoryService, ISettingRepository settingRepository) : base(gridRepository)
        {
            _courseRepo = courseRepo;
            _bookingLineRepo = bookingLineRepo;
            _log = logger;
            Configuration = configuration;
            _smsHistoryRepository = smsHistoryRepository;
            _mailService = mailService;
            _smsHistoryService = smsHistoryService;
            _holidayRepository = holidayRepository;
            _settingRepository = settingRepository;
            backendUrl = Configuration.GetSection("urlData").GetValue<string>("BackendUrl") + "/";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                backendUrl = Configuration.GetSection("urlData").GetValue<string>("BackendUrlPro") + "/";
            }
        }

        public override BookingDTO GetById(Guid Id)
        {
            var entity = _gridRepository.SingleOrDefault(Id);
            var lines = entity.BookingLines;
            var spLines = entity.BookingSpecialRequests;

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Booking, BookingDTO>()
                .ForMember(d => d.BookingTeetime, o => o.MapFrom(s => s.BookingLines))
                .AfterMap((s, d) => d.Golfers = d.BookingTeetime.Count())
                .AfterMap((s, d) =>
                {
                    for (var i = 0; i < s.BookingLines.Count; i++)
                    {
                        if (i > 0 && !string.IsNullOrEmpty(d.TotalTeetimeDisplay) && !d.TotalTeetimeDisplay.Contains(s.BookingLines[i].Tee_Time.Value.ToString("HH:mm")))
                        {
                            d.TotalTeetimeDisplay += "-" + s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                        }
                        else if (i == 0)
                        {
                            d.TotalTeetimeDisplay = s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                        }
                    }
                })
                .AfterMap((s, d) => d.CardEmail = s.AppUser?.Email)
                .AfterMap((s, d) => d.CardMobilePhone = s.AppUser?.PhoneNumber)
                .AfterMap((s, d) => d.CardFullName = s.AppUser?.FullName)
                .AfterMap((s, d) => d.CourseName = s.Course?.Name);
                c.CreateMap<BookingLine, BookingLineDTO>();
                c.CreateMap<Course, CourseDTO>();
                c.CreateMap<CustomerGroup, CustomerGroupDTO>();
                c.CreateMap<BookingSpecialRequest, BookingSpecialRequestDTO>();

            });
            var mapper = config.CreateMapper();
            var dto = mapper.Map<Booking, BookingDTO>(entity);
            var enumStatus = EnumHelper<StatusBookingLine>.Parse(dto.Status);
            dto.StatusName = EnumHelper.GetDisplayName(enumStatus);

            //dto.BookingTeetime = mapper.Map<List<BookingLine>, List<BookingLineDTO>>(lines);
            //dto.BookingSpecialRequests = mapper.Map<List<BookingSpecialRequest>, List<BookingSpecialRequestDTO>>(spLines);
            //if (entity.C_Course_Id.HasValue)
            //{
            //    dto.CourseName = entity.Course.Name;
            //}

            //dto.CardFullName = entity.AppUser?.FullName;
            //dto.CardMobilePhone = entity.AppUser?.PhoneNumber;
            //dto.CardEmail = entity.AppUser?.Email;

            var user = _gridRepository.GetMemberCard(entity.UserId);
            if (user.Any())
            {
                var memberByCourse = user.FirstOrDefault(x => x.MemberCardCourses.Select(s => s.C_Course_Id)
                                        .Contains(entity.C_Course_Id.Value));
                if (memberByCourse != null)
                {
                    dto.GolfCardNo = memberByCourse.Golf_CardNo;
                    dto.CardCourseName = entity.Course.Name;
                }
                else
                {
                    dto.GolfCardNo = user.FirstOrDefault()?.Golf_CardNo;
                }
            }

            if (!string.IsNullOrEmpty(dto.Cancel_User))
            {
                dto.Cancel_UserName = _gridRepository.GetAppUserById(dto.Cancel_User)?.FullName;
            }

            return dto;
        }

        public IEnumerable<BookingDTO> BookingHistory(BookingFilterModel filter)
        {

            var entitys = _gridRepository.BookingHistory(filter);

            var data = new List<BookingDTO>();

            var isConnectSDk = Configuration.GetValue<bool>("IsConnectSdk");
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Booking, BookingDTO>()
                .ForMember(d => d.BookingTeetime, o => o.MapFrom(s => s.BookingLines))
                .AfterMap((s, d) => d.IsConnectSdk = isConnectSDk)
                .AfterMap((s, d) => d.Golfers = d.BookingTeetime.Count())
                .AfterMap((s, d) =>
                {
                    for (var i = 0; i < s.BookingLines.Count; i++)
                    {
                        if (i > 0 && !string.IsNullOrEmpty(d.TotalTeetimeDisplay) && !d.TotalTeetimeDisplay.Contains(s.BookingLines[i].Tee_Time.Value.ToString("HH:mm")))
                        {
                            d.TotalTeetimeDisplay += "\n" + s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                        }
                        else if (i == 0)
                        {
                            d.TotalTeetimeDisplay = s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                        }
                    }
                })
                .AfterMap((s, d) => d.CourseName = s.Course?.Name);
                c.CreateMap<BookingLine, BookingLineDTO>();
                c.CreateMap<BookingSpecialRequest, BookingSpecialRequestDTO>();
                c.CreateMap<Course, CourseDTO>();
                c.CreateMap<CustomerGroup, CustomerGroupDTO>();
            });

            var mapper = config.CreateMapper();
            var dto = mapper.Map<IEnumerable<Booking>, IEnumerable<BookingDTO>>(entitys.ToList());

            //foreach (Booking bok in entitys)
            //{

            //    var lines = bok.BookingLines.OrderBy(x => x.Tee_Time).ToList();
            //    var spLines = bok.BookingSpecialRequests;
            //    bok.BookingSpecialRequests = null;
            //    var config = new MapperConfiguration(c =>
            //    {
            //        c.CreateMap<Booking, BookingDTO>()
            //        .ForMember(d => d.BookingTeetime,
            //         o => o.MapFrom(s => s.BookingLines))
            //        .AfterMap((s, d) => d.IsConnectSdk = isConnectSDk)
            //        .AfterMap((s, d) => d.Golfers = d.BookingTeetime.Count())
            //        .AfterMap((s, d) => d.CourseName = d.Courses?.Name);
            //        c.CreateMap<BookingLine, BookingLineDTO>();                    
            //    });

            //    var mapper = config.CreateMapper();
            //    var dto = mapper.Map<List<Booking>, List<BookingDTO>>(entitys.ToList());                
            //    //dto.IsConnectSdk = Configuration.GetValue<bool>("IsConnectSdk");

            //    dto.Golfers = dto.BookingTeetime.Count();
            //    dto.CourseName = bok.Course?.Name;
            //    data.Add(dto);
            //}

            return dto;
        }

        public RespondData BookingHistoryDetail(BookingFilterModel model)
        {
            try
            {
                var result = new RespondData();
                var bok = _gridRepository.BookingHistoryDetail(model.BookingId, model.UserId);
                if (bok == null)
                {
                    result.IsSuccess = false;
                    result.MsgCode = "45";
                    return result;
                }
                var customerGroupCodes = _gridRepository.GetAllCustomerGroup();
                var isConnectSDk = Configuration.GetValue<bool>("IsConnectSdk");
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Booking, BookingDTO>()
                    .ForMember(d => d.BookingTeetime, o => o.MapFrom(s => s.BookingLines.OrderBy(o => o.CreatedDate)))
                    .AfterMap((s, d) => d.Golfers = d.BookingTeetime.Count())
                    .AfterMap((s, d) =>
                    {
                        for (var i = 0; i < s.BookingLines.Count; i++)
                        {
                            d.BookingTeetime[i].CustomerGroupName = customerGroupCodes.Where(x => x.Id == d.BookingTeetime[i].MB_CustomerGroup_Id).FirstOrDefault()?.Name;
                            if (i > 0 && !string.IsNullOrEmpty(d.TotalTeetimeDisplay) && !d.TotalTeetimeDisplay.Contains(s.BookingLines[i].Tee_Time.Value.ToString("HH:mm")))
                            {
                                d.TotalTeetimeDisplay += "-" + s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                            }
                            else if (i == 0)
                            {
                                d.TotalTeetimeDisplay = s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                            }
                        }
                        d.BookingSpecialRequests = new List<BookingSpecialRequestDTO>();
                        foreach (var sp in s.BookingSpecialRequests)
                        {
                            var spItem = AutoMapperHelper.Map<BookingSpecialRequest, BookingSpecialRequestDTO>(sp);
                            spItem.Name = sp.BookingOtherType?.Name;
                            spItem.NameEn = sp.BookingOtherType?.NameEn;
                            spItem.Code = sp.BookingOtherType?.Code;
                            spItem.Description = sp.Description;
                            d.BookingSpecialRequests.Add(spItem);
                        }
                    })
                    .AfterMap((s, d) => d.CardEmail = s.AppUser?.Email)
                    .AfterMap((s, d) => d.CardMobilePhone = s.AppUser?.PhoneNumber)
                    .AfterMap((s, d) => d.CardFullName = s.AppUser?.FullName)
                    .AfterMap((s, d) =>
                    {
                        d.Courses = new CourseDTO
                        {
                            Id = s.Course.Id,
                            C_Org_Id = s.Course.Id,
                            Code = s.Course.Code,
                            OrgCode = s.Organization.Code,
                            Name = s.Course.Name,
                            OrganizationName = s.Organization.Name,
                            ImageUrl = s.Course.ImageUrl,
                            Full_Image_Url = (backendUrl + s.Course.ImageUrl).Replace("\\", "//"),
                            Full_IconUrl = (backendUrl + s.Course.IconUrl).Replace("\\", "//")
                        };
                    })
                    .AfterMap((s, d) => d.CourseName = s.Course?.Name);
                    c.CreateMap<BookingLine, BookingLineDTO>();
                    c.CreateMap<Organization, OrganizationDTO>();
                    c.CreateMap<M_Promotion, PromotionDTO>();
                    c.CreateMap<Course, CourseDTO>();
                    c.CreateMap<CustomerGroup, CustomerGroupDTO>();
                    c.CreateMap<BookingSpecialRequest, BookingSpecialRequestDTO>();
                    c.CreateMap<BookingSession, BookingSessionDTO>();
                });
                var mapper = config.CreateMapper();
                var dto = mapper.Map<Booking, BookingDTO>(bok);
                var enumStatus = EnumHelper<StatusBookingLine>.Parse(dto.Status);
                dto.StatusName = EnumHelper.GetDisplayName(enumStatus);

                if (bok.M_Promotion_Id != null && bok.M_Promotion_Id != Guid.Empty)
                {
                    var promotion = _gridRepository.GetPromotionById((Guid)bok.M_Promotion_Id).Result;
                    if (promotion != null)
                    {
                        dto.PromotionName = promotion.Name;
                    }
                }

                result.Data = dto;
                result.IsSuccess = true;
                return result;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "BookingHistoryDetail"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, MsgCode = "45" };
            }
        }

        public RespondData GetBookingTeetimeData(BookingTeeTime model)
        {
            try
            {
                _log.LogInformation("GetBookingTeetimeData-input =" + model.CourseId + "---PromotionId----" + model.PromotionId);
                var result = new RespondData { IsSuccess = true };
                var data = new BookingDTO();
                data.BookingDates = new List<DateTime>();
                data.UserId = model.UserId;

                for (var i = 1; i <= 7; i++)
                {
                    data.BookingDates.Add(DateTime.Now.AddDays(i));
                }
                if (model.BookingDate == null)
                {
                    model.BookingDate = data.BookingDates.FirstOrDefault();
                }

                // get member card
                var memberCard = _gridRepository.GetMemberCard(model.UserId);
                if (memberCard.Any())
                {
                    var member = memberCard.Where(x => x.MemberCardCourses.Select(s => s.C_Course_Id).Contains(model.CourseId.Value));
                    if (member.Any())
                    {
                        data.CardFullName = member.FirstOrDefault().Golf_FullName;
                        data.GolfCardNo = member.FirstOrDefault().Golf_CardNo;
                        data.CardMemberType = (int)Enums.MemberCardType.member;
                        data.CardMemberName = Enums.MemberCardType.member.ToString();
                    }
                    else
                    {
                        data.CardFullName = memberCard.FirstOrDefault().Golf_FullName;
                        data.GolfCardNo = memberCard.FirstOrDefault().Golf_CardNo;
                        data.CardMemberType = (int)Enums.MemberCardType.memberBrg;
                        data.CardMemberName = Enums.MemberCardType.memberBrg.ToString();
                    }
                }
                else
                {
                    data.CardFullName = _gridRepository.GetCustomerByUserId(model.UserId)?.FullName;
                    data.CardMemberType = (int)Enums.MemberCardType.visitor;
                    data.CardMemberName = Enums.MemberCardType.visitor.ToString();
                }


                if (model.CourseId != Guid.Empty && model.CourseId != null)
                {
                    var promotionList = _gridRepository.GetPromotionByBooking(model.CourseId.Value,
                                                        model.BookingDate.Value, model.BookingDate.Value.AddDays(15));
                    data.Promotions = AutoMapperHelper.Map<M_Promotion, PromotionDTO, List<M_Promotion>, List<PromotionDTO>>(promotionList);
                    foreach (var prom in data.Promotions)
                    {
                        prom.Full_Image_Url = backendUrl + prom.Img_Url;
                        prom.Full_Image_Url = prom.Full_Image_Url.Replace("\\", "//");
                        prom.Full_Image_Url_En = backendUrl + prom.Img_Url_En;
                        prom.Full_Image_Url_En = prom.Full_Image_Url_En.Replace("\\", "//");
                        prom.PromotionCourses = new List<PromotionCourseDTO>();
                        var proCourse = promotionList.FirstOrDefault(x => x.Id == prom.Id)?.PromotionCourse;
                        foreach (var aa in proCourse)
                        {
                            prom.PromotionCourses.Add(new PromotionCourseDTO
                            {
                                M_Promotion_Id = aa.M_Promotion_Id,
                                C_Course_Id = aa.C_Course_Id,
                                CourseName = aa.Course.Name,
                                CourseCode = aa.Course.Code
                            });
                        }
                    }
                    if (model.PromotionId != Guid.Empty && model.PromotionId != null)
                    {
                        var seletedPromotion = _gridRepository.GetPromotionById(model.PromotionId.Value).Result;
                        data.SeletedPromotion = AutoMapperHelper.Map<M_Promotion, PromotionDTO>(seletedPromotion);
                        data.SeletedPromotion.Full_Image_Url = backendUrl + data.SeletedPromotion.Img_Url;
                        data.SeletedPromotion.Full_Image_Url = data.SeletedPromotion.Full_Image_Url.Replace("\\", "//");
                        data.M_Promotion_Id = seletedPromotion.Id;
                        if (data.SeletedPromotion.Promotion_Type == Enums.PromotionType.seagolf.ToString())
                        {
                            data.Promotions = data.Promotions.Where(x => x.Promotion_Type == Enums.PromotionType.seagolf.ToString()).ToList();
                        }
                        else
                        {
                            data.Promotions = data.Promotions.Where(x => x.Promotion_Type == Enums.PromotionType.normal.ToString()).ToList();
                        }
                    }
                    else
                    {
                        data.Promotions = data.Promotions.Where(x => x.Promotion_Type == Enums.PromotionType.normal.ToString()).ToList();
                    }

                    var course = _gridRepository.GetCourseById(model.CourseId.Value);
                    data.Courses = AutoMapperHelper.Map<Course, CourseDTO>(course);
                    data.Courses.Full_IconUrl = backendUrl + data.Courses.IconUrl;
                    data.Courses.Full_IconUrl = data.Courses.Full_IconUrl.Replace("\\", "//");
                    data.Courses.Full_Image_Url = backendUrl + data.Courses.ImageUrl;
                    data.Courses.Full_Image_Url = data.Courses.Full_Image_Url.Replace("\\", "//");
                    data.Courses.OrgCode = course.Organization?.Code;
                    var courseline = _gridRepository.GetCourseTemplateLineByCourse(model.CourseId.Value, course.C_Org_Id, model.BookingDate.Value);
                    BuildTeeSheetTime(courseline, model.UserId, model.BookingDate.Value, data.SeletedPromotion, model.CourseId.Value,
                        course.C_Org_Id, data.CardMemberType.Value, ref data);
                }

                result.Data = data;
                if (!data.BookingTeetime.Any() || data.BookingTeetime.Count == 0)
                {
                    return new RespondData { IsSuccess = false, Data = data, MsgCode = "28" };
                }
                return result;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "GetBookingTeetimeData"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, MsgCode = "28" };
            }
        }

        private void BuildTeeSheetTime(IEnumerable<GF_CourseTemplateLine> courseLine, string userId,
                                        DateTime bookingDate, PromotionDTO promotion, Guid courseId, Guid orgId, int cardMemberType, ref BookingDTO data)
        {
            if (data.BookingTeetime == null)
            {
                data.BookingTeetime = new List<BookingLineDTO>();
            }
            DateTime promotionStart = DateTime.Now;
            DateTime promotionEnd = DateTime.Now;
            if (promotion != null)
            {
                if (!string.IsNullOrEmpty(promotion.ApplyTime_From))
                {
                    promotionStart = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day,
                                            Convert.ToInt32(promotion.ApplyTime_From.Split(':')[0]), Convert.ToInt32(promotion.ApplyTime_From.Split(':')[1]), 0, 0);
                }
                else
                {
                    promotionStart = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day, 0, 0, 1, 0);
                }
                if (!string.IsNullOrEmpty(promotion.ApplyTime_To))
                {
                    promotionEnd = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day,
                       Convert.ToInt32(promotion.ApplyTime_To.Split(':')[0]), Convert.ToInt32(promotion.ApplyTime_To.Split(':')[1]), 0, 0);
                }
                else
                {
                    promotionEnd = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day, 23, 59, 59, 0);
                }
            }

            foreach (var item in courseLine)
            {
                if (!string.IsNullOrEmpty(item.StartTime) && !string.IsNullOrEmpty(item.EndTime))
                {
                    var start = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day,
                        Convert.ToInt32(item.StartTime.Split(':')[0]), Convert.ToInt32(item.StartTime.Split(':')[1]), 0, 0);

                    var end = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day,
                         Convert.ToInt32(item.EndTime.Split(':')[0]), Convert.ToInt32(item.EndTime.Split(':')[1]), 0, 0);

                    for (var i = 0; i < 10000; i += item.Interval.Value)
                    {
                        var time = start;
                        if (i != 0)
                        {
                            time = start.AddMinutes(i);
                        }
                        if (time > end)
                        {
                            break;
                        }
                        //if (item.Part == "1")
                        //{
                        //    var a = 1;
                        //}
                        //if (item.Part == "2")
                        //{
                        //    var a = 1;
                        //}
                        //if (item.Part == "3")
                        //{
                        //    var a = 1;
                        //}
                        var isbook = _gridRepository.CheckTeesheet(time, userId, courseId, orgId, time.AddMinutes(item.Interval.Value), cardMemberType);
                        data.BookingTeetime.Add(new BookingLineDTO
                        {
                            IsSelected = false,
                            TimeDisplay = time.ToString("HH:mm"),
                            Tee_Time = time,
                            TeeTimeEnd = time.AddMinutes(item.Interval.Value),
                            Part = item.Part,
                            StartTee = Constants.DefualtStartTee,
                            AvailableFlightSeq = isbook,
                            IsBooked = isbook == 0 ? true : false,
                            IsPromotion = promotion != null && promotionStart <= start && promotionEnd >= start
                        });
                    }
                }
            }
        }

        public List<CustomerGroupDTO> GetCustomerGroupData(Guid orgId)
        {
            var data = AutoMapperHelper.Map<CustomerGroup, CustomerGroupDTO, List<CustomerGroup>, List<CustomerGroupDTO>>
                        (_gridRepository.GetCustomerGroup(orgId).ToList());
            return data;
        }

        public RespondData GetCustomerGroup(Guid orgId)
        {
            var data = GetCustomerGroupData(orgId);
            if (data.Any())
            {
                return new RespondData { IsSuccess = true, Data = data };
            }
            else
            {
                return new RespondData { IsSuccess = false, MsgCode = "32" };
            }
        }

        public RespondData GetBookingOtherType(Guid courseId)
        {
            var data = AutoMapperHelper.Map<BookingOtherType, BookingOtherTypeDTO, List<BookingOtherType>, List<BookingOtherTypeDTO>>
                        (_gridRepository.GetBookingOtherType(courseId).ToList());
            return new RespondData { IsSuccess = true, Data = data };
        }

        public object GetBookingData(BookingDTO model)
        {
            throw new NotImplementedException();
        }

        public string CheckSelectedTeesheet(List<BookingLineDTO> selectedTeeSheet, string userId, Guid courseId, Guid orgId)
        {
            return _gridRepository.CheckSelectedTeesheets(selectedTeeSheet.Select(s =>
            new SelectedTeeSheetTemp { Tee_Time = s.Tee_Time, TeeTimeEnd = s.TeeTimeEnd }), userId, courseId, orgId);
        }
        public bool CheckNumberAvailableSlot(List<DateTime> tees, string userId, Guid courseId, Guid orgId, int? cardType)
        {
            return _gridRepository.CheckNumberAvailableSlot(tees, userId, courseId, orgId, cardType);
        }
        public async ValueTask<BookingDTO> InsertBookingSession(BookingDTO model)
        {
            var bookingsession = AutoMapperHelper.Map<BookingDTO, BookingSession>(model);
            var teetime = new List<BookingSessionLine>();
            var lines = model.BookingTeetime.Where(x => x.IsSelected);
            var lockTeetime = Configuration.GetSection("loginTime").GetValue<int>("LockTeetime");
            foreach (var item in lines)
            {
                teetime.Add(new BookingSessionLine
                {
                    CreatedUser = model.CreatedUser,
                    CreatedDate = model.CreatedDate,
                    Tee_Time = item.Tee_Time,
                    Start_Time = DateTime.Now,
                    End_Time = DateTime.Now.AddMinutes(lockTeetime),
                    Status = StatusBookingLine.open.ToString(),
                    DateId = model.DateId,
                    IsActive = true
                });
            }
            bookingsession.C_Org_Id = model.Courses.C_Org_Id;
            bookingsession.C_Course_Id = model.Courses.Id;
            bookingsession.BookingSessionLines = teetime;
            bookingsession.BookingType = BookingButtonType.TeesheetStep.ToString();
            bookingsession.Status = StatusBookingLine.open.ToString();
            bookingsession.IsActive = true;
            bookingsession = await _gridRepository.InsertBookingSession(bookingsession);
            model.GF_Booking_Session_Id = bookingsession.Id;

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="userId"></param>
        /// <param name="isFromWeb">từ mobile = false từ web = true</param>
        /// <param name="bookingCode"></param>
        /// <param name="loginUser"></param>
        /// <param name="cancelId"> cancel tren mobile cancelId = null, trên web thì có giá trị</param>
        /// <param name="cancelDesc"></param>
        /// <returns></returns>
        public RespondData CancelBooking(Guid bookingId, string userId, bool isFromWeb, string bookingCode = "", string loginUser = "", string cancelId = "", string cancelDesc = "")
        {
            using (LogContext.PushProperty("MethodName", "CancelBooking"))
            {
                _log.LogInformation("BookingId: " + bookingId + " - BookingCode:" + bookingCode + " - UserId:" + userId + " -isFromWeb: " + isFromWeb + " -cancelId: " + cancelId);
            }

            var booking = _gridRepository.CancelBooking(bookingId, userId, cancelId, cancelDesc, isFromWeb);
            if (booking != null)
            {
                var bookingDto = new BookingDTO();
                bookingDto.Id = booking.Id;
                bookingDto.BookingCode = booking.BookingCode;
                bookingDto.NonRefundedFee = booking.NonRefundedFee;
                bookingDto.Cancel_Time = booking.Cancel_Time;
                var isConnectSdk = Configuration.GetValue<bool>("IsConnectSdk");
                if (isConnectSdk)
                {
                    var isReturn = _gridRepository.CheckReturnMoneyRuleWhenCancel(booking);
                    if (isReturn > 0)
                    {
                        var payment = booking.TransactionPayments.Where(x => x.IsActive).FirstOrDefault();
                        if (payment != null)
                        {
                            bookingDto.LinkCardAcctId = payment.LinkCardAcctId;
                            bookingDto.IsRefundMoney = true;
                            if (isReturn == 1)
                            {
                                bookingDto.TotalRefund = payment.TotalMoney / 2;
                            }
                            if (isReturn == 2)
                            {
                                bookingDto.TotalRefund = payment.TotalMoney;
                            }
                            bookingDto.Traceid = payment.Traceid; //bookingcode
                            bookingDto.Ftid = payment.Ftid;
                        }
                        else
                        {
                            return new RespondData { IsSuccess = false, MsgCode = "57" };
                        }
                    }
                }

                _gridRepository.CancelBookingNotifyInQueue(bookingId, userId);
                if (isFromWeb)
                {
                    _gridRepository.InsertCancelBookingNotifyInQueue(loginUser, bookingId, bookingCode, userId);
                }
                SendEmailCancelBooking(booking, isFromWeb);
                return new RespondData { IsSuccess = true, Data = bookingDto };
            }
            return new RespondData { IsSuccess = false, MsgCode = "66" };
        }

        private void SendEmailCancelBooking(Booking booking, bool isFromWeb)
        {
            try
            {
                var cust = _gridRepository.GetCustomerByUserId(booking.UserId);
                var emailTemp_title = _gridRepository.GetEmailTitleCancelBooking(Constants.LangVn);
                var emailTemp = _gridRepository.GetEmailTemplateCancelBooking(Constants.LangVn);
                if (cust.Languague == Constants.LangEn)
                {
                    emailTemp_title = _gridRepository.GetEmailTitleCancelBooking(Constants.LangEn);
                    emailTemp = _gridRepository.GetEmailTemplateCancelBooking(Constants.LangEn);
                }
                var title = string.Empty;
                if (emailTemp_title != null)
                {
                    title = emailTemp_title.Replace("[Mã booking]", booking.BookingCode);
                    title = title.Replace("[Tên User]", booking.AppUser.FullName);
                    title = title.Replace("[Mã sân]", booking.Course.Name);
                    var tee = booking.BookingLines.FirstOrDefault().Tee_Time.Value.ToString("HH:mm");
                    title = title.Replace("[Tee Time]", tee);
                    title = title.Replace("[ngày lên chơi đặt]", booking.DateId.Value.ToString("dd/MM/yyyy"));
                }

                if (emailTemp != null)
                {
                    var emailString = emailTemp;
                    emailString = emailString.Replace("[Mã booking]", booking.BookingCode);
                    emailString = emailString.Replace("[Tên User]", booking.AppUser?.FullName);
                    emailString = emailString.Replace("[Số thẻ tại org sân nếu có]", booking.BookingLines.FirstOrDefault()?.CardNo);
                    emailString = emailString.Replace("[Thời gian book thành công trên app]", booking.UpdatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss"));
                    emailString = emailString.Replace("[Số điện thoại hỗ trợ]", booking.Organization?.PhoneSupport);
                    emailString = emailString.Replace("[Email hỗ trợ]", booking.Organization?.EmailSupport);
                    var teeString = string.Empty;
                    foreach (var line in booking.BookingLines)
                    {
                        if (!teeString.Contains(line.Tee_Time.Value.ToString("HH:mm")))
                        {
                            teeString += line.Tee_Time.Value.ToString("HH:mm") + " -";
                        }
                    }

                    emailString = emailString.Replace("[Tee Time]", teeString.Remove(teeString.Length - 1, 1));
                    emailString = emailString.Replace("[Ngày lên chơi]", booking.DateId.Value.ToString("dd/MM/yyyy"));
                    emailString = emailString.Replace("[Sân đã đặt]", booking.Course.Name);
                    emailString = emailString.Replace("[Org]", booking.Organization.Name);
                    emailString = emailString.Replace("[n]", (booking.BookingLines.Count()).ToString());
                    emailString = emailString.Replace("[tổng phí ước tính]", String.Format("{0:C}", booking.TotalEstimateFee.Value));
                    emailString = emailString.Replace("[tổng phí đặt cọc đã thanh toán]", String.Format("{0:C}", booking.NonRefundedFee.Value));

                    var lineContext = string.Empty;
                    var lineStr = "Tên đặt chỗ: [Tên người chơi] - [Số thẻ nếu có] - Loại khách: [Loại khách] - [Phí ước tính] VND";
                    var lineStr1 = "Tên đặt chỗ: [Tên người chơi] - [Số thẻ nếu có] - Loại khách: [Loại khách] - [Phí ước tính]";

                    foreach (var line in booking.BookingLines)
                    {
                        lineStr = lineStr.Replace("[Tên người chơi]", line.CustomerFullName);
                        if (!string.IsNullOrEmpty(line.CardNo))
                        {
                            lineStr = lineStr.Replace("[Số thẻ nếu có]", line.CardNo);
                        }
                        else
                        {
                            lineStr = lineStr.Replace("[Số thẻ nếu có] -", "");
                        }
                        lineStr = lineStr.Replace("[Loại khách]", line.CustomerGroup.Name);
                        lineStr = lineStr.Replace("[Phí ước tính]", String.Format("{0:C}", line.EstimatedPrice.Value));
                        lineContext += lineStr + "<br>";
                        lineStr = lineStr1;
                    }
                    emailString = emailString.Replace(lineStr1, lineContext);
                    //emailString = emailString.Replace(lineStr, "");
                    var emailUser = _smsHistoryRepository.GetEmailCourse(booking.C_Org_Id);
                    var clientEmail = booking?.AppUser?.Email;
                    if (clientEmail != null)
                    {
                        _mailService.SendMailAsync(clientEmail, "", "", title, emailString, "");
                    }
                    foreach (var user in emailUser)
                    {
                        _mailService.SendMailAsync(user, "", "", title, emailString, "");
                    }
                }
                var inData = new BK_App_Import_BookingSaveModel();
                inData.OrgCode = booking.Organization.Code;
                inData.CourseCode = booking.Course.Code;
                inData.ActionType = "cancel";
                inData.BookingCode = booking.BookingCode;
                inData.CallLocation = isFromWeb ? "web" : "app";
                inData.UserName = booking.AppUser?.UserName;
                inData.Rows = new List<BK_App_Import_BookingDTO>();
                foreach (var line in booking.BookingLines)
                {
                    var child = new BK_App_Import_BookingDTO
                    {
                        CardNo = line.CardNo,
                        PlayerName = line.CustomerFullName,
                        FlightSeq = line.FlightSeq.Value,
                        Contact = booking.AppUser.PhoneNumber + " " + booking.AppUser.Email,
                        TeeTime = line.Tee_Time.Value,
                        Hole = line.Hole.Value,
                        StartTee = line.StartTee.Value,
                        BookingLineId = line.Id.ToString(),
                        BookingCode = booking.BookingCode
                    };

                    inData.Rows.Add(child);
                }

                var result = _smsHistoryService.SendDataBBookingToGolf(inData);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "SendEmailCancelBooking"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
            }
        }

        public RespondData GetBankInfo(string userId)
        {
            var data = new List<UserBankInfoDTO>();
            var result = _gridRepository.GetBankInfo(userId);
            foreach (var ite in result)
            {
                var bankInfo = new UserBankInfoDTO
                {
                    UserId = ite?.UserId,
                    CardNo = DecryptorText(ite?.Start_Month),
                    TokenResource = DecryptorText(ite?.Start_Year),
                    FullName = DecryptorText(ite?.FullName)
                };
                data.Add(bankInfo);
            }

            return new RespondData { IsSuccess = true, Data = data };
        }
        public RespondData InsertBankInfo(UserBankInfoDTO model)
        {
            var bankInfo = new UserBankInfo();
            bankInfo.FullName = EncryptorText(model.FullName);
            bankInfo.Start_Month = EncryptorText(model.CardNo);
            bankInfo.Start_Year = EncryptorText(model.TokenResource);
            bankInfo.UserId = model.UserId.Value;
            bankInfo.IsActive = true;

            var data = AutoMapperHelper.Map<UserBankInfoDTO, UserBankInfo>(model);
            RespondData result = _gridRepository.InsertBankInfo(data);

            return result;
        }

        private string EncryptorText(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(keyCryp), new byte[16]);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            var aaa = HttpUtility.UrlEncode(Convert.ToBase64String(array));
            return aaa;
        }

        private string DecryptorText(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return string.Empty;
            }
            byte[] iv = new byte[16];
            var aaa = HttpUtility.UrlDecode(cipherText);
            byte[] buffer = Convert.FromBase64String(aaa);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(keyCryp);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public RespondData GetPaymentType(string userId)
        {
            var data = _gridRepository.GetPaymentType();
            return new RespondData { IsSuccess = true, Data = data };
        }

        public List<BookingSpecialRequestDTO> GetBookingSpecialRequests(Guid? C_Org_Id)
        {
            var data = _gridRepository.GetBookingSpecialRequests(C_Org_Id);
            var result = new List<BookingSpecialRequestDTO>();
            if (data.Any())
            {
                foreach (var item in data)
                {
                    var boo = new BookingSpecialRequestDTO
                    {
                        C_Booking_OtherType_Id = item.Id,
                        Name = item.Name,
                        NameEn = item.NameEn,
                        IsSelected = false,
                        Description = string.Empty
                    };
                    result.Add(boo);
                }
                return result;
            }
            return null;
        }

        public RespondData CheckAllMemberCards(CheckCardDto input)
        {
            string baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApi");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApiPro");
            }
            string checkmemberUrl = Configuration.GetSection("urlData").GetValue<string>("CheckMemberBooking");

            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(checkmemberUrl, Method.POST);
            request.AddHeader("Accept", "*/*");
            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);

            var paramsRequest = new Dictionary<string, object>();
            var newPersons = new List<CheckCardTeetime>(input.CheckCardTeetimes.Count());

            foreach (var item in input.CheckCardTeetimes)
            {
                newPersons.Add(new CheckCardTeetime
                {
                    CardNo = item.CardNo,
                    CustomerFullName = item.CustomerFullName,
                    TeeTime = item.TeeTime
                });
            }

            paramsRequest.Add("OrgCode", input.OrgCode);
            paramsRequest.Add("CourseCode", input.CourseCode);
            paramsRequest.Add("CheckCardTeetimes", newPersons);
            request.AddJsonBody(paramsRequest);

            try
            {
                var memberCard = new List<MemberCardDTO>();

                IRestResponse<CheckCardDto> response = client.Execute<CheckCardDto>(request);
                if (response.IsSuccessful)
                {
                    var weatherForecast = JsonSerializer.Deserialize<GolfMemberBookingRespone>(response.Content);
                    input.CheckCardTeetimes = weatherForecast.Data;
                    if (weatherForecast.IsSuccess)
                    {
                        if (weatherForecast.Data.Any(x => x.Golf_CusGroupCode == ((int)MemberCardType.memberGuest).ToString())
                            && !weatherForecast.Data.Any(x => x.Golf_CusGroupCode == ((int)MemberCardType.member).ToString()))
                        {
                            return new RespondData { IsSuccess = false, Data = weatherForecast.Data, MsgCode = "80" };
                        }
                        return new RespondData { IsSuccess = true, Data = input, MsgCode = weatherForecast.ErrorCode, MsgParams = weatherForecast.MsgParams };
                    }
                    else
                    {
                        return new RespondData { IsSuccess = false, Data = input, MsgCode = weatherForecast.ErrorCode, MsgParams = weatherForecast.MsgParams };
                    }
                }
                else
                {
                    _log.LogError(Constants.GolfApiError + "-CheckAllMemberCards-" + response.ErrorMessage);
                    return new RespondData { IsSuccess = false, MsgCode = "46" };
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, MsgCode = "46" };
            }
        }



        //public RespondData CheckMemberCard(CheckCardDto input)
        //{
        //    string baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApi");
        //    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
        //    {
        //        baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApiPro");
        //    }
        //    string checkmemberUrl = Configuration.GetSection("urlData").GetValue<string>("CheckMemberBooking");

        //    IRestClient client = new RestClient(baseUrl);
        //    IRestRequest request = new RestRequest(checkmemberUrl, Method.POST);
        //    request.AddHeader("Accept", "*/*");
        //    request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);

        //    var paramsRequest = new Dictionary<string, object>();
        //    var newPersons = new List<CheckCardTeetime>(input.CheckCardTeetimes.Count());

        //    foreach (var item in input.CheckCardTeetimes)
        //    {
        //        newPersons.Add(new CheckCardTeetime
        //        {
        //            CardNo = item.CardNo,
        //            CustomerFullName = item.CustomerFullName,
        //            TeeTime = item.TeeTime
        //        });
        //    }

        //    paramsRequest.Add("OrgCode", input.OrgCode);
        //    paramsRequest.Add("CourseCode", input.CourseCode);
        //    paramsRequest.Add("CheckCardTeetimes", newPersons);
        //    request.AddJsonBody(paramsRequest);

        //    try
        //    {
        //        var memberCard = new List<MemberCardDTO>();

        //        IRestResponse<CheckCardDto> response = client.Execute<CheckCardDto>(request);
        //        if (response.IsSuccessful)
        //        {
        //            var weatherForecast = JsonSerializer.Deserialize<GolfMemberBookingRespone>(response.Content);

        //            if (weatherForecast.IsSuccess)
        //            {
        //                if (weatherForecast.Data.Any(x => x.Golf_CusGroupCode == ((int)MemberCardType.memberGuest).ToString())
        //                    && !weatherForecast.Data.Any(x => x.Golf_CusGroupCode == ((int)MemberCardType.member).ToString()))
        //                {
        //                    return new RespondData { IsSuccess = false, Data = weatherForecast.Data, MsgCode = "80" };
        //                }
        //                return new RespondData { IsSuccess = true, Data = weatherForecast.Data, MsgCode = weatherForecast.ErrorCode };
        //            }
        //            else
        //            {
        //                return new RespondData { IsSuccess = false, Data = weatherForecast.Data, MsgCode = weatherForecast.ErrorCode };
        //            }
        //        }
        //        else
        //        {
        //            _log.LogError(Constants.GolfApiError + "-CheckMemberCard-" + response.ErrorMessage);
        //            return new RespondData { IsSuccess = false, MsgCode = "46" };
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
        //        {
        //            _log.LogError(e.Message);
        //            _log.LogError(e.StackTrace);
        //        }
        //        return new RespondData { IsSuccess = false, MsgCode = "46" };
        //    }
        //}

        public RespondData GetPriceGolfPlayer(ref BookingDTO model)
        {
            try
            {
                var totalMoney = new decimal();
                var i = 0;
                var hasGuest = false;
                var hasMember = false;
                foreach (var pers in model.BookingTeetime)
                {
                    var org = _gridRepository.GetOrganization(model.Courses.C_Org_Id);

                    var inputPrice = new BookingTeeTime
                    {
                        CardNo = pers.CardNo,
                        CustomerFullName = pers.CustomerFullName,
                        OrgCode = org?.Code,
                        CourseCode = model.Courses.Code,
                        Golf_CusGroupCode = pers.Golf_CusGroupCode,
                        BookingTime = pers.Tee_Time,
                        BK_CusGroupCode = pers.CustomerGroupCode
                    };
                    var priceResult = CheckPriceBooking(inputPrice);
                    if (priceResult != null && priceResult.IsSuccess)
                    {
                        pers.Golf_Promotion_Name = priceResult.Data.PromotionPrice_Name;
                        pers.Golf_Promotion_Id = priceResult.Data.PromotionPrice_Id;
                        if (priceResult.Data.PublicPrice.HasValue)
                        {
                            pers.Public_Price = Math.Round(priceResult.Data.PublicPrice.Value, 2);
                            if (priceResult.Data.BuggyFee.HasValue)
                            {
                                pers.BuggyFee = priceResult.Data.BuggyFee;
                                pers.Public_Price = pers.Public_Price + Math.Round(priceResult.Data.BuggyFee.Value, 2);
                            }
                        }

                        if (!string.IsNullOrEmpty(priceResult.Data.PromotionPrice_Id))
                        {
                            pers.Promotion_Price = Math.Round(priceResult.Data.PromotionPrice.Value, 2);
                            if (priceResult.Data.BuggyFee.HasValue)
                            {
                                pers.BuggyFee = priceResult.Data.BuggyFee.Value;
                                pers.Promotion_Price = pers.Promotion_Price + Math.Round(priceResult.Data.BuggyFee.Value, 2);
                            }
                        }
                        else
                        {
                            pers.Promotion_Price = pers.Public_Price;
                        }

                        if (model.M_Promotion_Id.HasValue && i == 0)
                        {
                            var promotion = _gridRepository.GetSeagolfByCourse(model.Courses.Id, model.M_Promotion_Id);
                            var seagolf = promotion.Where(x => x.PromotionCustomerGroup.Any(a => a.CustomerGroup.Code == pers.CustomerGroupCode)).FirstOrDefault();

                            if (seagolf != null)
                            {
                                if (seagolf.Promotion_Formula == Enums.PromotionFormula.percent.ToString())
                                {
                                    var pubPrice = pers.Public_Price.Value - ((pers.Public_Price.Value * seagolf.PublicPrice_Percent.Value) / 100);
                                    var proPrice = pers.Promotion_Price - ((pers.Promotion_Price.Value * seagolf.PromotionPrice_Percent.Value) / 100);
                                    if (pers.Promotion_Price > 0 && pers.Promotion_Price < pers.Public_Price)
                                    {
                                        pers.SeagolfPrice = proPrice;
                                    }
                                    else
                                    {
                                        pers.SeagolfPrice = pubPrice;
                                    }
                                    if (priceResult.Data.BuggyFee.HasValue)
                                    {
                                        pers.BuggyFee = priceResult.Data.BuggyFee.Value;
                                        pers.SeagolfPrice = pers.SeagolfPrice + Math.Round(priceResult.Data.BuggyFee.Value, 2);
                                    }
                                }
                                if (seagolf.Promotion_Formula == Enums.PromotionFormula.price.ToString())
                                {
                                    pers.SeagolfPrice = seagolf.Promotion_Value;
                                    if (priceResult.Data.BuggyFee.HasValue)
                                    {
                                        pers.BuggyFee = priceResult.Data.BuggyFee.Value;
                                        pers.SeagolfPrice = pers.SeagolfPrice + Math.Round(priceResult.Data.BuggyFee.Value, 2);
                                    }
                                }
                            }
                        }

                        if (pers.Promotion_Price > 0 && pers.Promotion_Price <= pers.Public_Price)
                        {
                            pers.EstimatedPrice = pers.Promotion_Price.Value;
                            if (pers.SeagolfPrice.HasValue && pers.Promotion_Price > pers.SeagolfPrice)
                            {
                                totalMoney = totalMoney + pers.SeagolfPrice.Value;
                            }
                            else
                            {
                                totalMoney = totalMoney + pers.Promotion_Price.Value;
                            }
                        }
                        else
                        {
                            totalMoney = totalMoney + pers.Public_Price.Value;
                            pers.EstimatedPrice = pers.Public_Price.Value;
                        }
                    }
                    else
                    {
                        return new RespondData
                        {
                            IsSuccess = false,
                            Data = model,
                            MsgCode = "33",
                            MsgParams = new List<string> { pers.CardNo }
                        };
                    }
                    i++;
                }
                model.TotalEstimateFee = totalMoney;
                var lstPromotion = model.BookingTeetime.Where(x => !string.IsNullOrEmpty(x.Golf_Promotion_Name)).Select(s => s.Golf_Promotion_Name);
                if (lstPromotion.Any())
                {
                    foreach (var pro in lstPromotion)
                    {
                        if (model.PromotionName == null)
                        {
                            model.PromotionName = pro;
                        }
                        else
                        {
                            if (!model.PromotionName.Contains(pro))
                            {
                                model.PromotionName += pro + ", ";
                            }
                        }
                    }
                }
                else
                {
                    model.PromotionName = string.Empty;
                }

                var nonRefundedFee = Configuration.GetValue<int>("NonRefundedFee");
                model.NonRefundedFee = nonRefundedFee * model.BookingTeetime.Count();

                return new RespondData { IsSuccess = true, Data = model };
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, Data = model, MsgCode = "34" };
            }
        }

        public GolfPriceBookingRespone CheckPriceBooking(BookingTeeTime input)
        {
            _log.LogInformation(Constants.GolfApiError + "-CheckPriceBooking-input-" + Newtonsoft.Json.JsonConvert.SerializeObject(input));
            string baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApi");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApiPro");
            }
            string checkmemberUrl = Configuration.GetSection("urlData").GetValue<string>("CheckPrice");

            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(checkmemberUrl, Method.POST);
            request.AddHeader("Accept", "*/*");
            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            request.AddParameter("C_Org_Code", input.OrgCode, ParameterType.GetOrPost);
            request.AddParameter("C_Course_Code", input.CourseCode, ParameterType.GetOrPost);
            request.AddParameter("BK_CustomerGroup_Code", input.BK_CusGroupCode, ParameterType.GetOrPost);
            request.AddParameter("Golf_CustomerGroup_Code", input.Golf_CusGroupCode, ParameterType.GetOrPost);
            request.AddParameter("CardNo", "", ParameterType.GetOrPost);
            request.AddParameter("BookingTime", input.BookingTime, ParameterType.GetOrPost);
            request.AddParameter("Hole", 18, ParameterType.GetOrPost);
            try
            {

                IRestResponse<GolfPriceBookingRespone> response = client.Execute<GolfPriceBookingRespone>(request);
                if (response.IsSuccessful)
                {
                    var memberBookingRespone = JsonSerializer.Deserialize<GolfPriceBookingRespone>(response.Content);

                    if (memberBookingRespone != null)
                    {
                        return memberBookingRespone;
                    }
                    else
                    {
                        _log.LogError(Constants.GolfApiError + "-CheckPriceBooking-" + response.ErrorMessage);
                        return null;
                    }
                }
                else
                {
                    _log.LogError(Constants.GolfApiError + "-CheckPriceBooking-" + response.ErrorMessage);
                    return null;
                }
            }
            catch (Exception e)
            {
                _log.LogError(Constants.GolfApiError + "-CheckPriceBooking-" + e.Message);
                _log.LogError(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// insert data to booking table & booking session table
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<RespondData> InssertGolfPlayerStep(BookingDTO model)
        {
            try
            {
                var booKing = new Booking
                {
                    M_Promotion_Id = model.M_Promotion_Id,
                    PromotionName = model.PromotionName,
                    C_Course_Id = model.Courses.Id,
                    C_Org_Id = model.Courses.C_Org_Id,
                    UserId = model.UserId,
                    DateId = model.DateId,
                    BookingType = BookingButtonType.PersonInfoStep.ToString(),
                    GF_Booking_Session_Id = model.GF_Booking_Session_Id,
                    Status = StatusBookingLine.bookedTeetime.ToString(),
                    CreatedDate = DateTime.Now,
                    CreatedUser = model.UserId,
                    IsActive = true,
                    BookedCardNo = model.GolfCardNo,
                    TeeTimeDisplay = model.BookingTeetime.Any() ? string.Join(",", model.BookingTeetime.Select(e => e.Tee_Time.ToString("HH:mm")).Distinct()) : null
                };

                var memberCard = _gridRepository.GetMemberCard(model.UserId);
                if (memberCard.Any())
                {
                    var member = memberCard.Where(x => x.MemberCardCourses.Select(s => s.C_Course_Id).Contains(model.Courses.Id));
                    if (member.Any())
                    {
                        booKing.MemberType = Enums.MemberCardType.member.ToString();
                    }
                    else
                    {
                        booKing.MemberType = Enums.MemberCardType.memberBrg.ToString();
                    }
                }
                else
                {
                    booKing.MemberType = Enums.MemberCardType.visitor.ToString();
                }

                var customerGroupCodes = _gridRepository.GetAllCustomerGroup();

                var bookingLines = new List<BookingLine>();
                var hour = 0;
                var minute = 0;
                var currFlight = 1;
                var fromFlight = 1;
                var teetime = _gridRepository.GetLastBookingLine(model.Courses.Id, model.BookingTeetime.Select(s => s.Tee_Time), model.DateId);
                var teetimeDisplay = string.Empty;

                GF_CourseTemplateLine courseTemplateLine = null;
                if (model.BookingTeetime.Any())
                {
                    var filter = new BookingLineFilterModel
                    {
                        C_Org_Id = model.Courses.C_Org_Id,
                        Tee_Time = model.BookingTeetime.OrderBy(x => x.Tee_Time).FirstOrDefault()?.Tee_Time,
                        C_Course_Id = model.Courses.Id
                    };
                    courseTemplateLine = _bookingLineRepo.GetCourseTemplateLineByTeetime(filter);
                }

                foreach (var line in model.BookingTeetime)
                {
                    var item = new BookingLine();
                    item.IsActive = true;
                    item.C_Org_Id = model.Courses.C_Org_Id;
                    item.CardNo = line.CardNo;
                    item.DateId = model.DateId;
                    if (courseTemplateLine != null)
                    {
                        item.StartTee = courseTemplateLine.StartTee;
                    }
                    else
                    {
                        item.StartTee = Constants.DefualtStartTee;
                    }
                    item.Tee_Time = line.Tee_Time;
                    item.Hole = Constants.DefualtHole;
                    item.MB_CustomerGroup_Id = customerGroupCodes.Where(c => c.Code == line.CustomerGroupCode).FirstOrDefault()?.Id;

                    if (line.Tee_Time.Hour == hour && line.Tee_Time.Minute == minute)
                    {
                        currFlight += 1;
                    }
                    else
                    {
                        fromFlight = teetime.Count(x => x.Tee_Time.Value.Hour == line.Tee_Time.Hour
                                                    && x.Tee_Time.Value.Minute == line.Tee_Time.Minute) + 1;
                        currFlight = fromFlight;
                    }
                    item.FlightSeq = currFlight;
                    hour = line.Tee_Time.Hour;
                    minute = line.Tee_Time.Minute;
                    if (item.FlightSeq > 4)
                    {
                        return new RespondData { IsSuccess = false, Data = model, MsgCode = "79" };
                    }
                    item.Caddie_No = line.Caddie_No;
                    item.CustomerFullName = line.CustomerFullName;
                    item.MobilePhone = line.MobilePhone;
                    item.Email = line.Email;
                    item.Description = line.Description;
                    item.Public_Price = line.Public_Price;
                    item.BuggyFee = line.BuggyFee;
                    item.Promotion_Price = line.Promotion_Price;
                    item.Net_Ammount = line.Net_Ammount;
                    item.Discount_Amount = line.Discount_Amount;
                    item.Discount_Value = line.Discount_Value;
                    item.Total_Amount = line.Total_Amount;
                    item.Deposit_Amount = line.Deposit_Amount;
                    item.SeagolfPrice = line.SeagolfPrice;
                    item.EstimatedPrice = line.EstimatedPrice;
                    item.Golf_Price_Description = line.Golf_Price_Description;
                    item.Golf_Promotion_Name = line.Golf_Promotion_Name;
                    item.Golf_CusGroupCode = line.Golf_CusGroupCode;
                    item.Golf_Promotion_Id = line.Golf_Promotion_Id;
                    item.Part = line.Part;
                    item.TurnLength = line.TurnLength;
                    item.BookingStatus = StatusBookingLine.bookedTeetime.ToString();
                    item.CreatedDate = DateTime.Now;
                    item.CreatedUser = model.UserId;
                    item.Is_NoShow = false;
                    bookingLines.Add(item);
                }
                booKing.BookingLines = bookingLines;

                var bookingSpecialRequest = new List<BookingSpecialRequest>();
                foreach (var cad in model.BookingSpecialRequests.Where(s => s.IsSelected))
                {
                    bookingSpecialRequest.Add(new BookingSpecialRequest
                    {
                        C_Booking_OtherType_Id = cad.C_Booking_OtherType_Id,
                        C_Org_Id = model.Courses.C_Org_Id,
                        Description = cad.Description,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedUser = model.UserId
                    });
                }
                booKing.BookingSpecialRequests = bookingSpecialRequest;

                booKing.BookingCode = GenerateBookingCode(model.Courses.Id);

                if (string.IsNullOrEmpty(booKing.BookingCode))
                {
                    return new RespondData { IsSuccess = false, Data = model, MsgCode = "47" };
                }

                var result = await _gridRepository.InsertBooking(booKing);

                if (result.Id == Guid.Empty)
                {
                    return new RespondData { IsSuccess = false, Data = model, MsgCode = "47" };
                }

                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Booking, BookingDTO>()
                    .ForMember(d => d.BookingTeetime,
                     o => o.MapFrom(s => s.BookingLines))
                    .AfterMap((s, d) => d.Golfers = d.BookingTeetime.Count())
                    .AfterMap((s, d) =>
                    {
                        for (var i = 0; i < s.BookingLines.Count; i++)
                        {
                            if (i > 0 && !string.IsNullOrEmpty(d.TotalTeetimeDisplay) && !d.TotalTeetimeDisplay.Contains(s.BookingLines[i].Tee_Time.Value.ToString("HH:mm")))
                            {
                                d.TotalTeetimeDisplay += "-" + s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                            }
                            else if (i == 0)
                            {
                                d.TotalTeetimeDisplay += s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                            }
                        }
                    })
                    .AfterMap((s, d) =>
                    {
                        foreach (var item in d.BookingSpecialRequests)
                        {
                            item.Code = model.BookingSpecialRequests.FirstOrDefault(x => x.C_Booking_OtherType_Id == item.C_Booking_OtherType_Id)?.Code;
                            item.Name = model.BookingSpecialRequests.FirstOrDefault(x => x.C_Booking_OtherType_Id == item.C_Booking_OtherType_Id)?.Name;
                            item.NameEn = model.BookingSpecialRequests.FirstOrDefault(x => x.C_Booking_OtherType_Id == item.C_Booking_OtherType_Id)?.NameEn;
                        }
                    })
                    .AfterMap((s, d) => d.CourseName = s.Course?.Name);
                    c.CreateMap<BookingLine, BookingLineDTO>();
                    c.CreateMap<BookingSpecialRequest, BookingSpecialRequestDTO>();
                });

                var mapper = config.CreateMapper();
                var dto = mapper.Map<Booking, BookingDTO>(result);

                dto.TotalEstimateFee = model.TotalEstimateFee;
                dto.NonRefundedFee = model.NonRefundedFee;
                dto.PromotionName = model.PromotionName;
                dto.SeletedPromotion = model.SeletedPromotion;
                dto.Courses = model.Courses;
                dto.IsConnectSdk = model.IsConnectSdk;
                return new RespondData { IsSuccess = true, Data = dto };
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "InssertGolfPlayerStep"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, Data = model, MsgCode = "09" };
            }
        }

        public async Task<RespondData> PriceCheckStep(BookingDTO model)
        {
            try
            {
                var booKing = _gridRepository.GetBookingById(model.Id);
                if (booKing != null)
                {
                    if (model.BookingTeetime.Any())
                    {
                        booKing.BookingLines.RemoveAll(x => !model.BookingTeetime.Select(s => s.Id).Contains(x.Id));
                    }
                }

                booKing.BookingType = BookingButtonType.PriceCheckStep.ToString();
                booKing.Status = StatusBookingLine.priced.ToString();
                booKing.NonRefundedFee = booKing.BookingLines.Sum(x => x.NonRefundedFee);
                booKing.UpdatedUser = model.UpdatedUser;
                booKing.UpdatedDate = DateTime.Now;
                var nonRefundedFee = Configuration.GetValue<int>("NonRefundedFee");
                foreach (var line in booKing.BookingLines)
                {
                    line.BookingStatus = StatusBookingLine.priced.ToString();
                    line.NonRefundedFee = nonRefundedFee;
                    booKing.TotalEstimateFee = booKing.TotalEstimateFee + line.EstimatedPrice > line.SeagolfPrice && line.SeagolfPrice > 0
                                                ? line.SeagolfPrice : line.EstimatedPrice;
                    line.UpdatedUser = model.UpdatedUser;
                    line.UpdatedDate = model.UpdatedDate;
                }
                _gridRepository.Update(booKing);

                if (!model.IsConnectSdk)
                {
                    var fakepayment = new PaymentData
                    {
                        BookingId = model.Id,
                        IsPaymentSuccess = true,
                        IsConnectSdk = model.IsConnectSdk,
                        UserId = model.UserId
                    };
                    return GolfPaymentStep(fakepayment);
                }

                return new RespondData { IsSuccess = true, Data = model };
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "PriceCheckStep"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, Data = model };
            }
        }

        public RespondData GolfPaymentStep(PaymentData model)
        {
            var booking = new Booking();
            try
            {
                if (model.IsPaymentSuccess)
                {
                    booking = _gridRepository.GolfPaymentStep(model);

                    var config = new MapperConfiguration(c =>
                    {
                        c.CreateMap<Booking, BookingDTO>()
                        .ForMember(d => d.BookingTeetime, o => o.MapFrom(s => s.BookingLines))
                        .ForMember(d => d.Courses, o => o.MapFrom(s => s.Course))
                        .AfterMap((s, d) =>
                         {
                             for (var i = 0; i < s.BookingLines.Count; i++)
                             {
                                 if (i > 0 && !string.IsNullOrEmpty(d.TotalTeetimeDisplay) && !d.TotalTeetimeDisplay.Contains(s.BookingLines[i].Tee_Time.Value.ToString("HH:mm")))
                                 {
                                     d.TotalTeetimeDisplay += "-" + s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                                 }
                                 else if (i == 0)
                                 {
                                     d.TotalTeetimeDisplay = s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                                 }
                             }
                         });
                        c.CreateMap<BookingLine, BookingLineDTO>();
                        c.CreateMap<BookingSpecialRequest, BookingSpecialRequestDTO>();
                        c.CreateMap<Course, CourseDTO>();
                        c.CreateMap<Organization, OrganizationDTO>();
                        c.CreateMap<CustomerGroup, CustomerGroupDTO>();
                        //c.CreateMap<BookingSession, BookingSessionDTO>();
                        //c.CreateMap<BookingSessionLine, BookingSessionLineDTO>();
                    });
                    var mapper = config.CreateMapper();
                    var destinations = mapper.Map<Booking, BookingDTO>(booking);

                    // get member card
                    var memberCard = _gridRepository.GetMemberCard(model.UserId);
                    if (memberCard.Any())
                    {
                        var member = memberCard.Where(x => x.MemberCardCourses.Select(s => s.C_Course_Id).Contains(booking.C_Course_Id));
                        if (member.Any())
                        {
                            destinations.CardFullName = member.FirstOrDefault().Golf_FullName;
                            destinations.GolfCardNo = member.FirstOrDefault().Golf_CardNo;
                            destinations.CardMemberType = (int)Enums.MemberCardType.member;
                            destinations.CardMemberName = Enums.MemberCardType.member.ToString();
                        }
                        else
                        {
                            destinations.CardFullName = memberCard.FirstOrDefault().Golf_FullName;
                            destinations.GolfCardNo = memberCard.FirstOrDefault().Golf_CardNo;
                            destinations.CardMemberType = (int)Enums.MemberCardType.memberBrg;
                            destinations.CardMemberName = Enums.MemberCardType.memberBrg.ToString();
                        }
                    }
                    else
                    {
                        if (booking.AppUser != null)
                        {
                            destinations.CardFullName = booking.AppUser.FullName;
                        }

                        destinations.CardMemberType = (int)Enums.MemberCardType.visitor;
                        destinations.CardMemberName = Enums.MemberCardType.visitor.ToString();
                    }
                    destinations.CountPlayers = booking.BookingLines.Count();
                    destinations.DateDisplay = booking.DateId.Value.ToString("dd/MM/yyyy") + "(" + booking.DateId.Value.DayOfWeek + ")";

                    if (booking != null && model.IsPaymentSuccess)
                    {
                        var firstTeeTime = booking.BookingLines?.OrderBy(x => x.Tee_Time)?.FirstOrDefault()?.Tee_Time;
                        if (firstTeeTime != null)
                        {
                            _gridRepository.InsertBookingNotifyInQueue(firstTeeTime.Value, model.UserId, string.Empty, booking.Course.Name, booking.Id);
                        }

                        _gridRepository.SendEmailBookingToCourse(booking, model.UserId);

                        return new RespondData { IsSuccess = true, Data = destinations, MsgCode = "38" };
                    }
                }

                return new RespondData { IsSuccess = false, Data = model, MsgCode = "37" };
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "GolfPaymentStep"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                if (booking != null)
                {
                    return new RespondData { IsSuccess = true, Data = booking, MsgCode = "38" };
                }
                else
                {
                    return new RespondData { IsSuccess = false, MsgCode = "37" };
                }
            }
        }

        private string GenerateBookingCode(Guid id)
        {
            var course = _gridRepository.GetCourseById(id);
            var courseOrder = course.CodeInt;
            var oldbooking = _gridRepository.GetAllBookingByDate(DateTime.Now);
            var code = oldbooking.Select(s => s.BookingCode);
            Random rnd = new Random();

            string numBok = "001";
            var isFinish = false;
            if (!code.Any())
            {
                var bookingCode = DateTime.Now.ToString("yMMdd") + "0" + courseOrder + numBok;
                return bookingCode;
            }
            else
            {
                do
                {
                    string num = rnd.Next(1, 999).ToString();
                    if (code.Any())
                    {
                        if (num.Length == 1)
                        {
                            numBok = "00" + num;
                        }
                        if (num.Length == 2)
                        {
                            numBok = "0" + num;
                        }
                        if (num.Length == 3)
                        {
                            numBok = num;
                        }
                        var bookingCode = DateTime.Now.ToString("yMMdd") + "0" + courseOrder + numBok;
                        if (!code.Contains(bookingCode))
                        {
                            isFinish = true;
                            return bookingCode;
                        }
                    }
                } while (!isFinish);
            }
            return null;
        }

        public object PaymentStep(BookingDTO model, string userId)
        {
            throw new NotImplementedException();
        }

        public override PagingResponseEntityDTO<BookingDTO> GetPaging(BookingFilterModel pagingModel)
        {
            var paging = _gridRepository.GetPaging(pagingModel);
            var data = AutoMapperHelper.Map<Booking, BookingDTO, List<Booking>, List<BookingDTO>>(paging.Data.ToList());

            return new PagingResponseEntityDTO<BookingDTO>
            {
                Count = paging.Data.Count(),
                Data = data
            };
        }

        public RespondData CheckMemberCard(BookingTeeTime input)
        {
            throw new NotImplementedException();
        }

        public bool ControlPaymentData(DateTime dateTrans)
        {
            const string SbYesPartNo = "0111";
            const string SbNoPartYes = "0117";
            const string IncorrectInfo = "0119";
            const string ReturnCasd = "0115";

            //var inData = _gridRepository.GetInDataTransaction(dateTrans);
            //var bookingPaymentTransaction = _gridRepository.GetbookingPaymentTrans(dateTrans);

            //foreach (var item in inData)
            //{
            //    var checkPayment = bookingPaymentTransaction.Where(x => x.Traceid == item.FT_Id).FirstOrDefault();
            //    if (checkPayment != null)
            //    {
            //        item.Rc_code = SbYesPartNo;
            //    }
            //    if (checkPayment.TotalMoney != item.TotalMoney)
            //    {
            //        item.Rc_code = IncorrectInfo;
            //    }
            //}

            //var outData = inData.Where(x => !string.IsNullOrEmpty(x.Rc_code));
            //if (outData.Any())
            //{
            //    _gridRepository.AddOutTransactionData(outData);
            //}

            return true;
        }

        public RespondData SaveSbCancelReturn(SbCancelReturn model)
        {
            try
            {
                if (!model.BookingId.HasValue)
                {
                    _log.LogError("SaveSbCancelReturn - " + model.BookingId.Value.ToString());
                    return new RespondData { IsSuccess = false };
                }
                var payData = _gridRepository.SaveSbCancelReturn(model.BookingId.Value, model.Code, model.Description, model.TransactionNo);

                if (payData != null)
                {
                    return new RespondData { IsSuccess = true };
                }
                else
                {
                    using (LogContext.PushProperty("MethodName", "SaveSbCancelReturn"))
                    {
                        _log.LogError(model.BookingId.Value.ToString() + "- không tìm thấy thông tin thanh toán");
                    }
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "SaveSbCancelReturn"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(model.BookingId.ToString());
                    _log.LogError(e.StackTrace);
                }
            }

            return new RespondData { IsSuccess = false };
        }

        public RespondData GetBookingByCode(string code, string curOrgId, DateTime? bookedDate)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new RespondData { IsSuccess = false, MsgCode = "59", Message = "Nhập mã đặt vé" };
            }
            if (string.IsNullOrEmpty(curOrgId))
            {
                return new RespondData { IsSuccess = false, MsgCode = "60", Message = "Không tìm thấy đơn vị quản lý sân" };
            }

            Booking data = _gridRepository.GetBookingByCode(code, curOrgId, bookedDate);

            if (data != null)
            {
                if (curOrgId != data.C_Org_Id.ToString())
                {
                    var org = _gridRepository.GetOrganizationById(data.C_Org_Id);
                    return new RespondData
                    {
                        IsSuccess = false,
                        Message = string.Format("Booking Code {0} được đặt tại sân {1}", data.BookingCode, org.Name)
                    };
                }

                if (bookedDate.HasValue)
                {
                    if (bookedDate.Value.Date != data.DateId.Value.Date)
                    {
                        return new RespondData
                        {
                            IsSuccess = false,
                            Message = string.Format("Booking Code {0} được đặt chỗ ngày {1}", data.BookingCode, data.DateId.Value.ToString("dd/MM/yyyy"))
                        };
                    }
                }

                return new RespondData { IsSuccess = true, Data = data };
            }
            else
            {
                return new RespondData { IsSuccess = false, MsgCode = "58", Message = "Không lấy được thông tin đặt vé" };
            }
        }

        public RespondData SaveTimePlayerOnBoard(GolfBagDto model)
        {
            try
            {
                BookingLine bookingLine = _gridRepository.GetBookingLineById(model.BookingLineId);
                bookingLine.GolfBag = model.GolfBag;
                if (!string.IsNullOrEmpty(model.Cashier))
                {
                    bookingLine.CashierName = model.Cashier;
                }
                if (!string.IsNullOrEmpty(model.Description))
                {
                    bookingLine.GolfDescription = model.Description;
                }
                if (model.TimeOnCoursePlayer.HasValue && model.TimeOnCoursePlayer != null)
                {
                    bookingLine.TimeOnCoursePlayer = model.TimeOnCoursePlayer;
                    bookingLine.Is_NoShow = true;
                }
                else
                {
                    bookingLine.TimeOnCoursePlayer = null;
                    bookingLine.Is_NoShow = false;
                }
                bookingLine.UpdatedDate = DateTime.Now;
                bookingLine.UpdatedUser = model.UserId;

                BookingLine updateLine = _gridRepository.UpdateBookingLine(bookingLine);

                return new RespondData { IsSuccess = true, Data = updateLine };
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "SaveTimePlayerOnBoard"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(model.BookingLineId.ToString());
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false };
            }
        }

        public RespondData CancelGolfBag(GolfBagDto model)
        {
            throw new NotImplementedException();
        }

        public RespondData CancelTempBookingTeetime(Guid id)
        {
            RespondData result = _gridRepository.CancelTempBookingTeetime(id);
            return result;
        }

        public RespondData RefreshBookingCode(PaymentData model)
        {
            try
            {
                var book = _gridRepository.GetBookingById(model.BookingId.Value);
                var bookingCode = GenerateBookingCode(book.Course.Id);
                var bookData = _gridRepository.RefreshBookingCode(model, bookingCode);
                // sửa ở đây
                var config = new MapperConfiguration(c =>
                {
                    c.CreateMap<Booking, BookingDTO>()
                    .ForMember(d => d.BookingTeetime, o => o.MapFrom(s => s.BookingLines.OrderBy(o => o.CreatedDate)))
                    .AfterMap((s, d) => d.Golfers = d.BookingTeetime.Count())
                    .AfterMap((s, d) =>
                    {
                        for (var i = 0; i < s.BookingLines.Count; i++)
                        {
                            if (i > 0 && !string.IsNullOrEmpty(d.TotalTeetimeDisplay) && !d.TotalTeetimeDisplay.Contains(s.BookingLines[i].Tee_Time.Value.ToString("HH:mm")))
                            {
                                d.TotalTeetimeDisplay += "-" + s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                            }
                            else if (i == 0)
                            {
                                d.TotalTeetimeDisplay = s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                            }
                        }
                    })
                    .AfterMap((s, d) => d.CardEmail = s.AppUser?.Email)
                    .AfterMap((s, d) => d.CardMobilePhone = s.AppUser?.PhoneNumber)
                    .AfterMap((s, d) => d.CardFullName = s.AppUser?.FullName)
                    .AfterMap((s, d) =>
                    {
                        d.Courses = new CourseDTO
                        {
                            Id = s.Course.Id,
                            C_Org_Id = s.Course.Id,
                            Code = s.Course.Code,
                            OrgCode = s.Organization.Code,
                            Name = s.Course.Name,
                            OrganizationName = s.Organization.Name,
                            ImageUrl = s.Course.ImageUrl,
                            Full_Image_Url = (backendUrl + s.Course.ImageUrl).Replace("\\", "//"),
                            Full_IconUrl = (backendUrl + s.Course.IconUrl).Replace("\\", "//")
                        };
                    })
                    .AfterMap((s, d) => d.CourseName = s.Course?.Name);
                    c.CreateMap<BookingLine, BookingLineDTO>();
                    c.CreateMap<Organization, OrganizationDTO>();
                    c.CreateMap<M_Promotion, PromotionDTO>();
                    c.CreateMap<Course, CourseDTO>();
                    c.CreateMap<CustomerGroup, CustomerGroupDTO>();
                    c.CreateMap<BookingSpecialRequest, BookingSpecialRequestDTO>();
                    c.CreateMap<BookingSession, BookingSessionDTO>();
                });
                var mapper = config.CreateMapper();
                var dto = mapper.Map<Booking, BookingDTO>(bookData);
                if (dto != null)
                {
                    using (LogContext.PushProperty("MethodName", "RefreshBookingCode"))
                    {
                        _log.LogInformation("RefreshBookingCode-return: " + " BookingCode: " + dto.BookingCode + " CreatedDate: "
                            + dto.UpdatedDate + " BookingId:" + dto.Id);
                    }
                    return new RespondData { IsSuccess = true, Data = dto };
                }
                else
                {
                    return new RespondData { IsSuccess = false, MsgCode = "65" };
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "RefreshBookingCode"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false };
            }
        }

        public RespondData CheckRuleCancelBooking(Guid bookingId, string userId, bool v)
        {
            var booking = _gridRepository.GetBookingById(bookingId);
            if (booking != null)
            {
                if (!booking.BookingLines.Any(a => a.Tee_Time > DateTime.Now))
                {
                    return new RespondData { IsSuccess = false, MsgCode = "78" };
                }
                var isReturn = _gridRepository.CheckReturnMoneyRuleWhenCancel(booking);
                return new RespondData { IsSuccess = true, Data = isReturn };
            }
            return new RespondData { IsSuccess = false, MsgCode = "45" };
        }

        public void LockAccountCustomerDueNoShow()
        {
            var bookings = _gridRepository.GetAllBookingNoShow();
            var groupBook = bookings.GroupBy(o => o.UserId);
            foreach (var item in groupBook)
            {
                var listBook = item.Take(3);
                if (listBook.Count() == 3 && !listBook.Any(x => x.BookingLines.Count(x => x.Is_NoShow) > 0))
                {
                    _gridRepository.LockAccountCustomerDueNoShow(item.Key);
                    using (LogContext.PushProperty("MethodName", "LockAccountCustomerDueNoShow"))
                    {
                        _log.LogInformation("LockAccount: " + item.FirstOrDefault()?.AppUser.Name + "-Date:" + DateTime.Now);
                    }
                }
                else
                {
                    _gridRepository.ResetCountNoMoreShow(item.Key);
                }
            }
        }

        public bool CheckNumberFlightHoliday(DateTime dateId, Guid c_Org_Id, int lines)
        {
            var isHoliday = _holidayRepository.CheckDateHoliday(c_Org_Id, dateId);
            var weekend = dateId.DayOfWeek == DayOfWeek.Sunday || dateId.DayOfWeek == DayOfWeek.Saturday;
            var flightWeekend = _settingRepository.GetSetting("flightWeekend");
            if (isHoliday || weekend)
            {
                if (!string.IsNullOrEmpty(flightWeekend) && lines < Convert.ToInt32(flightWeekend))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
