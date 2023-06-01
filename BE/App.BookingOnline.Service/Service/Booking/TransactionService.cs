using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.MailService;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.IService.Common;
using App.Core;
using App.Core.Configs;
using App.Core.Domain;
using App.Core.Helper;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static App.Core.Enums;

namespace App.BookingOnline.Service
{
    public class TransactionService : BaseGridDataService<InTransactionDTO, InTransactionHeader
        , TransactionFilterModel, ITransactionRepository>, ITransactionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IFileService _fileService;
        private readonly IOrganizationRepository _orgRepository;
        private readonly ILogger _log;
        private readonly IMailService _mailService;
        private readonly ISettingRepository _settingRepo;
        private readonly IHolidayRepository _holidayRepo;
        private readonly ILogDateFileOutRepository _logDateFileOutRepo;
        private readonly ISftpService _sftpService;
        private readonly int _excelStartRow = 1;
        private readonly int _excelStartColumn = 1;
        private readonly int _startRow = 2;
        private readonly int _startColumn = 0;
        private readonly string _doiXoatFolder = Path.Combine(Directory.GetCurrentDirectory(), "Doi_xoat\\");

        /// <summary>
        /// column in excel
        /// </summary>
        private readonly ExcelCellValueMap[] _inExcelColumnsMap = new ExcelCellValueMap[] {
            new ExcelCellValueMap("Trans_type", 0, ExcelCellValueMapCellType.NumberType)
            ,new ExcelCellValueMap("CustomerId", 1, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("CustomerName", 2, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("TraceId", 3, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("OrgCode", 4, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("CreditAcc", 5, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("SourceType", 6, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("PAN_NUMBER", 7, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("DebitAcc", 8, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Trans_Time", 9, ExcelCellValueMapCellType.DateTimeType)
            ,new ExcelCellValueMap("Trans_Amt", 10, ExcelCellValueMapCellType.DecimalType)
            ,new ExcelCellValueMap("Charge_Type", 11, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Charge_Amt", 12, ExcelCellValueMapCellType.DecimalType)
            ,new ExcelCellValueMap("Total_Amt", 13, ExcelCellValueMapCellType.DecimalType)
            ,new ExcelCellValueMap("Payment_Detail", 14, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Co_Code", 15, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("FT_Id", 16, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("RRN", 17, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Return_Acc", 18, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Tien_hoan", 19, ExcelCellValueMapCellType.DecimalType)
            ,new ExcelCellValueMap("Refund_Ref", 20, ExcelCellValueMapCellType.StringType)
        };



        /// <summary>
        /// column in excel
        /// </summary>
        private readonly ExcelCellValueMap[] _inUploadExcelColumnsMap = new ExcelCellValueMap[] {
             new ExcelCellValueMap("Trans_type", 1, ExcelCellValueMapCellType.NumberType)
            ,new ExcelCellValueMap("CustomerId", 2, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("CustomerName", 3, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("TraceId",4, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("OrgCode", 5, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("CreditAcc",6, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("SourceType", 7, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("PAN_NUMBER", 8, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("DebitAcc", 9, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Trans_Time", 10, ExcelCellValueMapCellType.DateTimeType)
            ,new ExcelCellValueMap("Trans_Amt", 11, ExcelCellValueMapCellType.DecimalType)
            ,new ExcelCellValueMap("Charge_Type", 12, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Charge_Amt", 13, ExcelCellValueMapCellType.DecimalType)
            ,new ExcelCellValueMap("Total_Amt", 14, ExcelCellValueMapCellType.DecimalType)
            ,new ExcelCellValueMap("Payment_Detail", 15, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Co_Code", 16, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("FT_Id", 17, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("RRN", 18, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Return_Acc", 19, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Tien_hoan", 20, ExcelCellValueMapCellType.DecimalType)
            ,new ExcelCellValueMap("Refund_Ref", 21, ExcelCellValueMapCellType.StringType)
            ,new ExcelCellValueMap("Refund_Trans_Date", 22, ExcelCellValueMapCellType.DateTimeType)
        };


        private readonly string _sendEmailCssFileName = "SendEmailFileOut.css";

        public TransactionService(ITransactionRepository gridRepository, IUserRepository userRepository, IBookingRepository bookingRepository
            , IFileService fileService, ILogger<TransactionService> logger, IOrganizationRepository orgRepository, IMailService mailService
            , ISettingRepository settingRepo, ISftpService sftpService
            , IHolidayRepository holidayRepo, ILogDateFileOutRepository logDateFileOutRepo) : base(gridRepository)
        {
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _fileService = fileService;
            _log = logger;
            _orgRepository = orgRepository;
            _mailService = mailService;
            _settingRepo = settingRepo;
            _holidayRepo = holidayRepo;
            _logDateFileOutRepo = logDateFileOutRepo;
            _sftpService = sftpService;
        }

        #region CUD
        #region Import
        public async Task<InTransactionDTO> UploadInputFile(TransactionFilterModel filter)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                // save model
                var saveModel = await GetSaveModelFromUpload(filter);
                if (saveModel.InTransactionDetails.Any() && saveModel.DateTrans.Date != filter.FindDate.Value.Date)
                {
                    throw new Exception("Ngày trong file in khác ngày đã chọn");
                }

                // ten file
                var fileDate = DateTime.Today;
                if (saveModel != null && saveModel.InTransactionDetails != null && saveModel.InTransactionDetails.Any())
                {
                    var firstLine = saveModel.InTransactionDetails.FirstOrDefault();
                    fileDate = firstLine.Trans_Date;
                }

                // luu file
                var filePath = _fileService.SaveFile(filter.FileName, AppConfigs.INTRANSACTION_PATH, filter.FileContent, fileDate);
                saveModel.FilePath = filePath;

                _gridRepository.SetInactivePreviousFileIn(saveModel);

                var result = await _gridRepository.AddAsync(saveModel);
                var res = AutoMapperHelper.Map<InTransactionHeader, InTransactionDTO>(result);
                return res;
            }
            catch (Exception ex)
            {
                await WriteLog(string.Format("{0}", ex.Message), filter);
                throw new Exception(ex.Message);
            }
        }

        private async Task<InTransactionHeader> GetSaveModelFromUpload(TransactionFilterModel filter)
        {
            var saveModel = new InTransactionHeader()
            {
                FileName = filter.FileName,
                FilePath = filter.FilePath,
                CreatedDate = DateTime.Now,
                CreatedUser = filter.UserId,
                Status = TransBookStatus.created.ToString(),
                IsActive = true
            };
            var lines = new List<InTransactionDetails>();

            // get file data
            var file = new MemoryStream(filter.FileContent);
            ////////////////////
            var extension = Path.GetExtension(filter.FilePath);
            var colName = string.Empty;
            var rowIndex = _startRow;
            var currRow = rowIndex;

            #region Get data from excel
            using (ExcelPackage package = new ExcelPackage(file))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int colCount = worksheet.Dimension.End.Column;  //get Column Count
                int rowCount = worksheet.Dimension.End.Row;     //get row count
                for (int row = _startRow; row <= rowCount; row++)
                {
                    var line = GetExcelRowValues(worksheet, row);

                    // nếu các cột key không có dữ liệu, bỏ qua dòng
                    if (string.IsNullOrWhiteSpace(line.OrgCode) && string.IsNullOrWhiteSpace(line.TraceId) && string.IsNullOrWhiteSpace(line.CreditAcc)
                        && line.Trans_Time == DateTime.MinValue && line.Trans_Amt <= 0)
                    {
                        continue;
                    }
                    var org = _gridRepository.GetOrgByCode(line.OrgCode);
                    if (org == null)
                    {
                        await WriteLog(string.Format("Sân gôn không tồn tại. Mã = '{0}'", line.OrgCode), filter);
                        return null;
                    }

                    if (line.Trans_Time == DateTime.MinValue)
                    {
                        await WriteLog(string.Format("Sai định dạng cột ngày giờ giao dịch dòng {0}", row.ToString()), filter);
                        return null;
                    }
                    line.Trans_Date = line.Trans_Time.Date;

                    line.OrgName = org.Name;
                    line.OrganizationId = org.Id;
                    line.CreatedDate = DateTime.Now;
                    line.CreatedUser = filter.UserId;
                    line.Phone_Number = "";
                    line.IsActive = true;

                    lines.Add(line);
                }

                saveModel.InTransactionDetails = lines;
            }
            #endregion

            if (saveModel.InTransactionDetails != null && saveModel.InTransactionDetails.Any())
            {
                saveModel.DateTrans = saveModel.InTransactionDetails.FirstOrDefault().Trans_Date;
            }

            return saveModel;
        }

        public async Task<InTransactionDetails> GetDataFromExcelByRow(ISheet sheet, int rowIndex)
        {
            // lấy row hiện tại
            CultureInfo provider = CultureInfo.InvariantCulture;
            var nowRow = sheet.GetRow(rowIndex);
            var line = new InTransactionDetails();
            var objProps = typeof(InTransactionDetails).GetProperties();

            foreach (var cellConfig in _inExcelColumnsMap)
            {
                var prop = objProps.FirstOrDefault(x => x.Name == cellConfig.PropertyName);
                if (prop != null)
                {
                    switch (cellConfig.CellType)
                    {
                        case ExcelCellValueMapCellType.NumberType:
                            var value = nowRow.GetCell(cellConfig.ColumnIndex);
                            prop.SetValue(line, value.ToString());
                            break;
                        case ExcelCellValueMapCellType.StringType:
                            var stringValue = nowRow.GetCell(cellConfig.ColumnIndex);
                            prop.SetValue(line, stringValue?.ToString());
                            break;
                        case ExcelCellValueMapCellType.DecimalType:
                            var decimalValue = nowRow.GetCell(cellConfig.ColumnIndex);
                            var tmp_Amt = 0M;
                            if (decimalValue != null)
                            {
                                decimal.TryParse(decimalValue?.ToString(), out tmp_Amt);
                            }
                            prop.SetValue(line, tmp_Amt);
                            break;
                        case ExcelCellValueMapCellType.DateTimeType:
                            DateTime tmp_dateTime = DateTime.MinValue;
                            var dateTimeValue = nowRow.GetCell(cellConfig.ColumnIndex);
                            if (dateTimeValue != null)
                            {
                                DateTime.TryParseExact(dateTimeValue.ToString(), Constants.ExcelDateTimeFormat, provider, DateTimeStyles.None, out tmp_dateTime);
                                //tmp_dateTime = ws.Cells[rowIndex, _excelStartColumn + cellConfig.ColumnIndex].GetValue<DateTime>();
                            }
                            prop.SetValue(line, tmp_dateTime);
                            break;
                        case ExcelCellValueMapCellType.DateType:
                            DateTime tmp_date = DateTime.MinValue;
                            var dateValue = nowRow.GetCell(cellConfig.ColumnIndex);
                            if (dateValue != null)
                            {
                                DateTime.TryParseExact(dateValue.ToString(), Constants.ExcelDateFormat, provider, DateTimeStyles.None, out tmp_date);
                            }
                            prop.SetValue(line, tmp_date);
                            break;
                        default:
                            break;
                    }
                }
            }
            return line;
        }

        public async Task<RespondData> SaveFileinSeabankData(TransactionFilterModel filter)
        {
            var result = new RespondData();
            var colName = string.Empty;
            var rowIndex = _excelStartRow;
            var currRow = rowIndex;
            try
            {
                var saveModel = new InTransactionHeader()
                {
                    FileName = filter.FileName,
                    FilePath = filter.FilePath,
                    CreatedDate = DateTime.Now,
                    CreatedUser = filter.UserId,
                    DateTrans = filter.FindDate.Value,
                    Status = TransBookStatus.inApprove.ToString(),
                    IsActive = true
                };
                var lines = new List<InTransactionDetails>();
                var extension = Path.GetExtension(filter.FilePath);

                if (extension == ".xlsx")
                {
                    XSSFWorkbook wb = new XSSFWorkbook(filter.FilePath);
                    try
                    {
                        ISheet sheet = wb.GetSheetAt(0);
                        while (sheet.GetRow(rowIndex) != null && !string.IsNullOrEmpty(sheet.GetRow(rowIndex).GetCell(0)?.ToString())
                        && !string.IsNullOrEmpty(sheet.GetRow(rowIndex).GetCell(1)?.ToString()))
                        {
                            var line = await GetDataFromExcelByRow(sheet, rowIndex);

                            if (string.IsNullOrWhiteSpace(line.OrgCode) && string.IsNullOrWhiteSpace(line.TraceId) && string.IsNullOrWhiteSpace(line.CreditAcc)
                                    && line.Trans_Time == DateTime.MinValue && line.Trans_Amt <= 0)
                            {
                                continue;
                            }
                            var org = _gridRepository.GetOrgByCode(line.OrgCode);
                            if (org == null)
                            {
                                await WriteLog(string.Format("Sân gôn không tồn tại. Mã = '{0}'", line.OrgCode), filter);
                                return new RespondData { IsSuccess = false, Message = string.Format("Sân gôn không tồn tại. Mã = '{0}'", line.OrgCode) };
                            }

                            if (line.Trans_Time == DateTime.MinValue)
                            {
                                await WriteLog(string.Format("Sai định dạng cột ngày giờ giao dịch dòng {0}", line.Trans_Time.ToString()), filter);
                                return new RespondData { IsSuccess = false, Message = string.Format("Sai định dạng cột ngày giờ giao dịch dòng {0}", line.Trans_Time.ToString()) };
                            }
                            line.Trans_Date = line.Trans_Time.Date;

                            line.OrgName = org.Name;
                            line.OrganizationId = org.Id;
                            line.CreatedDate = DateTime.Now;
                            line.CreatedUser = filter.UserId;
                            line.Phone_Number = "";
                            line.IsActive = true;
                            lines.Add(line);
                            rowIndex++;
                        }
                        wb.Close();
                    }
                    catch (Exception e)
                    {
                        wb.Close();
                    }
                }
                else
                {
                    FileStream fs = new FileStream(filter.FilePath, FileMode.Open);
                    try
                    {
                        HSSFWorkbook wb = new HSSFWorkbook(fs);
                        ISheet sheet = wb.GetSheetAt(0);
                        while (sheet.GetRow(rowIndex) != null && !string.IsNullOrEmpty(sheet.GetRow(rowIndex).GetCell(0)?.ToString())
                        && !string.IsNullOrEmpty(sheet.GetRow(rowIndex).GetCell(1)?.ToString()))
                        {
                            var line = await GetDataFromExcelByRow(sheet, rowIndex);

                            if (string.IsNullOrWhiteSpace(line.OrgCode) && string.IsNullOrWhiteSpace(line.TraceId) && string.IsNullOrWhiteSpace(line.CreditAcc)
                                    && line.Trans_Time == DateTime.MinValue && line.Trans_Amt <= 0)
                            {
                                continue;
                            }
                            var org = _gridRepository.GetOrgByCode(line.OrgCode);
                            if (org == null)
                            {
                                await WriteLog(string.Format("Sân gôn không tồn tại. Mã = '{0}'", line.OrgCode), filter);
                                return new RespondData { IsSuccess = false, Message = string.Format("Sân gôn không tồn tại. Mã = '{0}'", line.OrgCode) };
                            }

                            if (line.Trans_Time == DateTime.MinValue)
                            {
                                await WriteLog(string.Format("Sai định dạng cột ngày giờ giao dịch dòng {0}", line.Trans_Time.ToString()), filter);
                                return new RespondData { IsSuccess = false, Message = string.Format("Sai định dạng cột ngày giờ giao dịch dòng {0}", line.Trans_Time.ToString()) };
                            }
                            line.Trans_Date = line.Trans_Time.Date;
                            line.OrgName = org.Name;
                            line.OrganizationId = org.Id;
                            line.CreatedDate = DateTime.Now;
                            line.CreatedUser = filter.UserId;
                            line.Phone_Number = "";
                            line.IsActive = true;
                            lines.Add(line);

                            rowIndex++;
                        }
                        fs.Close();
                    }
                    catch (Exception e)
                    {
                        fs.Close();
                    }
                }

                var fileDate = DateTime.Today;
                if (saveModel != null && saveModel.InTransactionDetails != null && saveModel.InTransactionDetails.Any())
                {
                    var firstLine = saveModel.InTransactionDetails.FirstOrDefault();
                    fileDate = firstLine.Trans_Date;
                }

                // luu file
                //var filePath = _fileService.SaveFile(filter.FileName, AppConfigs.INTRANSACTION_PATH, filter.FileContent, fileDate);
                //saveModel.FilePath = filePath;
                saveModel.InTransactionDetails = lines;
                _gridRepository.SetInactivePreviousFileIn(saveModel);
                var resultModel = await _gridRepository.AddAsync(saveModel);
                //var res = AutoMapperHelper.Map<InTransactionHeader, InTransactionDTO>(result);
                return new RespondData { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new RespondData { IsSuccess = false, Message = string.Format("Không đọc được dữ liệu row:{0} col: {1}", rowIndex, colName) };
            }
        }


        private InTransactionDetails GetExcelRowValues(ExcelWorksheet ws, int rowIndex)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var result = new InTransactionDetails();

            var objProps = typeof(InTransactionDetails).GetProperties();
            if (objProps != null && objProps.Length > 0)
            {
                foreach (var cellConfig in _inUploadExcelColumnsMap)
                {
                    var value = ws.Cells[rowIndex, _startColumn + cellConfig.ColumnIndex].Value;
                    var prop = objProps.FirstOrDefault(x => x.Name == cellConfig.PropertyName);
                    if (prop != null)
                    {
                        switch (cellConfig.CellType)
                        {
                            case ExcelCellValueMapCellType.StringType:
                                prop.SetValue(result, value?.ToString());
                                break;
                            case ExcelCellValueMapCellType.NumberType:
                                prop.SetValue(result, value?.ToString());
                                break;
                            case ExcelCellValueMapCellType.DecimalType:
                                var tmp_Amt = 0M;
                                if (value != null)
                                {
                                    decimal.TryParse(value.ToString(), out tmp_Amt);
                                }
                                prop.SetValue(result, tmp_Amt);
                                break;
                            case ExcelCellValueMapCellType.DateTimeType:
                                DateTime tmp_dateTime = DateTime.MinValue;
                                if (value != null)
                                {
                                    DateTime.TryParseExact(value.ToString(), Constants.ExcelDateTimeFormat, provider, DateTimeStyles.None, out tmp_dateTime);
                                    //tmp_dateTime = ws.Cells[rowIndex, _excelStartColumn + cellConfig.ColumnIndex].GetValue<DateTime>();
                                    prop.SetValue(result, tmp_dateTime);
                                }

                                break;
                            case ExcelCellValueMapCellType.DateType:
                                DateTime tmp_date = DateTime.MinValue;
                                if (value != null)
                                {
                                    DateTime.TryParseExact(value.ToString(), Constants.ExcelDateFormat, provider, DateTimeStyles.None, out tmp_date);
                                    prop.SetValue(result, tmp_date);
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private async Task WriteLog(string message, TransactionFilterModel filter, string logName = null)
        {
            var log = new TransactionLogs()
            {
                CreatedDate = DateTime.Now,
                CreatedUser = filter.UserName,
                DateTrans = filter.FindDate.Value,
                FileName = filter.FileName,
                FilePath = filter.FilePath,
                LogText = message,
                LogName = logName
            };
            await _gridRepository.WriteLog(log);
        }

        private MemoryStream GetFileContent(TransactionFilterModel filter)
        {
            return new MemoryStream(filter.FileContent);

            // return GetFtpFileContent(filter);
        }

        private MemoryStream GetFtpFileContent(TransactionFilterModel filter)
        {
            // Create the streams.
            MemoryStream destination = new MemoryStream();
            //string url = "ftp://192.168.85.17/INC_SB_partner_20220522.xlsx";
            string url = filter.FilePath;

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(filter.FptConfig.UserName, filter.FptConfig.Password);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                responseStream.CopyTo(destination);
                //destination.ToArray();
            }

            return destination;
        }

        /// <summary>
        /// Get file path and file name
        /// </summary>
        /// <param name="filter">Transaction filter model</param>
        /// <returns>Item1: file path. Item2: file name</returns>
        private Tuple<string, string> GetFilePath(TransactionFilterModel filter)
        {
            var fName = string.Format("INC_SB_partner_{0}.xlsx", filter.FindDate.Value.ToString("yyyyMMdd"));
            var fPath = string.Format("{0}/{1}", filter.FptConfig.Url, fName);

            return new Tuple<string, string>(fPath, fName);
        }
        #endregion

        #region Email
        public async Task SaveOutEmailSetting(BookingOutEmailSetting obj)
        {
            await _gridRepository.SaveOutEmailSetting(obj);
        }
        #endregion

        public async Task<string> ApproveIn(TransactionFilterModel filter)
        {
            try
            {
                filter.PageIndex = 0;
                filter.PageSize = 10000;
                var inHeaders = _gridRepository.GetPaging(filter);
                if (inHeaders != null && inHeaders.Data != null && inHeaders.Data.Any())
                {
                    // save objects
                    var saveObjs = new List<InTransactionHeader>();
                    foreach (var inHdr in inHeaders.Data)
                    {
                        var obj = _gridRepository.SingleOrDefault(inHdr.Id);
                        if (obj != null && obj.Status != TransBookStatus.inApprove.ToString())
                        {
                            obj.Status = TransBookStatus.inApprove.ToString();
                            obj.ApproveDate = DateTime.Now;
                            obj.ApproverUserId = filter.UserId;
                            saveObjs.Add(obj);
                        }
                    }
                    await _gridRepository.ApproveIn(saveObjs, filter.UserId);
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "ApproveIn"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return e.Message;
            }
            return null;
        }

        public async Task<string> ApproveOut(OutTransactionHeaderDTO entityDTO)
        {
            try
            {
                var obj = _gridRepository.GetOutTransactionHeaderById(entityDTO.Id);
                if (obj != null)
                {
                    obj.Status = TransBookStatus.outApprove.ToString();
                    obj.ApproveDate = DateTime.Now;
                    obj.ApproverUserId = entityDTO.UpdatedUser;

                    // tao file excel
                    var excelFile = ExportExcelFileOut(new TransactionFilterModel()
                    {
                        OutTransHeaderId = entityDTO.Id
                    }, true);

                    var fPath = _fileService.SaveFile(excelFile.Item1, AppConfigs.OUTTRANSACTION_PATH, excelFile.Item2.ToArray(), obj.DateTrans);

                    obj.FilePath = fPath;

                    await _gridRepository.UpdateOutTransactionHeaderStatus(obj);
                }

            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "ApproveOut"))
                {
                    _log.LogError(e.Message);
                    _log.LogTrace(e.StackTrace);
                }
                return e.Message;
            }
            return null;
        }

        public async Task<string> UnApproveOut(OutTransactionHeaderDTO entityDTO)
        {
            try
            {
                var obj = _gridRepository.GetOutTransactionHeaderById(entityDTO.Id);
                if (obj != null)
                {
                    obj.IsActive = false;
                    obj.UpdatedDate = DateTime.Now;
                    obj.UpdatedUser = entityDTO.UpdatedUser;
                    foreach (var i in obj.OutTransactionDetails)
                    {
                        i.IsActive = false;
                        i.UpdatedDate = DateTime.Now;
                        i.UpdatedUser = entityDTO.UpdatedUser;
                    }

                    await _gridRepository.UpdateOutTransactionHeaderStatus(obj);
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "UnApproveOut"))
                {
                    _log.LogError(e.Message);
                    _log.LogTrace(e.StackTrace);
                }
                return e.Message;
            }
            return null;
        }

        public async Task<string> SaveCompare(SaveCompareModel obj)
        {
            try
            {
                if (obj.Rows != null && obj.Rows.Any())
                {
                    // kiểm tra có tồn tại file out đã được duyệt trong ngày chưa
                    var firstRow = obj.Rows.FirstOrDefault(x => x.InTransactionDetails != null && x.InTransactionDetails.TraceId != null);
                    if (firstRow != null)
                    {
                        var outFilter = new TransactionFilterModel() { FindDate = obj.FindDate };
                        var outFiles = _gridRepository.GetOutTransactionByDate(outFilter).ToList();
                        if (outFiles != null && outFiles.Any() && outFiles.Exists(x => x.Status == TransBookStatus.outApprove.ToString() && x.OrganizationId == new Guid(obj.OrgId)))
                        {
                            return string.Format("File out của ngày {0} đã duyệt", outFilter.FindDate.Value.ToString("dd/MM/yyyy"));
                        }
                    }
                    obj.Rows.RemoveAt(obj.Rows.Count - 1);
                    // gán user rc code
                    foreach (var row in obj.Rows)
                    {
                        var transType = row.InTransactionDetails.Trans_type;
                        if (row.InTransactionDetails == null || row.InTransactionDetails.TraceId == null)
                        {
                            row.InTransactionDetails = new InTransactionDetailsDTO();
                            row.InTransactionDetails.Trans_type = transType;
                            row.InTransactionDetails.Trans_Date = row.BookingDTO.Payment_Time.Value;
                            row.InTransactionDetails.IsActive = true;
                            row.InTransactionDetails.OrganizationId = (Guid)row.BookingDTO.OrgId;
                            var org = await _orgRepository.GetById(row.InTransactionDetails.OrganizationId);
                            row.InTransactionDetails.OrgCode = org != null ? org.Code : null;

                            row.InTransactionDetails.TraceId = row.BookingDTO?.Traceid;
                            row.InTransactionDetails.Trans_Amt = row.BookingDTO.NonRefundedFee.HasValue ? (decimal)row.BookingDTO.NonRefundedFee : 0;
                            row.InTransactionDetails.Trans_Time = row.BookingDTO.Payment_Time.Value;
                        }
                        if (!string.IsNullOrEmpty(row.Rc_code))
                        {
                            row.InTransactionDetails.Rc_Code = row.Rc_code;
                        }
                        if (!string.IsNullOrEmpty(row.UserRc_code))
                        {
                            row.InTransactionDetails.UserRc_code = row.UserRc_code;
                        }

                        row.InTransactionDetails.CreatedDate = DateTime.Now;
                        row.InTransactionDetails.CreatedUser = obj.UserId;
                        if (!string.IsNullOrEmpty(row.Rc_code) || !string.IsNullOrEmpty(row.UserRc_code))
                        {
                            row.InTransactionDetails.StatusLine = (int)StatusTransLine.NotMap;
                        }
                        else
                        {
                            row.InTransactionDetails.StatusLine = (int)StatusTransLine.Map;
                        }

                        if (!row.InTransactionDetails.Tien_hoan.HasValue && row.BookingDTO.Tien_hoan.HasValue)
                        {
                            row.InTransactionDetails.Tien_hoan = row.BookingDTO.Tien_hoan;
                        }
                    }
                    //var outData = obj.Rows.Where(x => !string.IsNullOrWhiteSpace(x.Rc_code) || !string.IsNullOrWhiteSpace(x.UserRc_code))
                    //    .Select(x => x.InTransactionDetails);
                    var outData = obj.Rows.Select(x => x.InTransactionDetails);
                    if (outData.Any())
                    {
                        var saveObjs = AutoMapperHelper.Map<InTransactionDetailsDTO, OutTransactionDetails
                        , IEnumerable<InTransactionDetailsDTO>, IEnumerable<OutTransactionDetails>>(outData);
                        await _bookingRepository.AddOutTransactionData(saveObjs, obj.UserId, obj.OrgId, obj.FindDate);
                    }
                    else
                    {
                        var saveObjs = AutoMapperHelper.Map<InTransactionDetailsDTO, OutTransactionDetails
                       , IEnumerable<InTransactionDetailsDTO>, IEnumerable<OutTransactionDetails>>(outData);
                        await _bookingRepository.AddOutTransactionData(saveObjs, obj.UserId, obj.OrgId, obj.FindDate);
                        // tạo header rỗng
                        //await _gridRepository.AddEmptyOutTransactionHeader(obj.Rows.FirstOrDefault().InTransactionDetails.Trans_Time, obj.UserId, obj.OrgId);
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "Service:SaveCompare"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return e.Message;
            }
        }
        #endregion

        #region Query
        public override PagingResponseEntityDTO<InTransactionDTO> GetPaging(TransactionFilterModel pagingModel)
        {
            var paging = _gridRepository.GetPaging(pagingModel);
            var data = AutoMapperHelper.Map<InTransactionHeader, InTransactionDTO, List<InTransactionHeader>, List<InTransactionDTO>>(paging.Data.ToList());

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    Guid guid = Guid.Empty;
                    Guid.TryParse(item.CreatedUser, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.CreatedUserName = user?.FullName;
                    }

                    guid = Guid.Empty;
                    Guid.TryParse(item.UpdatedUser, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.UpdatedUserUserName = user?.FullName;
                    }

                    guid = Guid.Empty;
                    Guid.TryParse(item.ApproverUserId, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.ApproverUserName = user?.FullName;
                    }

                    item.ApproveDate = item.ApproveDate == DateTime.MinValue ? null : item.ApproveDate;
                }
            }

            return new PagingResponseEntityDTO<InTransactionDTO>
            {
                Count = paging.Count,
                Data = data
            };
        }

        public PagingResponseEntityDTO<InTransactionDetailsDTO> GetDetails(TransactionFilterModel filter)
        {
            var paging = _gridRepository.GetDetails(filter);
            var data = AutoMapperHelper.Map<InTransactionDetails, InTransactionDetailsDTO, List<InTransactionDetails>, List<InTransactionDetailsDTO>>(paging.Data.ToList());

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    Guid guid = Guid.Empty;
                    Guid.TryParse(item.CreatedUser, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.CreatedUserName = user?.FullName;
                    }
                }
            }

            return new PagingResponseEntityDTO<InTransactionDetailsDTO>
            {
                Count = paging.Count,
                Data = data
            };
        }

        public string ExportToBase64String(TransactionFilterModel filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PaymentCompare> GetTransactionCompare(TransactionFilterModel filter)
        {
            var inData = _bookingRepository.GetInDataTransaction(filter);
            var inDataDTO = AutoMapperHelper.Map<InTransactionDetails, InTransactionDetailsDTO
                , IEnumerable<InTransactionDetails>, IEnumerable<InTransactionDetailsDTO>>(inData);

            var bookingPaymentData = _bookingRepository.GetbookingPaymentTrans(filter);
            var bookingPayment = bookingPaymentData.Where(x => x.Status != StatusBookingLine.close.ToString());

            var data = new List<PaymentCompare>();
            var aaaaa = "23032901001";
            foreach (var item in inDataDTO)
            {
                var comData = new PaymentCompare();
                comData.InTransactionDetails = item;
                //if (item.TraceId == aaaaa)
                //{
                //    var a = 1;
                //}
                var checkBooking = bookingPayment.FirstOrDefault(x => x.Traceid == item.TraceId);
                if (checkBooking != null)
                {
                    comData.BookingDTO = new BookingDTO
                    {
                        BookingCode = checkBooking.Booking.BookingCode,
                        CardFullName = checkBooking.Booking.AppUser.FullName,
                        DateId = checkBooking.Booking.DateId.Value,
                        CourseName = checkBooking.Booking.Course.Name,
                        Payment_Time = checkBooking.DatePayment,
                        Traceid = checkBooking.Traceid,
                        NonRefundedFee = checkBooking.Booking.NonRefundedFee,
                        Cancel_Time = checkBooking.CancelTime,
                        Tien_hoan = checkBooking.SdkRefundMoney
                    };
                    var teetimes = checkBooking.Booking.BookingLines.Select(s => s.Tee_Time);
                    foreach (var line in teetimes)
                    {
                        if (!string.IsNullOrEmpty(comData.BookingDTO.TotalTeetimeDisplay))
                        {
                            if (!comData.BookingDTO.TotalTeetimeDisplay.Contains(line.Value.ToString("HH:mm")))
                            {
                                comData.BookingDTO.TotalTeetimeDisplay += line.Value.ToString("HH:mm") + " ";
                            }
                        }
                        else
                        {
                            comData.BookingDTO.TotalTeetimeDisplay += line.Value.ToString("HH:mm") + " ";
                        }
                    }
                    comData.Rc_code = "";
                    //check sai thong tin

                    if (item.Trans_type == "1")
                    {
                        if (checkBooking.Booking.NonRefundedFee != item.Trans_Amt
                        || checkBooking.DatePayment.Date != item.Trans_Time.Value.Date)
                        {
                            comData.Rc_code = Constants.IncorrectInfo;
                        }
                    }
                    if (item.Trans_type == "0")
                    {
                        comData.BookingDTO.Cancel_Time = checkBooking.Booking.Cancel_Time;
                        comData.BookingDTO.Tien_hoan = checkBooking.SdkRefundMoney;
                        if (checkBooking.Booking.NonRefundedFee != item.Trans_Amt
                            || checkBooking.SdkRefundMoney != item.Tien_hoan
                            || checkBooking.DatePayment.Date != item.Trans_Time.Value.Date)
                        {
                            comData.Rc_code = Constants.IncorrectInfo;
                        }
                    }
                }
                else
                {
                    comData.Rc_code = Constants.SbYesPartNo;
                }
                // mặc định user rc code = rc code
                comData.UserRc_code = comData.Rc_code;

                data.Add(comData);
            }
            // truong hop Brg có Sb không
            foreach (var item in bookingPayment)
            {
                //if (item.Traceid == aaaaa)
                //{
                //    var a = 1;
                //}
                var checkPayment = inDataDTO.Where(x => x.TraceId == item.Traceid).FirstOrDefault();
                if (checkPayment == null)
                {
                    var comData = new PaymentCompare();
                    comData.BookingDTO = new BookingDTO
                    {
                        BookingCode = item.Booking.BookingCode,
                        CardFullName = item.Booking.AppUser.FullName,
                        DateId = item.Booking.DateId.Value,
                        CourseName = item.Booking.Course.Name,
                        NonRefundedFee = item.Booking.NonRefundedFee,
                        Payment_Time = item.DatePayment,
                        Traceid = item.Traceid,
                        OrgId = item.OrgId,
                        Tien_hoan = bookingPayment.FirstOrDefault(x => x.BookingId == item.BookingId)?.SdkRefundMoney
                    };
                    var teetimes = item.Booking.BookingLines.Select(s => s.Tee_Time);
                    foreach (var line in teetimes)
                    {
                        if (!string.IsNullOrEmpty(comData.BookingDTO.TotalTeetimeDisplay))
                        {
                            if (!comData.BookingDTO.TotalTeetimeDisplay.Contains(line.Value.ToString("HH:mm")))
                            {
                                comData.BookingDTO.TotalTeetimeDisplay += line.Value.ToString("HH:mm") + " ";
                            }
                        }
                        else
                        {
                            comData.BookingDTO.TotalTeetimeDisplay += line.Value.ToString("HH:mm") + " ";
                        }
                    }
                    comData.Rc_code = Constants.SbNoPartYes;
                    // mặc định user rc code = rc code
                    comData.UserRc_code = comData.Rc_code;
                    comData.InTransactionDetails = new InTransactionDetailsDTO { Trans_type = "1" };
                    data.Add(comData);
                }

                if (item.CancelTime.HasValue && item.SdkRefundMoney.Value > 0)
                {
                    var checkCancelPayment = inDataDTO.Where(x => x.TraceId == item.Traceid && x.Trans_type == "0").FirstOrDefault();
                    if (checkCancelPayment == null)
                    {
                        var comData = new PaymentCompare();
                        comData.BookingDTO = new BookingDTO
                        {
                            BookingCode = item.Booking.BookingCode,
                            CardFullName = item.Booking.AppUser.FullName,
                            DateId = item.Booking.DateId.Value,
                            CourseName = item.Booking.Course.Name,
                            NonRefundedFee = item.Booking.NonRefundedFee,
                            Payment_Time = item.DatePayment,
                            Cancel_Time = item.CancelTime,
                            Traceid = item.Traceid,
                            OrgId = item.OrgId,
                            Tien_hoan = bookingPayment.FirstOrDefault(x => x.BookingId == item.BookingId)?.SdkRefundMoney
                        };
                        var teetimes = item.Booking.BookingLines.Select(s => s.Tee_Time);
                        foreach (var line in teetimes)
                        {
                            if (!string.IsNullOrEmpty(comData.BookingDTO.TotalTeetimeDisplay))
                            {
                                if (!comData.BookingDTO.TotalTeetimeDisplay.Contains(line.Value.ToString("HH:mm")))
                                {
                                    comData.BookingDTO.TotalTeetimeDisplay += line.Value.ToString("HH:mm") + " ";
                                }
                            }
                            else
                            {
                                comData.BookingDTO.TotalTeetimeDisplay += line.Value.ToString("HH:mm") + " ";
                            }
                        }
                        comData.Rc_code = Constants.SbNoPartYes;
                        // mặc định user rc code = rc code
                        comData.UserRc_code = comData.Rc_code;
                        comData.InTransactionDetails = new InTransactionDetailsDTO { Trans_type = "0" };
                        data.Add(comData);
                    }
                }
            }

            #region RC code đã lưu
            var outTransHeader = _gridRepository.GetOutTransactionByDate(filter);
            if (outTransHeader != null && outTransHeader.Any())
            {
                foreach (var item in outTransHeader)
                {
                    if (item.OutTransactionDetails != null && item.OutTransactionDetails.Any())
                    {
                        foreach (var outTrans in item.OutTransactionDetails)
                        {
                            var resRow = data.FirstOrDefault(x => x.InTransactionDetails != null && x.InTransactionDetails.FT_Id == outTrans.FT_Id
                                                              && x.InTransactionDetails.Trans_type == outTrans.Trans_type);
                            if (resRow != null)
                            {
                                resRow.UserRc_code = outTrans.UserRc_code;
                            }
                            else
                            {
                                resRow = data.FirstOrDefault(x => x.BookingDTO != null && x.BookingDTO.Traceid == outTrans.FT_Id);
                                if (resRow != null)
                                {
                                    resRow.UserRc_code = outTrans.UserRc_code;
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Sắp xếp ưu tiên những giao dịch không khớp lên đầu danh sách
            var notValidRow = data.Where(x => (!string.IsNullOrEmpty(x.Rc_code) || !string.IsNullOrEmpty(x.UserRc_code)) && x.UserRc_code != "0");
            var validRow = data.Where(x => (string.IsNullOrEmpty(x.Rc_code) && string.IsNullOrEmpty(x.UserRc_code) || x.UserRc_code == "0"));

            if (notValidRow.Any())
            {
                data = new List<PaymentCompare>();
                data.AddRange(notValidRow);
                if (validRow.Any())
                {
                    data.AddRange(validRow);
                }
            }
            #endregion

            // summary row
            var sumRow = new PaymentCompare();
            sumRow.InTransactionDetails = new InTransactionDetailsDTO();
            sumRow.IsSummary = true;
            sumRow.InTransactionDetails.Trans_type = "1";
            sumRow.InTransactionDetails.Trans_type = data.Count().ToString();
            sumRow.InTransactionDetails.Trans_Amt = data.Where(x => x.InTransactionDetails.Trans_type == "1").Sum(s => s.InTransactionDetails.Trans_Amt);
            sumRow.InTransactionDetails.Tien_hoan = data.Sum(s => s?.InTransactionDetails?.Tien_hoan);
            sumRow.BookingDTO = new BookingDTO();
            sumRow.BookingDTO.NonRefundedFee = data.Where(x => x.InTransactionDetails.Trans_type == "1").Sum(s => s?.BookingDTO?.NonRefundedFee);
            sumRow.BookingDTO.Tien_hoan = data.Where(x => x.InTransactionDetails.Trans_type == "0").Sum(s => s?.BookingDTO?.Tien_hoan);
            data.Add(sumRow);

            return data;
        }

        public PagingResponseEntityDTO<OutTransactionHeaderDTO> GetPagingOutFile(TransactionFilterModel filter)
        {

            var pagingData = _gridRepository.GetPagingOutFile(filter);
            var data = AutoMapperHelper.Map<OutTransactionHeader, OutTransactionHeaderDTO
                , IEnumerable<OutTransactionHeader>, IEnumerable<OutTransactionHeaderDTO>>(pagingData.Data);

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    Guid guid = Guid.Empty;
                    Guid.TryParse(item.CreatedUser, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.CreatedUserName = user?.FullName;
                    }

                    guid = Guid.Empty;
                    Guid.TryParse(item.UpdatedUser, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.UpdatedUserUserName = user?.FullName;
                    }

                    guid = Guid.Empty;
                    Guid.TryParse(item.ApproverUserId, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.ApproverUserName = user?.FullName;
                    }

                    item.ApproveDate = item.ApproveDate == DateTime.MinValue ? null : item.ApproveDate;
                }
            }
            return new PagingResponseEntityDTO<OutTransactionHeaderDTO>
            {
                Count = pagingData.Count,
                Data = data
            };

        }

        public PagingResponseEntityDTO<OutTransactionHeaderDTO> GetPagingOutFileAll(TransactionFilterModel filter)
        {
            var pagingData = _gridRepository.GetPagingOutFileAll(filter);
            var data = AutoMapperHelper.Map<OutTransactionHeader, OutTransactionHeaderDTO
                , IEnumerable<OutTransactionHeader>, IEnumerable<OutTransactionHeaderDTO>>(pagingData.Data);

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    Guid guid = Guid.Empty;
                    Guid.TryParse(item.CreatedUser, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.CreatedUserName = user?.FullName;
                    }

                    guid = Guid.Empty;
                    Guid.TryParse(item.UpdatedUser, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.UpdatedUserUserName = user?.FullName;
                    }

                    guid = Guid.Empty;
                    Guid.TryParse(item.ApproverUserId, out guid);
                    if (guid != Guid.Empty)
                    {
                        var user = _userRepository.GetById(guid);
                        item.ApproverUserName = user?.FullName;
                    }

                    item.ApproveDate = item.ApproveDate == DateTime.MinValue ? null : item.ApproveDate;
                }
            }
            return new PagingResponseEntityDTO<OutTransactionHeaderDTO>
            {
                Count = pagingData.Count,
                Data = data
            };

        }

        public IEnumerable<OutTransactionDetailDTO> GetDetailOutFile(TransactionFilterModel filter)
        {
            var data = _gridRepository.GetDetailOutFile(filter);
            var returnData = AutoMapperHelper.Map<OutTransactionDetails, OutTransactionDetailDTO
                , IEnumerable<OutTransactionDetails>, IEnumerable<OutTransactionDetailDTO>>(data);

            return returnData;
        }

        #region Export
        public Tuple<string, MemoryStream> ExportExcelFileOut(TransactionFilterModel filter, bool fromApprove = false)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            MemoryStream res = new MemoryStream();
            var fileName = "";

            var obj = _gridRepository.GetOutTransactionHeaderById(filter.OutTransHeaderId.Value);
            if (obj != null)
            {
                fileName = obj.FileName;

                // neu da co file
                var fullPath = !string.IsNullOrWhiteSpace(obj.FilePath) ? Path.Combine(_fileService.GetRootPath(), obj.FilePath.Remove(0, 1)) : _fileService.GetRootPath();
                //if (File.Exists(fullPath))
                //{
                //    using (var fs = new FileStream(fullPath, FileMode.Open))
                //    {
                //        // send to return stream
                //        fs.CopyTo(res);
                //    }
                //    res.Position = 0;
                //    return new Tuple<string, MemoryStream>(obj.FileName, res);
                //}
                //else
                //{
                #region Get data from excel
                using (ExcelPackage package = new ExcelPackage())
                {
                    //get the first worksheet in the workbook
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Data");

                    // fill data
                    var dbObj = _gridRepository.GetExcelExportOutData(filter, fromApprove);
                    if (dbObj != null)
                    {
                        var data = GetExcelExportOutData(dbObj);
                        FillExcelDataFileOut(ws, data);
                    }

                    // send to return stream
                    res = new MemoryStream(package.GetAsByteArray());
                }
                #endregion
                // }
            }
            return new Tuple<string, MemoryStream>(fileName, res);
        }

        private void FillExcelDataFileOut(ExcelWorksheet ws, ExcelExportOutTransactionDTO data)
        {
            int startRow = 1, startDataRow = 2, rowIdx = startRow, colIdx = 1, totalCol = 6;

            #region Header
            //var headers = new string[] { "Số giao dịch Đối tác", "Mã sân golf", "Trans_time", "Số tiền", "Số giao dịch SB", "RC_Code" };
            var headers = new string[] { "TRANS_TYPE", "SOURCE_TYPE", "Trans.id", "San gofl", "trans.time", "Trans_amt", "Refund_amt", "SB_FT", "RC" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cells[rowIdx, colIdx++].Value = headers[i];
            }

            var headerCells = ws.Cells[rowIdx, 1, rowIdx, colIdx - 1];
            headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(Color.Silver);
            #endregion

            rowIdx = startDataRow;
            if (data != null && data.Details != null && data.Details.Any())
            {
                foreach (var row in data.Details)
                {
                    if (row.UserRc_code == "0")
                    {
                        continue;
                    }
                    colIdx = 1;
                    ws.Cells[rowIdx, colIdx++].Value = row.Trans_type;
                    ws.Cells[rowIdx, colIdx++].Value = row.SourceType;
                    ws.Cells[rowIdx, colIdx++].Value = row.TraceId;
                    ws.Cells[rowIdx, colIdx++].Value = row.OrgCode;
                    ws.Cells[rowIdx, colIdx++].Value = row.Trans_Time.ToString(Constants.ExcelDateTimeFormat);
                    ws.Cells[rowIdx, colIdx++].Value = row.Trans_Amt;
                    ws.Cells[rowIdx, colIdx++].Value = row.Tien_hoan;
                    ws.Cells[rowIdx, colIdx++].Value = row.FT_Id;
                    ws.Cells[rowIdx, colIdx++].Value = !string.IsNullOrWhiteSpace(row.UserRc_code) ? row.UserRc_code : row.Rc_code;
                    rowIdx++;
                }
                ExcelHelper.SetExcelNumberFormat(ws.Cells[startDataRow, 4, rowIdx - 1, 4]);
                ws.Cells[startRow, 1, rowIdx - 1, totalCol].AutoFitColumns();
                if (data.ContentError != null)
                {
                    ws.Cells[rowIdx + 3, 1].Value = data.ContentError;
                    ws.Cells[rowIdx + 3, 1, rowIdx + 10, 18].Merge = true;
                    ws.Cells[rowIdx + 3, 1].Style.WrapText = true;
                }
            }
        }

        private ExcelExportOutTransactionDTO GetExcelExportOutData(OutTransactionHeader data)
        {
            if (data != null)
            {
                var obj = AutoMapperHelper.Map<OutTransactionHeader, ExcelExportOutTransactionDTO>(data);
                if (obj != null && obj.OutTransactionDetails != null && obj.OutTransactionDetails.Any())
                {
                    var details = new List<ExcelExportOutTransactionDetailsDTO>();
                    foreach (var item in obj.OutTransactionDetails)
                    {
                        var detail = AutoMapperHelper.Map<OutTransactionDetails, ExcelExportOutTransactionDetailsDTO>(item);
                        if (!string.IsNullOrWhiteSpace(detail.UserRc_code))
                        {
                            detail.Rc_code = detail.UserRc_code;
                        }
                        if (!string.IsNullOrWhiteSpace(detail.Rc_code))
                        {
                            details.Add(detail);
                        }
                    }
                    obj.Details = details;
                }
                return obj;
            }
            return new ExcelExportOutTransactionDTO();
        }

        public Tuple<string, MemoryStream> ExportExcelFileOutAll(TransactionFilterModel filter)
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            MemoryStream res = new MemoryStream();
            var fileName = "";
            var ftpContent = new StringBuilder("");
            ftpContent.Append(string.Format("* Ngày {0}", filter.FindDate.Value.ToString("dd/MM/yyyy")));

            var orgs = _gridRepository.GetAllOrganization().Where(x => x.Code != Constants.BrgCode).OrderBy(x => x.OrderValue);
            filter.EmailFilterDates = new List<DateTime> { filter.FindDate.Value };
            var inHeaders = _gridRepository.GetInHeaderForEmail(filter);
            var bookByDate = _gridRepository.GetbookingByCreatedDate(filter);
            #region Get data from excel
            using (ExcelPackage package = new ExcelPackage())
            {
                //get the first worksheet in the workbook
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Data");

                // fill data
                IEnumerable<OutTransactionHeader> dbObjs = _gridRepository.GetExcelExportOutDataAll(filter);
                if (dbObjs != null && dbObjs.Any())
                {
                    var data = GetExcelExportOutDataAll(dbObjs);
                    if (data != null && data.Details != null && data.Details.Any())
                    {
                        fileName = string.Format("SB_OUT_BRG_{0}", data.Details.FirstOrDefault().Trans_Date.ToString("yyyyMMdd"));
                    }

                    foreach (var org in orgs)
                    {
                        var outHeader = dbObjs.FirstOrDefault(x => x.OrganizationId == org.Id && x.DateTrans.Date == filter.FindDate.Value.Date
                                        && x.OutTransactionDetails.Any(d => !string.IsNullOrEmpty(d.Rc_code) || (!string.IsNullOrEmpty(d.Rc_code) && d.UserRc_code != "0")));
                        if (outHeader != null)
                        {
                            ftpContent.Append(string.Format("\r\n + Sân {0} đã đối soát {1} sai lệch", org.Name,
                                outHeader.OutTransactionDetails != null && outHeader.OutTransactionDetails.Any() ? "có" : "không"));
                        }
                        // Có dữ liệu in
                        else if (inHeaders.Any(x => x.InTransactionDetails != null && x.InTransactionDetails.Count() > 0d
                                && x.DateTrans.Date == filter.FindDate.Value.Date && x.InTransactionDetails.Any(y => y.OrganizationId == org.Id)))
                        {
                            ftpContent.Append(string.Format("\r\n + Sân {0} chưa đối soát vui lòng liên hệ trực tiếp với sân {0}", org.Name));
                        }
                        if (!bookByDate.Where(x => x.C_Org_Id == org.Id).Any() && outHeader == null)
                        {
                            ftpContent.Append(string.Format("\r\n + Sân {0} Không phát sinh giao dịch", org.Name));
                        }
                    }
                    if (ftpContent.Length != 0)
                    {
                        data.ContentError = ftpContent;
                    }
                    FillExcelDataFileOut(ws, data);
                }

                // send to return stream
                res = new MemoryStream(package.GetAsByteArray());
            }
            #endregion

            return new Tuple<string, MemoryStream>(fileName, res);
        }

        public Tuple<string, MemoryStream> ExportExcelConfirmMoney(TransactionFilterModel filter)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            MemoryStream res = new MemoryStream();
            var fileName = "";

            #region Get data from excel
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Data");
                var dbObjs = _gridRepository.GetDetailOutMoney(filter);
                if (dbObjs != null && dbObjs.Any())
                {
                    fileName = string.Format("xac_nhan_tien_ve{0}", filter.FindDate.Value.ToString("yyyyMMdd"));

                    FillExcelConfirmMoneyData(ws, dbObjs);
                }

                // send to return stream
                res = new MemoryStream(package.GetAsByteArray());
            }
            #endregion

            return new Tuple<string, MemoryStream>(fileName, res);
        }

        private void FillExcelConfirmMoneyData(ExcelWorksheet ws, IEnumerable<BookingTransactionPayment> data)
        {
            int startRow = 1, startDataRow = 2, rowIdx = startRow, colIdx = 1, totalCol = 6;

            #region Header
            var headers = new string[] { "STT", "Mã KH SB", "Tên KH SB", "Mã Đặt chỗ", "Sân đặt chỗ"
                , "Ngày đặt", "TK Ghi có", "Loại TK", "Số tiền đặt", "Trạng thái", "Số tiền về", "Ngày tiền về", "Người cập nhật" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cells[rowIdx, colIdx++].Value = headers[i];
            }

            var headerCells = ws.Cells[rowIdx, 1, rowIdx, colIdx - 1];
            headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(Color.Silver);
            #endregion

            rowIdx = startDataRow;
            if (data != null)
            {
                foreach (var row in data)
                {
                    colIdx = 1;
                    ws.Cells[rowIdx, colIdx++].Value = rowIdx - 1;
                    ws.Cells[rowIdx, colIdx++].Value = row.CustomerId;
                    ws.Cells[rowIdx, colIdx++].Value = row.CustomerName;
                    ws.Cells[rowIdx, colIdx++].Value = row.BookingCode;
                    ws.Cells[rowIdx, colIdx++].Value = row.OrgCode;
                    ws.Cells[rowIdx, colIdx++].Value = row.DatePayment.ToString("dd/MM/yyyy");
                    ws.Cells[rowIdx, colIdx++].Value = row.CreditAcc;
                    ws.Cells[rowIdx, colIdx++].Value = row.SourceType;
                    ws.Cells[rowIdx, colIdx++].Value = row.TotalMoney;
                    ws.Cells[rowIdx, colIdx++].Value = row.StatusLine;
                    ws.Cells[rowIdx, colIdx++].Value = row.TotalMoneyAcc;
                    ws.Cells[rowIdx, colIdx++].Value = row.InMoneyDate;
                    ws.Cells[rowIdx, colIdx++].Value = row.InMoneyUser;
                    rowIdx++;
                }
                ExcelHelper.SetExcelNumberFormat(ws.Cells[startDataRow, 4, rowIdx - 1, 4]);
                ws.Cells[startRow, 1, rowIdx - 1, totalCol].AutoFitColumns();
            }
        }
        private ExcelExportOutTransactionDTO GetExcelExportOutDataAll(IEnumerable<OutTransactionHeader> data)
        {
            if (data != null && data.Any())
            {
                var header = new ExcelExportOutTransactionDTO();
                var lines = new List<ExcelExportOutTransactionDetailsDTO>();
                foreach (var item in data)
                {
                    if (item != null && item.OutTransactionDetails != null && item.OutTransactionDetails.Any())
                    {
                        var details = new List<ExcelExportOutTransactionDetailsDTO>();
                        foreach (var line in item.OutTransactionDetails)
                        {
                            if (line.StatusLine == (int)StatusTransLine.NotMap)
                            {
                                var detail = AutoMapperHelper.Map<OutTransactionDetails, ExcelExportOutTransactionDetailsDTO>(line);
                                details.Add(detail);
                            }
                        }
                        lines.AddRange(details);
                    }
                }
                header.Details = lines != null && lines.Any() ? lines.OrderBy(x => x.OrgCode).ToList() : lines;
                return header;
            }
            return new ExcelExportOutTransactionDTO();
        }

        private ExcelExportOutTransactionDTO GetExcelExportOutDataForEmail(IEnumerable<OutTransactionHeader> data)
        {
            if (data != null && data.Any())
            {
                var header = new ExcelExportOutTransactionDTO();
                var lines = new List<ExcelExportOutTransactionDetailsDTO>();
                foreach (var item in data)
                {
                    if (item != null && item.OutTransactionDetails != null && item.OutTransactionDetails.Any())
                    {
                        var details = new List<ExcelExportOutTransactionDetailsDTO>();
                        foreach (var line in item.OutTransactionDetails)
                        {
                            if ((string.IsNullOrEmpty(line.UserRc_code) && !string.IsNullOrEmpty(line.Rc_code) && line.Rc_code != "0")
                                || (!string.IsNullOrEmpty(line.UserRc_code) && line.UserRc_code != "0"))
                            {
                                var detail = AutoMapperHelper.Map<OutTransactionDetails, ExcelExportOutTransactionDetailsDTO>(line);
                                details.Add(detail);
                            }
                        }
                        lines.AddRange(details);
                    }
                }
                header.Details = lines != null && lines.Any() ? lines.OrderBy(x => x.OrgCode).ToList() : lines;
                header.DateTrans = data.FirstOrDefault().DateTrans;
                return header;
            }
            return new ExcelExportOutTransactionDTO();
        }
        #endregion

        #region Email
        public async Task<string> SendEmailFileOut(TransactionFilterModel filter)
        {
            try
            {
                var orgs = _gridRepository.GetAllOrganization().Where(x => x.Code != Constants.BrgCode).OrderBy(x => x.OrderValue);
                //if (orgs != null && orgs.Count() > 0)
                //{
                //    var golfOrgs = orgs.Where(x => string.Compare(x.Code, "brg", StringComparison.InvariantCultureIgnoreCase) != 0).ToList();
                //    if (golfOrgs != null && golfOrgs.Any())
                //    {
                //        orgs = golfOrgs;
                //        // sắp xếp đơn vị
                //        orgs = orgs.OrderBy(x => x.OrderValue).ToList();
                //    }
                //    else
                //    {
                //        golfOrgs = null;
                //    }
                //}

                if (filter.EmailFilterDates != null && filter.EmailFilterDates.Count > 0)
                {
                    if (orgs != null && orgs.Count() > 0)
                    {
                        var to = _settingRepo.GetSetting("transaction_out_email_to");
                        var cc = _settingRepo.GetSetting("transaction_out_email_cc");
                        var bcc = _settingRepo.GetSetting("transaction_out_email_bcc");
                        var subject = "";
                        var style = "";

                        var htmlBegin = string.Format(@"<!doctype html><html><head><style>{0}</style></head><body><div class=""first-line"">BRG gửi thông tin đối soát của các sân Golf:</div>", style);
                        var ftpBegin = @"BRG gửi thông tin đối soát của các sân Golf:</div>";
                        var htmlEnd = @"</body></html>";
                        var content = new StringBuilder(htmlBegin);
                        var ftpContent = new StringBuilder("");

                        // Danh sách in/out headers
                        var outHeaders = _gridRepository.GetOutHeaderForEmail(filter);
                        var inHeaders = _gridRepository.GetInHeaderForEmail(filter);
                        var bookByDate = _gridRepository.GetbookingByCreatedDate(filter);
                        List<ExcelExportOutTransactionDTO> excelData = new List<ExcelExportOutTransactionDTO>();

                        filter.EmailFilterDates = filter.EmailFilterDates.OrderBy(x => x).ToList();

                        if (inHeaders != null && inHeaders.Any())
                        {
                            // Các ngày có dữ liêu
                            var hasInDataDates = filter.EmailFilterDates.Where(d => inHeaders.Any(i => i.DateTrans.Date == d.Date)).OrderBy(x => x.Date);

                            foreach (var inDate in hasInDataDates)
                            {
                                content.Append(string.Format("<div class=\"date-header\">* Ngày {0}</div>", inDate.ToString("dd/MM/yyyy")));
                                ftpContent.Append(string.Format("* Ngày {0}", inDate.ToString("dd/MM/yyyy")));
                                foreach (var org in orgs)
                                {
                                    // Có dữ liệu out
                                    var outHeader = outHeaders.FirstOrDefault(x => x.OrganizationId == org.Id && x.DateTrans.Date == inDate.Date
                                        && x.OutTransactionDetails.Any(d => !string.IsNullOrEmpty(d.Rc_code) || (!string.IsNullOrEmpty(d.Rc_code) && d.UserRc_code != "0")));
                                    if (outHeader != null)
                                    {
                                        content.Append(string.Format("<div class=\"date-line\">+ Sân <span class=\"org-name\">{0}</span> chưa đối soát vui lòng liên hệ trực tiếp với sân <span class=\"org-name\">{0}</span></div>", org.Name));
                                        ftpContent.Append(string.Format("\r\n + Sân {0} đã đối soát {1} sai lệch", org.Name,
                                            outHeader.OutTransactionDetails != null && outHeader.OutTransactionDetails.Any() ? "có" : "không"));
                                    }
                                    // Có dữ liệu in
                                    else if (inHeaders.Any(x => x.InTransactionDetails != null && x.InTransactionDetails.Count() > 0d
                                            && x.DateTrans.Date == inDate.Date && x.InTransactionDetails.Any(y => y.OrganizationId == org.Id)))
                                    {
                                        content.Append(string.Format("<div class=\"date-line\">+ Sân <span class=\"org-name\">{0}</span> chưa đối soát vui lòng liên hệ trực tiếp với sân <span class=\"org-name\">{0}</span></div>", org.Name));
                                        ftpContent.Append(string.Format("\r\n + Sân {0} chưa đối soát vui lòng liên hệ trực tiếp với sân {0}", org.Name));
                                    }
                                    if (!bookByDate.Where(x => x.C_Org_Id == org.Id && x.CreatedDate.Date == inDate.Date).Any() && outHeader == null)
                                    {
                                        content.Append(string.Format("<div class=\"date-line\">+ Sân <span class=\"org-name\">{0}</span> không phát sinh giao dịch</div>", org.Name));
                                        ftpContent.Append(string.Format("\r\n + Sân {0} Không phát sinh giao dịch", org.Name));
                                    }
                                    content.Append('\n');
                                }

                                // Outheader của ngày
                                var outHeaderRows = outHeaders.Where(x => x.DateTrans.Date == inDate.Date).ToList();

                                // Không có dòng đối soát: tạo dòng trống để truyền ngày giao dịch tạo tên file trắng
                                if (outHeaderRows == null || !outHeaderRows.Any())
                                {
                                    outHeaderRows = new List<OutTransactionHeader>() { new OutTransactionHeader() { DateTrans = inDate } };
                                }
                                var excelDataRow = GetExcelExportOutDataForEmail(outHeaderRows);
                                // add content org chưa đối xoát
                                if (ftpContent.Length != 0)
                                {
                                    excelDataRow.ContentError = ftpContent;
                                }
                                excelData.Add(excelDataRow);
                            }
                            subject = string.Format("[Golf App] Đối soát ngày {0}", hasInDataDates != null && hasInDataDates.Any() ?
                            string.Join(',', hasInDataDates.Select(x => x.ToString("dd/MM/yyyy"))) : "");

                            content.Append(htmlEnd);

                            var attFiles = GetAttachFiles(excelData);

                            //send file via ftp
                            foreach (var i in attFiles)
                            {
                                var jobHistory = new FtpTransJobHistory();
                                jobHistory.FileName = i.Item2;
                                jobHistory.FilePath = i.Item1;
                                jobHistory.SeabankFilePath = @"brg";
                                jobHistory.DateRun = DateTime.Now;
                                jobHistory.DateId = i.Item3;
                                jobHistory.IsReadFile = true;
                                var uploadError = _sftpService.UploadFileTransc(i.Item1, i.Item2, "FtpService", i.Item3);
                                if (string.IsNullOrEmpty(uploadError))
                                {
                                    jobHistory.IsFtpFile = true;
                                    jobHistory.Status = FtpStatus.upload.ToString();
                                }
                                else
                                {
                                    jobHistory.IsFtpFile = false;
                                    jobHistory.DescriptionError = uploadError;
                                }

                                await _gridRepository.SaveFtpTransJobHistory(jobHistory);
                            }
                            //if (attFiles.Any())
                            //{
                            //    await _mailService.SendMailAsync(to, cc, bcc, subject, content.ToString(), filter.UserName, false, true, attFiles.Select(s => s.Item1).ToList());
                            //}
                        }
                        return "";
                    }
                    else
                    {
                        return "Không có danh sách sân gôn";
                    }
                }
                else
                {
                    return "Không có ngày tìm kiếm";
                }
            }
            catch (Exception e)
            {
                //MethodBase.GetCurrentMethod().ReflectedType.FullName;
                using (LogContext.PushProperty("MethodName", "SendEmailFileOut"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return e.Message;
            }
        }

        private List<Tuple<string, string, DateTime>> GetAttachFiles(List<ExcelExportOutTransactionDTO> data)
        {
            List<Tuple<string, string, DateTime>> res = new List<Tuple<string, string, DateTime>>();

            foreach (var row in data)
            {
                var fileName = row.Details != null && row.Details.Any() ? row.Details.FirstOrDefault().Trans_Date.ToString("yyyyMMdd") : row.DateTrans.ToString("yyyyMMdd");
                var name = String.Format("OUT_SB_BRG_{0}.xlsx", fileName);
                var fResult = ExportExcelFileForEmail(row);
                var dateFolderPath = row.Details != null && row.Details.Any() ? row.Details.FirstOrDefault().Trans_Date.ToString("yyyy-MM").Replace("-", "\\")
                                                           : DateTime.Now.ToString("yyyy-MM").Replace("-", "\\");
                var newPath = String.Format("{0}\\{1}", AppConfigs.OUTTRANSACTION_SEABANK_PATH, dateFolderPath);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                var filePath = _fileService.SaveFile(name, newPath, fResult.Item2.ToArray());
                res.Add(new Tuple<string, string, DateTime>(filePath, name, row.DateTrans));
            }
            return res;
        }

        public Tuple<string, MemoryStream> ExportExcelFileForEmail(ExcelExportOutTransactionDTO data)
        {
            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            MemoryStream res = new MemoryStream();
            var fileName = "";

            #region Get data from excel
            using (ExcelPackage package = new ExcelPackage())
            {
                //get the first worksheet in the workbook
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Data");

                if (data != null)
                {
                    if (data.Details != null && data.Details.Any())
                    {
                        fileName = string.Format("SB_OUT_BRG_{0}", data.Details.FirstOrDefault().Trans_Date.ToString("yyyyMMdd"));
                    }

                    FillExcelDataFileOut(ws, data);
                }

                // send to return stream
                res = new MemoryStream(package.GetAsByteArray());
            }
            #endregion

            return new Tuple<string, MemoryStream>(fileName, res);
        }

        public async Task SendFileOutToSeabank()
        {
            try
            {
                // Email settings
                var mailSetting = GetOutEmailSetting();
                if (mailSetting == null)
                {
                    using (LogContext.PushProperty("MethodName", "SendFileOutToSeabank"))
                    {
                        _log.LogInformation("Email out: Không có cài đặt");
                    }
                    throw new Exception("Email out: Không có cài đặt");
                }

                if (!string.IsNullOrWhiteSpace(mailSetting.transaction_out_email_dow))
                {
                    var holiday = _holidayRepo.Find(x => x.DateId.Date == DateTime.Today).FirstOrDefault();
                    var todayDow = GetDowString(DateTime.Today);

                    // Ngày gửi mail: không phải ngày nghỉ và ngày trong tuần được thiết lập
                    if (holiday == null && mailSetting.transaction_out_email_dow.Contains(todayDow, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var notSentLogDateFileOut = _logDateFileOutRepo.Find(x => x.Date.Date < DateTime.Today && x.Status == LogDateFileOutStatus.created.ToString()
                                                    && x.FileType == Enums.FileType.Out.ToString()).ToList();
                        if (notSentLogDateFileOut != null && notSentLogDateFileOut.Any())
                        {
                            try
                            {
                                var filter = new TransactionFilterModel()
                                {
                                    EmailFilterDates = notSentLogDateFileOut.Select(x => x.Date.Date).ToList(),
                                    UserName = "system"
                                };

                                var sendRes = await SendEmailFileOut(filter);

                                if (string.IsNullOrWhiteSpace(sendRes))
                                {
                                    foreach (var item in notSentLogDateFileOut)
                                    {
                                        item.UpdatedDate = DateTime.Now;
                                        item.Status = LogDateFileOutStatus.sent.ToString();
                                    }

                                    using (LogContext.PushProperty("MethodName", "SendFileOutToSeabank"))
                                    {
                                        _log.LogInformation("Email out: ***>> 1.Update trạng thái log");
                                        _log.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(notSentLogDateFileOut));
                                    }
                                    _logDateFileOutRepo.UpdateDates(notSentLogDateFileOut);
                                    using (LogContext.PushProperty("MethodName", "SendFileOutToSeabank"))
                                    {
                                        _log.LogInformation("Email out:  2.Update trạng thái log <<***");
                                    }
                                }
                                else
                                {
                                    using (LogContext.PushProperty("MethodName", "SendFileOutToSeabank"))
                                    {
                                        _log.LogError(sendRes);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                using (LogContext.PushProperty("MethodName", "SendFileOutToSeabank"))
                                {
                                    _log.LogError(ex.Message);
                                    _log.LogTrace(ex.StackTrace);
                                }
                                throw ex;
                            }
                        }
                    }

                    // Tạo log ngày hiện tại
                    await this._gridRepository.CreateNewLogFileOutDate(FileType.Out.ToString());
                }
                else
                {
                    using (LogContext.PushProperty("MethodName", "SendFileOutToSeabank"))
                    {
                        _log.LogInformation("Email out: Không có cài đặt ngày trong tuần 'transaction_out_email_dow'");
                    }
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "SendFileOutToSeabank"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
            }
        }

        private string GetDowString(DateTime dateObject)
        {
            var res = "";
            switch (dateObject.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    res = "1";
                    break;
                case DayOfWeek.Monday:
                    res = "2";
                    break;
                case DayOfWeek.Tuesday:
                    res = "3";
                    break;
                case DayOfWeek.Wednesday:
                    res = "4";
                    break;
                case DayOfWeek.Thursday:
                    res = "5";
                    break;
                case DayOfWeek.Friday:
                    res = "6";
                    break;
                case DayOfWeek.Saturday:
                    res = "7";
                    break;
                default:
                    break;
            }
            return res;
        }

        public BookingOutEmailSetting GetOutEmailSetting()
        {
            var res = new BookingOutEmailSetting()
            {
                transaction_out_email_to = _settingRepo.GetSetting("transaction_out_email_to"),
                transaction_out_email_bcc = _settingRepo.GetSetting("transaction_out_email_bcc"),
                transaction_out_email_cc = _settingRepo.GetSetting("transaction_out_email_cc"),
                transaction_out_email_dow = _settingRepo.GetSetting("transaction_out_email_dow"),
                transaction_out_email_hour = _settingRepo.GetSetting("transaction_out_email_hour"),
                HourGetFileIn = _settingRepo.GetSetting("transaction_in_file_hour")
            };
            return res;
        }

        public async Task TestSendMail(BookingOutEmailSetting obj)
        {
            var to = obj.transaction_out_email_to;
            var cc = obj.transaction_out_email_cc;
            var bcc = obj.transaction_out_email_bcc;

            await _mailService.SendMailAsync(to, cc, bcc, "test transaction out file email", "test transaction out file email", "system", false, true);
        }
        #endregion

        public SettingDTO GetNumOfDayNotCompare(Guid orgId)
        {
            var numOfDayNotCompare = _gridRepository.GetNumOfDayNotCompare(orgId);
            return new SettingDTO()
            {
                Value = numOfDayNotCompare.ToString()
            };
        }

        public IEnumerable<TransactionNotCompareLineDTO> GetNotCompareDate(TransactionNotCompareFilter filter)
        {
            var res = new List<TransactionNotCompareLineDTO>();

            var notCompareDays = _gridRepository.GetNotCompareDate(filter);
            if (notCompareDays != null && notCompareDays.Count() > 0)
            {
                var days = AutoMapperHelper.Map<TransactionNotCompareLine, TransactionNotCompareLineDTO
                       , IEnumerable<TransactionNotCompareLine>, IEnumerable<TransactionNotCompareLineDTO>>(notCompareDays);
                foreach (var cmpDay in days)
                {
                    var rows = GetTransactionCompare(new TransactionFilterModel() { FindDate = cmpDay.TransDate, UserOrgId = filter.UserOrgId });

                    // chỉ lấy những row import đã duyệt
                    if (rows != null && rows.Count() > 0)
                    {
                        cmpDay.NoOfRecord = rows.Count();
                        cmpDay.PaymentsCompare = rows;
                        res.Add(cmpDay);
                    }
                }
            }
            return res;
        }

        public SettingDTO GetNumOfDayNotApprove(Guid guid)
        {
            var numOfDayNotApprove = _gridRepository.GetNumOfDayNotApprove(guid);
            return new SettingDTO()
            {
                Value = numOfDayNotApprove.ToString()
            };
        }

        public IEnumerable<TransactionNotApproveLineDTO> GetNotApproveDate(TransactionNotApproveFilter filter)
        {
            var res = new List<TransactionNotApproveLineDTO>();

            var notApproveDays = _gridRepository.GetNotApproveDate(filter);
            if (notApproveDays != null && notApproveDays.Count() > 0)
            {
                var days = AutoMapperHelper.Map<TransactionNotApproveLine, TransactionNotApproveLineDTO
                       , IEnumerable<TransactionNotApproveLine>, IEnumerable<TransactionNotApproveLineDTO>>(notApproveDays);
                foreach (var cmpDay in days)
                {
                    var rows = GetPagingOutFile(new TransactionFilterModel()
                    {
                        FromDate = cmpDay.TransDate,
                        ToDate = cmpDay.TransDate,
                        UserOrgId = filter.UserOrgId,
                        PageSize = 1000,
                        PageIndex = 0
                    });

                    // chỉ lấy những row import đã duyệt
                    if (rows != null && rows.Data != null && rows.Data.Count() > 0)
                    {
                        cmpDay.OutHeader = rows.Data.FirstOrDefault();
                        var outDetails = GetDetailOutFile(new TransactionFilterModel() { OutTransHeaderId = cmpDay.OutHeader.Id, UserOrgId = filter.UserOrgId });
                        cmpDay.NoOfRecord = outDetails != null ? outDetails.Count() : 0;
                        res.Add(cmpDay);
                    }
                }
            }
            return res;
        }

        public IEnumerable<BookingTransactionPayment> GetDetailOutMoney(TransactionFilterModel filter)
        {
            var data = _gridRepository.GetDetailOutMoney(filter).ToList();

            return data;
        }

        public async Task<RespondData> CancelOutData(IEnumerable<BookingTransactionPayment> data, string loginUser, string userId)
        {

            return await _gridRepository.CancelOutData(data, loginUser, userId);
        }

        public async Task<RespondData> ConfirmMoneyData(IEnumerable<BookingTransactionPayment> data, string userName, string userId)
        {
            return await _gridRepository.ConfirmMoneyData(data, userName, userId);
        }

        public async Task<RespondData> ManualMoneyData(OutTransactionDetailDTO data, string userName, string userId)
        {
            var returnData = AutoMapperHelper.Map<OutTransactionDetailDTO, OutTransactionDetails>(data);

            return await _gridRepository.ManualMoneyData(returnData, userName, userId);
        }

        public int GetNumOfDayNotConfirmMoney(Guid guid)
        {
            return _gridRepository.GetNumOfDayNotConfirmMoney(guid);
        }

        public IEnumerable<NotConfirmMoneyList> GetNotConfirmMoneyDate(TransactionNotCompareFilter filter)
        {

            var data = GetDataNotConfirmMoneyDate(filter);
            return data.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize);
        }

        public int GetNotConfirmMoneyDateCount(TransactionNotCompareFilter filter)
        {
            var data = GetDataNotConfirmMoneyDate(filter);
            return data.Count();

        }

        private IEnumerable<NotConfirmMoneyList> GetDataNotConfirmMoneyDate(TransactionNotCompareFilter filter)
        {
            var result = new List<NotConfirmMoneyList>();
            IEnumerable<BookingTransactionPayment> data = _gridRepository.GetNotConfirmMoneyDate(filter);
            var groupData = data.GroupBy(o => o.DatePayment.Date);
            foreach (var item in groupData)
            {
                var outItem = new NotConfirmMoneyList();
                outItem.DateId = item.FirstOrDefault()?.DatePayment.Date;
                foreach (var i in item)
                {
                    outItem.NumberNotConfirm += 1;
                    outItem.TotalMoney += i.TotalMoney;
                }
                result.Add(outItem);
            }
            return result;
        }

        public async Task<RespondData> CancelConfirmMoneyData(IEnumerable<BookingTransactionPayment> sendata, string userName, string userId)
        {
            return await _gridRepository.CancelConfirmMoneyData(sendata, userName, userId);
        }
        public string CurrentFolderDoiXoat() => _doiXoatFolder + "Files\\In\\" + DateTime.Now.ToString("yyyy") + "\\" + DateTime.Now.ToString("MM") + "\\";
        public async Task<FtpTransJobHistory> SaveFtpTransJobHistory(FtpTransJobHistory model)
        {
            return await _gridRepository.SaveFtpTransJobHistory(model);
        }

        public async void DownloadFileInViaFtp()
        {
            var filename = "INC_SB_BRG_" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + ".xlsx";
            var mailSetting = GetOutEmailSetting();
            var todayDow = GetDowString(DateTime.Today);
            var holiday = _holidayRepo.Find(x => x.DateId.Date == DateTime.Today).FirstOrDefault();

            if (holiday == null && mailSetting.transaction_out_email_dow.Contains(todayDow, StringComparison.InvariantCultureIgnoreCase))
            {
                var notSentLogDateFileOut = new List<LogDateFileOut>();
                notSentLogDateFileOut = _logDateFileOutRepo.Find(x => x.Date.Date < DateTime.Today && x.Status == LogDateFileOutStatus.created.ToString()
                                                    && x.FileType == Enums.FileType.In.ToString()).ToList();
                notSentLogDateFileOut.Add(new LogDateFileOut
                {
                    Id = Guid.Empty,
                    Date = DateTime.Today.AddDays(-1),
                });
                if (notSentLogDateFileOut != null && notSentLogDateFileOut.Any())
                {
                    try
                    {
                        foreach (var item in notSentLogDateFileOut)
                        {
                            filename = "INC_SB_BRG_" + item.Date.ToString("yyyyMMdd") + ".xlsx";
                            DownloadFileTransc(filename, filename, "FTP Service", item.Date);

                            if (item.Id != Guid.Empty)
                            {
                                item.Status = LogDateFileOutStatus.sent.ToString();
                                item.UpdatedDate = DateTime.Today;
                                item.UpdatedUser = "FTP Service";
                                _logDateFileOutRepo.Update(item);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        using (LogContext.PushProperty("MethodName", "DownloadFileInViaFtp"))
                        {
                            _log.LogError(e.Message);
                            _log.LogError(e.StackTrace);
                        }
                    }
                }
            }
            else
            {
                await this._gridRepository.CreateNewLogFileOutDate(FileType.In.ToString());
            }
        }


        public async void DownloadFileTransc(string remoteFilePath, string filename, string userId, DateTime dateTrans)
        {
            //var dateIn = _gridRepository.GetDateNotHaveFileIn();
            var localPath = CurrentFolderDoiXoat() + filename;
            var ftpFile = await _sftpService.DownloadFileTransc(remoteFilePath, CurrentFolderDoiXoat() + filename, filename, userId, dateTrans);
            if (ftpFile.IsFtpFile)
            {
                var saveModel = new TransactionFilterModel()
                {
                    FileName = filename,
                    FilePath = localPath,
                    FindDate = dateTrans,
                    UserId = userId
                };
                var result = await SaveFileinSeabankData(saveModel);
                if (result.IsSuccess)
                {
                    ftpFile.IsReadFile = true;
                    ftpFile.IsInsertData = true;
                }
                else
                {
                    ftpFile.IsReadFile = false;
                    ftpFile.IsInsertData = false;
                    ftpFile.DescriptionError = result.Message;
                }
            }
            await SaveFtpTransJobHistory(ftpFile);
        }

        public PagingResponseEntity<FtpTransJobHistory> GetFtpTransJobHistory(TransactionFilterModel pagingModel)
        {
            PagingResponseEntity<FtpTransJobHistory> paging = _gridRepository.GetGetFtpTransJobHistoryPaging(pagingModel);


            return new PagingResponseEntity<FtpTransJobHistory>
            {
                Count = paging.Count,
                Data = paging.Data
            };
        }

        public bool CheckBrgAmin(string userId)
        {
            var user = _userRepository.GetById(new Guid(userId));
            if (user != null)
            {
                var org = user.Organization;
                if (org != null)
                {
                    if (org.Code == Constants.BrgCode)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string CheckTransCompare(DateTime? findDate)
        {
            var outHeader = _gridRepository.CheckTransCompare(findDate.Value);
            if (outHeader != null)
            {
                var org = outHeader.OutTransactionDetails.Where(x => x.IsActive).Select(s => s.OrgCode).Distinct();
                var mes = "Sân đã đối xoát: ";
                foreach (var i in org)
                {
                    mes += i + ",";
                }
                return mes;
            }
            return string.Empty;
        }

        public string GetStatusCompareTrans(TransactionFilterModel filter)
        {
            OutTransactionHeader outHeader = _gridRepository.GetStatusCompareTrans(filter);
            if (outHeader != null)
            {
                return outHeader.Status;
            }
            return string.Empty;
        }



        #endregion

        private struct ExcelCellValueMap
        {
            public ExcelCellValueMap(string propertyName, int columnIndex, ExcelCellValueMapCellType cellType)
            {
                PropertyName = propertyName;
                ColumnIndex = columnIndex;
                CellType = cellType;
            }

            public string PropertyName { get; }
            public int ColumnIndex { get; }
            public ExcelCellValueMapCellType CellType { get; }
        }

        private enum ExcelCellValueMapCellType
        {
            StringType = 0,
            DecimalType = 1,
            DateTimeType = 2,
            DateType = 3,
            NumberType = 4,
        }
    }
}
