using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.Repositories
{
    public interface ITransactionRepository : IBaseGridRepository<InTransactionHeader, TransactionFilterModel>
    {
        Organization GetOrgByCode(string orgCode);
        Task WriteLog(TransactionLogs log);
        PagingResponseEntity<InTransactionDetails> GetDetails(TransactionFilterModel filter);
        OutTransactionHeader GetExcelExportOutData(TransactionFilterModel filter, bool fromApprove);
        PagingResponseEntity<OutTransactionHeader> GetPagingOutFile(TransactionFilterModel filter);
        PagingResponseEntity<OutTransactionHeader> GetPagingOutFileAll(TransactionFilterModel filter);
        IEnumerable<OutTransactionDetails> GetDetailOutFile(TransactionFilterModel filter);
        OutTransactionHeader GetOutTransactionHeaderById(Guid id);
        Task UpdateOutTransactionHeaderStatus(OutTransactionHeader obj);
        IEnumerable<OutTransactionHeader> GetOutTransactionByDate(TransactionFilterModel filter);
        IEnumerable<OutTransactionHeader> GetExcelExportOutDataAll(TransactionFilterModel filter);
        Task ApproveIn(IEnumerable<InTransactionHeader> obj, string userId);
        IEnumerable<OutTransactionHeader> GetOutHeaderForEmail(TransactionFilterModel filter);
        Task AddEmptyOutTransactionHeader(DateTime dateTrans, string userId, string orgId);
        int GetNumOfDayNotCompare(Guid orgId);
        IEnumerable<TransactionNotCompareLine> GetNotCompareDate(TransactionNotCompareFilter filter);
        int GetNumOfDayNotApprove(Guid orgId);
        IEnumerable<TransactionNotApproveLine> GetNotApproveDate(TransactionNotApproveFilter filter);
        Task SaveOutEmailSetting(BookingOutEmailSetting obj);
        Task CreateNewLogFileOutDate(string v);
        IEnumerable<Organization> GetAllOrganization();
        IEnumerable<InTransactionHeader> GetInHeaderForEmail(TransactionFilterModel filter);
        IEnumerable<BookingTransactionPayment> GetDetailOutMoney(TransactionFilterModel filter);
        Task<RespondData> CancelOutData(IEnumerable<BookingTransactionPayment> data, string loginUser, string userId);
        Task<RespondData> ConfirmMoneyData(IEnumerable<BookingTransactionPayment> outTransactionDetails, string loginUser, string userId);
        Task<RespondData> ManualMoneyData(OutTransactionDetails returnData, string loginUser, string userId);
        int GetNumOfDayNotConfirmMoney(Guid guid);
        IEnumerable<BookingTransactionPayment> GetNotConfirmMoneyDate(TransactionNotCompareFilter filter);
        int GetBookingByListCode(IEnumerable<string> enumerable);
        Task<RespondData> CancelConfirmMoneyData(IEnumerable<BookingTransactionPayment> sendata, string userName, string userId);
        IEnumerable<BookingTransactionPayment> ExportExcelConfirmMoneyData(TransactionFilterModel filter);
        Task<FtpTransJobHistory> SaveFtpTransJobHistory(FtpTransJobHistory model);
        PagingResponseEntity<FtpTransJobHistory> GetGetFtpTransJobHistoryPaging(TransactionFilterModel pagingModel);
        bool SetInactivePreviousFileIn(InTransactionHeader saveModel);
        List<DateTime> GetDateNotHaveFileIn();
        OutTransactionHeader CheckTransCompare(DateTime dateTrans);
        OutTransactionHeader GetStatusCompareTrans(TransactionFilterModel filter);
        IQueryable<Booking> GetbookingByCreatedDate(TransactionFilterModel filter);
    }
}
