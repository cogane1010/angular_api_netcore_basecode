using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace App.BookingOnline.Api.Controllers
{

    public class CancelReasonController : BaseGridController<CancelReasonDTO, CancelReasonPagingModel, ICancelReasonService>
    {
        protected IBookingService _bookingService;
        public CancelReasonController(ICancelReasonService service, IBookingService bookingService) : base(service)
        {
            _bookingService = bookingService;
        }

        [HttpPost("GetCancelReason")]
        public async Task<RespondData> GetCancelReason()
        {
            try
            {
                CancelReasonPagingModel filter = new CancelReasonPagingModel();
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                return Success(await _service.GetCancelReason(filter));
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

        [HttpPost("CancelBookingsFromWeb")]
        public RespondData CancelBookingsFromWeb(IEnumerable<BookingDTO> bookings)
        {
            try
            {
                foreach (var item in bookings)
                {
                    var data = _bookingService.CancelBooking(item.Id, item.UserId, true, item.BookingCode, UserId, item.Cancel_Reason_Id.ToString(), item.Cancel_Description);
                    if (!data.IsSuccess)
                    {
                        return Failure("", "Mã đặt vé này quá thời gian hủy");
                    }
                }

                return Success();
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }
    }



}