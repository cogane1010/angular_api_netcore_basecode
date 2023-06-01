using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.Core.Service;

namespace App.BookingOnline.Service
{
    public class CustomerGroupService : BaseGridService<CustomerGroupDTO, CustomerGroup, CustomerGroupPagingModel, ICustomerGroupRepository>, ICustomerGroupService
    {  
        public CustomerGroupService(ICustomerGroupRepository repo) : base(repo)
        {
           
        }
    }
    public class CustomerGroupMappingService : BaseGridService<CustomerGroupMappingDTO, CustomerGroupMapping, CustomerGroupMappingPagingModel, ICustomerGroupMappingRepository>, ICustomerGroupMappingService
    {
        public CustomerGroupMappingService(ICustomerGroupMappingRepository repo) : base(repo)
        {

        }
    }
}

