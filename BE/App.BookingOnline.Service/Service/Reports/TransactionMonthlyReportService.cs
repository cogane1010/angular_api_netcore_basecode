using App.BookingOnline.Data;
using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Reports;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.BookingOnline.Service.IService.Common;
using App.BookingOnline.Service.Service.Reports;
using App.Core;
using App.Core.Configs;
using App.Core.Domain;
using App.Core.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RestSharp;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace App.BookingOnline.Service.Service.Common
{
    public class TransactionMonthlyReportService :
        BaseGridDataService<TransactionMonthlyReportDTO, TransactionMonthlyReport, TransactionMonthlyReportFilterModel, ITransactionMonthlyReportRepository>,
        ITransactionMonthlyReportService
    {
        private readonly ILogger _log;
        private readonly IAppUserRepository appUserRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IFileService _fileService;
        private readonly string _excelFileName = "transaction_month_rpt.xlsx";
        private readonly IOrganizationRepository _orgRepository;

        public IConfiguration Configuration { get; }

        public TransactionMonthlyReportService(ITransactionMonthlyReportRepository gridRepository, IAppUserRepository appUserRepository, IConfiguration configuration
          , INotificationRepository notificationRepository, ILogger<SmsHistoryService> logger, IFileService fileService, IOrganizationRepository orgRepository) : base(gridRepository)
        {
            this.appUserRepository = appUserRepository;
            _notificationRepository = notificationRepository;
            Configuration = configuration;
            _fileService = fileService;
            _log = logger;
            _orgRepository = orgRepository;
        }

        public TransactionMonthlyReportResult GetReportData(TransactionMonthlyReportFilterModel pagingModel)
        {
            var res = new TransactionMonthlyReportResult();
            var paging = _gridRepository.GetPagingTransactionMonthlyReport(pagingModel);

            var pagingData = new PagingResponseEntityDTO<TransactionMonthlyReportDTO>
            {
                Count = paging.Data.Any() ? paging.Data.FirstOrDefault().TotalRow : 0,
                Data = AutoMapperHelper.Map<TransactionMonthlyReport, TransactionMonthlyReportDTO,
                IEnumerable<TransactionMonthlyReport>, IEnumerable<TransactionMonthlyReportDTO>>(paging.Data)
            };
            res.PagingData = pagingData;
            if (pagingData.Count > 0)
            {
                res.NotReceivedAmtAfterPrevMonth = pagingData.Data.Sum(x => x.NotReceivedAmtAfterPrevMonth);
                res.MonthBookedAmt = pagingData.Data.Sum(x => x.MonthBookedAmt);
                res.MonthTransAmtForPrevMonths = pagingData.Data.Sum(x => x.MonthTransAmtForPrevMonths);
                res.MonthTransAmt = pagingData.Data.Sum(x => x.MonthTransAmt);
                res.TotalMonthAmt = pagingData.Data.Sum(x => x.TotalMonthAmt);
                res.RemainNotReceivedAmtAfterPrevMonth = pagingData.Data.Sum(x => x.RemainNotReceivedAmtAfterPrevMonth);
                res.MonthNotReceivedAmt = pagingData.Data.Sum(x => x.MonthNotReceivedAmt);
                res.TotalNotReceivedAmt = pagingData.Data.Sum(x => x.TotalNotReceivedAmt);
            }
            return res;
        }

        public Tuple<MemoryStream, string> ExportExcel(TransactionMonthlyReportFilterModel filter)
        {
            string filePath = string.Format("{0}\\{1}\\{2}", _fileService.GetRootPath(), AppConfigs.REPORT_PATH, _excelFileName);
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                MemoryStream stream;
                // If you use EPPlus in a noncommercial context
                // according to the Polyform Noncommercial license:
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(fs))
                {
                    var _ws = package.Workbook.Worksheets["Sheet1"];

                    #region Fill data
                    int currColIdx = 1, totalCols = 12, _startRow = 7, currRowIdx = _startRow;

                    // Data
                    filter.PageIndex = -1;
                    var data = GetReportData(filter);

                    // Tên, địa chỉ
                    var org = _orgRepository.GetById(new Guid(filter.UserOrgId)).Result;
                    if (org != null && org.OrganizationInfos != null && org.OrganizationInfos.Any())
                    {
                        _ws.Cells[1, 1].Value = org.OrganizationInfos[0].InvoiceName;
                        _ws.Cells[2, 1].Value = "Địa chỉ: " + org.OrganizationInfos[0].InvoiceAddress;
                    }

                    // Filter information
                    _ws.Cells[3, 1].Value = string.Format("THÁNG {0} NĂM {1}", filter.FilterDate.Value.ToString("MM"), filter.FilterDate.Value.ToString("yyyy"));

                    // dữ liệu
                    if (data.PagingData.Count > 0)
                    {
                        int i = 1;
                        foreach (var row in data.PagingData.Data)
                        {
                            currColIdx = 1;
                            _ws.Cells[currRowIdx, currColIdx++].Value = i;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.SeaBankCustomerCode;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.CustomerName;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.GolfCourseCode;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.NotReceivedAmtAfterPrevMonth;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.MonthBookedAmt;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.MonthTransAmtForPrevMonths;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.MonthTransAmt;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.TotalMonthAmt;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.RemainNotReceivedAmtAfterPrevMonth;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.MonthNotReceivedAmt;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.TotalNotReceivedAmt;

                            i++;
                            currRowIdx++;
                        }

                        // Dòng tổng
                        _ws.Cells[currRowIdx, 1].Value = "Tổng";
                        _ws.Cells[currRowIdx, 5].Value = data.NotReceivedAmtAfterPrevMonth;
                        _ws.Cells[currRowIdx, 6].Value = data.MonthBookedAmt;
                        _ws.Cells[currRowIdx, 7].Value = data.MonthTransAmtForPrevMonths;
                        _ws.Cells[currRowIdx, 8].Value = data.MonthTransAmt;
                        _ws.Cells[currRowIdx, 9].Value = data.TotalMonthAmt;
                        _ws.Cells[currRowIdx, 10].Value = data.RemainNotReceivedAmtAfterPrevMonth;
                        _ws.Cells[currRowIdx, 11].Value = data.MonthNotReceivedAmt;
                        _ws.Cells[currRowIdx, 12].Value = data.TotalNotReceivedAmt;
                        _ws.Cells[currRowIdx, 1, currRowIdx, 4].Merge = true;
                        _ws.Cells[currRowIdx, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    // Assign borders
                    var modelTable = _ws.Cells[_startRow, 1, currRowIdx, totalCols];
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Font.Size = 12;

                    // autofit columns
                    _ws.Cells[_startRow - 1, 1, currRowIdx, totalCols].AutoFitColumns();

                    // Format text
                    ExcelCommon.SetExcelNumberFormat(_ws.Cells[_startRow, 5, currRowIdx, totalCols], true);

                    // Column width
                    _ws.Column(6).Width = 15;

                    // font
                    _ws.Cells[1, 1, currRowIdx, totalCols].Style.Font.Name = "Times New Roman";
                    #endregion

                    stream = new MemoryStream(package.GetAsByteArray());
                }

                return new Tuple<MemoryStream, string>(stream, _excelFileName);
            }
        }
    }


}
