using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO.Common;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.BookingOnline.Service.IService.Common
{
    public interface INotificationService : IBaseGridDataService<NotificationDTO, NotificationFilterModel>
    {
        Task<RespondData> GetAllNotificationByUserAsync(string userId);
        RespondData GetNotificationById(Guid id, string userId);
        Task<RespondData> GetNumberOpenNotiByUser(string userId);        
        Task<RespondData> UpdateNumberOpenNotiByUser(string userId);
    }
}
