using App.BookingOnline.AppApi.Controllers;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Service.DTO.Common;
using App.BookingOnline.Service.IService.Common;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace App.BookingOnline.MobileApi.Controllers.Common
{
    public class NotificationController : GridController<NotificationDTO, NotificationFilterModel, INotificationService>
    {
        private readonly ILogger _log;
        public NotificationController(INotificationService service, ILogger<NotificationController> logger) : base(service)
        {
            _log = logger;
        }

        /// <summary>
        /// Lấy chi tiết 1 thông báo theo Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        public override RespondData Get(Guid Id)
        {
            try
            {
                var data = _service.GetNotificationById(Id,UserId);                
                return data;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                }
                return new RespondData { IsSuccess = false, MsgCode = "44" };
            }

        }

        /// <summary>
        /// Lấy tất cả thông báo đã gửi tới điện thoại của login user
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllNotificationByUser")]
        public async Task<RespondData> GetAllNotificationByUser()
        {
            try
            {
                var data = await _service.GetAllNotificationByUserAsync(UserId);
                return data;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                }
                return new RespondData { IsSuccess = false, MsgCode = "44" };
            }

        }

        /// <summary>
        /// thông báo chưa đọc
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNumberOpenNotiByUser")]
        public async Task<RespondData> GetNumberOpenNotiByUser()
        {
            try
            {
                var data = await _service.GetNumberOpenNotiByUser(UserId);
                return data;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                }
                return new RespondData { IsSuccess = false, MsgCode = "44" };
            }
        }

        /// <summary>
        /// read all noti
        /// </summary>
        /// <returns></returns>
        [HttpGet("UpdateNumberOpenNotiByUser")]
        public async Task<RespondData> UpdateNumberOpenNotiByUser()
        {
            try
            {
                var data = await _service.UpdateNumberOpenNotiByUser(UserId);
                return data;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                }
                return new RespondData { IsSuccess = false, MsgCode = "44" };
            }

        }
    }
}
