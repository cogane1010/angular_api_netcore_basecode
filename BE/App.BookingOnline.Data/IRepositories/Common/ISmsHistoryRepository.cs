using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Models;
using App.Core.Repositories;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.IRepositories.Common
{
    public interface ISmsHistoryRepository : IBaseGridRepository<SmsHistory, SmsHistoryFilterModel>
    {
        object GetAllUser();
        IEnumerable<string> GetEmailCourse(Guid orgId);
        Setting GetEmailTemplateBooking(string langVn);
        Setting GetEmailTitleBooking(string lang);
    }
}
