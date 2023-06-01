using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static App.Core.Enums;

namespace App.BookingOnline.AppApi.Controllers
{
    public class BookingController : GridController<BookingDTO, BookingFilterModel, IBookingService>
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger _log;
        public BookingController(IBookingService service, ILogger<BookingController> logger, IConfiguration configuration) : base(service)
        {
            _log = logger;
            Configuration = configuration;
        }

        /// <summary>
        /// lấy thông tin booking theo mã
        /// </summary>
        /// <param name="code"></param>
        /// <param name="bookedDate"></param>
        /// <returns></returns>
        [HttpGet("GetBookingByCode")]
        public RespondData GetBookingByCode(string code, DateTime? bookedDate)
        {
            RespondData data = _service.GetBookingByCode(code, CurOrgId, bookedDate);
            return data;
        }

        /// <summary>
        /// Lưu thông tin người chơi đến sân
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SaveTimePlayerOnBoard")]
        public RespondData SaveTimePlayerOnBoard(GolfBagDto model)
        {
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
            model.UserId = UserId;
            RespondData data = _service.SaveTimePlayerOnBoard(model);
            return data;
        }

        /// <summary>
        /// Hủy golf bag
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CancelGolfBag")]
        public RespondData CancelGolfBag(GolfBagDto model)
        {
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
            model.UserId = UserId;
            model.GolfBag = string.Empty;
            RespondData data = _service.SaveTimePlayerOnBoard(model);
            return data;
        }

