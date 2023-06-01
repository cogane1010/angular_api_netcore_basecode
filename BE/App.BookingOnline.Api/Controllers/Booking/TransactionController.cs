using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.IService.Common;
using App.BookingOnline.WebApi.Jobs;
using App.Core.Domain;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NPOI.HPSF;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static App.Core.Enums;

namespace App.BookingOnline.WebApi.Controllers.Booking
{
    public class TransactionController : GridController<InTransactionDTO, TransactionFilterModel, ITransactionService>
    {
        public IConfiguration Configuration { get; }
        private readonly ISftpService _sftpService;
        private readonly ILogger _log;
        public TransactionController(ITransactionService service, IConfiguration configuration
            , ILogger<TransactionService> logger, ISftpService sftpService) : base(service)
        {
            Configuration = configuration;
            _sftpService = sftpService;
            _log = logger;
        }

        #region Query
        [HttpPost("getDetails")]
        public virtual RespondData GetDetails(TransactionFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                return Success(_service.GetDetails(filter));
            }
            catch (Exception e)
            {
                return Failure("500", e.Message);
            }
        }

        [HttpPost("exportExcelFileOut")]
        public async Task<IActionResult> ExportExcelFileOut(TransactionFilterModel filter)
        {
            filter.UserName = UserName;
            filter.UserOrgId = CurOrgId;
            filter.UserId = UserId;

            var result = await Task.Run(() => _service.ExportExcelFileOut(filter));

            return File(result.Item2, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.Item1);
        }

        [HttpPost("exportExcelFileOutAll")]
        public async Task<IActionResult> ExportExcelFileOutAll(TransactionFilterModel filter)
        {
            filter.UserName = UserName;
            filter.UserOrgId = CurOrgId;
            filter.UserId = UserId;

            var result = await Task.Run(() => _service.ExportExcelFileOutAll(filter));

            return File(result.Item2, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.Item1);
        }

