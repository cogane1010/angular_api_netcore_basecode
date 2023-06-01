using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO.Booking;
using App.BookingOnline.Service.DTO.Common;
using App.BookingOnline.Service.Service.Common;
using App.Core.Domain;
using System;

namespace App.BookingOnline.Service.IService.Common
{
    public interface ISmsHistoryService : IBaseGridDataService<SmsHistoryDTO, SmsHistoryFilterModel>
    {
        bool PushNotificationToDevice(NotificationQueue model);
        bool PushNotificationForAllDevice(NotificationDTO model);
        bool SendEmailBookingCourse(Guid? bookingId);
        GolfPriceBookingRespone SendDataBBookingToGolf(BK_App_Import_BookingSaveModel inData);
    }
}
