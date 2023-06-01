using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.Repositories
{
    public interface ILogDateFileOutRepository : IBaseGridRepository<LogDateFileOut, LogDateFileOutPagingModel>
    {
        void UpdateDates(List<LogDateFileOut> notSentLogDateFileOut);
        IEnumerable<LogDateFileOut> Find(Expression<Func<LogDateFileOut, bool>> predicate);
    }
}