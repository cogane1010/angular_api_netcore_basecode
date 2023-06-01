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
    public class TransactionSummaryReportRepository : BaseGridRepository<TransactionSummaryReport, TransactionSummaryReportFilterModel>, ITransactionSummaryReportRepository
    {
        private readonly IBaseRepository<Booking> _bookingRepo;
        private readonly IBaseRepository<BookingLine> _bookingLineRepo;
        private readonly IBaseRepository<Customer> _customerRepo;
        private readonly ILogger _log;
        public TransactionSummaryReportRepository(ILogger<TransactionSummaryReportRepository> logger, IUnitOfWork unitOfWork) : base(unitOfWork)
        {

            _log = logger;
            _bookingRepo = _unitOfWork.GetDataRepository<Booking>();
            _bookingLineRepo = _unitOfWork.GetDataRepository<BookingLine>();
            _customerRepo = _unitOfWork.GetDataRepository<Customer>();
        }

        public PagingResponseEntity<TransactionSummaryReport> GetPagingTransactionSummaryReport(TransactionSummaryReportFilterModel pagingModel)
        {
            var paggingData = GetPagingTransactionSummaryReportData(pagingModel);

            var data = new PagingResponseEntity<TransactionSummaryReport>
            {
                Data = paggingData
            };
            data.Count = paggingData.Count();

            return data;
        }

        public IEnumerable<TransactionSummaryReport> GetPagingTransactionSummaryReportData(TransactionSummaryReportFilterModel pagingModel)
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

            var result = _unitOfWork.ExecuteStoredProc<TransactionSummaryReport>("Transaction_summary_Reports", procParams).Result;
            if (result.Any())
            {
                return result;
            }

            return new List<TransactionSummaryReport>();
        }
    }


}
