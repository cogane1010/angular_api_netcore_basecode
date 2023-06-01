using App.BookingOnline.Data;
using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.Core;
using App.Core.Domain;
using App.Core.Helper;
using App.Core.Service;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public class CancelReasonService : BaseGridService<CancelReasonDTO, CancelReason, CancelReasonPagingModel, ICancelReasonRepository>, ICancelReasonService
    {
        public CancelReasonService(ICancelReasonRepository repo) : base(repo)
        {
        }

        public void CancelBookingsFromWeb(IEnumerable<BookingDTO> bookings)
        {
            var booking = AutoMapperHelper.Map<BookingDTO, Booking, IEnumerable<BookingDTO>, IEnumerable<Booking>>(bookings);
            _repo.CancelBookingsFromWeb(booking);
        }

        public async Task<IEnumerable<CancelReasonDTO>> GetCancelReason(CancelReasonPagingModel pagingModel)
        {
            var result = await _repo.GetCancelReason(pagingModel);

            var res = AutoMapperHelper.Map<CancelReason, CancelReasonDTO, IEnumerable<CancelReason>, IEnumerable<CancelReasonDTO>>(result);
            return res;
        }


    }
}

