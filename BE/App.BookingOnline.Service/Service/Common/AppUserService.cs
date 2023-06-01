using App.BookingOnline.Data;
using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.MailService;
using App.BookingOnline.Data.Migrations;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Booking;
using App.BookingOnline.Service.DTO.Common;
using App.Core;
using App.Core.Configs;
using App.Core.Domain;
using App.Core.Helper;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static App.Core.Enums;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace App.BookingOnline.Service.Service.Common
{
    public class AppUserService : BaseGridDataService<CustomerDTO, Customer, UserPagingModel, IAppUserRepository>
        , IAppUserService
    {
        private readonly IMailService _mailService;
        private readonly ISequenceRepository seqRepo;
        private readonly ILogger _log;
        public IConfiguration Configuration { get; }
        private readonly IBookingRepository _bookingRepository;
        private string backendMobileUrl;

        public AppUserService(IAppUserRepository gridRepository, IBookingRepository bookingRepository,
            ISequenceRepository seqRepo, IMailService mailService, IConfiguration configuration,
            ILogger<AppUserService> logger) : base(gridRepository)
        {
            _mailService = mailService;
            this.seqRepo = seqRepo;
            _log = logger;
            Configuration = configuration;
            _bookingRepository = bookingRepository;
            backendMobileUrl = Configuration.GetSection("urlData").GetValue<string>("SwaggerUrl");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                backendMobileUrl = Configuration.GetSection("urlData").GetValue<string>("SwaggerUrlPro");
            }
        }

        public async Task<RespondData> ChangePassword(string email, string password, string oldPassword)
        {
            return await _gridRepository.ChangePassword(email, password, oldPassword);
        }

        public RespondData CheckCreateAccount(CustomerDTO model)
        {
            var data = _gridRepository.CheckCreateAccount(model.Email, model.MobilePhone);
            return new RespondData { IsSuccess = data.IsSuccess, Data = model, MsgCode = data.MsgCode };
        }

        public async Task<RespondData> CreateAccount(CustomerDTO model)
        {
            try
            {
                var folderName = string.Empty;
                var data = string.Empty;
                var pathToSave = string.Empty;
                var customerGroup = _bookingRepository.GetAllCustomerGroup();
                var memberCards = new List<MemberCard>();

                if (Configuration.GetValue<bool>("AllowNonMemberAccount"))
                {
                    foreach (var ite in model.MemberCards)
                    {
                        if (memberCards.Select(s => s.Golf_CardNo).Contains(ite.Golf_CardNo)
                            && memberCards.Select(s => s.C_Org_Id).Contains(ite.C_Org_Id))
                        {
                            return new RespondData { IsSuccess = false, MsgCode = "54" };
                        }

                        var config = new MapperConfiguration(c =>
                        {
                            c.CreateMap<MemberCardDTO, MemberCard>();
                            c.CreateMap<OrganizationDTO, Organization>();
                            c.CreateMap<CourseDTO, Course>();
                        });
                        var mapper = config.CreateMapper();
                        var listMemberCard = mapper.Map<MemberCardDTO, MemberCard>(ite);
                        //var listMemberCard = AutoMapperHelper.Map<MemberCardDTO, MemberCard>(ite);
                        listMemberCard.IsActive = true;
                        var memberCardCourses = new List<MemberCardCourse>();
                        listMemberCard.IsActive = true;
                        if (ite.CoursesMemberCard != null)
                        {
                            foreach (var cou in ite.CoursesMemberCard)
                            {
                                var membercourse = new MemberCardCourse();
                                membercourse.GolfCode = cou.Golf_CusGroupCode;
                                membercourse.GolfName = cou.Golf_CusGroupName;
                                membercourse.C_Course_Id = cou.C_Course_Id;
                                membercourse.ValidFrom = cou.ValidFrom;
                                membercourse.ValidTo = cou.ValidTo;
                                membercourse.MB_CustomerGroup_Id = customerGroup.FirstOrDefault(x => x.Code == cou.BK_CusGroupCode
                                                                                                && ite.C_Org_Id == x.C_Org_Id)?.Id;
                                memberCardCourses.Add(membercourse);
                            }
                            listMemberCard.MemberCardCourses = memberCardCourses;
                        }
                        memberCards.Add(listMemberCard);
                    }
                }
                else
                {
                    if (model.MemberCards != null && model.MemberCards.Any())
                    {
                        foreach (var ite in model.MemberCards)
                        {
                            if (memberCards.Select(s => s.Golf_CardNo).Contains(ite.Golf_CardNo)
                                && memberCards.Select(s => s.C_Org_Id).Contains(ite.C_Org_Id))
                            {
                                return new RespondData { IsSuccess = false, MsgCode = "54" };
                            }

                            var config = new MapperConfiguration(c =>
                            {
                                c.CreateMap<MemberCardDTO, MemberCard>();
                                c.CreateMap<OrganizationDTO, Organization>();
                                c.CreateMap<CourseDTO, Course>();
                            });
                            var mapper = config.CreateMapper();
                            var listMemberCard = mapper.Map<MemberCardDTO, MemberCard>(ite);
                            listMemberCard.IsActive = true;
                            var memberCardCourses = new List<MemberCardCourse>();
                            listMemberCard.IsActive = true;
                            if (ite.CoursesMemberCard != null)
                            {
                                foreach (var cou in ite.CoursesMemberCard)
                                {
                                    var membercourse = new MemberCardCourse();
                                    membercourse.GolfCode = cou.Golf_CusGroupCode;
                                    membercourse.GolfName = cou.Golf_CusGroupName;
                                    membercourse.C_Course_Id = cou.C_Course_Id;
                                    membercourse.ValidFrom = cou.ValidFrom;
                                    membercourse.ValidTo = cou.ValidTo;
                                    membercourse.MB_CustomerGroup_Id = customerGroup.FirstOrDefault(x => x.Code == cou.BK_CusGroupCode
                                                                                                    && ite.C_Org_Id == x.C_Org_Id)?.Id;

                                    memberCardCourses.Add(membercourse);
                                }
                                listMemberCard.MemberCardCourses = memberCardCourses;
                            }
                            memberCards.Add(listMemberCard);
                        }
                    }
                    else
                    {
                        return new RespondData { IsSuccess = false, MsgCode = "74" };
                    }
                }

                model.MemberCards = null;
                var entity = AutoMapperHelper.Map<CustomerDTO, Customer>(model);
                entity.IsUpdateErrCode = true;
                entity.CustomerCode = seqRepo.GetCode("CUSTOMERCODE", SequenceModes.Year);

                if (!string.IsNullOrEmpty(model.ImageData))
                {
                    var imageData = model.ImageData.Split(';');
                    var nameExtenstion = imageData[0].Split('/')[1];
                    var fileName = "\\" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "." + nameExtenstion;
                    string fileUrl = Configuration.GetSection("fileUpload").GetValue<string>("fileUrl");
                    if (!Directory.Exists(fileUrl))
                    {
                        Directory.CreateDirectory(fileUrl);
                    }
                    pathToSave = fileUrl + fileName;
                    entity.Img_Url = fileName;
                    data = imageData[1];
                }

                var result = await _gridRepository.CreateAccount(entity, model.Password, memberCards);

                if (result != null && (data != null && !string.IsNullOrEmpty(data))
                    && !string.IsNullOrEmpty(model.ImageData))
                {
                    var newBytes = Convert.FromBase64String(data.Split(',')[1]);
                    File.WriteAllBytes(pathToSave, newBytes);
                }
                return result;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, MsgCode = "09" };
            }
        }

        public RespondData GetAccountInfo(string userId)
        {
            var customer = new CustomerDTO();
            var cus = _gridRepository.GetCustomerByUserId(userId);
            if (cus == null)
            {
                return new RespondData { IsSuccess = false, MsgCode = "10" };
            }
            customer = AutoMapperHelper.Map<Customer, CustomerDTO>(cus);
            customer.Full_Image_Url = backendMobileUrl + "\\" + AppConfigs.UPLOAD_PATH + customer.Img_Url;
            var memberCards = _gridRepository.GetMemberCardByCustId(customer.Id);
            if (memberCards.Any())
            {
                customer.MemberCards = new List<MemberCardDTO>();
                foreach (var mem in memberCards)
                {
                    var config = new MapperConfiguration(c =>
                    {
                        c.CreateMap<MemberCard, MemberCardDTO>();
                        c.CreateMap<Organization, OrganizationDTO>();
                        c.CreateMap<Course, CourseDTO>();
                    });
                    var mapper = config.CreateMapper();
                    var memberCardDto = mapper.Map<MemberCard, MemberCardDTO>(mem);

                    //var memberCardDto = AutoMapperHelper.Map<MemberCard, MemberCardDTO>(mem);
                    memberCardDto.OrgCode = mem.Organization?.Code;
                    memberCardDto.OrgName = mem.Organization?.Name;

                    if (mem.MemberCardCourses.Any())
                    {
                        memberCardDto.CoursesMemberCard = new List<MemberCardCourseDTO>();
                        foreach (var cardCourse in mem.MemberCardCourses)
                        {
                            var memberCourse = AutoMapperHelper.Map<MemberCardCourse, MemberCardCourseDTO>(cardCourse);
                            memberCourse.CourseName = _gridRepository.GetCourseById(memberCourse.C_Course_Id.Value)?.Name;
                            memberCardDto.CoursesMemberCard.Add(memberCourse);
                        }
                    }
                    customer.MemberCards.Add(memberCardDto);
                }
            }

            return new RespondData { IsSuccess = true, Data = customer };
        }
        public async Task<RespondData> UpdateAccount(CustomerDTO model)
        {
            try
            {
                var fileName = string.Empty;
                var data = string.Empty;
                var pathToSave = string.Empty;
                if (!string.IsNullOrEmpty(model.ImageData))
                {
                    var imageData = model.ImageData.Split(';');
                    var nameExtenstion = imageData[0].Split('/')[1];
                    data = imageData[1];
                    if (string.IsNullOrEmpty(model.Img_Url))
                    {
                        fileName = "\\" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "." + nameExtenstion;
                        string fileUrl = Configuration.GetSection("fileUpload").GetValue<string>("fileUrl").ToString();
                        if (!Directory.Exists(fileUrl))
                        {
                            Directory.CreateDirectory(fileUrl);
                        }
                        pathToSave = fileUrl + fileName;

                    }
                    else
                    {
                        fileName = model.Img_Url;
                    }
                }
                else
                {
                    fileName = model.Img_Url;
                }

                var memberCards = new List<MemberCard>();

                foreach (var ite in model.MemberCards)
                {
                    if (memberCards.Select(s => s.Golf_CardNo).Contains(ite.Golf_CardNo)
                            && memberCards.Select(s => s.C_Org_Id).Contains(ite.C_Org_Id))
                    {
                        return new RespondData { IsSuccess = false, MsgCode = "54" };
                    }
                    var config = new MapperConfiguration(c =>
                    {
                        c.CreateMap<MemberCardDTO, MemberCard>();
                        c.CreateMap<OrganizationDTO, Organization>();
                        c.CreateMap<CourseDTO, Course>();
                    });
                    var mapper = config.CreateMapper();
                    var listMemberCard = mapper.Map<MemberCardDTO, MemberCard>(ite);
                    //var listMemberCard = AutoMapperHelper.Map<MemberCardDTO, MemberCard>(ite);
                    listMemberCard.IsActive = true;
                    listMemberCard.MB_Customer_Id = model.Id;
                    var memberCardCourses = new List<MemberCardCourse>();

                    foreach (var cou in ite.CoursesMemberCard)
                    {
                        var membercourse = new MemberCardCourse();
                        membercourse.Id = cou.Id;
                        membercourse.GolfCode = cou.Golf_CusGroupCode;
                        membercourse.GolfName = cou.Golf_CusGroupName;
                        membercourse.C_Course_Id = cou.C_Course_Id;
                        membercourse.ValidFrom = cou.ValidFrom;
                        membercourse.ValidTo = cou.ValidTo;
                        membercourse.MB_CustomerGroup_Id = _bookingRepository.GetCustomerGroupByCode(cou.BK_CusGroupCode)?.Id;

                        memberCardCourses.Add(membercourse);
                    }
                    listMemberCard.MemberCardCourses = memberCardCourses;

                    memberCards.Add(listMemberCard);
                }

                model.MemberCards = null;
                var entity = AutoMapperHelper.Map<CustomerDTO, Customer>(model);

                entity.Img_Url = fileName;
                var result = await _gridRepository.UpdateAccount(entity, memberCards);

                if (!string.IsNullOrEmpty(model.ImageData) && (data != null && !string.IsNullOrEmpty(data)))
                {
                    var newBytes = Convert.FromBase64String(data.Split(',')[1]);
                    File.WriteAllBytes(pathToSave, newBytes);
                }
                return result;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, MsgCode = "09" };
            }
        }

        public async Task<RespondData> ForgotPasswordAsync(string email)
        {
            var result = await _gridRepository.ForgotPasswordAsync(email);
            return result;
        }

        public async Task<string> GetByPhoneAsync(string phoneNumber)
        {
            var result = await _gridRepository.GetByPhoneForLoginAsync(phoneNumber);
            return result;
        }

        public async Task<RespondData> RegisterMemberCard(MemberRequestDTO model)
        {
            var entity = AutoMapperHelper.Map<MemberRequestDTO, MemberRequest>(model);
            entity.Request_Date = DateTime.Now;
            entity.Request_Status = MemberCardRequestStatus.open.ToString();
            return await _gridRepository.RegisterMemberCard(entity);
        }

        public RespondData GetInfoMemberCard(string userId)
        {
            return _gridRepository.GetInfoMemberCard(userId);
        }

        public RespondData ListMemberCard(string userId)
        {
            return _gridRepository.ListMemberCard(userId);
        }

        public RespondData GetAllOrg()
        {
            var orgInfo = new List<DTO.OrganizationInfoDTO>();
            var org = _gridRepository.GetAllOrg();
            foreach (var item in org)
            {
                var info = new DTO.OrganizationInfoDTO
                {
                    C_Org_Id = item.Id,
                    OrgName = item.Name,
                    InvoiceAddress = item.OrganizationInfos?.FirstOrDefault()?.InvoiceAddress,
                    Email = item.OrganizationInfos?.FirstOrDefault()?.Email,
                    InvoiceBankAccount = item.OrganizationInfos?.FirstOrDefault()?.InvoiceBankAccount,
                    InvoiceName = item.OrganizationInfos?.FirstOrDefault()?.InvoiceName,
                    ShortAddress = item.OrganizationInfos?.FirstOrDefault()?.ShortAddress,
                    Telephone = item.OrganizationInfos?.FirstOrDefault()?.Telephone,
                    TaxCode = item.OrganizationInfos?.FirstOrDefault()?.TaxCode,
                    Fax = item.OrganizationInfos?.FirstOrDefault()?.Fax,
                    InvoiceBankName = item.OrganizationInfos?.FirstOrDefault()?.InvoiceBankName
                };
                orgInfo.Add(info);
            }

            return new RespondData { IsSuccess = true, Data = orgInfo };
        }

        public async Task<RespondData> GetOtpCode(string userId, string lang, string mobilePhone, string email)
        {
            try
            {
                var checkNumer = _gridRepository.CheckNumberOtpCode(mobilePhone, email);
                if (checkNumer.Any())
                {
                    var listOtp = checkNumer.Where(x => x.SendDate.Date == DateTime.Now.Date);

                    if (listOtp.Count() > 10)
                    {
                        return new RespondData { IsSuccess = false, MsgCode = "51" };
                    }
                    if (listOtp.Count() > 0 && listOtp.Count() < 10)
                    {
                        var lastSend = checkNumer.First();
                        if ((DateTime.Now - lastSend.SendDate).TotalMinutes <= 3)
                        {
                            return new RespondData { IsSuccess = false, MsgCode = "50" };
                        }
                    }
                }

                var chars = "0123456789";
                var code = new char[6];
                var random = new Random();
                for (int i = 0; i < code.Length; i++)
                {
                    code[i] = chars[random.Next(chars.Length)];
                }

                //var codeOtp = new String("123456"); // remove khi có sms service
                var codeOtp = string.Join("", code);
                var user = _gridRepository.GetUserByEmailOrPhone(mobilePhone, email);
                var data = new SmsHistory();
                if (user != null)
                {
                    if (string.IsNullOrEmpty(email))
                    {
                        email = user.Email;
                    }
                    data = await _gridRepository.SaveOtp(user.Id, codeOtp, mobilePhone);
                }
                else
                {
                    data = await _gridRepository.SaveOtp(userId, codeOtp, mobilePhone);
                }

                if (data != null)
                {
                    var smsContent = Configuration.GetValue<string>("OtpSms");
                    smsContent = string.Format(smsContent, data.Code, data.TimeLife);
                    if (lang == Constants.LangEn)
                    {
                        smsContent = Configuration.GetValue<string>("OtpSmsEn");
                        smsContent = string.Format(smsContent, data.Code, data.TimeLife);
                    }
                    //TODO: goi sms service cua seabank

                    //Gui OTP vao email   
                    var subject = "Booking online OTP";
                    if (!string.IsNullOrEmpty(email))
                    {
                        await _mailService.SendMailAsync(email, "", "", subject, smsContent, String.Empty);
                    }

                    data.Code = string.Empty;
                    return new RespondData { IsSuccess = true, Data = data };
                }
                return new RespondData { IsSuccess = false, MsgCode = "19" };
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, MsgCode = "19" };
            }
        }

        public RespondData CheckOtpCode(SmsHistoryDTO model)
        {
            var entity = AutoMapperHelper.Map<SmsHistoryDTO, SmsHistory>(model);
            var result = _gridRepository.CheckOtpCode(entity);
            if (result != null)
            {
                var timeline = result.SendDate.AddMinutes(result.TimeLife);
                if (DateTime.Now > timeline)
                {
                    return new RespondData { IsSuccess = false, MsgCode = "21" };
                }
                return new RespondData { IsSuccess = true };
            }
            else
            {
                return new RespondData { IsSuccess = false, MsgCode = "20" };
            }
        }

        public RespondData GetuploadFolder()
        {
            string fileUrl = Configuration.GetSection("fileUpload").GetValue<string>("fileUrl");
            return new RespondData { IsSuccess = true, Data = fileUrl };
        }

        public RespondData CheckMemberCard(string cardNumber, string fullName)
        {
            try
            {
                var listCard = new List<MemberCardMobile>();
                var memberCard = new MemberCardMobile
                {
                    Golf_MemberCardId = "1235-9864-5236",
                    Golf_IsActive = true,
                    DateValid = "01/01/2022-31/12/2022",
                    GolfCourse = "Lengend Hill Golf Resort"
                };
                listCard.Add(memberCard);
                return new RespondData { IsSuccess = true, Data = listCard };

            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, MsgCode = "22" };
            }
        }

        public RespondData CheckGoflBrgCard(string cardNumber, string fullname)
        {
            string baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApi");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApiPro");
            }
            string checkmemberUrl = Configuration.GetSection("urlData").GetValue<string>("Checkmember");

            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(checkmemberUrl, Method.POST);
            request.AddHeader("Accept", "*/*");
            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            request.AddParameter("CardNo", cardNumber, ParameterType.GetOrPost);
            request.AddParameter("CustomerFullName", fullname, ParameterType.GetOrPost);
            try
            {
                var memberCard = new List<MemberCardDTO>();

                IRestResponse<GolfMemberRespone> response = client.Execute<GolfMemberRespone>(request);
                if (response.IsSuccessful)
                {
                    var cardData = JsonSerializer.Deserialize<GolfMemberRespone>(response.Content);

                    foreach (var item in cardData.Data)
                    {
                        var card = AutoMapperHelper.Map<BookOnlineMemberCard, MemberCardDTO>(item);

                        bool checkMemberCard = _gridRepository.CheckMemberCardByUser(card.Golf_CardNo, card.OrgCode);
                        if (checkMemberCard)
                        {
                            return new RespondData { IsSuccess = false, MsgCode = "64" };
                        }
                        card.CoursesMemberCard = AutoMapperHelper.Map<BookOnlineMemberCardCourse, MemberCardCourseDTO,
                            List<BookOnlineMemberCardCourse>, List<MemberCardCourseDTO>>
                            (item.MemberCardCourses.Where(x => x.BK_CusGroupCode == ((int)MemberCardType.member).ToString()).ToList());

                        memberCard.Add(card);
                    }

                    if (memberCard.Any())
                    {
                        _log.LogInformation("CheckGoflBrgCard - data" + JsonConvert.SerializeObject(memberCard, Formatting.Indented));
                        return new RespondData { IsSuccess = true, Data = memberCard };
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(cardData.ErrorCode))
                        {
                            return new RespondData { IsSuccess = false, MsgCode = cardData.ErrorCode };
                        }
                        else
                        {
                            return new RespondData { IsSuccess = false, MsgCode = "31" };
                        }
                    }
                }
                else
                {
                    _log.LogError(Constants.GolfApiError + "CheckGoflBrgCard" + response.ErrorMessage);
                    return new RespondData { IsSuccess = false, MsgCode = "31" };
                }
            }
            catch (Exception e)
            {
                _log.LogError(e.StackTrace);
                _log.LogError(Constants.GolfApiError + "CheckGoflBrgCard" + e.Message);
                return new RespondData { IsSuccess = false, MsgCode = "31" };
            }
        }

        public RespondData SearchGoflBrgCard(string cardNumber, string fullname, string orgName)
        {
            string baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApi");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApiPro");
            }
            string checkmemberUrl = Configuration.GetSection("urlData").GetValue<string>("Checkmember");

            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(checkmemberUrl, Method.POST);
            request.AddHeader("Accept", "*/*");
            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            request.AddParameter("CardNo", cardNumber, ParameterType.GetOrPost);
            request.AddParameter("CustomerFullName", fullname, ParameterType.GetOrPost);
            try
            {
                var memberCard = new List<MemberCardDTO>();

                IRestResponse<GolfMemberRespone> response = client.Execute<GolfMemberRespone>(request);
                if (response.IsSuccessful)
                {
                    var weatherForecast = JsonSerializer.Deserialize<GolfMemberRespone>(response.Content);
                    if (!weatherForecast.IsSuccess)
                    {
                        return new RespondData { IsSuccess = false, Message = weatherForecast.InfoMessage };
                    }
                    foreach (var item in weatherForecast.Data)
                    {
                        var card = AutoMapperHelper.Map<BookOnlineMemberCard, MemberCardDTO>(item);
                        bool checkMemberCard = _gridRepository.CheckMemberCardByUser(card.Golf_CardNo, card.OrgCode);
                        if (checkMemberCard && weatherForecast.Data.Count() == 1)
                        {
                            return new RespondData { IsSuccess = false, MsgCode = "64", Message = "Thẻ đã gán cho người dùng khác" };
                        }

                        card.CoursesMemberCard = AutoMapperHelper.Map<BookOnlineMemberCardCourse, MemberCardCourseDTO,
                            List<BookOnlineMemberCardCourse>, List<MemberCardCourseDTO>>(item.MemberCardCourses);
                        if (!checkMemberCard)
                        {
                            memberCard.Add(card);
                        }
                    }
                    if (!string.IsNullOrEmpty(orgName))
                    {
                        memberCard = memberCard.Where(x => x.OrgCode == orgName).ToList();
                    }
                    return new RespondData { IsSuccess = true, Data = memberCard };
                }
                else
                {
                    return new RespondData { IsSuccess = false, Message = "Không tìm thấy thẻ" };
                }
            }
            catch (Exception e)
            {
                _log.LogError(Constants.GolfApiError + "-AddGoflBrgCard-" + e.Message);
                _log.LogError(e.StackTrace);
                return new RespondData { IsSuccess = false, Message = "Không tìm thấy thẻ" };
            }
        }
    }
}
