using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models.Reports;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using System;
using System.IO;

namespace App.BookingOnline.Service.IService.Common
{
    public interface ITransactionMonthlyReportService : IBaseGridDataService<TransactionMonthlyReportDTO, TransactionMonthlyReportFilterModel>
    {
        TransactionMonthlyReportResult GetReportData(TransactionMonthlyReportFilterModel pagingModel);
        Tuple<MemoryStream, string> ExportExcel(TransactionMonthlyReportFilterModel filter);
    }
}
