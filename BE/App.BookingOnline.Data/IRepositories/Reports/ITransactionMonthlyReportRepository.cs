using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Reports;
using App.Core.Domain;
using App.Core.Repositories;

namespace App.BookingOnline.Data.IRepositories.Common
{
    public interface ITransactionMonthlyReportRepository : IBaseGridRepository<TransactionMonthlyReport, TransactionMonthlyReportFilterModel>
    {
        PagingResponseEntity<TransactionMonthlyReport> GetPagingTransactionMonthlyReport(TransactionMonthlyReportFilterModel pagingModel);
    }
}
