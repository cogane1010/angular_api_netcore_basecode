using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Reports;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.BookingOnline.Data.Repositories.Common
{
    public class TransactionMonthlyReportRepository : BaseGridRepository<TransactionMonthlyReport, TransactionMonthlyReportFilterModel>, ITransactionMonthlyReportRepository
    {
        private readonly IBaseRepository<Booking> _bookingRepo;
        private readonly IBaseRepository<BookingLine> _bookingLineRepo;
        private readonly IBaseRepository<Customer> _customerRepo;
        private readonly ILogger _log;
        public TransactionMonthlyReportRepository(ILogger<TransactionMonthlyReportRepository> logger, IUnitOfWork unitOfWork) : base(unitOfWork)
        {

            _log = logger;
            _bookingRepo = _unitOfWork.GetDataRepository<Booking>();
            _bookingLineRepo = _unitOfWork.GetDataRepository<BookingLine>();
            _customerRepo = _unitOfWork.GetDataRepository<Customer>();
        }

        public PagingResponseEntity<TransactionMonthlyReport> GetPagingTransactionMonthlyReport(TransactionMonthlyReportFilterModel pagingModel)
        {
            var paggingData = GetPagingTransactionMonthlyReportData(pagingModel);

            var data = new PagingResponseEntity<TransactionMonthlyReport>
            {
                Data = paggingData
            };
            data.Count = paggingData.Count();

            return data;
        }

        public IEnumerable<TransactionMonthlyReport> GetPagingTransactionMonthlyReportData(TransactionMonthlyReportFilterModel pagingModel)
        {
            var fromDate = new DateTime(pagingModel.FilterDate.Value.Year, pagingModel.FilterDate.Value.Month, 1);
            var toDate = new DateTime(pagingModel.FilterDate.Value.AddMonths(1).Year, pagingModel.FilterDate.Value.AddMonths(1).Month, 1).AddDays(-1);
            var procParams = new Dictionary<string, object>()
            {
                {"@UserOrgId", pagingModel.UserOrgId},
                {"@FromDate", fromDate},
                {"@ToDate", toDate},
                {"@UserId", pagingModel.UserId},
                {"@pageIndex", pagingModel.PageIndex},
                {"@pageSize", pagingModel.PageSize}
            };

            var result = _unitOfWork.ExecuteStoredProc<TransactionMonthlyReport>("Transaction_monthly_Reports", procParams).Result;
            if (result.Any())
            {
                return result;
            }

            return new List<TransactionMonthlyReport>();
        }
    }


}
