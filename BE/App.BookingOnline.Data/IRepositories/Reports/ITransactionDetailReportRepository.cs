using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Models;
using App.Core.Domain;
using App.Core.Repositories;

namespace App.BookingOnline.Data.IRepositories.Common
{
    public interface ITransactionDetailReportRepository : IBaseGridRepository<TransactionDetailReport, TransactionDetailReportFilterModel>
    {
        PagingResponseEntity<TransactionDetailReport> GetPagingTransactionDetailReport(TransactionDetailReportFilterModel pagingModel);
    }
}
