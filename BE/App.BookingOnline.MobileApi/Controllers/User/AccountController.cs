using App.BookingOnline.AppApi.Controllers;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.MobileApi.ViewModel;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.BookingOnline.MobileApi.Controllers.User
{
    public class AccountController : GridController<CustomerDTO, UserPagingModel, IAppUserService>
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger _log;

        public AccountController(IAppUserService service, IConfiguration configuration, ILogger<AccountController> logger) : base(service)
        {
            Configuration = configuration;
            _log = logger;
        }

        /// <summary>
        /// quên mật khẩu
        /// </summary>
        /// <param name="forgotPasswordModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<RespondData> ForgotPassword(ForgotPasswordViewModel model)
        {
            //TODO: check 10' an quen mat khua
            if (!string.IsNullOrEmpty(model.Email))
            {
                return await _service.ForgotPasswordAsync(model.Email);
            }
            return new RespondData { IsSuccess = false, MsgCode = "42" };
        }

        /// <summary>
        /// thay đổi mật khẩu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("ChangePassword")]
        public async Task<RespondData> ChangePassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var username = UserName;
                return await _service.ChangePassword(username, model.Password, model.OldPassword);
            }
            return new RespondData { IsSuccess = false, MsgCode = "01" };
        }

        /// <summary>
        /// lấy mã OTP
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetOtpCode")]
        public async Task<RespondData> GetOtpCode(string lang, string mobilePhone, string email = "")
        {
            if (string.IsNullOrEmpty(mobilePhone))
            {
                return new RespondData { IsSuccess = false, MsgCode = "49" };
            }
            RespondData result = await _service.GetOtpCode(string.Empty, lang, mobilePhone, email);
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(result) + " lang:" + lang + " phone:" + mobilePhone);
            }
            return result;
        }

        /// <summary>
        /// Kiểm tra OTP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("CheckOtpCode")]
        public RespondData CheckOtpCode(SmsHistoryDTO model)
        {
            RespondData result = _service.CheckOtpCode(model);
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(model) + " SmsHistoryDTO:" + JsonConvert.SerializeObject(result));
            }
            return result;
        }

        /// <summary>
        /// lấy đường link lưu avatar trên server
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetuploadFolder")]
        public RespondData GetuploadFolder()
        {
            RespondData result = _service.GetuploadFolder();
            return result;
        }

        /// <summary>
        /// tạo tài khoản user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<RespondData> CreateAccount(CustomerDTO model)
        {
            if (ModelState.IsValid)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogInformation(JsonConvert.SerializeObject(model));
                }
                if (!string.IsNullOrEmpty(model.OtpCode) && model.OtpId != Guid.Empty && model.OtpId != null)
                {
                    SmsHistoryDTO smsHistoryDTO = new SmsHistoryDTO
                    {
                        Id = model.OtpId.Value,
                        Code = model.OtpCode
                    };
                    RespondData resultOtp = _service.CheckOtpCode(smsHistoryDTO);
                    if (!resultOtp.IsSuccess)
                    {
                        return new RespondData { IsSuccess = false, MsgCode = resultOtp.MsgCode };
                    }
                    if (!string.IsNullOrEmpty(model.MobilePhone))
                    {
                        var phone = RemoveWhitespace(model.MobilePhone);
                        model.MobilePhone = phone;
                    }

                    RespondData result = await _service.CreateAccount(model);
                    return result;
                }
                else
                {
                    return _service.CheckCreateAccount(model);
                }
            }
            return new RespondData { IsSuccess = false, MsgCode = "02" };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string RemoveWhitespace(string input)
        {
            if (input == null)
                return null;
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        /// <summary>
        /// Kiểm tra có phải thẻ hội viên ở màn hình tạo tài khoản
        /// "CardNo": "DR001",
        /// "CustomerFullName": "LÊ MINH TRUNG"
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="fullname"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("CheckGoflBrgCard")]
        public RespondData CheckGoflBrgCard(string cardNumber, string fullname)
        {
            return _service.CheckGoflBrgCard(cardNumber, fullname);
        }

        /// <summary>
        /// gọi hàm này lấy thông tin user trước khi update
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAccountInfo")]
        public RespondData GetAccountInfo()
        {
            RespondData result = _service.GetAccountInfo(UserId);
            return result;
        }

        /// <summary>
        /// sửa thông tin tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<RespondData> UpdateAccount(CustomerDTO model)
        {
            using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
            {
                _log.LogInformation(JsonConvert.SerializeObject(model));
            }
            if (ModelState.IsValid)
            {
                model.UpdatedDate = DateTime.Now;
                model.UpdatedUser = UserId;

                RespondData result = await _service.UpdateAccount(model);

                return result;
            }
            return new RespondData { IsSuccess = false, MsgCode = "03" };
        }

        /// <summary>
        /// Lấy Danh sách thẻ hội viên của user đã login
        /// </summary>
        /// <returns></returns>
        //[Authorize(Policy = "customer")]
        [HttpGet("ListMemberCard")]
        public RespondData ListMemberCard()
        {
            return _service.ListMemberCard(UserId);
        }

        /// <summary>
        /// 4.2.5	Đăng ký hội viên sân Golf BRG
        /// lấy thông tin login user trước khi đăng ký hội viên
        /// </summary>
        /// <returns></returns>
        [HttpGet("RegisterMemberCard")]
        public RespondData RegisterMemberCard()
        {
            return _service.GetInfoMemberCard(UserId);
        }

        /// <summary>
        /// đơn vị sân Golf muốn đăng ký mở thẻ
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllOrg")]
        public RespondData GetAllOrg()
        {
            return _service.GetAllOrg();
        }

        /// <summary>
        /// 4.2.5	Đăng ký hội viên sân Golf BRG
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("RegisterMemberCard")]
        public async Task<RespondData> RegisterMemberCard(MemberRequestDTO model)
        {
            model.CreatedUser = UserId;
            model.CreatedDate = DateTime.Now;
            model.UserId = UserId;
            return await _service.RegisterMemberCard(model);
        }

    }
}
