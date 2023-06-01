using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static App.Core.Enums;

namespace App.BookingOnline.Data.Repositories.Common
{
    public class NotificationRepository : BaseGridRepository<GF_Notification, NotificationFilterModel>, INotificationRepository
    {
        private readonly IBaseRepository<NotificationQueue> _notificationQueueRepo;
        private readonly ILogger _log;
        public NotificationRepository(IUnitOfWork unitOfWork, ILogger<NotificationRepository> logger) : base(unitOfWork)
        {
            _notificationQueueRepo = _unitOfWork.GetDataRepository<NotificationQueue>();
            _log = logger;
        }

        //public Task GetAllNotificationByUser(string userId)
        //{
        //    var guidId = new Guid(userId);
        //    var data = _notificationQueueRepo.SelectWhere(x => x.UserId == guidId).ToList();
        //}

        public override PagingResponseEntity<GF_Notification> GetPaging(NotificationFilterModel pagingModel)
        {
            var paggingData = GetPagingData(pagingModel);

            var data = new PagingResponseEntity<GF_Notification>
            {
                Data = paggingData
            };
            data.Count = paggingData.Count();

            return data;
        }

        public override async ValueTask<GF_Notification> AddAsync(GF_Notification entityDTo)
        {
            entityDTo.Sent_Date = DateTime.Now;
            var res = await _repo.AddAsync(entityDTo);
            _unitOfWork.SaveChanges();
            return res;
        }

        public override void Update(GF_Notification entityDto)
        {
            entityDto.Sent_Date = DateTime.Now;
            _repo.Update(entityDto);
            _unitOfWork.SaveChanges();
        }

        public IEnumerable<GF_Notification> GetPagingData(NotificationFilterModel filter)
        {
            var result = _repo.SelectWhere(x => (x.Code == filter.Code || string.IsNullOrEmpty(filter.Code))
                        && (x.Message_Title.Contains(filter.Messange_title) || string.IsNullOrEmpty(filter.Messange_title))
                        && (x.Status == filter.Status || filter.Status == null)
                        && ((filter.SendDate.HasValue && x.Sent_Date.Value.Date == filter.SendDate.Value.Date) || filter.SendDate == null)
                        && (x.Sent_User.Contains(filter.SendUser) || string.IsNullOrEmpty(filter.SendUser)))
                            .Skip(filter.PageIndex * filter.PageSize)
                            .Take(filter.PageSize);

            return result;
        }

        public async Task<IEnumerable<NotificationQueue>> GetAllNotificationByUser(string userId)
        {
            var guidId = new Guid(userId);
            return _notificationQueueRepo.SelectWhere(x => x.UserId == guidId && x.IsSend.Value && x.IsActive.Value
            && x.NotificationType != FcmNotifiType.SendEmailBookingCourse.ToString()).OrderByDescending(o => o.CreatedDate);
        }

        public NotificationQueue GetNotificationById(Guid id, string userId)
        {
            var data = _notificationQueueRepo.SingleOrDefault(x => x.Id == id);
            data.IsRead = true;
            data.UpdatedDate = DateTime.Now;
            data.UpdatedUser = userId;
            _notificationQueueRepo.Update(data);
            return data;
        }

        public async void InsertNotificationQueue(List<NotificationQueue> notificationQueue)
        {
            foreach (var item in notificationQueue)
            {
                try
                {
                    await _notificationQueueRepo.AddAsync(item);
                }
                catch (Exception e)
                {
                    using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                    {
                        _log.LogError(e.Message);
                    }
                }
            }
            _unitOfWork.SaveChanges();
        }

        public GF_Notification GetNotificationByCode(string code)
        {
            return _repo.SelectWhere(x => x.Code == code).FirstOrDefault();
        }

        public int GetNumberOpenNotiByUser(string userId)
        {
            return _notificationQueueRepo.SelectWhere(x => x.IsActive.Value && (!x.IsRead.Value || !x.IsRead.HasValue) && x.UserId == new Guid(userId)).Count();
        }

        public RespondData UpdateNumberOpenNotiByUser(string userId)
        {
            var data = _notificationQueueRepo.SelectWhereNoTracking(x => x.IsActive.Value && x.UserId == new Guid(userId)).ToList();
            foreach (var item in data)
            {
                item.IsRead = true;
                _notificationQueueRepo.Update(item);
            }
            _unitOfWork.SaveChanges();
            return new RespondData { IsSuccess = true };
        }
    }
}
