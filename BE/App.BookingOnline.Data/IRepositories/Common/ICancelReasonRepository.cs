using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.Repositories
{
    public interface ICancelReasonRepository : IGridRepository<CancelReason, CancelReasonPagingModel>
    {
        Task<IEnumerable<CancelReason>> GetCancelReason(CancelReasonPagingModel pagingModel);
        void CancelBookingsFromWeb(IEnumerable<Booking> bookings);
    }
}