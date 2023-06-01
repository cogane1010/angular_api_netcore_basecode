using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using Serilog.Context;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using static App.Core.Enums;

namespace App.BookingOnline.Data.Repositories
{
    public class LogDateFileOutRepository : BaseGridRepository<LogDateFileOut, LogDateFileOutPagingModel>, ILogDateFileOutRepository
    {
        protected readonly BookingOnlineDbContext _context;
        private readonly ILogger _log;

        public LogDateFileOutRepository(IUnitOfWork unitOfWork, BookingOnlineDbContext context, ILogger<LogDateFileOutRepository> logger)
            : base(unitOfWork)
        {
            _context = context;
            _log = logger;
        }

        public IEnumerable<LogDateFileOut> Find(Expression<Func<LogDateFileOut, bool>> predicate)
        {
            return this._repo.AsQueryable().Where(predicate);
        }

        public override PagingResponseEntity<LogDateFileOut> GetPaging(LogDateFileOutPagingModel pagingModel)
        {
            var query = this._repo.AsQueryable()
                            .Where(x => pagingModel.Date.Date == pagingModel.Date.Date)
                            .Where(x => pagingModel.Status == pagingModel.Status && x.FileType == FileType.Out.ToString());
            var result = new PagingResponseEntity<LogDateFileOut>
            {
                Data = query.OrderBy(x => x.CreatedDate)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList(),
                Count = query.Count()
            };

            return result;
        }

        public void UpdateDates(List<LogDateFileOut> notSentLogDateFileOut)
        {
            try
            {
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    foreach (var item in notSentLogDateFileOut)
                    {
                        item.UpdatedDate = DateTime.Now;
                        _repo.Update(item);
                    }
                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName))
                {
                    _log.LogError(ex.Message);
                }
                throw ex;
            }
        }
        //return _context.Set<LogDateFileOut>().Where(predicate);
    }
}
