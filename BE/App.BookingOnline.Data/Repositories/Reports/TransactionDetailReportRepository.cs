using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace App.BookingOnline.Data.Repositories.Common
{
    public class TransactionDetailReportRepository : BaseGridRepository<TransactionDetailReport, TransactionDetailReportFilterModel>, ITransactionDetailReportRepository
    {
        private readonly IBaseRepository<Booking> _bookingRepo;
        private readonly IBaseRepository<BookingLine> _bookingLineRepo;
        private readonly IBaseRepository<Customer> _customerRepo;
        private readonly ILogger _log;
        public TransactionDetailReportRepository(ILogger<TransactionDetailReportRepository> logger, IUnitOfWork unitOfWork) : base(unitOfWork)
        {

            _log = logger;
            _bookingRepo = _unitOfWork.GetDataRepository<Booking>();
            _bookingLineRepo = _unitOfWork.GetDataRepository<BookingLine>();
            _customerRepo = _unitOfWork.GetDataRepository<Customer>();
        }

        public PagingResponseEntity<TransactionDetailReport> GetPagingTransactionDetailReport(TransactionDetailReportFilterModel pagingModel)
        {
            var paggingData = GetPagingTransactionDetailReportData(pagingModel);

            var data = new PagingResponseEntity<TransactionDetailReport>
            {
                Data = paggingData
            };
            data.Count = paggingData.Count();

            return data;
        }

        public IEnumerable<TransactionDetailReport> GetPagingTransactionDetailReportData(TransactionDetailReportFilterModel pagingModel)
        {
            var procParams = new Dictionary<string, object>()
            {
                {"@UserOrgId", pagingModel.UserOrgId},
                {"@FromDate", pagingModel.FromDate},
                {"@ToDate", pagingModel.ToDate},
                {"@UserId", pagingModel.UserId},
                {"@pageIndex", pagingModel.PageIndex},
                {"@pageSize", pagingModel.PageSize}
            };

            var result = _unitOfWork.ExecuteStoredProc<TransactionDetailReport>("Transaction_Detail_Reports", procParams).Result;
            if (result.Any())
            {
                return result;
            }

            return new List<TransactionDetailReport>();
        }
    }

   
}
