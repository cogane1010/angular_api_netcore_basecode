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
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RestSharp;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.IO;
using System.Linq;

namespace App.BookingOnline.Service.Service.Common
{
    public class ReportsService :
        BaseGridDataService<BookingDTO, Booking, BookingFilterModel, IReportsRepository>,
        IReportsService
    {
        private readonly ILogger _log;
        private readonly IAppUserRepository appUserRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IFileService _fileService;
        public IConfiguration Configuration { get; }

        public ReportsService(IReportsRepository gridRepository, IAppUserRepository appUserRepository, IConfiguration configuration
          , IFileService fileService, IBookingRepository bookingRepository, INotificationRepository notificationRepository, ILogger<SmsHistoryService> logger) : base(gridRepository)
        {
            this.appUserRepository = appUserRepository;
            _notificationRepository = notificationRepository;
            _bookingRepository = bookingRepository;
            Configuration = configuration;
            _log = logger;
            _fileService = fileService;
        }

        public PagingResponseEntityDTO<ReportCancelBookingDTO> GetPagingCancelReport(BookingFilterModel pagingModel)
        {
            var paging = _gridRepository.GetPagingCancelReport(pagingModel);

            return new PagingResponseEntityDTO<ReportCancelBookingDTO>
            {
                Count = paging.Data.Any() ? paging.Data.FirstOrDefault().TotalRow : 0,
                Data = AutoMapperHelper.Map<ReportCancelBooking, ReportCancelBookingDTO,
                IEnumerable<ReportCancelBooking>, IEnumerable<ReportCancelBookingDTO>>(paging.Data)
            };
        }

        public PagingResponseEntityDTO<CustomerDTO> GetPagingRegistrationReport(BookingFilterModel filter)
        {
            var userOrg = _bookingRepository.GetOrganizationById(new Guid(filter.UserOrgId));
            if (userOrg.Code != Constants.BrgCode)
            {
                filter.C_Org_Id = new Guid(filter.UserOrgId);
            }
            else
            {
                if (filter.C_Org_Id == userOrg.Id)
                {
                    filter.C_Org_Id = Guid.Empty;
                }
            }
            var paging = _gridRepository.GetPagingRegistrationReport(filter);

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Customer, CustomerDTO>()
                .AfterMap((s, d) => d.Total_Golf_CardNo = String.Join(", ", s.MemberCards.Select(a => a.Golf_CardNo).Distinct()))
                .AfterMap((s, d) => d.Total_Course = String.Join(", ", s.MemberCards?.SelectMany(a => a.MemberCardCourses
                                        .Select(i => i.Course?.Name)).Distinct()))
                .AfterMap((s, d) => d.Total_Org = String.Join(", ", s.MemberCards?.SelectMany(a => a.MemberCardCourses
                                        .Select(i => i.Course?.Organization?.Code)).Distinct()));
                c.CreateMap<Organization, OrganizationDTO>();
                c.CreateMap<Course, CourseDTO>();
                c.CreateMap<MemberCard, MemberCardDTO>()
                .ForMember(d => d.CoursesMemberCard, o => o.MapFrom(s => s.MemberCardCourses));
                c.CreateMap<MemberCardCourse, MemberCardCourseDTO>();
                c.CreateMap<CustomerGroup, CustomerGroupDTO>();
            });
            var mapper = config.CreateMapper();
            var cusData = mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDTO>>(paging.Data);

