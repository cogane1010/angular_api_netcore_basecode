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
    public class TransactionDetailReportService :
        BaseGridDataService<TransactionDetailReportDTO, TransactionDetailReport, TransactionDetailReportFilterModel, ITransactionDetailReportRepository>,
        ITransactionDetailReportService
    {
        private readonly ILogger _log;
        private readonly IAppUserRepository appUserRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IFileService _fileService;
        private readonly string _excelFileName = "transaction_detail_rpt.xlsx";
        private readonly IOrganizationRepository _orgRepository;

        public IConfiguration Configuration { get; }

        public TransactionDetailReportService(ITransactionDetailReportRepository gridRepository, IAppUserRepository appUserRepository, IConfiguration configuration
          , INotificationRepository notificationRepository, ILogger<SmsHistoryService> logger, IFileService fileService, IOrganizationRepository orgRepository) : base(gridRepository)
        {
            this.appUserRepository = appUserRepository;
            _notificationRepository = notificationRepository;
            Configuration = configuration;
            _fileService = fileService;
            _log = logger;
            _orgRepository = orgRepository;
        }

        public TransactionDetailReportResult GetReportData(TransactionDetailReportFilterModel pagingModel)
        {
            var res = new TransactionDetailReportResult();
            var paging = _gridRepository.GetPagingTransactionDetailReport(pagingModel);

            var pagingData= new PagingResponseEntityDTO<TransactionDetailReportDTO>
            {
                Count = paging.Data.Any() ? paging.Data.FirstOrDefault().TotalRow : 0,
                Data = AutoMapperHelper.Map<TransactionDetailReport, TransactionDetailReportDTO,
                IEnumerable<TransactionDetailReport>, IEnumerable<TransactionDetailReportDTO>>(paging.Data)
            };
            res.PagingData = pagingData;
            if (pagingData.Count > 0)
            {
                res.TransAmt = pagingData.Data.Sum(x => x.TransAmt);
                res.CusCancelReturnBf24HAmt = pagingData.Data.Sum(x => x.CusCancelReturnBf24HAmt);
                res.NotReceivedAmt = pagingData.Data.Sum(x => x.NotReceivedAmt);
                res.TotalReceived = pagingData.Data.Sum(x => x.TotalReceived);
            }
            return res;
        }

        public Tuple<MemoryStream, string> ExportExcel(TransactionDetailReportFilterModel filter)
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
                    int currColIdx = 1, totalCols = 16, _startRow = 7, currRowIdx = _startRow;

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
                    _ws.Cells[4, 1].Value = string.Format("Từ ngày {0} Đến ngày {1}", filter.FromDate.Value.ToString("dd/MM/yyyy"), filter.ToDate.Value.ToString("dd/MM/yyyy"));

                    // dữ liệu
                    if (data.PagingData.Count > 0)
                    {
                        int i = 1;
                        foreach (var row in data.PagingData.Data)
                        {
                            currColIdx = 1;
                            _ws.Cells[currRowIdx, currColIdx++].Value = i;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.SeaBankCustomerCode;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.SeaBankCustomerName;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.BookingCode;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.GolfOrgCode;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.BookingDate;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.CreditAccount;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.card_type;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.CardMaskingNo;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.DebitAccountNo;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.TransDesciption;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.TransDate;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.TransAmt;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.CusCancelReturnBf24HAmt;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.NotReceivedAmt;
                            _ws.Cells[currRowIdx, currColIdx++].Value = row.TotalReceived;

                            i++;
                            currRowIdx++;
                        }

                        // Dòng tổng
                        _ws.Cells[currRowIdx, 1].Value = "Tổng";
                        _ws.Cells[currRowIdx, 13].Value = data.TransAmt;
                        _ws.Cells[currRowIdx, 14].Value = data.CusCancelReturnBf24HAmt;
                        _ws.Cells[currRowIdx, 15].Value = data.NotReceivedAmt;
                        _ws.Cells[currRowIdx, 16].Value = data.TotalReceived;
                        _ws.Cells[currRowIdx, 1, currRowIdx, 10].Merge = true;
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
                    ExcelCommon.SetExcelNumberFormat(_ws.Cells[_startRow, 13, currRowIdx, totalCols], true);
                    ExcelCommon.SetExcelDateFormat(_ws.Cells[_startRow, 6, currRowIdx, 6]);
                    ExcelCommon.SetExcelTimeFormat(_ws.Cells[_startRow, 12, currRowIdx, 12]);

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
