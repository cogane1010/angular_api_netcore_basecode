using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using App.Core.Helper;
using System.Data.SqlClient;
using Dapper;
using System;
using Microsoft.EntityFrameworkCore;
using static App.Core.Enums;
using App.BookingOnline.Data.Identity;
using App.Core;
using App.BookingOnline.Data.Models.Golf;
using RestSharp.Extensions;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace App.BookingOnline.Data.Repositories
{
    public class BookingLineRepository : BaseGridRepository<BookingLine, BookingLineFilterModel>, IBookingLineRepository
    {
        protected readonly BookingOnlineDbContext _context;
        private readonly IBaseRepository<Booking> _bookingRepo;
        private readonly IBaseRepository<BookingSpecialRequest> _bookingSpecialRequestRepo;
        private readonly IBaseRepository<Organization> _organizationRepo;
        private readonly IBaseRepository<AppUser> _userRepo;
        private readonly IBaseRepository<Course> _courseRepo;
        private readonly IBaseRepository<GF_CourseTemplate> _courseTemplateRepo;
        private readonly IBaseRepository<GF_CourseTemplateLine> _courseTemplateLineRepo;
        private readonly IHolidayRepository _holidayRepository;
        private readonly ILogger _log;


        public BookingLineRepository(IUnitOfWork unitOfWork, BookingOnlineDbContext context, IHolidayRepository holidayRepository,
                        ILogger<BookingLineRepository> logger) : base(unitOfWork)
        {
            _context = context;
            _bookingRepo = _unitOfWork.GetDataRepository<Booking>();
            _bookingSpecialRequestRepo = _unitOfWork.GetDataRepository<BookingSpecialRequest>();
            _organizationRepo = _unitOfWork.GetDataRepository<Organization>();
            _userRepo = _unitOfWork.GetDataRepository<AppUser>();
            _courseRepo = _unitOfWork.GetDataRepository<Course>();
            _courseTemplateRepo = _unitOfWork.GetDataRepository<GF_CourseTemplate>();
            _courseTemplateLineRepo = _unitOfWork.GetDataRepository<GF_CourseTemplateLine>();
            _holidayRepository = holidayRepository;
            _log = logger;
        }

        public IEnumerable<BookingLine> GetPagingBookedData(BookingLineFilterModel pagingModel)
        {
            var query = _repo.SelectWhere(x => (pagingModel.DateId == null || (pagingModel.DateId != null && x.DateId.Value.Date == pagingModel.DateId.Value.Date))
                                               && (x.BookingStatus == StatusBookingLine.booked.ToString())
                                               && (pagingModel.Part == null || x.Part == pagingModel.Part)
                                               && (pagingModel.C_Org_Id == null || x.Booking.C_Org_Id == pagingModel.C_Org_Id)
                                               && (pagingModel.C_Course_Id == null || x.Booking.C_Course_Id == pagingModel.C_Course_Id)
                                               && (pagingModel.Search == null || x.Booking.AppUser.FullName.Contains(pagingModel.Search)
                                                || x.Booking.AppUser.PhoneNumber.Contains(pagingModel.Search))
                                               )
                .Include(i => i.Booking).Include(i => i.Booking.AppUser).OrderBy(o => o.Tee_Time);

            List<BookingLine> data = new List<BookingLine>();
            if (pagingModel.PageSize > 0)
            {
                data = query.Skip(pagingModel.PageIndex * pagingModel.PageSize)
                        .Take(pagingModel.PageSize).ToList();
            }
            else
            {
                data = query.ToList();
            }

            return data;
        }

        public List<BookStatByOrg> GetStats(BookingStatisticFilterModel filter)
        {
            //string conString = _context.Database.GetDbConnection().ConnectionString;

            //using (var conn = new SqlConnection(conString))
            //{
            //    var p = new DynamicParameters();
            //    p.Add("date", filter.DateId);
            //    p.Add("orgId", filter.C_Org_Id);
            //    var res = conn.ExecuteProcedure<BookingLineStatistic>("getBookingLineStatistic", p);
            //    return res.Item1.ToList();
            //}
            var orgs = _courseRepo.SelectWhere(x => x.IsActive && (x.C_Org_Id == filter.C_Org_Id || filter.C_Org_Id == null)).Include(i => i.Organization)
                            .ToList().GroupBy(g => g.C_Org_Id);
            var book = _repo.SelectWhere(x => x.IsActive && x.DateId.Value.Date == filter.DateId.Value.Date
                                && (x.C_Org_Id == filter.C_Org_Id || filter.C_Org_Id == null)
                                && x.BookingStatus == StatusBookingLine.booked.ToString())
                                .Include(i => i.Booking);

            List<BookStatByOrg> data = new List<BookStatByOrg>();
            foreach (var org in orgs)
            {
                var byOrg = new BookStatByOrg();
                byOrg.OrganizationName = org.FirstOrDefault().Organization.Name;
                byOrg.C_Org_Id = org.FirstOrDefault().Organization.Id;
                byOrg.NumRows = org.Count() * 3;
                byOrg.BookByCourse = new List<BookingLineStatistic>();

                foreach (var co in org)
                {
                    var bookDa = book.Where(i => i.Booking.C_Course_Id == co.Id).OrderBy(o => o.Part);

                    var item = new BookingLineStatistic();
                    item.CourseName = co.Name;
                    item.NoBooking = bookDa.Count();
                    item.NoReality = bookDa.Count(c => !c.Is_NoShow);
                    item.NumRows = org.Count();
                    item.BookStatByParts = new List<BookStatByPart>();

                    for (var a = 1; a <= 3; a++)
                    {
                        var part = new BookStatByPart();
                        var partDa = bookDa.Where(x => x.Part == a.ToString());
                        if (a == 1)
                        {
                            part.PartName = "Buối sáng";
                        }
                        if (a == 2)
                        {
                            part.PartName = "Buổi chiều";
                        }
                        if (a == 3)
                        {
                            part.PartName = "Buổi tối";
                        }
                        part.NoBooking = partDa.Count();
                        part.NoReality = partDa.Count(c => !c.Is_NoShow);
                        item.BookStatByParts.Add(part);
                    }
                    byOrg.BookByCourse.Add(item);
                    byOrg.TotalBooking += item.NoBooking;
                    byOrg.TotalNoReality += item.NoReality;
                }
                data.Add(byOrg);
            }
            return data;
        }

        public List<BookingLine> GetbyBookingId(Guid BookingId)
        {
            //string conString = _context.Database.GetDbConnection().ConnectionString;

            //using (var conn = new SqlConnection(conString))
            //{
            //    var p = new DynamicParameters();
            //    p.Add("date", filter.DateId);
            //    p.Add("orgId", filter.C_Org_Id);
            //    var res = conn.ExecuteProcedure<BookingLineStatistic>("getBookingLineStattistic", p);
            //    return res.Item1.ToList();
            //}

            return this._repo.SelectWhere(x => x.Id == BookingId).ToList();

        }

        public IEnumerable<BookingSpecialRequest> GetOtherBooking(Guid gF_Booking_Id)
        {
            return _bookingSpecialRequestRepo.SelectWhere(x => x.IsActive && x.GF_Booking_Id == gF_Booking_Id).Include(i => i.BookingOtherType);
        }

        public IEnumerable<Organization> GetOrgByUserId(string userId)
        {
            var result = _userRepo.SelectWhere(a => a.Id == userId)
                            .Join(_organizationRepo.SelectWhere(x => x.IsActive).Include(i => i.Courses), pro => pro.C_Org_Id, cou => cou.Id,
                (pro, cou) => new { pro, cou }).Select(s => new Organization
                {
                    Id = s.cou.Id,
                    Name = s.cou.Name,
                    Code = s.cou.Code,
                    C_OrgType_Id = s.cou.C_OrgType_Id,
                    OrderValue = s.cou.OrderValue,
                    IsActive = s.cou.IsActive,
                    IsSummary = s.cou.IsSummary,
                    Courses = s.cou.Courses
                });

            if (result.Count() == 1)
            {
                if (result.FirstOrDefault().Code == Constants.BrgCode)
                {
                    result = _organizationRepo.SelectWhere(x => x.IsActive).Include(i => i.Courses).OrderBy(o => o.OrderValue);
                }
            }
            else
            {
                if (result.Any(x => x.Code == Constants.BrgCode))
                {
                    foreach (var item in result)
                    {
                        if (item.Code == Constants.BrgCode)
                        {
                            item.Courses = _courseRepo.SelectWhere(x => x.IsActive).ToList();
                        }
                    }
                }
            }

            return result;

        }

        public IEnumerable<GF_CourseTemplateLine> GetDataTeesheet(BookingLineFilterModel filter)
        {
            var dw = (int)filter.DateId.Value.DayOfWeek + 1;
            var isHoliday = _holidayRepository.CheckDateHoliday(filter.C_Org_Id.Value, filter.DateId.Value);

            var course = _courseTemplateRepo.SelectWhere(x => x.IsActive && x.C_Org_Id == filter.C_Org_Id
                                    && x.StartDate.Value.Date <= filter.DateId.Value.Date && x.EndDate.Value.Date >= filter.DateId.Value.Date
                                    && (x.DOW.Contains(dw.ToString()) || (isHoliday && x.DOW.Contains("0")))).FirstOrDefault();
            if (course != null)
            {
                return _courseTemplateLineRepo.SelectWhere(x => x.IsActive && x.C_Org_Id == filter.C_Org_Id
                                        && x.C_Course_Id == filter.C_Course_Id && x.Part == filter.Part
                                        && x.GF_CourseTemplate_Id == course.Id).OrderBy(o => o.Part);
            }
            return null;
        }

        public GF_CourseTemplateLine GetCourseTemplateLineByTeetime(BookingLineFilterModel filter)
        {
            var dw = (int)filter.Tee_Time.Value.DayOfWeek + 1;
            try
            {
                var isHoliday = _holidayRepository.CheckDateHoliday(filter.C_Org_Id.Value, filter.Tee_Time.Value);

                var courseTemplate = _courseTemplateRepo.SelectWhere(x => x.IsActive && x.C_Org_Id == filter.C_Org_Id
                                        && x.StartDate.Value.Date <= filter.Tee_Time.Value.Date && x.EndDate.Value.Date >= filter.Tee_Time.Value.Date
                                        && (x.DOW.Contains(dw.ToString()) || (isHoliday && x.DOW.Contains("0")))).FirstOrDefault();
                if (courseTemplate != null)
                {
                    var lines = _courseTemplateLineRepo.SelectWhere(x => x.IsActive && x.C_Org_Id == filter.C_Org_Id
                                            && x.C_Course_Id == filter.C_Course_Id 
                                            && x.GF_CourseTemplate_Id == courseTemplate.Id);
                    foreach (var line in lines)
                    {
                        var timeStart = line.StartTime.Split(':');
                        var start = new DateTime(filter.Tee_Time.Value.Year, filter.Tee_Time.Value.Month,
                               filter.Tee_Time.Value.Day, Convert.ToInt32(timeStart[0]), Convert.ToInt32(timeStart[1]), 0);

                        var endStart = line.EndTime.Split(':');
                        var end = new DateTime(filter.Tee_Time.Value.Year, filter.Tee_Time.Value.Month,
                            filter.Tee_Time.Value.Day, Convert.ToInt32(endStart[0]), Convert.ToInt32(endStart[1]), 0);

                        if (filter.Tee_Time.Value >= start && filter.Tee_Time.Value <= end)
                        {
                            return line;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "GetCourseTemplateLineByTeetime"))
                {
                    _log.LogError(e.Message);
                    _log.LogTrace(e.StackTrace);
                }
            }

            return null;
        }



        public void UpdateNoShow(Guid id, bool is_NoShow, string caddie_No, int hole, string description)
        {
            var line = _repo.SelectWhere(x => x.IsActive && x.Id == id).FirstOrDefault();
            if (line != null)
            {
                line.Is_NoShow = is_NoShow;
                line.Caddie_No = caddie_No;
                line.Hole = hole;
                line.Description = description;
                _repo.Update(line);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
