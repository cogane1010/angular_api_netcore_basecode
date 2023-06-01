using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Service.DTO.Common;
using App.BookingOnline.Service.IService.Common;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace App.BookingOnline.WebApi.Controllers.Common
{
    [ApiController]
    public class SmsHistoryController : GridController<SmsHistoryDTO, SmsHistoryFilterModel, ISmsHistoryService>
    {
        public SmsHistoryController(ISmsHistoryService service) : base(service)
        {


        }

        [HttpPost("PushNotification")]
        public RespondData PushNotificationForAllDevice(NotificationDTO model)
        {
            model.CreatedUser = UserId;
            model.Sent_User = UserName;
            return new RespondData { IsSuccess = _service.PushNotificationForAllDevice(model) };
        }
    }
}
