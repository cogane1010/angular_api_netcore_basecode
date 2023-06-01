using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace App.BookingOnline.Data.Repositories
{
    public class HolidayRepository : GridRepository<Holiday, HolidayPagingModel>, IHolidayRepository
    {
        public HolidayRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public bool CheckDateHoliday(Guid orgId, DateTime dateId)
        {
            var query = this.dbSet.AsQueryable().Where(x => x.C_Org_Id == orgId && x.DateId == dateId && x.IsActive);
            if (query.Any())
            {
                return true;
            }
            return false;
        }

        public override PagingResponseEntity<Holiday> GetPaging(HolidayPagingModel pagingModel)
        {
            var org = Context.Set<Organization>().Where(x => x.IsActive && x.Id == new Guid(pagingModel.UserOrgId)).FirstOrDefault();
            if (org.IsSummary)
            {
                pagingModel.UserOrgId = String.Empty;
            }
            var query = this.dbSet.AsQueryable()
                            .Where(x => string.IsNullOrEmpty(pagingModel.UserOrgId) || x.C_Org_Id == new Guid(pagingModel.UserOrgId))
                            .Where(x => string.IsNullOrEmpty(pagingModel.Code) || x.Code.Contains(pagingModel.Code))
                            .Where(x => string.IsNullOrEmpty(pagingModel.Name) || x.Name.Contains(pagingModel.Name))
                            .Include(x => x.Organization);
            var result = new PagingResponseEntity<Holiday>
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