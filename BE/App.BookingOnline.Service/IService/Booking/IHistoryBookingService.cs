using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface IHistoryBookingService : IBaseGridDataService<BookingDTO, BookingFilterModel>
    {
        IEnumerable<PaymentCompare> GetCheckPayment(BookingFilterModel filter);
    }
}
