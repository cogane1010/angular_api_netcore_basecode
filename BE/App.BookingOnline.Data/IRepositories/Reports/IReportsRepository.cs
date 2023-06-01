using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Reports;
using App.Core.Domain;
using App.Core.Repositories;
using System.Collections.Generic;

namespace App.BookingOnline.Data.IRepositories.Common
{
    public interface IReportsRepository : IBaseGridRepository<Booking, BookingFilterModel>
    {
        IEnumerable<Customer> GetCustomersInfo(IEnumerable<string> enumerable);
        IEnumerable<Booking> GetExportRBookingData(BookingFilterModel filter);
        PagingResponseEntity<Booking> GetPagingBookingReport(BookingFilterModel filter);
        PagingResponseEntity<ReportCancelBooking> GetPagingCancelReport(BookingFilterModel pagingModel);
        PagingResponseEntity<Customer> GetPagingRegistrationReport(BookingFilterModel filter);
    }
}