            return new PagingResponseEntityDTO<CustomerDTO>
            {
                Count = paging.Count,
                Data = cusData
            };
        }

        public PagingResponseEntityDTO<BookingDTO> GetPagingBookingReport(BookingFilterModel filter)
        {
            var userOrg = _bookingRepository.GetOrganizationById(new Guid(filter.UserOrgId));
            if (userOrg.Code != Constants.BrgCode)
            {
                filter.C_Org_Id = new Guid(filter.UserOrgId);
            }
            else
            {
                if (filter.C_Org_Id == userOrg.Id)
                {
                    filter.C_Org_Id = Guid.Empty;
                }
            }
            var paging = _gridRepository.GetPagingBookingReport(filter);
            var customerList = _gridRepository.GetCustomersInfo(paging.Data.Select(s => s.UserId));

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Booking, BookingDTO>()
                .ForMember(d => d.BookingTeetime,
                 o => o.MapFrom(s => s.BookingLines))
                .AfterMap((s, d) => d.Golfers = d.BookingTeetime.Count())
                .AfterMap((s, d) => d.CardFullName = s.AppUser?.FullName)
                .AfterMap((s, d) => d.CustomerCode = s.AppUser?.UserName)
                .AfterMap((s, d) => d.ShowUpCount = s.BookingLines?.Count(b => b.Is_NoShow && b.IsActive))
                .AfterMap((s, d) =>
                {
                    var memberCard = customerList.FirstOrDefault(x => x.UserId == s.UserId)?.MemberCards
                    .FirstOrDefault(a => a.MemberCardCourses.Any(b => b.C_Course_Id == s.C_Course_Id) && !a.IsDelete && a.IsActive);
                    if (memberCard != null)
                    {
                        d.GolfCardNo = memberCard.Golf_CardNo + "-Member";
                    }
                    else
                    {
                        memberCard = customerList.FirstOrDefault(x => x.UserId == s.UserId)?.MemberCards
                    .FirstOrDefault();
                        if (memberCard != null)
                        {
                            d.GolfCardNo = memberCard.Golf_CardNo + "-Member Brg";
                        }
                        else
                        {
                            d.GolfCardNo = "Guest";
                        }
                    }
                })
                .AfterMap((s, d) =>
                {
                    for (var i = 0; i < s.BookingLines.Count; i++)
                    {
                        if (i > 0 && !d.TotalTeetimeDisplay.Contains(s.BookingLines[i].Tee_Time.Value.ToString("HH:mm")))
                        {
                            d.TotalTeetimeDisplay += "-" + s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                        }
                        else if (i == 0)
                        {
                            d.TotalTeetimeDisplay = s.BookingLines[i].Tee_Time.Value.ToString("HH:mm");
                        }
                    }
                })
                .AfterMap((s, d) => d.OrgName = s.Organization?.Name)
                .AfterMap((s, d) => d.CourseName = s.Course?.Name);
                c.CreateMap<BookingLine, BookingLineDTO>();
                c.CreateMap<Course, CourseDTO>();
                c.CreateMap<CustomerGroup, CustomerGroupDTO>();
                c.CreateMap<BookingSpecialRequest, BookingSpecialRequestDTO>();

            });
            var mapper = config.CreateMapper();
            var dto = mapper.Map<IEnumerable<Booking>, IEnumerable<BookingDTO>>(paging.Data);

            return new PagingResponseEntityDTO<BookingDTO>
            {
                Count = paging.Count,
                Data = dto
            };
        }

        public MemoryStream GetExportRBookingExcel(BookingFilterModel filter)
        {
            var dbObjs = GetPagingBookingReport(filter);
            if (dbObjs != null && dbObjs.Data.Any())
            {
                var res = FillExcelExportRBooking(dbObjs.Data, filter);
                return res;
            }
            return null;
        }

        private MemoryStream FillExcelExportRBooking(IEnumerable<BookingDTO> data, BookingFilterModel filter)
        {
            int startDataRow = 7, rowIdx = startDataRow, colIdx = 1, totalCol = 11;
            string filePath = string.Format("{0}\\{1}\\{2}", _fileService.GetRootPath(), AppConfigs.REPORT_PATH, "Bao_cao_dat_cho.xlsx");
            if (data != null)
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    MemoryStream stream;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(fs))
                    {
                        var ws = package.Workbook.Worksheets["Sheet1"];
                        filter.PageIndex = -1;

                        if (data.Any())
                        {
                            int i = 1;
                            foreach (var row in data)
                            {
                                colIdx = 1;
                                ws.Cells[rowIdx, colIdx++].Value = i++;
                                ws.Cells[rowIdx, colIdx++].Value = row.CustomerCode;
                                ws.Cells[rowIdx, colIdx++].Value = row.CardFullName;
                                ws.Cells[rowIdx, colIdx++].Value = row.GolfCardNo;
                                ws.Cells[rowIdx, colIdx++].Value = row.CreatedDate.ToString("dd/MM/yyyy");
                                ws.Cells[rowIdx, colIdx++].Value = row.BookingCode;
                                ws.Cells[rowIdx, colIdx++].Value = row.OrgName;
                                ws.Cells[rowIdx, colIdx++].Value = row.CourseName;
                                ws.Cells[rowIdx, colIdx++].Value = row.DateId.ToString("dd/MM/yyyy");
                                ws.Cells[rowIdx, colIdx++].Value = row.TotalTeetimeDisplay;
                                ws.Cells[rowIdx, colIdx++].Value = row.Golfers;
                                ws.Cells[rowIdx, colIdx++].Value = row.ShowUpCount;
                                ws.Cells[rowIdx, colIdx++].Value = row.Status;
                                rowIdx++;
                            }

                            ws.Cells[rowIdx + 2, colIdx++].Value = data.Count();
                            ws.Cells[rowIdx + 2, 11].Value = data.Sum(s => s.Golfers);
                            ws.Cells[rowIdx + 2, 12].Value = data.Sum(s => s.ShowUpCount);
                        }
                        ws.Cells[4, 5].Value = string.Format("Thời gian từ: {0}", filter.TimeFrom.Value.Date.ToString("dd/MM/yyyy"));
                        ws.Cells[4, 9].Value = string.Format("Thời gian đến: {0}", filter.TimeTo.Value.Date.ToString("dd/MM/yyyy"));
                        stream = new MemoryStream(package.GetAsByteArray());
                    }
                    return stream;
                }
            }
            return null;
        }

        public MemoryStream GetExportRegistrationExcel(BookingFilterModel filter)
        {
            int startDataRow = 7, rowIdx = startDataRow, colIdx = 1, totalCol = 11;
            string filePath = string.Format("{0}\\{1}\\{2}", _fileService.GetRootPath(), AppConfigs.REPORT_PATH, "bao_cao_tai_khoan.xlsx");
            var data = GetPagingRegistrationReport(filter);
            if (data.Data != null && data.Data.Any())
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    MemoryStream stream;
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(fs))
                    {
                        var ws = package.Workbook.Worksheets["Sheet1"];
                        filter.PageIndex = -1;
                        int i = 1;
                        foreach (var row in data.Data)
                        {
                            colIdx = 1;
                            ws.Cells[rowIdx, colIdx++].Value = i++;
                            ws.Cells[rowIdx, colIdx++].Value = row.CustomerCode;
                            ws.Cells[rowIdx, colIdx++].Value = row.FullName;
                            ws.Cells[rowIdx, colIdx++].Value = row.CreatedDate.ToString("dd/MM/yyyy");
                            ws.Cells[rowIdx, colIdx++].Value = row.Total_Org;
                            ws.Cells[rowIdx, colIdx++].Value = row.Total_Course;
                            ws.Cells[rowIdx, colIdx++].Value = row.Total_Golf_CardNo;
                            ws.Cells[rowIdx, colIdx++].Value = row.MobilePhone;
                            ws.Cells[rowIdx, colIdx++].Value = row.Email;
                            rowIdx++;
                        }

                        ws.Cells[4, 4].Value = string.Format("Thời gian từ: {0}", filter.TimeFrom.Value.Date.ToString("dd/MM/yyyy"));
                        ws.Cells[4, 7].Value = string.Format("Thời gian đến: {0}", filter.TimeTo.Value.Date.ToString("dd/MM/yyyy"));
                        stream = new MemoryStream(package.GetAsByteArray());
                    }
                    return stream;
                }
            }
            return null;
        }
    }


}
