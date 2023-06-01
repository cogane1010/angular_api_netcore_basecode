using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Reports;
using App.Core.Domain;
using App.Core.Repositories;

namespace App.BookingOnline.Data.IRepositories.Common
{
    public interface ITransactionSummaryReportRepository : IBaseGridRepository<TransactionSummaryReport, TransactionSummaryReportFilterModel>
    {
        PagingResponseEntity<TransactionSummaryReport> GetPagingTransactionSummaryReport(TransactionSummaryReportFilterModel pagingModel);
    }
}
