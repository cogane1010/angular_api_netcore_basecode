using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models.Reports;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using System;
using System.IO;

namespace App.BookingOnline.Service.IService.Common
{
    public interface ITransactionDetailReportService : IBaseGridDataService<TransactionDetailReportDTO, TransactionDetailReportFilterModel>
    {
        TransactionDetailReportResult GetReportData(TransactionDetailReportFilterModel pagingModel);
        Tuple<MemoryStream, string> ExportExcel(TransactionDetailReportFilterModel filter);
    }
}
