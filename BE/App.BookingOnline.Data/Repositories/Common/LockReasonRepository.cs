using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using App.Core.Helper;

namespace App.BookingOnline.Data.Repositories
{
    public class LockReasonRepository : GridRepository<LockReason, LockReasonPagingModel>, ILockReasonRepository
    {
        public LockReasonRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public override PagingResponseEntity<LockReason> GetPaging(LockReasonPagingModel pagingModel)
        {
            var query = this.dbSet.AsQueryable()
                            .Where(x => pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code))
                            .Where(x => pagingModel.Name.IsNullOrEmpty() || x.Name.Contains(pagingModel.Name))
                            .Include(x => x.Organization);
            var result = new PagingResponseEntity<LockReason>
            {
                Data = query.OrderBy(x => x.CreatedDate)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList(),
                Count = query.Count()
            };

            return result;
        }
    }




}