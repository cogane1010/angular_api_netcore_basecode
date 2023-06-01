using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Reports;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static App.Core.Enums;

namespace App.BookingOnline.Data.Repositories.Common
{
    public class ReportsRepository : BaseGridRepository<Booking, BookingFilterModel>, IReportsRepository
    {
        private readonly IBaseRepository<Booking> _bookingRepo;
        private readonly IBaseRepository<BookingLine> _bookingLineRepo;
        private readonly IBaseRepository<Customer> _customerRepo;
        private readonly ILogger _log;
        public ReportsRepository(ILogger<ReportsRepository> logger, IUnitOfWork unitOfWork) : base(unitOfWork)
        {

            _log = logger;
            _bookingRepo = _unitOfWork.GetDataRepository<Booking>();
            _bookingLineRepo = _unitOfWork.GetDataRepository<BookingLine>();
            _customerRepo = _unitOfWork.GetDataRepository<Customer>();
        }


        public PagingResponseEntity<ReportCancelBooking> GetPagingCancelReport(BookingFilterModel pagingModel)
        {
            var paggingData = GetPagingCancelReportData(pagingModel);

            var data = new PagingResponseEntity<ReportCancelBooking>
            {
                Data = paggingData
            };
            data.Count = paggingData.Count();

            return data;
        }

        public IEnumerable<ReportCancelBooking> GetPagingCancelReportData(BookingFilterModel pagingModel)
        {
            var procParams = new Dictionary<string, object>()
            {
                {"@OrgId", pagingModel.UserOrgId},
                {"@Telephone", pagingModel.PhoneNumber},
                {"@FromTime", pagingModel.TimeFrom},
                {"@ToTime", pagingModel.TimeTo},
                {"@pageIndex", pagingModel.PageIndex},
                {"@pageSize", pagingModel.PageSize}
            };

            var result = _unitOfWork.ExecuteStoredProc<ReportCancelBooking>("Cancel_Booking_Reports", procParams).Result;
            if (result.Any())
            {
                return result;
            }

            return new List<ReportCancelBooking>();
        }

        public PagingResponseEntity<Customer> GetPagingRegistrationReport(BookingFilterModel filter)
        {
            var customer = _customerRepo.SelectWhere(x =>
                            (string.IsNullOrEmpty(filter.Fullname) || x.FullName.Contains(filter.Fullname))
                            && (filter.C_Org_Id == null || filter.C_Org_Id == Guid.Empty || x.MemberCards.Any(m => m.C_Org_Id == filter.C_Org_Id))
                            && (string.IsNullOrEmpty(filter.Mobilephone) || x.MobilePhone.Contains(filter.Mobilephone))
                            && (string.IsNullOrEmpty(filter.Email) || x.Email == filter.Email)
                            && (string.IsNullOrEmpty(filter.CustomerCode) || x.CustomerCode == filter.CustomerCode)
                            && (string.IsNullOrEmpty(filter.CardNo) || x.MemberCards.Any(a => a.Golf_CardNo == filter.CardNo && !a.IsDelete && a.IsActive))
                            && x.CreatedDate.Date >= filter.TimeFrom.Value.Date
                            && x.CreatedDate.Date <= filter.TimeTo.Value.Date)
                    .Include(i => i.MemberCards).ThenInclude(i => i.MemberCardCourses)
                    .ThenInclude(i => i.Course).ThenInclude(t => t.Organization)
                    .OrderByDescending(o => o.CreatedDate);

            var data = new PagingResponseEntity<Customer>
            {
                Data = filter.IsExcelExport ? customer.ToList() : customer.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize).ToList(),
                Count = customer.Count()
            };
            return data;
        }

        public PagingResponseEntity<Booking> GetPagingBookingReport(BookingFilterModel filter)
        {
            var bookings = GetExportRBookingData(filter);

            var data = new PagingResponseEntity<Booking>
            {
                Data = filter.IsExcelExport ? bookings : bookings.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize),
                Count = bookings.Count()
            };
            return data;
        }

        public IEnumerable<Customer> GetCustomersInfo(IEnumerable<string> userIds)
        {
            var customer = _customerRepo.SelectWhere(x => x.IsActive
                           && userIds.Contains(x.UserId))
                    .Include(i => i.MemberCards).ThenInclude(t => t.CustomerGroup)
                    .Include(i => i.MemberCards).ThenInclude(i => i.MemberCardCourses).ThenInclude(i => i.Course);
            return customer;
        }

        public IEnumerable<Booking> GetExportRBookingData(BookingFilterModel filter)
        {
            var bookings = _bookingRepo.SelectWhere(x => x.IsActive
                            && (filter.C_Org_Id == null || filter.C_Org_Id == Guid.Empty || x.C_Org_Id == filter.C_Org_Id)
                            && (x.Status == StatusBookingLine.booked.ToString() || x.Status == StatusBookingLine.cancel.ToString())
                            && (string.IsNullOrEmpty(filter.Status) || x.Status == filter.Status)
                            && (!filter.TimeFrom.HasValue || x.DateId.Value.Date >= filter.TimeFrom.Value.Date)
                            && (!filter.TimeTo.HasValue || x.DateId.Value.Date <= filter.TimeTo.Value.Date)
                            && (string.IsNullOrEmpty(filter.Fullname) || x.AppUser.FullName.Contains(filter.Fullname))
                            && (string.IsNullOrEmpty(filter.CardNo) || x.BookingLines.Any(a => a.CardNo == filter.CardNo)))
                    .Include(i => i.AppUser).Include(i => i.BookingLines).Include(i => i.Course).Include(i => i.Organization)
                    .OrderByDescending(o => o.CreatedDate);
            return bookings;
        }
    }


}
