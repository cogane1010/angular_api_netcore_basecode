using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using static App.Core.Enums;
using System.Threading.Tasks;
using App.Core.Domain;
using Newtonsoft.Json;
using Serilog.Context;
using Microsoft.Extensions.Logging;
using IdentityServer4.Extensions;
using System.Collections.Immutable;
using App.Core;

namespace App.BookingOnline.Data.Repositories
{
    public class TransactionRepository : BaseGridRepository<InTransactionHeader, TransactionFilterModel>
        , ITransactionRepository
    {
        protected readonly BookingOnlineDbContext _context;
        private readonly IBaseRepository<Organization> _organizationRepo;
        private readonly IBaseRepository<TransactionLogs> _transactionLogsRepo;
        private readonly IBaseRepository<InTransactionDetails> _inDetailsRepo;
        private readonly IBaseRepository<OutTransactionHeader> _outTransHeaderRepo;
        private readonly IBaseRepository<OutTransactionHeader> _outTransactionHeaderRepo;
        private readonly IBaseRepository<OutTransactionDetails> _outTransactionDetailsRepo;
        private readonly ILogger _log;
        private readonly ISettingRepository _settingRepo;
        private readonly ILogDateFileOutRepository _logDateFileOutRepo;
        private readonly IBaseRepository<Booking> _bookingRepo;
        private readonly IBaseRepository<BookingTransactionPayment> _transactionPaymentRepo;
        private readonly IBaseRepository<FtpTransJobHistory> _ftpTransJobHistoryRepo;
        public TransactionRepository(IUnitOfWork unitOfWork, BookingOnlineDbContext context, ILogger<BookingRepository> logger,
            ISettingRepository settingRepo, ILogDateFileOutRepository logDateFileOutRepo) : base(unitOfWork)
        {
            _context = context;
            _organizationRepo = _unitOfWork.GetDataRepository<Organization>();
            _transactionLogsRepo = _unitOfWork.GetDataRepository<TransactionLogs>();
            _inDetailsRepo = _unitOfWork.GetDataRepository<InTransactionDetails>();
            _outTransHeaderRepo = _unitOfWork.GetDataRepository<OutTransactionHeader>();
            _outTransactionHeaderRepo = _unitOfWork.GetDataRepository<OutTransactionHeader>();
            _outTransactionDetailsRepo = _unitOfWork.GetDataRepository<OutTransactionDetails>();
            _bookingRepo = _unitOfWork.GetDataRepository<Booking>();
            _transactionPaymentRepo = _unitOfWork.GetDataRepository<BookingTransactionPayment>();
            _log = logger;
            _settingRepo = settingRepo;
            _logDateFileOutRepo = logDateFileOutRepo;
            _ftpTransJobHistoryRepo = _unitOfWork.GetDataRepository<FtpTransJobHistory>();
        }

        #region Query
        public Organization GetOrgByCode(string orgCode)
        {
            return _organizationRepo.SelectWhere(x => x.Code == orgCode).FirstOrDefault();
        }

        public override PagingResponseEntity<InTransactionHeader> GetPaging(TransactionFilterModel pagingModel)
        {
            var dataOut = _repo.SelectWhere(x => x.DateTrans.Date == pagingModel.FindDate.Value.Date).OrderByDescending(o => o.CreatedDate);

            if (pagingModel.FindDate.HasValue)
            {
                var data = new PagingResponseEntity<InTransactionHeader>
                {
                    Data = dataOut.Skip(pagingModel.PageIndex * pagingModel.PageSize).Take(pagingModel.PageSize)
                };
                data.Count = dataOut.Count();
                return data;
            }
            return new PagingResponseEntity<InTransactionHeader>();
        }

        public PagingResponseEntity<InTransactionDetails> GetDetails(TransactionFilterModel pagingModel)
        {
            if (!string.IsNullOrWhiteSpace(pagingModel.HeaderId))
            {
                var findId = Guid.Empty;
                Guid.TryParse(pagingModel.HeaderId, out findId);

                if (findId != Guid.Empty)
                {
                    var dataIn = _inDetailsRepo.SelectWhere(x => x.InTransactionHeaderId == findId);
                    var data = new PagingResponseEntity<InTransactionDetails>
                    {
                        Data = dataIn.Skip(pagingModel.PageIndex * pagingModel.PageSize).Take(pagingModel.PageSize).ToList()
                    };
                    data.Count = dataIn.Count();
                    return data;
                }
            }
            return new PagingResponseEntity<InTransactionDetails>();
        }

        public OutTransactionHeader GetExcelExportOutData(TransactionFilterModel filter, bool fromApprove)
        {
            #region Get by orgId
            //var orgId = Guid.Empty;
            //Guid.TryParse(filter.UserOrgId, out orgId);
            //if (orgId != Guid.Empty)
            //{
            //    var res = _outTransHeaderRepo.AsQueryable().Where(x => x.DateTrans.Date == filter.FindDate.Date && x.OrganizationId == orgId
            //           && x.Status == TransBookStatus.outApprove.ToString()).Include(h => h.OutTransactionDetails).FirstOrDefault();
            //    return res;
            //} 
            #endregion

            // goi tu ham duyet file out: lay du lieu de luu ra file excel
            if (fromApprove)
            {
                var res = _outTransHeaderRepo.AsQueryable().Where(x => x.IsActive && x.Id == filter.OutTransHeaderId
                            && x.Status == TransBookStatus.created.ToString())
                    .Include(h => h.OutTransactionDetails.Where(i => !string.IsNullOrEmpty(i.Rc_code) && i.UserRc_code != "0"))
                    .Include(h => h.Organization).FirstOrDefault();
                return res;
            }
            else // goi tu ham export excel file out da duyet
            {
                var res = _outTransHeaderRepo.AsQueryable().Where(x => x.Id == filter.OutTransHeaderId && x.Status == TransBookStatus.outApprove.ToString())
                    .Include(h => h.OutTransactionDetails.Where(i => !string.IsNullOrEmpty(i.Rc_code) && i.UserRc_code != "0"))
                    .Include(h => h.Organization).FirstOrDefault();
                return res;
            }
        }

        public PagingResponseEntity<OutTransactionHeader> GetPagingOutFile(TransactionFilterModel filter)
        {
            var dataOut = _outTransactionHeaderRepo.SelectWhere(x => x.DateTrans.Date >= filter.FromDate.Date && x.DateTrans.Date <= filter.ToDate.Date
                            && x.OrganizationId == new Guid(filter.UserOrgId) && x.IsActive);

            var data = new PagingResponseEntity<OutTransactionHeader>
            {
                Data = dataOut.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize).ToList()
            };
            data.Count = dataOut.Count();

            return data;
        }