        /// <summary>
        /// Hủy đặt vé.
        /// Nếu api trả về IsRefundMoney = true thì gọi sdk sang seabank để hoàn tiền cho khách.
        /// TotalRefund là số tiền hoàn truyền sang sdk
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet("CancelBooking")]
        public RespondData CancelBooking(Guid bookingId)
        {
            try
            {
                RespondData data = _service.CancelBooking(bookingId, UserId, false);
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogInformation(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
                return data;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new RespondData { IsSuccess = false, MsgCode = "81" };
            }
        }

        /// <summary>
        /// Truyền bookingId lên để kiểm tra có đặt vé sau 24h không.
        /// Nếu trả ra data = 0, không cần hoàn lại tiền đặt cọc  
        /// Nếu trả ra data = 1, hoàn 50% số tiền đặt cọc.
        /// Nếu trả ra data = 2, hoàn 100% số tiền đặt cọc
        /// </summary>
        /// <param name="bookingId">bookingId</param>
        /// <returns></returns>
        [HttpGet("CheckRuleCancelBooking")]
        public RespondData CheckRuleCancelBooking(Guid bookingId)
        {
            RespondData data = _service.CheckRuleCancelBooking(bookingId, UserId, false);
            return data;
        }


        /// <summary>
        /// Sau khi gọi sdk hoàn tiền thì gọi api để lưu thông tin trả về
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SaveSdkCancelReturn")]
        public RespondData SaveSdkCancelReturn(SbCancelReturn model)
        {
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
            RespondData data = _service.SaveSbCancelReturn(model);
            return data;
        }

        /// <summary>
        /// api hủy tee time khi ấn nút back
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("CancelTempBookingTeetime")]
        public RespondData CancelTempBookingTeetime(Guid id)
        {
            RespondData data = _service.CancelTempBookingTeetime(id);
            return data;
        }


        /// <summary>
        /// gọi API này để lấy dữ liệu màn hình khởi đầu chọn teetime
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("BookingTeeTimeStep")]
        public RespondData BookingTeeTimeStep(BookingTeeTime model)
        {
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
            var userName = UserName;
            model.UserId = UserId;
            var data = _service.GetBookingTeetimeData(model);
            return data;
        }

        /// <summary>
        /// sau khi chọn teetime người dùng gọi API này để tiến hành đặt vé.
        /// bước 1: sau khi chọn xong xong teetime thì gửi lên server với ButtonType = "TeesheetStep". 
        /// bước 2: sau khi chọn xong người chơi thì gửi lên server với ButtonType = "PersonInfoStep". 
        /// bước 3: Sau khi check gia xong thì gửi lên server với ButtonType = "PriceCheckStep".          
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Booking")]
        public async Task<RespondData> Booking(BookingDTO model)
        {
            try
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogInformation(JsonConvert.SerializeObject(model, Formatting.Indented));
                }

                var isConnectSdk = Configuration.GetValue<bool>("IsConnectSdk");
                model.UserId = UserId;
                model.UpdatedDate = DateTime.Now;
                model.UpdatedUser = UserId;

                if (model.ButtonType == BookingButtonType.TeesheetStep.ToString())
                {
                    model.CreatedUser = UserId;
                    model.CreatedDate = DateTime.Now;
                    var selectedTeeSheet = new List<BookingLineDTO>();
                    selectedTeeSheet.AddRange(model.BookingTeetime.Where(x => x.IsSelected));

                    foreach (var tee in selectedTeeSheet)
                    {
                        if (model.DateId.Date != tee.Tee_Time.Date)
                        {
                            return new RespondData { IsSuccess = false, Data = model, MsgCode = "77" };
                        }
                    }

                    if (selectedTeeSheet.Any())
                    {
                        var checkSlot = _service.CheckNumberAvailableSlot(selectedTeeSheet.Select(s => s.Tee_Time).ToList(), UserId, model.Courses.Id, model.Courses.C_Org_Id, model.CardMemberType);
                        if (!checkSlot)
                        {
                            return new RespondData
                            {
                                IsSuccess = false,
                                Data = model,
                                MsgCode = "72"
                            };
                        }

                        string teeSheetMesage = _service.CheckSelectedTeesheet(selectedTeeSheet, UserId, model.Courses.Id, model.Courses.C_Org_Id);
                        var msgSplit = teeSheetMesage.Split('-');
                        if (!string.IsNullOrEmpty(teeSheetMesage))
                        {
                            return new RespondData
                            {
                                IsSuccess = false,
                                Data = model,
                                MsgCode = msgSplit[0],
                                MsgParams = new List<string> { msgSplit[1] }
                            };
                        }
                    }
                    else
                    {
                        return new RespondData { IsSuccess = false, Data = model, MsgCode = "15" };
                    }

                    var data = await _service.InsertBookingSession(model);
                    model.BookingSpecialRequests = _service.GetBookingSpecialRequests(model.Courses.C_Org_Id);
                    model.MemberCardTypes = _service.GetCustomerGroupData(model.Courses.C_Org_Id);
                    model.IsConnectSdk = isConnectSdk;
                    return new RespondData { IsSuccess = true, Data = model };
                }
                if (model.ButtonType == BookingButtonType.PersonInfoStep.ToString())
                {
                    if (!CheckHavingMember(model.BookingTeetime.Where(x => x.IsSelected)))
                    {
                        return new RespondData { IsSuccess = false, MsgCode = "80" };
                    }
                    var checkNumberFlight = _service.CheckNumberFlightHoliday(model.DateId, model.Courses.C_Org_Id, model.BookingTeetime.Count());
                    if (!checkNumberFlight)
                    {
                        return new RespondData { IsSuccess = false, Data = model, MsgCode = "76" };
                    }

                    if (!isConnectSdk)
                    {
                        //bản stag bỏ đoan check nay
                        if (model.GolfCardNo.IsNullOrEmpty() || !model.BookingTeetime.Select(s => s.CardNo.ToLower()).Contains(model.GolfCardNo.ToLower()))
                        {
                            return new RespondData { IsSuccess = false, Data = model, MsgCode = "75" };
                        }
                    }

                    var data = _service.GetPriceGolfPlayer(ref model);
                    if (data.IsSuccess)
                    {
                        model.IsConnectSdk = isConnectSdk;
                        var result = await _service.InssertGolfPlayerStep(model);
                        return result;
                    }
                    else
                    {
                        return new RespondData { IsSuccess = false, MsgCode = data.MsgCode, MsgParams = data.MsgParams };
                    }
                }

                if (model.ButtonType == BookingButtonType.PriceCheckStep.ToString())
                {
                    if (!CheckHavingMember(model.BookingTeetime.Where(x => x.IsSelected)))
                    {
                        return new RespondData { IsSuccess = false, MsgCode = "80" };
                    }
                    model.IsConnectSdk = isConnectSdk;
                    return await _service.PriceCheckStep(model);
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                    if (e.InnerException != null)
                    {
                        _log.LogError(e.InnerException.Message);
                    }
                }
                return new RespondData { IsSuccess = false, Data = model, Message = e.Message };
            }
            return new RespondData { IsSuccess = false, Data = model };
        }

        private bool CheckHavingMember(IEnumerable<BookingLineDTO> bookingLines)
        {
            if (bookingLines.Any(x => x.CustomerGroupCode == ((int)MemberCardType.memberGuest).ToString())
                            && !bookingLines.Any(x => x.CustomerGroupCode == ((int)MemberCardType.member).ToString()))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gọi api sau khi gọi sdk seabank thanh toán tiền thành công.
        /// Tham số truyền vào : tham số sdk trả ra và IsPaymentSuccess = true      
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("BookingPayment")]
        public RespondData BookingPayment(PaymentData model)
        {
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
            model.UserId = UserId;
            var data = _service.GolfPaymentStep(model);
            return data;
        }

        /// <summary>
        /// sau khi gọi sdk thank toán mà báo lỗi thì gọi api này để resresh bookingcode. nếu người dùng chọn tài khoản rồi thanh toán tiếp thì thay đổi bookingcode mà api này trả ra.
        /// Đầu vào cần truyền mỗi bookingId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("RefreshBookingCode")]
        public RespondData RefreshBookingCode(PaymentData model)
        {
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
            model.UserId = UserId;
            model.IsPaymentSuccess = false;
            var data = _service.RefreshBookingCode(model);

            return data;
        }

        /// <summary>
        /// kiem ta 1 the member card
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[HttpPost("CheckMemberCard")]
        //public RespondData CheckMemberCard(BookingTeeTime input)
        //{
        //    RespondData result = _service.CheckMemberCard(input);
        //    return result;
        //}

        /// <summary>
        /// kiểm tra các thẻ ở màn hình booking
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CheckAllMemberCard")]
        public RespondData CheckAllMemberCards(CheckCardDto input)
        {
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(input, Formatting.Indented));
            }

            RespondData result = _service.CheckAllMemberCards(input);
            return result;
        }

        /// <summary>
        /// Lấy thông tin các tài khoản ngân hàng đã liên kết
        /// </summary>
        /// <returns></returns>
        //[HttpGet("GetBankInfo")]
        //public RespondData GetBankInfo()
        //{
        //    return _service.GetBankInfo(UserId);
        //}

        /// <summary>
        /// lưu thông tin sau khi liên kết thành công
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost("InsertBankInfo")]
        //public RespondData InsertBankInfo(UserBankInfoDTO model)
        //{
        //    model.UserId = new Guid(UserId);
        //    return _service.InsertBankInfo(model);
        //}

        /// <summary>
        /// api để lấy các loại thanh toán cho phép
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPaymentType")]
        public RespondData GetPaymentType()
        {
            return _service.GetPaymentType(UserId);
        }

        /// <summary>
        /// Chọn loại khách của khách đặt chỗ
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [HttpGet("GetCustomerGroup")]
        public RespondData GetCustomerGroup(Guid orgId)
        {
            return _service.GetCustomerGroup(orgId);
        }

        [HttpGet("GetBookingOtherType")]
        public RespondData GetBookingOtherType(Guid courseId)
        {
            return _service.GetBookingOtherType(courseId);
        }

        /// <summary>
        /// Lấy lịch sử booking 
        /// </summary>
        /// <returns></returns>
        [HttpGet("BookingHistory")]
        public RespondData BookingHistory()
        {
            try
            {
                BookingFilterModel model = new BookingFilterModel();
                model.UserId = UserId;
                return Success(_service.BookingHistory(model));
            }
            catch (Exception e)
            {
                return new RespondData { IsSuccess = false, MsgCode = "63" };
            }
        }


        /// <summary>
        /// Lấy thông tin chi tiết của booking hiện tại
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpGet("BookingHistoryDetail")]
        public RespondData BookingHistoryDetail(Guid bookingId)
        {
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation("BookingId: " + bookingId.ToString());
            }
            try
            {
                BookingFilterModel model = new BookingFilterModel();
                model.UserId = UserId;
                model.BookingId = bookingId;
                RespondData data = _service.BookingHistoryDetail(model);
                return data;
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }

        [HttpGet("Readlogfile")]
        public RespondData Readlogfile(DateTime date, string type)
        {
            if (type == "mobile")
            {
                var curFolder = Directory.GetCurrentDirectory() + "\\logs\\log-" + date.ToString("yyyy-MM-dd") + ".txt";
                //var copyfileFolder = Directory.GetCurrentDirectory() + "\\logs\\copyfile\\log-" + date.ToString("yyyy-MM-dd") + ".txt" ;
                //var sourceFile = new FileInfo(curFolder);
                //sourceFile.CopyTo(copyfileFolder, true);
                //System.IO.File.Copy(curFolder, copyfileFolder);
                using (StreamReader sr = System.IO.File.OpenText(curFolder))
                {
                    string s;
                    while ((s = sr.ReadToEnd()) != null)
                    {
                        Console.WriteLine(s);
                        return new RespondData { IsSuccess = true, Data = s };
                    }
                }
            }
            return new RespondData { IsSuccess = false };
        }



        public override RespondData Delete(Guid Id)
        {
            return Failure("Not permis");
        }


    }
}
