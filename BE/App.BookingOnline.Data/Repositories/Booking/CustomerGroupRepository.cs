using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using App.Core.Helper;

namespace App.BookingOnline.Data.Repositories
{
    public class CustomerGroupRepository : GridRepository<CustomerGroup,CustomerGroupPagingModel>, ICustomerGroupRepository
    {
        public CustomerGroupRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public override PagingResponseEntity<CustomerGroup> GetPaging(CustomerGroupPagingModel pagingModel)
        {
            var query = this.dbSet.Where(x => pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code))
                                .Where(x => pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code))
                                .Where(x => pagingModel.C_Org_Id == null || x.C_Org_Id == pagingModel.C_Org_Id)
                                .Where(x => pagingModel.IsActive == null || x.IsActive == pagingModel.IsActive)
                                .Include(x => x.Organization);

            var result = new PagingResponseEntity<CustomerGroup>
            {
                Data = query.OrderBy(x => x.OrderValue)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList(),
                Count = query.Count()
            };
            return result;
        }
    }

    public class CustomerGroupMappingRepository : GridRepository<CustomerGroupMapping, CustomerGroupMappingPagingModel>, ICustomerGroupMappingRepository
    {
        public CustomerGroupMappingRepository(BookingOnlineDbContext context)
            : base(context)
        { }
    }




}