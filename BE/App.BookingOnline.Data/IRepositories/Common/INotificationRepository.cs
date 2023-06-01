using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.IRepositories.Common
{
    public interface INotificationRepository : IBaseGridRepository<GF_Notification, NotificationFilterModel>
    {
        IEnumerable<GF_Notification> GetPagingData(NotificationFilterModel pagingModel);
        Task<IEnumerable<NotificationQueue>> GetAllNotificationByUser(string userId);
        NotificationQueue GetNotificationById(Guid id, string userId);
        void InsertNotificationQueue(List<NotificationQueue> notificationQueue);
        GF_Notification GetNotificationByCode(string code);
        int GetNumberOpenNotiByUser(string userId);
        RespondData UpdateNumberOpenNotiByUser(string userId);
    }
}