        [HttpPost("GetPagingOutFile")]
        public RespondData GetPagingOutFile(TransactionFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                return Success(_service.GetPagingOutFile(filter));
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("GetPagingOutFileAll")]
        public RespondData GetPagingOutFileAll(TransactionFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                return Success(_service.GetPagingOutFileAll(filter));
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("GetDetailOutFile")]
        public RespondData GetDetailOutFile(TransactionFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                var data = _service.GetDetailOutFile(filter);
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }

        }

        [HttpPost("sendEmailFileOut")]
        public async Task<RespondData> SendEmailFileOut(TransactionFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                await _service.SendFileOutToSeabank();
                return Success();
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("getNumOfDayNotCompare")]
        public RespondData GetNumOfDayNotCompare()
        {
            try
            {
                var data = _service.GetNumOfDayNotCompare(new Guid(CurOrgId));
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("getStatusCompareTrans")]
        public RespondData GetStatusCompareTrans(TransactionFilterModel filter)
        {
            try
            {
                filter.UserOrgId = CurOrgId;
                string data = _service.GetStatusCompareTrans(filter);
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }


        [HttpPost("GetNumOfDayNotConfirmMoney")]
        public RespondData GetNumOfDayNotConfirmMoney()
        {
            try
            {
                var data = _service.GetNumOfDayNotConfirmMoney(new Guid(CurOrgId));
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("getNotCompareDate")]
        public RespondData GetNotCompareDate(TransactionNotCompareFilter filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                var data = _service.GetNotCompareDate(filter);
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("getNumOfDayNotApprove")]
        public RespondData GetNumOfDayNotApprove()
        {
            try
            {
                var data = _service.GetNumOfDayNotApprove(new Guid(CurOrgId));
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("getNotApproveDate")]
        public RespondData GetNotApproveDate(TransactionNotApproveFilter filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                var data = _service.GetNotApproveDate(filter);
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }

        }

        #region Out email setting
        [HttpGet("getOutEmailSetting")]
        public RespondData GetOutEmailSetting()
        {
            try
            {
                var data = _service.GetOutEmailSetting();
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("testSendMail")]
        public async Task<RespondData> TestSendMail(BookingOutEmailSetting obj)
        {
            try
            {
                await _service.TestSendMail(obj);
                return Success();
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }
        #endregion
        #endregion

        #region CUD        

        [HttpPost("getTransactionCompare")]
        public RespondData GetTransactionCompare(TransactionFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                var data = _service.GetTransactionCompare(filter);
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        /// <summary>
        /// xác nhận phần đối xoát
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("saveCompare")]
        public async Task<RespondData> SaveCompare(SaveCompareModel obj)
        {
            try
            {
                obj.OrgId = CurOrgId;
                obj.UserId = UserId;
                var result = await _service.SaveCompare(obj);
                return string.IsNullOrWhiteSpace(result) ? Success() : Failure("500", result);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("approveOut")]
        public async Task<RespondData> ApproveOut(OutTransactionHeaderDTO entityDTO)
        {
            try
            {
                entityDTO.UpdatedDate = DateTime.Now;
                entityDTO.UpdatedUser = this.UserId;
                var result = await _service.ApproveOut(entityDTO);
                return string.IsNullOrWhiteSpace(result) ? Success() : Failure("500", result);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("unApproveOut")]
        public async Task<RespondData> UnApproveOut(OutTransactionHeaderDTO entityDTO)
        {
            try
            {
                entityDTO.UpdatedDate = DateTime.Now;
                entityDTO.UpdatedUser = this.UserId;
                var result = await _service.UnApproveOut(entityDTO);
                return string.IsNullOrWhiteSpace(result) ? Success() : Failure("500", result);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("approveIn")]
        public async Task<RespondData> ApproveIn(TransactionFilterModel filter)
        {
            try
            {
                filter.UserId = this.UserId;
                var result = await _service.ApproveIn(filter);
                return string.IsNullOrWhiteSpace(result) ? Success() : Failure("500", result);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        #region Out email setting
        [HttpPost("saveOutEmailSetting")]
        public async Task<RespondData> SaveOutEmailSetting(BookingOutEmailSetting obj)
        {
            try
            {
                await _service.SaveOutEmailSetting(obj);
                if (!string.IsNullOrEmpty(obj.transaction_out_email_hour))
                {
                    var hourOut = obj.transaction_out_email_hour.Split(':');
                    var cronOut = Cron.Daily(Convert.ToInt32(hourOut[0]), Convert.ToInt32(hourOut[1]));
                    RecurringJob.AddOrUpdate<SendFileOutJobs>("SendFileOut", (myJob) => myJob.SendFileOutToSbAsync(), cronOut, TimeZoneInfo.Local);
                }
                if (!string.IsNullOrEmpty(obj.HourGetFileIn))
                {
                    var hourIn = obj.HourGetFileIn.Split(':');
                    var hourGetFilein = Convert.ToInt32(hourIn[1]) + " " + Convert.ToInt32(hourIn[0]) + " * * ";
                    var cronIn = Cron.Daily(Convert.ToInt32(hourIn[0]), Convert.ToInt32(hourIn[1]));
                    RecurringJob.AddOrUpdate<GetFileInSeabankJobs>("GetFileInSeabankJobs", (myJob) => myJob.GetFileInFromSbAsync(), cronIn, TimeZoneInfo.Local);
                }

                return Success();
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }
        #endregion
        #endregion

        [HttpPost("GetDetailOutMoney")]
        public RespondData GetDetailOutMoney(TransactionFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                var data = _service.GetDetailOutMoney(filter);
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }

        }

        [HttpPost("CancelOutData")]
        public async Task<RespondData> CancelOutData(IEnumerable<BookingTransactionPayment> obj)
        {
            try
            {
                RespondData result = await _service.CancelOutData(obj, UserName, UserId);
                return result.IsSuccess ? Success() : Failure("500", result.Message);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("ConfirmMoneyData")]
        public async Task<RespondData> ConfirmMoneyData(IEnumerable<BookingTransactionPayment> data)
        {
            try
            {
                var sendata = data.Where(x => x.StatusLine == (int)StatusTransLine.Map);
                RespondData result = await _service.ConfirmMoneyData(sendata, UserName, UserId);
                return result.IsSuccess ? Success() : Failure("500", result.Message);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("CancelConfirmMoneyData")]
        public async Task<RespondData> CancelConfirmMoneyData(IEnumerable<BookingTransactionPayment> data)
        {
            try
            {
                RespondData result = await _service.CancelConfirmMoneyData(data, UserName, UserId);
                return result.IsSuccess ? Success() : Failure("500", result.Message);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("ManualMoneyData")]
        public async Task<RespondData> ManualMoneyData(OutTransactionDetailDTO data)
        {
            try
            {
                RespondData result = await _service.ManualMoneyData(data, UserName, UserId);
                return result.IsSuccess ? Success() : Failure("500", result.Message);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("GetNotConfirmMoneyDate")]
        public RespondData GetNotConfirmMoneyDate(TransactionNotCompareFilter filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                IEnumerable<NotConfirmMoneyList> data = _service.GetNotConfirmMoneyDate(filter);
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("GetNotConfirmMoneyDateCount")]
        public RespondData GetNotConfirmMoneyDateCount(TransactionNotCompareFilter filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                int data = _service.GetNotConfirmMoneyDateCount(filter);
                return Success(data);
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }

        }

        [HttpPost("exportExcelConfirmMoney")]
        public async Task<IActionResult> ExportExcelConfirmMoney(TransactionFilterModel filter)
        {
            filter.UserName = UserName;
            filter.UserOrgId = CurOrgId;
            filter.UserId = UserId;

            var result = await Task.Run(() => _service.ExportExcelConfirmMoney(filter));

            return File(result.Item2, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.Item1);
        }


        [HttpPost("getFtpTransPaging")]
        public async Task<RespondData> GetFtpTransPaging(TransactionFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                return Success(_service.GetFtpTransJobHistory(filter));
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }

        }


        [HttpPost("getSeabankFile")]
        public async Task<RespondData> GetSeabankFile(TransactionFilterModel filter)
        {
            try
            {
                //INC_SB_BRG_20220823.xls
                var filename = "INC_SB_BRG_" + DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM")
                    + DateTime.Now.AddDays(-1).ToString("dd") + ".xlsx";
                if (filter.FindDate.HasValue)
                {
                    filename = "INC_SB_BRG_" + filter.FindDate.Value.ToString("yyyy") + filter.FindDate.Value.ToString("MM")
                    + filter.FindDate.Value.ToString("dd") + ".xlsx";
                }
                else
                {
                    filter.FindDate = DateTime.Now.AddDays(-1);
                }
                
                bool isBrg = _service.CheckBrgAmin(UserId);
                if (isBrg)
                {
                    var mess = _service.CheckTransCompare(filter.FindDate);
                    if (string.IsNullOrEmpty(mess))
                    {
                        _service.DownloadFileTransc(filename, filename, UserId, filter.FindDate.Value);
                    }
                    else
                    {
                        return Failure("500", mess + ". Bạn có muốn upload lại không?");
                    }
                }
                else
                {
                    return Failure("500", "Chỉ có admin Brg mới lấy được file in");
                }

                return Success();
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("uploadFtpExcel")]
        public RespondData UploadFtpExcel()
        {
            try
            {
                _service.SendFileOutToSeabank();
                return Success();
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }

        [HttpPost("UploadFile")]
        public async Task<RespondData> UploadFile()
        {
            try
            {
                bool isBrg = _service.CheckBrgAmin(UserId);
                if (!isBrg)
                {
                    return Failure("500", "Chỉ có admin Brg mới upload được file in");
                }

                var files = Request.Form.Files;
                var postedFile = files[0];
                // save db  
                var memStream = new MemoryStream();
                postedFile.CopyTo(memStream);
                var filter = new TransactionFilterModel()
                {
                    FileName = postedFile.FileName,
                    FindDate = DateTime.Parse(postedFile.Name),
                    UserName = UserName,
                    UserOrgId = CurOrgId,
                    UserId = UserId,
                    FileContent = memStream.ToArray()
                };
                var res = await this._service.UploadInputFile(filter);

                return Success();
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("", e.Message);
            }
        }

        [HttpPost("CheckSaveCompareTrans")]
        public RespondData CheckSaveCompareTrans(TransactionFilterModel filter)
        {
            try
            {
                var mess = _service.CheckTransCompare(filter.FindDate);
                if (string.IsNullOrEmpty(mess))
                {
                    return Success();
                }
                else
                {
                    return Failure("500", mess + ". Bạn có muốn upload lại không?");
                }

            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return Failure("500", e.Message);
            }
        }
    }
}
