using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using System;
using System.Threading.Tasks;

namespace App.BookingOnline.WebApi.Controllers
{
    public class BookingController : GridController<BookingDTO, BookingFilterModel, IBookingService>
    {
        public BookingController(IBookingService service) : base(service)
        {
        }


        public override async ValueTask<RespondData> AddOrEdit(BookingDTO entityDTO)
        {
            try
            {
                if (entityDTO.Id == null || entityDTO.Id == Guid.Empty)
                {
                    var message = "Bạn không có quyền tạo mới!";
                    return Failure(message);
                }
                else
                {
                    entityDTO.UpdatedDate = DateTime.Now;
                    entityDTO.UpdatedUser = this.UserName;
                    _service.Update(entityDTO);
                }
                return Success(entityDTO.Id);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }
        public override RespondData Delete(Guid Id)
        {
            var message = "Bạn không có quyền xóa!";
            return Failure(message);
        }
    }
}
