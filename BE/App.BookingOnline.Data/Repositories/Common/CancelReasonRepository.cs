using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using App.Core.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core;

namespace App.BookingOnline.Data.Repositories
{
    public class CancelReasonRepository : GridRepository<CancelReason, CancelReasonPagingModel>, ICancelReasonRepository
    {
        public CancelReasonRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public void CancelBookingsFromWeb(IEnumerable<Booking> bookings)
        {
            foreach(var item in bookings)
            {

            }
        }

        public async Task<IEnumerable<CancelReason>> GetCancelReason(CancelReasonPagingModel pagingModel)
        {
            return this.dbSet.AsQueryable().Where(x => x.C_Org_Id == new System.Guid(pagingModel.UserOrgId) 
                                || pagingModel.UserOrgId == null
                                || x.Organization.Code == Constants.BrgCode);
        }

        public override PagingResponseEntity<CancelReason> GetPaging(CancelReasonPagingModel pagingModel)
        {
            var query = this.dbSet.AsQueryable()
                            .Where(x => pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code))
                            .Where(x => pagingModel.Name.IsNullOrEmpty() || x.Name.Contains(pagingModel.Name))
                            .Where(x => pagingModel.UserOrgId.IsNullOrEmpty() || x.C_Org_Id == new System.Guid(pagingModel.UserOrgId))
                            .Include(x => x.Organization);
            var result = new PagingResponseEntity<CancelReason>
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