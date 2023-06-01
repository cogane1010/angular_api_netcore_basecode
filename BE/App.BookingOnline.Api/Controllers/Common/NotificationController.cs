using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Service.DTO.Common;
using App.BookingOnline.Service.IService.Common;

namespace App.BookingOnline.WebApi.Controllers.Common
{
    public class NotificationController : GridController<NotificationDTO, NotificationFilterModel, INotificationService>
    {
        public NotificationController(INotificationService service) : base(service)
        {
            
        }
    }
}
