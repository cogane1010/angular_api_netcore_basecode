using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface ITransactionService : IBaseGridDataService<InTransactionDTO, TransactionFilterModel>
    {
        Task<InTransactionDTO> UploadInputFile(TransactionFilterModel filter);
        string ExportToBase64String(TransactionFilterModel filter);
        PagingResponseEntityDTO<OutTransactionHeaderDTO> GetPagingOutFile(TransactionFilterModel filter);
        PagingResponseEntityDTO<OutTransactionHeaderDTO> GetPagingOutFileAll(TransactionFilterModel filter);
        IEnumerable<OutTransactionDetailDTO> GetDetailOutFile(TransactionFilterModel filter);
        Tuple<string, MemoryStream> ExportExcelFileOut(TransactionFilterModel filter, bool fromApprove = false);
        Tuple<string, MemoryStream> ExportExcelFileOutAll(TransactionFilterModel filter);
        PagingResponseEntityDTO<InTransactionDetailsDTO> GetDetails(TransactionFilterModel filter);
        IEnumerable<PaymentCompare> GetTransactionCompare(TransactionFilterModel filter);
        Task<string> ApproveOut(OutTransactionHeaderDTO entityDTO);
        Task<string> UnApproveOut(OutTransactionHeaderDTO entityDTO);
        Task<string> ApproveIn(TransactionFilterModel filter);
        Task<string> SaveCompare(SaveCompareModel obj);
        Task<string> SendEmailFileOut(TransactionFilterModel filter);
        SettingDTO GetNumOfDayNotCompare(Guid orgId);
        IEnumerable<TransactionNotCompareLineDTO> GetNotCompareDate(TransactionNotCompareFilter filter);

        SettingDTO GetNumOfDayNotApprove(Guid guid);
        IEnumerable<TransactionNotApproveLineDTO> GetNotApproveDate(TransactionNotApproveFilter filter);
        BookingOutEmailSetting GetOutEmailSetting();
        Task TestSendMail(BookingOutEmailSetting obj);
        Task SaveOutEmailSetting(BookingOutEmailSetting obj);
        Task SendFileOutToSeabank();
        IEnumerable<BookingTransactionPayment> GetDetailOutMoney(TransactionFilterModel filter);
        Task<RespondData> CancelOutData(IEnumerable<BookingTransactionPayment> data, string loginUser, string userId);
        Task<RespondData> ConfirmMoneyData(IEnumerable<BookingTransactionPayment> obj, string loginUser, string userId);
        Task<RespondData> ManualMoneyData(OutTransactionDetailDTO data, string loginUser, string userId);
        int GetNumOfDayNotConfirmMoney(Guid guid);
        IEnumerable<NotConfirmMoneyList> GetNotConfirmMoneyDate(TransactionNotCompareFilter filter);
        int GetNotConfirmMoneyDateCount(TransactionNotCompareFilter filter);
        Task<RespondData> CancelConfirmMoneyData(IEnumerable<BookingTransactionPayment> sendata, string userName, string userId);
        Tuple<string, MemoryStream> ExportExcelConfirmMoney(TransactionFilterModel filter);
        Task<RespondData> SaveFileinSeabankData(TransactionFilterModel filter);
        Task<FtpTransJobHistory> SaveFtpTransJobHistory(FtpTransJobHistory model);
        void DownloadFileInViaFtp();
        void DownloadFileTransc(string remoteFilePath, string filename, string userId, DateTime dateTrans);
        string CurrentFolderDoiXoat();
        PagingResponseEntity<FtpTransJobHistory> GetFtpTransJobHistory(TransactionFilterModel pagingModel);
        bool CheckBrgAmin(string userId);
        string CheckTransCompare(DateTime? findDate);
        string GetStatusCompareTrans(TransactionFilterModel filter);
    }
}
