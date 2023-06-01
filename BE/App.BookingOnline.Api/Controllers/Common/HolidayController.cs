
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using System.Web.Http;

namespace App.BookingOnline.Api.Controllers
{

    public class HolidayController : BaseGridController<HolidayDTO, HolidayPagingModel, IHolidayService>
    {
        public HolidayController(IHolidayService service) : base(service)
        {
        }
    }


    
}