
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;


namespace App.BookingOnline.Api.Controllers
{

    public class PaymentTypeController : BaseGridController<PaymentTypeDTO, PaymentTypePagingModel, IPaymentTypeService>
    {
        public PaymentTypeController(IPaymentTypeService service) : base(service)
        {
        }
    }


    
}