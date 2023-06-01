using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace App.BookingOnline.WebApi.Controllers
{
    public class HistoryBookingController : GridController<BookingDTO, BookingFilterModel, IHistoryBookingService>
    {
        public HistoryBookingController(IHistoryBookingService service) : base(service)
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


        [HttpPost("GetCheckPayment")]
        public virtual RespondData GetCheckPayment(BookingFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                return Success(_service.GetCheckPayment(filter));
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
