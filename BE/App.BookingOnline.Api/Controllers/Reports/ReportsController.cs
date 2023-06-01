using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.IService.Common;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace App.BookingOnline.WebApi.Controllers.Reports
{
    [ApiController]
    public class ReportsController : GridController<BookingDTO, BookingFilterModel, IReportsService>
    {
        public ReportsController(IReportsService service) : base(service)
        {


        }

        [HttpPost("GetPagingCancelReport")]
        public RespondData GetPagingCancelReport(BookingFilterModel filter)
        {
            try
            {
                filter.UserId = UserId;
                return Success(_service.GetPagingCancelReport(filter));
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }

        [HttpPost("GetPagingRegistrationReport")]
        public RespondData GetPagingRegistrationReport(BookingFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                return Success(_service.GetPagingRegistrationReport(filter));
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }

        [HttpPost("GetPagingBookingReport")]
        public RespondData GetPagingBookingReport(BookingFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;
                return Success(_service.GetPagingBookingReport(filter));
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }

        [HttpPost("GetExportRBookingExcel")]
        public IActionResult GetExportRBookingExcel(BookingFilterModel filter)
        {
            filter.UserName = UserName;
            filter.UserOrgId = CurOrgId;
            filter.UserId = UserId;
            filter.IsExcelExport= true;
            var fileName = string.Format("bao_cao_dat_ve_{0}", DateTime.Now.ToString("yyyyMMdd"));
            var result = _service.GetExportRBookingExcel(filter);

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost("GetExportRegistrationExcel")]
        public IActionResult GetExportRegistrationExcel(BookingFilterModel filter)
        {
            filter.UserName = UserName;
            filter.UserOrgId = CurOrgId;
            filter.UserId = UserId;
            filter.IsExcelExport = true;
            var fileName = string.Format("bao_cao_tai_khoan_{0}", DateTime.Now.ToString("yyyyMMdd"));
            var result = _service.GetExportRegistrationExcel(filter);

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
