using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Core.Helper;

namespace App.BookingOnline.Data.Repositories
{
    public class TeeSheetLockRepository : GridRepository<TeeSheetLock,TeeSheetLockPagingModel>, ITeeSheetLockRepository
    {
        public TeeSheetLockRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public override PagingResponseEntity<TeeSheetLock> GetPaging(TeeSheetLockPagingModel pagingModel)
        {

            var query = this.dbSet.Where(x => pagingModel.C_Org_Id == null || x.C_Org_Id == pagingModel.C_Org_Id)
                                  .Where(x => pagingModel.C_LockReason_Id == null || x.C_LockReason_Id == pagingModel.C_LockReason_Id)
                                  .Where(x => pagingModel.Description.IsNullOrEmpty() || x.Description.Contains(pagingModel.Description))
                                  .Where(x => pagingModel.IsActive == null || x.IsActive == pagingModel.IsActive)
                                  .Include(x => x.Organization).Include(x => x.LockReason);

            var result = new PagingResponseEntity<TeeSheetLock>
            {
                Data = query.OrderByDescending(x => x.StartDate)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList(),
                Count = query.Count()
            };
            return result;

        }

        public override Task<TeeSheetLock> SingleOrDefaultAsync(Expression<Func<TeeSheetLock, bool>> predicate)
        {
            return this.dbSet.Include(x => x.TeeSheetLockLines).ThenInclude(x => x.Course).SingleOrDefaultAsync(predicate);
        }

    }


    public class TeeSheetLockLineRepository : GridRepository<TeeSheetLockLine, TeeSheetLockLinePagingModel>, ITeeSheetLockLineRepository
    {
        public TeeSheetLockLineRepository(BookingOnlineDbContext context)
            : base(context)
        { }
    }



}