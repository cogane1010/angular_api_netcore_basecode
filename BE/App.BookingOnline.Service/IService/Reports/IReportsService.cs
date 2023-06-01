using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models.Reports;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using System.IO;

namespace App.BookingOnline.Service.IService.Common
{
    public interface IReportsService : IBaseGridDataService<BookingDTO, BookingFilterModel>
    {
        MemoryStream GetExportRBookingExcel(BookingFilterModel filter);
        MemoryStream GetExportRegistrationExcel(BookingFilterModel filter);
        PagingResponseEntityDTO<BookingDTO> GetPagingBookingReport(BookingFilterModel filter);
        PagingResponseEntityDTO<ReportCancelBookingDTO> GetPagingCancelReport(BookingFilterModel pagingModel);
        PagingResponseEntityDTO<CustomerDTO> GetPagingRegistrationReport(BookingFilterModel filter);
    }
}
