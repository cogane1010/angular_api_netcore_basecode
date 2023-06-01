﻿using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models.Reports;
using App.BookingOnline.Service.DTO.Reports;
using App.BookingOnline.Service.IService.Common;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace App.BookingOnline.WebApi.Controllers.Reports
{
    [ApiController]
    public class TransactionSummaryReportController : GridController<TransactionSummaryReportDTO, TransactionSummaryReportFilterModel, ITransactionSummaryReportService>
    {
        public TransactionSummaryReportController(ITransactionSummaryReportService service) : base(service)
        {

        }

        [HttpPost("GetReportData")]
        public RespondData GetReportData(TransactionSummaryReportFilterModel filter)
        {
            try
            {
                filter.UserId = UserId;
                filter.UserOrgId = CurOrgId;
                return Success(_service.GetReportData(filter));
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

        [HttpPost]
        [Route("exportExcel")]
        public async Task<IActionResult> ExportExcel(TransactionSummaryReportFilterModel model)
        {
            model.UserId = UserId;
            model.UserOrgId = CurOrgId;
            var result = await Task.Run(() => _service.ExportExcel(model));
            return File(result.Item1, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.Item2);
        }
    }
}