        public PagingResponseEntity<OutTransactionHeader> GetPagingOutFileAll(TransactionFilterModel filter)
        {
            var dataOut = _outTransactionHeaderRepo.SelectWhere(x => x.DateTrans.Date == filter.FindDate.Value.Date
                            && x.IsActive && x.Status == TransBookStatus.outApprove.ToString());

            var data = new PagingResponseEntity<OutTransactionHeader>
            {
                Data = dataOut.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize).ToList()
            };
            data.Count = dataOut.Count();
            return data;
        }

        public IEnumerable<OutTransactionDetails> GetDetailOutFile(TransactionFilterModel filter)
        {
            return _outTransactionDetailsRepo.SelectWhere(x => x.OutTransactionHeaderId == filter.OutTransHeaderId
                    && (!string.IsNullOrEmpty(x.Rc_code) || (!string.IsNullOrEmpty(x.Rc_code) && x.UserRc_code != "0")));
        }

        public object GetInDataTransaction(object dateTrans)
        {
            throw new NotImplementedException();
        }

        public object GetbookingPaymentTrans(object dateTrans)
        {
            throw new NotImplementedException();
        }

        public OutTransactionHeader GetOutTransactionHeaderById(Guid id)
        {
            return _outTransactionHeaderRepo.SelectWhere(x => x.Id == id).Include(x => x.Organization)
                .Include(i => i.OutTransactionDetails).FirstOrDefault();
        }

        public IEnumerable<OutTransactionHeader> GetOutTransactionByDate(TransactionFilterModel filter)
        {
            return _outTransactionHeaderRepo.SelectWhere(x => x.DateTrans.Date == filter.FindDate.Value.Date && x.IsActive).Include(x => x.OutTransactionDetails);
        }

        public IEnumerable<OutTransactionHeader> GetExcelExportOutDataAll(TransactionFilterModel filter)
        {
            var res = _outTransHeaderRepo.AsQueryable().Where(x => x.DateTrans.Date == filter.FindDate.Value.Date
            && x.Status == TransBookStatus.outApprove.ToString() && x.IsActive)
                .Include(h => h.OutTransactionDetails.Where(i => i.StatusLine == (int)StatusTransLine.NotMap)).Include(h => h.Organization).ToList();
            return res;
        }

        public IEnumerable<OutTransactionHeader> GetOutHeaderForEmail(TransactionFilterModel filter)
        {
            if (filter.EmailFilterDates != null && filter.EmailFilterDates.Count > 0)
            {
                var res = _outTransHeaderRepo.SelectWhere(x => filter.EmailFilterDates.Contains(x.DateTrans.Date)
                && x.Status == TransBookStatus.outApprove.ToString() && x.IsActive)
                    .Include(h => h.OutTransactionDetails.Where(i => i.StatusLine == (int)StatusTransLine.NotMap))
                    .Include(h => h.Organization).ToList();
                return res;
            }
            return null;
        }

        public int GetNumOfDayNotCompare(Guid orgId)
        {
            var filter = new TransactionNotCompareFilter { UserOrgId = orgId.ToString() };
            var res = GetNotCompareDate(filter);
            return res != null ? res.Count() : 0;
        }

        public IEnumerable<TransactionNotCompareLine> GetNotCompareDate(TransactionNotCompareFilter filter)
        {
            var guidOrgId = new Guid(filter.UserOrgId);
            //var res = (from sb_dti in _inDetailsRepo.AsQueryable()
            //           where !_outTransactionHeaderRepo.AsQueryable().Any(sbo => sbo.DateTrans.Date == sb_dti.Trans_Time.Date && sbo.OrganizationId == guidOrgId)
            //               && sb_dti.OrganizationId == guidOrgId //&& 
            //           group sb_dti by sb_dti.Trans_Time.Date into newGroup
            //           orderby newGroup.Key
            //           select new TransactionNotCompareLine() { TransDate = newGroup.Key }).ToList();
            var res = _inDetailsRepo.SelectWhere(x => x.IsActive && x.OrganizationId == guidOrgId
                        && !_outTransactionHeaderRepo.AsQueryable().Any(sbo => sbo.DateTrans.Date == x.Trans_Time.Date && sbo.OrganizationId == guidOrgId
                                                                        && sbo.IsActive))
                        .GroupBy(g => g.Trans_Time.Date).OrderBy(o => o.Key)
                        .Select(s => new TransactionNotCompareLine() { TransDate = s.Key });

            return res;
        }

        public int GetNumOfDayNotApprove(Guid orgId)
        {
            //var res = (from sbo in _outTransactionHeaderRepo.AsQueryable()
            //           join sb_dto in _outTransactionDetailsRepo.AsQueryable() on sbo.Id equals sb_dto.OutTransactionHeaderId
            //           where sbo.OrganizationId == orgId && sbo.IsActive && sbo.Status == TransBookStatus.created.ToString()
            //           select new { TransDate = sbo.DateTrans.Date }).Distinct().ToList();
            var res = _outTransactionHeaderRepo.SelectWhere(s => s.IsActive && s.OrganizationId == orgId
                        && s.Status == TransBookStatus.created.ToString()).Select(s => s.DateTrans).Distinct();
            return res != null ? res.Count() : 0;
        }

        public IEnumerable<TransactionNotApproveLine> GetNotApproveDate(TransactionNotApproveFilter filter)
        {
            var guidOrgId = new Guid(filter.UserOrgId);
            var res = (from sbo in _outTransactionHeaderRepo.AsQueryable()
                       join sb_dto in _outTransactionDetailsRepo.AsQueryable() on sbo.Id equals sb_dto.OutTransactionHeaderId
                       where sbo.OrganizationId == guidOrgId && sbo.IsActive && sbo.Status == TransBookStatus.created.ToString()
                       group sbo by sbo.DateTrans.Date into newGroup
                       orderby newGroup.Key
                       select new TransactionNotApproveLine() { TransDate = newGroup.Key }).ToList();
            return res;
        }

        public IEnumerable<Organization> GetAllOrganization()
        {
            return _organizationRepo.GetAll();
        }

        public IEnumerable<InTransactionHeader> GetInHeaderForEmail(TransactionFilterModel filter)
        {
            if (filter.EmailFilterDates != null && filter.EmailFilterDates.Count > 0)
            {
                var res = _repo.AsQueryable().Where(x => filter.EmailFilterDates.Contains(x.DateTrans.Date) && x.Status == TransBookStatus.inApprove.ToString()
                            && x.IsActive)
                    .Include(h => h.InTransactionDetails).ToList();
                return res;
            }
            return null;
        }
        #endregion

        #region CUD
        public override async ValueTask<InTransactionHeader> AddAsync(InTransactionHeader entityDTo)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var res = await _repo.AddAsync(entityDTo);

                    // Tạo log ngày giao dịch
                    await CreateNewLogFileOutDate(entityDTo.DateTrans);

                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                    return res;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task WriteLog(TransactionLogs log)
        {
            await _transactionLogsRepo.AddAsync(log);
        }

        public async Task UpdateOutTransactionHeaderStatus(OutTransactionHeader obj)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _outTransactionHeaderRepo.Update(obj);

                    var log = new TransactionLogs()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedUser = obj.IsActive ? obj.ApproverUserId : obj.UpdatedUser,
                        DateTrans = DateTime.Now,
                        FileName = "",
                        LogText = JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        LogName = obj.IsActive ? "Approve out Transaction" : "Unapprove out Transaction"
                    };
                    await _transactionLogsRepo.AddAsync(log);

                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task ApproveIn(IEnumerable<InTransactionHeader> inTransHeaders, string userId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    foreach (var inHdr in inTransHeaders)
                    {
                        Update(inHdr);
                        var log = new TransactionLogs()
                        {
                            CreatedDate = DateTime.Now,
                            CreatedUser = userId,
                            DateTrans = DateTime.Now,
                            FileName = "",
                            LogText = JsonConvert.SerializeObject(inHdr),
                            LogName = "Approve In Transaction"
                        };
                        await _transactionLogsRepo.AddAsync(log);
                    }
                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task AddEmptyOutTransactionHeader(DateTime dateTrans, string userId, string orgId)
        {
            try
            {
                var guidOrg = new Guid(orgId);

                var org = _organizationRepo.SingleOrDefault(x => x.Id == guidOrg);
                var fileName = string.Format("OUT_SB_{0}_{1}", org != null ? org.Code : orgId, dateTrans.ToString("yyyyMMdd"));

                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    #region inactive dữ liệu out trong ngày
                    var headers = _outTransactionHeaderRepo.SelectWhere(x => x.DateTrans.Date == dateTrans.Date && x.OrganizationId == guidOrg)
                        .Include(x => x.OutTransactionDetails.Where(i => i.StatusLine == (int)StatusTransLine.NotMap)).ToList();
                    if (headers != null && headers.Any())
                    {
                        for (int i = 0; i < headers.Count; i++)
                        {
                            var item = headers[i];
                            if (item.OutTransactionDetails != null && item.OutTransactionDetails.Any())
                            {
                                var lines = item.OutTransactionDetails.ToList();
                                for (int j = 0; j < lines.Count; j++)
                                {
                                    var line = lines[j];
                                    line.IsActive = false;
                                    line.UpdatedDate = DateTime.Now;
                                    line.UpdatedUser = userId;
                                    _outTransactionDetailsRepo.Update(line);
                                }
                            }
                            item.IsActive = false;
                            item.UpdatedDate = DateTime.Now;
                            item.UpdatedUser = userId;
                            _outTransactionHeaderRepo.Update(item);
                            var logInActiveHeader = new TransactionLogs()
                            {
                                CreatedDate = DateTime.Now,
                                CreatedUser = userId,
                                DateTrans = DateTime.Now,
                                FileName = "",
                                LogText = JsonConvert.SerializeObject(item, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                                LogName = "Inactive out Transaction"
                            };
                            await _transactionLogsRepo.AddAsync(logInActiveHeader);
                        }
                    }
                    #endregion

                    var outHeader = new OutTransactionHeader
                    {
                        FileName = fileName,
                        DateTrans = dateTrans.Date,
                        CreatedDate = DateTime.Now,
                        CreatedUser = userId,
                        OrganizationId = new Guid(orgId),
                        Status = TransBookStatus.created.ToString(),
                        IsActive = true
                    };
                    outHeader = await _outTransactionHeaderRepo.AddAsync(outHeader);
                    var logHeader = new TransactionLogs()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedUser = userId,
                        DateTrans = DateTime.Now,
                        FileName = "",
                        LogText = JsonConvert.SerializeObject(outHeader, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                        LogName = "create out Transaction"
                    };
                    await _transactionLogsRepo.AddAsync(logHeader);

                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
            }
        }

        public async Task SaveOutEmailSetting(BookingOutEmailSetting obj)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await DoSaveOutEmailSetting("transaction_out_email_to", obj.transaction_out_email_to);
                await DoSaveOutEmailSetting("transaction_out_email_bcc", obj.transaction_out_email_bcc);
                await DoSaveOutEmailSetting("transaction_out_email_cc", obj.transaction_out_email_cc);
                await DoSaveOutEmailSetting("transaction_out_email_dow", obj.transaction_out_email_dow);
                await DoSaveOutEmailSetting("transaction_out_email_hour", obj.transaction_out_email_hour);
                await DoSaveOutEmailSetting("transaction_in_file_hour", obj.HourGetFileIn);

                _unitOfWork.SaveChanges();
                transaction.Commit();
            }
        }

        private async Task DoSaveOutEmailSetting(string code, string value)
        {
            var sett = _settingRepo.Find(x => x.Code == code).FirstOrDefault();
            if (sett != null)
            {
                sett.Value = value;
                _settingRepo.Update(sett);
            }
            else
            {
                var newSett = new Setting() { Value = value, Code = code, CreatedDate = DateTime.Now };
                await _settingRepo.AddAsync(newSett);
            }
        }

        public async Task CreateNewLogFileOutDate(string fileType)
        {
            var logDateFileOut = _logDateFileOutRepo.Find(x => x.Date.Date == DateTime.Today).FirstOrDefault();
            // Chưa có trong bảng log -> Thêm vào bảng log
            if (logDateFileOut == null)
            {
                var logDate = new LogDateFileOut()
                {
                    CreatedDate = DateTime.Now,
                    CreatedUser = "system",
                    Date = DateTime.Today,
                    FileType = fileType,
                    Status = LogDateFileOutStatus.created.ToString()
                };
                await _logDateFileOutRepo.AddAsync(logDate);
            }
        }

        public async Task CreateNewLogFileOutDate(DateTime transDate)
        {
            var logDateFileOut = _logDateFileOutRepo.Find(x => x.Date.Date == transDate.Date).FirstOrDefault();
            // Chưa có trong bảng log -> Thêm vào bảng log
            if (logDateFileOut == null)
            {
                var logDate = new LogDateFileOut()
                {
                    CreatedDate = DateTime.Now,
                    CreatedUser = "system",
                    Date = transDate.Date,
                    FileType = FileType.Out.ToString(),
                    Status = LogDateFileOutStatus.created.ToString()
                };
                await _logDateFileOutRepo.AddAsync(logDate);
            }
        }

        public IEnumerable<BookingTransactionPayment> GetDetailOutMoney(TransactionFilterModel filter)
        {
            var outData = _outTransactionDetailsRepo.SelectWhere(x => x.IsActive && x.Trans_Time.Date == filter.FindDate.Value.Date
                                                                           && x.OrganizationId == new Guid(filter.UserOrgId)).ToList();

            var data = _transactionPaymentRepo.SelectWhere(x => x.IsActive && x.OrgId == new Guid(filter.UserOrgId)
                                 && x.DatePayment.Date == filter.FindDate.Value.Date
                                 && (x.Status == StatusBookingLine.booked.ToString() || x.Status == StatusBookingLine.cancel.ToString()
                                     || x.Status == StatusTransLine.MoneyGoAcc.ToString()))
                                .Include(i => i.Booking).ThenInclude(t => t.BookingLines)
                                .Include(i => i.Booking).ThenInclude(t => t.AppUser)
                                .Include(i => i.Organization)
                                .Where(a => string.IsNullOrEmpty(filter.Status) || a.Status == filter.Status)
                              .Select(
                                    s => new BookingTransactionPayment
                                    {
                                        Id = s.Id,
                                        Ftid = s.Ftid,
                                        OrgCode = s.Organization.Code,
                                        CustomerId = s.CustomerId,
                                        CustomerName = s.EmbosingName,
                                        BookingCode = s.Booking.BookingCode,
                                        DatePayment = s.DatePayment,
                                        SourceType = s.SourceType,
                                        TotalMoney = s.TotalMoney,
                                        CancelStatus = s.CancelStatus,
                                        CancelTime = s.CancelTime,
                                        SdkRefundMoney = s.SdkRefundMoney,
                                        InMoneyDate = s.InMoneyDate,
                                        OutMoneyDate = s.OutMoneyDate,
                                        TotalMoneyAcc = s.TotalMoneyAcc,
                                        InMoneyUser = s.InMoneyUser,
                                        OutMoneyUser = s.OutMoneyUser,
                                        Booking = s.Booking,
                                        CardFullName = s.Booking.AppUser.FullName,
                                        CreditAcc = s.CreditAcc ?? s.CardNumber,
                                        CreatedDate = s.CreatedDate,
                                        CreatedUser = s.CreatedUser,
                                        UpdatedDate = s.UpdatedDate,
                                        UpdatedUser = s.UpdatedUser
                                    }).ToList();

            var moneyData = new List<BookingTransactionPayment>();

            foreach (var s in data)
            {
                s.StatusLine = outData.FirstOrDefault(o => o.TraceId == s.BookingCode) == null ? (s.CancelStatus == ((int)StatusTransLine.CancelLine).ToString()
                                        ? (int)StatusTransLine.CancelLine : (int)StatusTransLine.NotMap)
                                            : outData.FirstOrDefault(o => o.TraceId == s.BookingCode).StatusLine;
                s.UserRc_code = outData.FirstOrDefault(o => o.TraceId == s.BookingCode) == null ? null
                                : outData.FirstOrDefault(o => o.TraceId == s.BookingCode).UserRc_code;
                s.Trans_type = "1";

                if (s.TotalMoneyAcc != null)
                {
                    s.StatusLine = (int)StatusTransLine.MoneyGoAcc;
                    s.MoneyAtAcc = s.InMoneyDate;
                    s.StatusOrder = 1;
                }
                if (s.UserRc_code == "0")
                {
                    s.TotalMoneyAcc = s.TotalMoney;
                    s.MoneyAtAcc = s.DatePayment;
                }

                s.TotalTeetimeDisplay = string.Join(",", s.Booking.BookingLines.SelectMany(e => e.Tee_Time.Value.ToString("HH:mm").Distinct()));
                s.PlayerCount = s.Booking.BookingLines.Count();
                moneyData.Add(s);

                if (s.CancelTime.HasValue)
                {
                    if (s.CancelStatus != ((int)CancelStatus.notRefund).ToString())
                    {
                        var newCancel = new BookingTransactionPayment();
                        newCancel.Id = s.Id;
                        newCancel.OrgCode = s.OrgCode;
                        newCancel.CustomerId = s.CustomerId;
                        newCancel.CustomerName = s.CustomerName;
                        newCancel.BookingCode = s.BookingCode;
                        newCancel.DatePayment = s.DatePayment;
                        newCancel.SourceType = s.SourceType;
                        newCancel.TotalMoney = s.TotalMoney;
                        newCancel.CancelStatus = s.CancelStatus;
                        newCancel.CancelTime = s.CancelTime;
                        newCancel.StatusLine = s.StatusLine;
                        newCancel.SdkRefundMoney = s.SdkRefundMoney;
                        newCancel.InMoneyUser = s.OutMoneyUser;
                        newCancel.Booking = s.Booking;
                        newCancel.CardFullName = s.CardFullName;
                        newCancel.Trans_type = "0";
                        newCancel.TotalMoneyAcc = -s.SdkRefundMoney;
                        newCancel.Ftid = s.TransactionNo;
                        newCancel.CreditAcc = s.CreditAcc;
                        newCancel.CreatedDate = s.CreatedDate;
                        newCancel.CreatedUser = s.CreatedUser;
                        newCancel.UpdatedDate = s.UpdatedDate;
                        newCancel.UpdatedUser = s.UpdatedUser;

                        if (s.SdkRefundMoney.HasValue)
                        {
                            newCancel.StatusLine = (int)StatusTransLine.MoneyOutAcc;
                        }
                        else
                        {
                            newCancel.StatusLine = (int)StatusTransLine.RefundMoney;
                        }
                        if (s.OutMoneyDate.HasValue)
                        {
                            newCancel.MoneyAtAcc = s.OutMoneyDate;
                        }
                        else
                        {
                            newCancel.MoneyAtAcc = s.CancelTime;
                        }

                        newCancel.StatusLine = outData.FirstOrDefault(o => o.TraceId == s.BookingCode) == null ? (s.CancelStatus == ((int)StatusTransLine.CancelLine).ToString()
                                       ? (int)StatusTransLine.CancelLine : (int)StatusTransLine.NotMap)
                                           : outData.FirstOrDefault(o => o.TraceId == s.BookingCode).StatusLine;
                        newCancel.UserRc_code = outData.FirstOrDefault(o => o.TraceId == s.BookingCode) == null ? null
                                        : outData.FirstOrDefault(o => o.TraceId == s.BookingCode).UserRc_code;


                        moneyData.Add(newCancel);
                    }
                }
            }

            return moneyData;
        }

        public async Task<RespondData> CancelOutData(IEnumerable<BookingTransactionPayment> data, string loginUser, string userId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    foreach (var i in data)
                    {
                        var transData = _transactionPaymentRepo.SelectWhere(x => x.Id == i.Id).FirstOrDefault();
                        if (transData != null)
                        {
                            transData.Status = StatusTransLine.CancelLine.ToString();

                            transData.CancelTime = DateTime.Now;
                            transData.CancelUer = userId;
                            transData.InMoneyUser = loginUser;
                            transData.TotalMoneyAcc = null;
                            transData.InMoneyDate = null;
                            transData.UpdatedDate = DateTime.Now;
                            transData.UpdatedUser = userId;
                            _transactionPaymentRepo.Update(transData);

                            var outData = _outTransactionDetailsRepo.SelectWhere(x => x.FT_Id == transData.Ftid).FirstOrDefault();
                            if (outData != null)
                            {
                                outData.StatusLine = (int)StatusTransLine.CancelLine;
                                outData.UpdatedDate = new DateTime();
                                outData.UpdatedUser = loginUser;
                                _outTransactionDetailsRepo.Update(outData);
                            }
                            WritelogPayment(transData, userId);
                        }
                    }
                    transaction.Commit();
                    return new RespondData { IsSuccess = true, Data = data };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        private void WritelogPayment(BookingTransactionPayment transData, string userId)
        {
            var item = new TransactionLogs();
            item.FileName = transData.Id.ToString();
            item.LogName = "BookingTransactionPayment";
            item.LogText = JsonConvert.SerializeObject(transData);
            item.CreatedDate = DateTime.Now;
            item.CreatedUser = userId;
            _transactionLogsRepo.AddAsync(item);
        }
        private TransactionLogs GetSecondLastlogPayment(BookingTransactionPayment transData, string userId)
        {
            return _transactionLogsRepo.SelectWhere(x => x.FileName == transData.Id.ToString() && x.LogName == "BookingTransactionPayment")
                        .OrderByDescending(o => o.CreatedDate).Take(2).LastOrDefault();
        }

        public async Task<RespondData> ConfirmMoneyData(IEnumerable<BookingTransactionPayment> data, string loginUser, string userId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    foreach (var i in data)
                    {
                        var transData = _transactionPaymentRepo.SelectWhere(x => x.Id == i.Id).FirstOrDefault();
                        if (transData != null)
                        {
                            if (i.Trans_type == "1")
                            {
                                transData.Status = StatusTransLine.MoneyGoAcc.ToString();
                                transData.InMoneyUser = loginUser;
                                transData.TotalMoneyAcc = i.TotalMoneyAcc;
                                transData.InMoneyDate = i.MoneyAtAcc;
                            }
                            else
                            {
                                transData.Status = StatusTransLine.MoneyOutAcc.ToString();
                                transData.InMoneyUser = loginUser;
                                transData.OutMoneyAcc = i.TotalMoneyAcc;
                                transData.OutMoneyDate = i.MoneyAtAcc;
                            }
                            transData.UpdatedDate = DateTime.Now;
                            transData.UpdatedUser = userId;
                            _transactionPaymentRepo.Update(transData);

                            if (i.Trans_type == "1")
                            {
                                var outData = _outTransactionDetailsRepo.SelectWhere(x => x.FT_Id == transData.Ftid).FirstOrDefault();
                                if (outData != null)
                                {
                                    outData.StatusLine = (int)StatusTransLine.MoneyGoAcc;
                                    outData.UpdatedDate = DateTime.Now;
                                    outData.UpdatedUser = userId;
                                    _outTransactionDetailsRepo.Update(outData);
                                }
                            }
                            else
                            {
                                var outData = _outTransactionDetailsRepo.SelectWhere(x => x.Refund_Ref == transData.Ftid).FirstOrDefault();
                                if (outData != null)
                                {
                                    outData.StatusLine = (int)StatusTransLine.MoneyOutAcc;
                                    outData.UpdatedDate = DateTime.Now;
                                    outData.UpdatedUser = userId;
                                    _outTransactionDetailsRepo.Update(outData);
                                }
                            }
                            WritelogPayment(transData, userId);
                        }
                    }

                    transaction.Commit();
                    return new RespondData { IsSuccess = true, Data = data };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public async Task<RespondData> ManualMoneyData(OutTransactionDetails returnData, string userName, string userId)
        {
            try
            {
                var transData = _transactionPaymentRepo.SelectWhere(x => x.Id == returnData.Id && x.IsActive).FirstOrDefault();
                if (transData != null)
                {
                    WritelogPayment(transData, userId);

                    transData.CustomerId = returnData.CustomerId;
                    transData.CustomerName = returnData.CustomerName;
                    transData.CreditAcc = returnData.CreditAcc;
                    transData.SourceType = returnData.SourceType;
                    transData.TotalMoneyAcc = returnData.TotalMoneyAcc;
                    transData.InMoneyDate = returnData.MoneyAtAcc;
                    transData.InMoneyUser = userName;
                    transData.StatusLine = (int)StatusTransLine.MoneyGoAcc;
                    transData.UpdatedDate = DateTime.Now;
                    transData.UpdatedUser = userId;
                    if (returnData.Trans_type == "1")
                    {
                        transData.TotalMoneyAcc = returnData.TotalMoneyAcc;
                        transData.InMoneyDate = returnData.MoneyAtAcc;
                        transData.InMoneyUser = userName;
                    }
                    else
                    {
                        transData.OutMoneyAcc = returnData.TotalMoneyAcc;
                        transData.OutMoneyDate = returnData.MoneyAtAcc;
                        transData.OutMoneyUser = userName;
                    }

                    _transactionPaymentRepo.Update(transData);

                    if (returnData.Trans_type == "1")
                    {
                        var outData = _outTransactionDetailsRepo.SelectWhere(x => x.FT_Id == transData.Ftid).FirstOrDefault();
                        if (outData != null)
                        {
                            outData.StatusLine = (int)StatusTransLine.MoneyGoAcc;
                            outData.UpdatedDate = DateTime.Now;
                            outData.UpdatedUser = userId;
                            _outTransactionDetailsRepo.Update(outData);
                        }
                    }
                    else
                    {
                        var outData = _outTransactionDetailsRepo.SelectWhere(x => x.Refund_Ref == transData.Ftid).FirstOrDefault();
                        if (outData != null)
                        {
                            outData.StatusLine = (int)StatusTransLine.MoneyOutAcc;
                            outData.UpdatedDate = DateTime.Now;
                            outData.UpdatedUser = userId;
                            _outTransactionDetailsRepo.Update(outData);
                        }
                    }
                }
                return new RespondData { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new RespondData { IsSuccess = false };
            }
        }

        public int GetNumOfDayNotConfirmMoney(Guid guid)
        {
            var res = _transactionPaymentRepo.SelectWhere(x => x.IsActive && x.OrgId == guid
            && x.Status == StatusBookingLine.booked.ToString() && !x.TotalMoneyAcc.HasValue);

            var groupData = res.GroupBy(o => o.DatePayment.Date);

            return res != null ? groupData.Count() : 0;
        }

        public IEnumerable<BookingTransactionPayment> GetNotConfirmMoneyDate(TransactionNotCompareFilter filter)
        {
            var res = _transactionPaymentRepo.SelectWhere(x => x.IsActive && x.OrgId == new Guid(filter.UserOrgId)
            && x.Status == StatusBookingLine.booked.ToString() && !x.TotalMoneyAcc.HasValue).OrderBy(o => o.DatePayment);
            return res;
        }

        public int GetBookingByListCode(IEnumerable<string> codes)
        {
            var booking = _bookingRepo.SelectWhere(x => x.IsActive && codes.Contains(x.BookingCode));
            if (booking.Any())
            {
                return (int)booking.Sum(s => s.NonRefundedFee);
            }
            return 0;
        }

        public async Task<RespondData> CancelConfirmMoneyData(IEnumerable<BookingTransactionPayment> data, string userName, string userId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    foreach (var i in data)
                    {
                        var transData = _transactionPaymentRepo.SelectWhere(x => x.Id == i.Id).FirstOrDefault();
                        if (transData != null)
                        {
                            var logData = GetSecondLastlogPayment(transData, userId);
                            if (logData != null)
                            {
                                var logPayment = JsonConvert.DeserializeObject<BookingTransactionPayment>(logData.LogText);
                                transData.Status = logPayment.Status;
                                transData.InMoneyUser = logPayment.InMoneyUser;
                                transData.TotalMoneyAcc = logPayment.TotalMoneyAcc;
                                transData.InMoneyDate = logPayment.InMoneyDate;
                                transData.OutMoneyDate = logPayment.OutMoneyDate;
                                transData.OutMoneyAcc = logPayment.OutMoneyAcc;
                                transData.OutMoneyUser = logPayment.OutMoneyUser;
                                transData.MoneyChangeDate = logPayment.MoneyChangeDate;
                                transData.UpdatedDate = logPayment.UpdatedDate;
                                transData.UpdatedUser = logPayment.UserId.ToString();

                                if (logPayment.CancelStatus == ((int)StatusTransLine.CancelLine).ToString())
                                {
                                    transData.CancelStatus = logPayment.CancelStatus;
                                    transData.CancelTime = logPayment.CancelTime;
                                    transData.CancelUer = logPayment.CancelUer;
                                }
                                _transactionPaymentRepo.Update(transData);
                                WritelogPayment(transData, userId);
                            }
                        }
                    }

                    transaction.Commit();
                    return new RespondData { IsSuccess = true, Data = data };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public IEnumerable<BookingTransactionPayment> ExportExcelConfirmMoneyData(TransactionFilterModel filter)
        {
            throw new NotImplementedException();
        }

        public async Task<FtpTransJobHistory> SaveFtpTransJobHistory(FtpTransJobHistory model)
        {
            try
            {
                return await _ftpTransJobHistoryRepo.AddAsync(model);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return null;
            }
        }

        public PagingResponseEntity<FtpTransJobHistory> GetGetFtpTransJobHistoryPaging(TransactionFilterModel pagingModel)
        {
            try
            {
                var data = _ftpTransJobHistoryRepo.SelectWhere(x => x.IsActive
                        && (!pagingModel.FindDate.HasValue || x.CreatedDate.Date == pagingModel.FindDate.Value.Date))
                        .OrderByDescending(o => o.CreatedDate);
                return new PagingResponseEntity<FtpTransJobHistory>
                {
                    Data = data,
                    Count = data.Count()
                };
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return null;
            }
        }

        public bool SetInactivePreviousFileIn(InTransactionHeader saveModel)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var header = _repo.SelectWhere(x => x.DateTrans.Date == saveModel.DateTrans.Date && x.IsActive).Include(i => i.InTransactionDetails).ToList();
                    foreach (var item in header)
                    {
                        item.IsActive = false;
                        foreach (var de in item.InTransactionDetails)
                        {
                            de.IsActive = false;
                            _inDetailsRepo.Update(de);
                        }
                        _repo.Update(item);
                    }
                    var outHeader = _outTransactionHeaderRepo.SelectWhere(x => x.DateTrans.Date == saveModel.DateTrans.Date && x.IsActive).Include(i => i.OutTransactionDetails).ToList();
                    foreach (var item in outHeader)
                    {
                        item.IsActive = false;
                        foreach (var de in item.OutTransactionDetails)
                        {
                            de.IsActive = false;
                            _outTransactionDetailsRepo.Update(de);
                        }
                        _outTransactionHeaderRepo.Update(item);
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    _log.LogError(e.Message);
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public List<DateTime> GetDateNotHaveFileIn()
        {
            var dateIn = new List<DateTime>();
            try
            {
                var fileIn = _repo.SelectWhere(x => x.IsActive).OrderByDescending(o => o.DateTrans.Date).FirstOrDefault();
                var countDate = (DateTime.Now.Date - fileIn.DateTrans.Date).TotalDays;
                for (var i = 0; i < countDate; i++)
                {
                    var j = i + 1;
                    dateIn.Add(DateTime.Now.AddDays(-j));
                }
                return dateIn;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "GetDateNotHaveFileIn"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return new List<DateTime>();
            }
        }

        public OutTransactionHeader CheckTransCompare(DateTime dateTrans)
        {
            var data = _outTransactionHeaderRepo.SelectWhere(x => x.IsActive && x.DateTrans.Date == dateTrans.Date)
                .Include(i => i.OutTransactionDetails).FirstOrDefault();
            if (data != null && data.OutTransactionDetails.Any())
            {
                return data;
            }
            return null;
        }

        public OutTransactionHeader GetStatusCompareTrans(TransactionFilterModel filter)
        {
            var data = _outTransactionHeaderRepo.SelectWhere(x => x.IsActive && x.DateTrans.Date == filter.FindDate.Value.Date).FirstOrDefault();
            return data != null ? data : null;
        }

        public IQueryable<Booking> GetbookingByCreatedDate(TransactionFilterModel filter)
        {
            return _bookingRepo.SelectWhere(x => x.IsActive && filter.EmailFilterDates.Contains(x.CreatedDate.Date)
            && (x.Status == StatusBookingLine.booked.ToString() || x.Status == StatusBookingLine.cancel.ToString()));
        }
        #endregion
    }
}
