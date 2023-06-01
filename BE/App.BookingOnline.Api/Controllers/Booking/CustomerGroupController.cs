using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;


namespace App.BookingOnline.Api.Controllers
{

    public class CustomerGroupController : BaseGridController<CustomerGroupDTO, CustomerGroupPagingModel, ICustomerGroupService>
    {

        public CustomerGroupController(ICustomerGroupService service) : base(service)
        {

        }
    }

    public class CustomerGroupMappingController : BaseGridController<CustomerGroupMappingDTO, CustomerGroupMappingPagingModel, ICustomerGroupMappingService>
    {

        public CustomerGroupMappingController(ICustomerGroupMappingService service) : base(service)
        {

        }
    }
}