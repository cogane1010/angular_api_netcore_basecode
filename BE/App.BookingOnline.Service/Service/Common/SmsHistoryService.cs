using App.BookingOnline.Data;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.MailService;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO.Booking;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.BookingOnline.Service.IService.Common;
using App.Core;
using App.Core.Domain;
using App.Core.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Text.Json;
using System.Globalization;
using static App.Core.Enums;

namespace App.BookingOnline.Service.Service.Common
{
    public class SmsHistoryService :
        BaseGridDataService<SmsHistoryDTO, SmsHistory, SmsHistoryFilterModel, ISmsHistoryRepository>,
        ISmsHistoryService
    {
        private readonly ILogger _log;
        private readonly IAppUserRepository appUserRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMailService _mailService;

        public IConfiguration Configuration { get; }

        public SmsHistoryService(ISmsHistoryRepository gridRepository, IAppUserRepository appUserRepository, IConfiguration configuration
          , INotificationRepository notificationRepository, ILogger<SmsHistoryService> logger
          , IBookingRepository bookingRepository, IMailService mailService) : base(gridRepository)
        {
            this.appUserRepository = appUserRepository;
            _notificationRepository = notificationRepository;
            _bookingRepository = bookingRepository;
            _mailService = mailService;
            Configuration = configuration;
            _log = logger;
        }

        public override PagingResponseEntityDTO<SmsHistoryDTO> GetPaging(SmsHistoryFilterModel pagingModel)
        {
            var paging = _gridRepository.GetPaging(pagingModel);
            var data = AutoMapperHelper.Map<SmsHistory, SmsHistoryDTO, List<SmsHistory>, List<SmsHistoryDTO>>(paging.Data.ToList());

            return new PagingResponseEntityDTO<SmsHistoryDTO>
            {
                Count = paging.Data.Count(),
                Data = data
            };
        }

        public bool PushNotificationToDevice(NotificationQueue model)
        {
            string baseUrl = "https://fcm.googleapis.com/";
            try
            {
                string promotionImageUrl = Configuration.GetSection("fileUpload").GetValue<string>("BackendUrlPro") + "/";
                var serverKey = Configuration.GetValue<string>("FirebaseServerKey");
                var stringKey = "key=" + serverKey;

                IRestClient client = new RestClient(baseUrl);
                IRestRequest request = new RestRequest("fcm/send", Method.POST);
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Authorization", stringKey);
                request.AddHeader("Content-Type", "application/json");

                var paramsRequest = new Dictionary<string, object>();
                var newPersons = new Notification()
                {
                    title = model.Title,
                    body = model.Content,
                    image = promotionImageUrl + model.Img_url,
                    id = model.Id.ToString()
                };
                if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.SendTo))
                {
                    return false;
                }
                paramsRequest.Add("to", model.SendTo);
                paramsRequest.Add("notification", newPersons);
                request.AddJsonBody(paramsRequest);

                IRestResponse<RespondFirebase> response = client.Execute<RespondFirebase>(request);
                if (response.IsSuccessful)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "PushNotificationToDevice"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return false;
            }
        }

        public bool PushNotificationForAllDevice(NotificationDTO model)
        {
            try
            {
                var users = appUserRepository.GetAllCustomer().Where(x => !string.IsNullOrEmpty(x.FcmTokenDevice));
                if (users.Any())
                {
                    // insert in table notificationqueue
                    var data = new List<NotificationQueue>();
                    foreach (var item in users)
                    {

                        var notifi = new NotificationQueue
                        {
                            SendTo = item.FcmTokenDevice,
                            UserId = new Guid(item.UserId),
                            BookingId = model.Id,
                            Title = model.Message_Title,
                            Content = model.Message_Content,
                            TitleEn = model.Message_TitleEn,
                            ContentEn = model.Message_ContentEn,
                            IsActive = true,
                            IsSend = false,
                            Img_url = model.Img_Url,
                            NotificationType = Enums.FcmNotifiType.notifiAll.ToString(),
                            SendDate = DateTime.Now,
                            CreatedDate = DateTime.Now,
                            CreatedUser = model.CreatedUser
                        };
                        //if (item.Languague == Constants.LangEn)
                        //{
                        //    notifi.Title = !string.IsNullOrEmpty(model.Message_TitleEn)
                        //        ? model.Message_TitleEn : model.Message_Title;
                        //    notifi.Content = !string.IsNullOrEmpty(model.Message_ContentEn)
                        //        ? model.Message_ContentEn : model.Message_Content;
                        //}
                        if (!string.IsNullOrEmpty(notifi.SendTo) && !string.IsNullOrEmpty(notifi.Title))
                        {
                            //if ("cyydj5xVS2yu6o4eRyrjgl:APA91bGy5fXYQNj0akCKD1CVRhfmARn_SwohbG9cj_eASYw6jsDbMn3bLevB4EmHokyqKEKkE3JTWZR5Z6EdhkShHz-xDJ4uRRyKkp8F5NsCD4LcrgbjUBytbU_jUGHcZaD_sKgOTqL_" == item.FcmTokenDevice)
                            //{

                            //}
                            data.Add(notifi);
                        }
                    }
                    if (data.Any())
                    {
                        _notificationRepository.InsertNotificationQueue(data);
                    }
                }

                var noti = AutoMapperHelper.Map<NotificationDTO, GF_Notification>(model);
                noti.Status = 1;
                noti.Sent_Date = DateTime.Now;
                noti.Sent_User = model.Sent_User;
                _notificationRepository.Update(noti);
                return true;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "PushNotificationForAllDevice"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return false;
            }
        }

        public bool SendEmailBookingCourse(Guid? bookingId)
        {
            try
            {
                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                var booking = _bookingRepository.GetBookingById(bookingId.Value);
                if (booking != null)
                {
                    var cust = _bookingRepository.GetCustomerByUserId(booking.UserId);
                    var firstBooking = _bookingRepository.CheckFirstBookingForVoucher(booking);
                    try
                    {
                        var trantrong = "<i>Trân trọng cảm ơn,</i>";
                        var endcontent = "Chúc mừng quý khách đã nhận voucher 200.000 VND phí xe điện cho thanh toán đặt cọc thành công lần đầu tiên trên BRG Golf App " +
                            "<br>*Ưu đãi chỉ áp dụng cho khách hàng [Tên User] Mã đặt chơi [Mã booking]-[Mã sân], trong trường hợp quý khách hủy đặt chỗ hoặc không lên sân thì ưu đãi sẽ không còn hiệu lực. ";
                        var emailTemp_title = _gridRepository.GetEmailTitleBooking(Constants.LangVn);
                        var emailTemp = _gridRepository.GetEmailTemplateBooking(Constants.LangVn);
                        if (cust.Languague == Constants.LangEn)
                        {
                            emailTemp_title = _gridRepository.GetEmailTitleBooking(Constants.LangEn);
                            emailTemp = _gridRepository.GetEmailTemplateBooking(Constants.LangEn);
                            endcontent = "Congratulations on receiving a 200.000 VND golf cart voucher for your first successful deposit payment on BRG Golf App " +
                                "<br>*Offer only applies to [Tên User] - Booking Code [Mã booking]-[Mã sân], if you cancel your reservation or don't show up, the offer will be invalid.";
                            trantrong = "<i>Best regards</i>";
                        }

                        #region send email
                        var title = string.Empty;
                        if (emailTemp_title != null)
                        {
                            title = emailTemp_title.Value.Replace("[Mã booking]", booking.BookingCode);
                            title = title.Replace("[Tên User]", booking.AppUser.FullName);
                            title = title.Replace("[Mã sân]", booking.Course.Name);
                            var tee = booking.BookingLines.FirstOrDefault().Tee_Time.Value.ToString("HH:mm");
                            title = title.Replace("[Tee Time]", tee);
                            title = title.Replace("[ngày lên chơi đặt]", booking.DateId.Value.ToString("dd/MM/yyyy"));
                            if (firstBooking)
                            {
                                if (cust.Languague == Constants.LangEn)
                                {
                                    title = title.Replace("You're booked successfully", "The first successful deposit payment");
                                }
                                else
                                {
                                    title = title.Replace("Thông báo đặt lịch thành công", "Thông báo Thanh toán Đặt cọc thành công lần đầu tiên");
                                }
                            }                            
                        }

                        if (emailTemp != null)
                        {
                            var emailString = emailTemp.Value;
                            emailString = emailString.Replace("[Mã booking]", booking.BookingCode);
                            emailString = emailString.Replace("[Tên User]", booking.AppUser.FullName);
                            var cardNo = cust.MemberCards.FirstOrDefault(x => booking.BookingLines.Select(s => s.CardNo).Contains(x.Golf_CardNo))?.Golf_CardNo;
                            if (string.IsNullOrEmpty(cardNo))
                            {
                                cardNo = cust.MemberCards.FirstOrDefault()?.Golf_CardNo;
                            }
                            emailString = emailString.Replace("[Số thẻ tại org sân nếu có]", cardNo);
                            emailString = emailString.Replace("[Thời gian book thành công trên app]", booking.UpdatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss"));
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
                            emailString = emailString.Replace("[tổng phí ước tính]", booking.TotalEstimateFee.Value.ToString("#,###", cul.NumberFormat) + " VND");
                            if (booking.NonRefundedFee.Value == 0)
                            {
                                emailString = emailString.Replace("[tổng phí đặt cọc đã thanh toán]", "0 VND");
                            }
                            else
                            {
                                emailString = emailString.Replace("[tổng phí đặt cọc đã thanh toán]", booking.NonRefundedFee.Value.ToString("#,###", cul.NumberFormat) + " VND");
                            }


                            if (firstBooking)
                            {
                                endcontent = endcontent.Replace("[Tên User]", booking.AppUser.FullName).Replace("[Mã booking]", booking.BookingCode).Replace("[Mã sân]", booking.Course.Name);
                                emailString = emailString + "<br>" + endcontent + "<br><br>";
                            }
                            else
                            {
                                emailString = emailString + "<br><br>";
                            }
                            var lineContext = string.Empty;
                            var lineStr = "Tên đặt chỗ: [Tên người chơi] - [Số thẻ nếu có] - Loại khách: [Loại khách] - 18 hố - [Phí ước tính]";
                            var lineStr1 = "Tên đặt chỗ: [Tên người chơi] - [Số thẻ nếu có] - Loại khách: [Loại khách] - 18 hố - [Phí ước tính]";
                            if (cust.Languague == Constants.LangEn)
                            {
                                lineStr = "Customer: [Tên người chơi] - [Số thẻ nếu có] -  [Loại khách] - 18 holes - [Phí ước tính]";
                                lineStr1 = "Customer: [Tên người chơi] - [Số thẻ nếu có] -  [Loại khách] - 18 holes - [Phí ước tính]";
                            }

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
                                if (line.SeagolfPrice.HasValue)
                                {
                                    lineStr = lineStr.Replace("[Phí ước tính]", line.SeagolfPrice.Value.ToString("#,###", cul.NumberFormat) + " VND");
                                }
                                else
                                {
                                    lineStr = lineStr.Replace("[Phí ước tính]", line.EstimatedPrice.Value.ToString("#,###", cul.NumberFormat) + " VND");
                                }

                                lineContext += lineStr + "<br>";
                                lineStr = lineStr1;
                            }
                            emailString = emailString.Replace(lineStr1, lineContext);

                            var serviceLine = "[Ghi chú dịch vụ thêm]";
                            var serviceLine1 = "";
                            if (booking.BookingSpecialRequests.Any())
                            {
                                if (cust.Languague == Constants.LangEn)
                                {
                                    serviceLine1 = "Extra services: " + string.Join(", ", booking.BookingSpecialRequests.Select(s => string.Format("{0}-{1}", s.BookingOtherType?.NameEn, s.Description)));
                                    emailString = emailString.Replace(serviceLine, serviceLine1);
                                }
                                else
                                {
                                    serviceLine1 = "Dịch vụ thêm: " + string.Join(", ", booking.BookingSpecialRequests.Select(s => string.Format("{0}-{1}", s.BookingOtherType?.Name, s.Description)));
                                    emailString = emailString.Replace(serviceLine, serviceLine1);
                                }
                            }
                            else
                            {
                                emailString = emailString.Replace(serviceLine, "");
                            }


                            emailString = emailString + "<br>" + trantrong;
                            //emailString = emailString.Replace(lineStr, "");
                            var emailUser = _gridRepository.GetEmailCourse(booking.C_Org_Id);
                            var clientEmail = booking?.AppUser?.Email;
                            if (clientEmail != null)
                            {
                                _mailService.SendMailAsync(clientEmail, "", "", title, emailString, "");
                            }
                            foreach (var user in emailUser)
                            {
                                if (user.Contains(","))
                                {
                                    var listMail = user.Split(',');
                                    foreach (var em in listMail)
                                    {
                                        if (!string.IsNullOrEmpty(em))
                                        {
                                            _mailService.SendMailAsync(em.Trim(), "", "", title, emailString, "");
                                        }
                                    }
                                }
                                else
                                {
                                    _mailService.SendMailAsync(user, "", "", title, emailString, "");
                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception e)
                    {
                        using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                        {
                            _log.LogError(e.Message);
                            _log.LogError(e.StackTrace);
                        }
                    }

                    #region send data booking to golf software
                    var inData = new BK_App_Import_BookingSaveModel();
                    inData.OrgCode = booking.Organization.Code;
                    inData.CourseCode = booking.Course.Code;
                    inData.BookingCode = booking.BookingCode;
                    inData.CallLocation = "app";
                    inData.ActionType = "book";
                    inData.UserName = booking.AppUser?.UserName;
                    inData.Rows = new List<BK_App_Import_BookingDTO>();
                    foreach (var line in booking.BookingLines)
                    {
                        var child = new BK_App_Import_BookingDTO
                        {
                            CardNo = line.CardNo,
                            CustomerGroupCode = line.CustomerGroup?.Code,
                            PlayerName = line.CustomerFullName,
                            FlightSeq = line.FlightSeq.Value,
                            Contact = booking.AppUser.PhoneNumber + " " + booking.AppUser.Email,
                            TeeTime = line.Tee_Time.Value,
                            Hole = line.Hole.Value,
                            StartTee = line.StartTee.Value,
                            BookingLineId = line.Id.ToString(),
                            BookingCode = booking.BookingCode
                        };
                        var specialRequest = _bookingRepository.GetspecialRequest(booking.Id);
                        foreach (var req in specialRequest)
                        {
                            child.Note = child.Note + req.BookingOtherType.Code + " " + req.Description + ",";
                        }
                        if (!string.IsNullOrEmpty(child.Note))
                        {
                            child.Note.Remove(child.Note.Length - 1, 1);
                        }
                        if (child.CustomerGroupCode == ((int)MemberCardType.memberBrg).ToString())
                        {
                            child.Note = child.Note + "-" + child.CardNo;
                        }
                        inData.Rows.Add(child);
                    }

                    var result = SendDataBBookingToGolf(inData);
                    if (result != null & result.IsSuccess)
                    {
                        return true;
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "SendEmailBookingCourse"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return false;
            }
            return true;
        }

        public GolfPriceBookingRespone SendDataBBookingToGolf(BK_App_Import_BookingSaveModel inData)
        {
            _log.LogInformation(Constants.GolfApiError + "-SendDataBBookingToGolf-input-" + Newtonsoft.Json.JsonConvert.SerializeObject(inData));
            string baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApiPro-ConfirmBooking");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApiPro-ConfirmBooking");
            }
            string checkmemberUrl = Configuration.GetSection("urlData").GetValue<string>("ImportBooking");

            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(checkmemberUrl, Method.POST);
            request.AddHeader("Accept", "*/*");
            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            request.AddHeader("Content-Type", "application/json");



            var paramsRequest = new Dictionary<string, object>();

            paramsRequest.Add("OrgCode", inData.OrgCode);
            paramsRequest.Add("CourseCode", inData.CourseCode);
            paramsRequest.Add("BookingCode", inData.BookingCode);
            paramsRequest.Add("BookingId", inData.BookingId);
            paramsRequest.Add("ActionType", inData.ActionType);
            paramsRequest.Add("CancelNote", inData.CancelNote);
            paramsRequest.Add("UserName", inData.UserName);
            paramsRequest.Add("CallLocation", inData.CallLocation);
            paramsRequest.Add("Rows", inData.Rows);
            request.AddJsonBody(paramsRequest);
            try
            {

                IRestResponse<GolfPriceBookingRespone> response = client.Execute<GolfPriceBookingRespone>(request);
                if (response.IsSuccessful)
                {
                    var bookingRespone = JsonSerializer.Deserialize<GolfPriceBookingRespone>(response.Content);

                    if (bookingRespone != null)
                    {
                        return bookingRespone;
                    }
                    else
                    {
                        _log.LogError(Constants.GolfApiError + "-SendDataBBookingToGolf-" + response.ErrorMessage);
                        return null;
                    }
                }
                else
                {
                    _log.LogError(Constants.GolfApiError + "-SendDataBBookingToGolf-" + response.ErrorMessage);
                    return null;
                }
            }
            catch (Exception e)
            {
                _log.LogError(Constants.GolfApiError + "-SendDataBBookingToGolf-" + e.Message);
                _log.LogError(e.StackTrace);
                return null;
            }
        }
    }

    public class BK_App_Import_BookingSaveModel
    {
        public string OrgCode { get; set; }
        public string CourseCode { get; set; }
        // "book"/"cancel"
        public string ActionType { get; set; }
        public string CancelNote { get; set; }
        // "app"/"web"
        public string BookingId { get; set; }
        public string BookingCode { get; set; }
        public string CallLocation { get; set; }
        public string UserName { get; set; }
        public List<BK_App_Import_BookingDTO> Rows { get; set; }
    }

    public class BK_App_Import_BookingDTO
    {
        public string BookingLineId { get; set; }
        public string BookingCode { get; set; }
        public string CardNo { get; set; }
        public string CustomerGroupCode { get; set; }
        public string PlayerName { get; set; }

        public Int32 FlightSeq { get; set; }

        public string Contact { get; set; }

        public string Note { get; set; }

        public DateTime TeeTime { get; set; }

        public Int32 Hole { get; set; }

        public Int32 StartTee { get; set; }
    }
}
