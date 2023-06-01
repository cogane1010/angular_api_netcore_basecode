using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO.Reports;
using App.Core.Domain;
using System;
using System.IO;

namespace App.BookingOnline.Service.IService.Common
{
    public interface ITransactionSummaryReportService : IBaseGridDataService<TransactionSummaryReportDTO, TransactionSummaryReportFilterModel>
    {
        TransactionSummaryReportResult GetReportData(TransactionSummaryReportFilterModel pagingModel);
        Tuple<MemoryStream, string> ExportExcel(TransactionSummaryReportFilterModel filter);
    }
}
