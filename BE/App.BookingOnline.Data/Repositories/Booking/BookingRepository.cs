using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using App.BookingOnline.Data.Models.Golf;
using App.BookingOnline.Data.Models.Common;
using System.Threading.Tasks;
using static App.Core.Enums;
using Microsoft.Extensions.Logging;
using App.Core;
using Microsoft.Extensions.Configuration;
using Serilog.Context;
using System.Linq.Expressions;
using App.BookingOnline.Data.Identity;
using Newtonsoft.Json;
using System.Security.Cryptography;
using App.BookingOnline.Data.Migrations;

namespace App.BookingOnline.Data.Repositories
{
    public class BookingRepository : BaseGridRepository<Booking, BookingFilterModel>, IBookingRepository
    {
        protected readonly BookingOnlineDbContext _context;
        private readonly ILogger _log;
        private readonly IBaseRepository<BookingLine> _bookingLineRepo;
        private readonly IBaseRepository<BookingSession> _bookingSessionRepo;
        private readonly IBaseRepository<BookingSessionLine> _bookingSessionLineRepo;
        private readonly IBaseRepository<TeeSheetLockLine> _teeSheetLockLineRepo;
        private readonly IBaseRepository<BookingSpecialRequest> _bookingSpecialRequestRepo;
        private readonly IBaseRepository<GF_CourseTemplateLine> _courseTemplateLineRepo;
        private readonly IBaseRepository<GF_CourseTemplate> _courseTemplateRepo;
        private readonly IBaseRepository<Course> _courseRepo;
        private readonly IBaseRepository<M_Promotion> _promotionRepo;
        private readonly IBaseRepository<M_Promotion_Course> _promotion_CourseRepo;
        private readonly IBaseRepository<CustomerGroup> _customerGroupRepo;
        private readonly IBaseRepository<Customer> _customerRepo;
        private readonly IBaseRepository<Organization> _organizationRepo;
        private readonly IBaseRepository<BookingOtherType> _bookingOtherTypeRepo;
        private readonly IBaseRepository<MemberCard> _memberCardRepo;
        private readonly IBaseRepository<MemberCardCourse> _memberCardCourseRepo;
        private readonly IBaseRepository<UserBankInfo> _userBankInfoRepo;
        private readonly IBaseRepository<PaymentType> _paymentTypeRepo;
        private readonly IBaseRepository<NotificationQueue> _notificationQueueRepo;
        private readonly IBaseRepository<CancelReason> _cancelReasonRepo;
        private readonly IBaseRepository<AppUser> _appUserRepo;
        private readonly IBaseRepository<BookingTransactionPayment> _transactionPaymentRepo;
        private readonly IBaseRepository<InTransactionHeader> _inTransactionHeaderRepo;
        private readonly IBaseRepository<InTransactionDetails> _inTransactionDetailsRepo;
        private readonly IBaseRepository<OutTransactionHeader> _outTransactionHeaderRepo;
        private readonly IBaseRepository<OutTransactionDetails> _outInTransactionDetailsRepo;
        private readonly IBaseRepository<TransactionLogs> _transactionLogsRepo;
        private readonly IBaseRepository<Setting> _settingRepo;
        private readonly IBaseRepository<TeeSheetLock> _teeSheetLockRepo;
        private readonly IBaseRepository<Holiday> _holidayRepo;

        public IConfiguration Configuration { get; }

        public BookingRepository(IUnitOfWork unitOfWork, BookingOnlineDbContext context,
            ILogger<BookingRepository> logger, IConfiguration configuration) : base(unitOfWork)
        {
            _context = context;
            _log = logger;
            Configuration = configuration;
            _bookingLineRepo = _unitOfWork.GetDataRepository<BookingLine>();
            _bookingSessionRepo = _unitOfWork.GetDataRepository<BookingSession>();
            _bookingSpecialRequestRepo = _unitOfWork.GetDataRepository<BookingSpecialRequest>();
            _courseTemplateLineRepo = _unitOfWork.GetDataRepository<GF_CourseTemplateLine>();
            _courseTemplateRepo = _unitOfWork.GetDataRepository<GF_CourseTemplate>();
            _courseRepo = _unitOfWork.GetDataRepository<Course>();
            _promotionRepo = _unitOfWork.GetDataRepository<M_Promotion>();
            _promotion_CourseRepo = _unitOfWork.GetDataRepository<M_Promotion_Course>();
            _customerGroupRepo = _unitOfWork.GetDataRepository<CustomerGroup>();
            _customerRepo = _unitOfWork.GetDataRepository<Customer>();
            _organizationRepo = _unitOfWork.GetDataRepository<Organization>();
            _bookingOtherTypeRepo = _unitOfWork.GetDataRepository<BookingOtherType>();
            _bookingSessionLineRepo = _unitOfWork.GetDataRepository<BookingSessionLine>();
            _memberCardRepo = _unitOfWork.GetDataRepository<MemberCard>();
            _memberCardCourseRepo = _unitOfWork.GetDataRepository<MemberCardCourse>();
            _userBankInfoRepo = _unitOfWork.GetDataRepository<UserBankInfo>();
            _paymentTypeRepo = _unitOfWork.GetDataRepository<PaymentType>();
            _teeSheetLockLineRepo = _unitOfWork.GetDataRepository<TeeSheetLockLine>();
            _notificationQueueRepo = _unitOfWork.GetDataRepository<NotificationQueue>();
            _cancelReasonRepo = _unitOfWork.GetDataRepository<CancelReason>();
            _appUserRepo = _unitOfWork.GetDataRepository<AppUser>();
            _transactionPaymentRepo = _unitOfWork.GetDataRepository<BookingTransactionPayment>();
            _inTransactionHeaderRepo = _unitOfWork.GetDataRepository<InTransactionHeader>();
            _inTransactionDetailsRepo = _unitOfWork.GetDataRepository<InTransactionDetails>();
            _outTransactionHeaderRepo = _unitOfWork.GetDataRepository<OutTransactionHeader>();
            _outInTransactionDetailsRepo = _unitOfWork.GetDataRepository<OutTransactionDetails>();
            _transactionLogsRepo = _unitOfWork.GetDataRepository<TransactionLogs>();
            _settingRepo = _unitOfWork.GetDataRepository<Setting>();
            _teeSheetLockRepo = _unitOfWork.GetDataRepository<TeeSheetLock>();
            _holidayRepo = _unitOfWork.GetDataRepository<Holiday>();
        }

        public Booking GetLastBooking()
        {
            var entity = _repo.SelectWhere(x => x.IsActive && x.DateId.Value.Month == DateTime.Now.Month
                                        && x.DateId.Value.Year == DateTime.Now.Year)
                .OrderByDescending(o => o.BookingOrder).FirstOrDefault();
            return entity;
        }

        public override Booking SingleOrDefault(Guid id)
        {
            var entity = _repo.AsQueryable()
                .Include(x => x.Course)
                .Include(x => x.BookingSpecialRequests).ThenInclude(x => x.BookingOtherType)
                .Include(x => x.BookingLines).ThenInclude(x => x.CustomerGroup)
                .Include(x => x.AppUser)
                .Where(x => x.Id == id)
                .FirstOrDefault();
            return entity;
        }

        public IEnumerable<BookingLine> GetAllBookingLine()
        {
            return _bookingLineRepo.GetAll();
        }

        public IEnumerable<BookingSpecialRequest> GetAllBookingSpecialRequest()
        {
            return _bookingSpecialRequestRepo.GetAll();
        }

        public IEnumerable<Booking> GetAllBooking()
        {
            return _repo.GetAll();
        }

        public IEnumerable<Booking> GetAllBookingNoShow()
        {
            return _repo.SelectWhere(x => x.IsActive && x.Status == StatusBookingLine.booked.ToString()
                && x.DateId.Value.Date < DateTime.Now.Date)
                .Include(i => i.AppUser).Include(i => i.BookingLines)
                .OrderByDescending(o => o.CreatedDate);
        }

        public IEnumerable<Booking> GetAllBookingByDate(DateTime date)
        {
            return _repo.SelectWhere(x => x.CreatedDate.Date == date.Date);
        }

        public Course GetCourseById(Guid courseId)
        {
            return _courseRepo.SelectWhere(x => x.Id == courseId && x.IsActive).Include(i => i.Organization).FirstOrDefault();
        }

        public List<M_Promotion> GetPromotionByBooking(Guid courseId, DateTime fromDate, DateTime toDate)
        {

            var data = _promotionRepo.SelectWhere(x => x.IsActive && x.Start_Date <= fromDate && toDate <= x.End_Date)
                .Join(_promotion_CourseRepo.SelectWhere(x => x.IsActive && x.C_Course_Id == courseId),
                        tem => tem.Id,
                        line => line.M_Promotion_Id,
                        (stud, stand) => new { tem = stud, line = stand })
                        .Select(s => s.tem).Include(i => i.PromotionCourse).ThenInclude(t => t.Course);
            return data.ToList();
        }


        public IEnumerable<Booking> BookingHistory(BookingFilterModel filter)
        {
            var querry = _repo.SelectWhere(x => x.UserId == filter.UserId
            && (x.Status == Enums.StatusBookingLine.booked.ToString()
                || x.Status == Enums.StatusBookingLine.cancel.ToString()))
                .Include(i => i.BookingLines).Include(c => c.Course)
                .OrderByDescending(d => d.CreatedDate);

            return querry;
        }

        public Booking BookingHistoryDetail(Guid? bookingId, string userId)
        {
            var data = _repo.SelectWhere(x => x.UserId == userId && x.Id == bookingId).Include(i => i.BookingLines)
                .Include(i => i.AppUser)
                .Include(a => a.BookingSpecialRequests).ThenInclude(t => t.BookingOtherType)
                .Include(i => i.Organization).ThenInclude(t => t.Courses).FirstOrDefault();
            return data;
        }

        public bool CheckDayHolidayByOrg(Guid c_Org_Id, DateTime bookingDate)
        {
            return _holidayRepo.SelectWhere(x => x.IsActive && x.C_Org_Id == c_Org_Id && x.DateId.Date == bookingDate.Date).Any();
        }

        public IEnumerable<GF_CourseTemplateLine> GetCourseTemplateLineByCourse(Guid courseId, Guid c_Org_Id, DateTime bookingDate)
        {
            //var now = DateTime.Now;
            var curDay = GetDow(bookingDate);
            var isHoli = false;
            if (c_Org_Id != null)
            {
                isHoli = CheckDayHolidayByOrg(c_Org_Id, bookingDate.Date);
            }


            if (isHoli)
            {
                var data = _courseTemplateRepo.SelectWhere(x => x.IsActive && (isHoli && x.DOW.Contains("0"))
                                            && x.StartDate.Value.Date <= bookingDate.Date && bookingDate.Date <= x.EndDate.Value.Date)
                .Join(_courseTemplateLineRepo.SelectWhere(a => a.IsActive && a.C_Course_Id == courseId),
                        tem => tem.Id,
                        line => line.GF_CourseTemplate_Id,
                        (stud, stand) => new { tem = stud, line = stand })
                        .Select(s => s.line).OrderBy(b => b.Part);
                if (!data.Any())
                {
                    data = _courseTemplateRepo.SelectWhere(x => x.IsActive && (x.DOW.Contains(curDay) || (isHoli && x.DOW.Contains("0")))
                                            && x.StartDate.Value.Date <= bookingDate.Date && bookingDate.Date <= x.EndDate.Value.Date)
                    .Join(_courseTemplateLineRepo.SelectWhere(a => a.IsActive && a.C_Course_Id == courseId),
                            tem => tem.Id,
                            line => line.GF_CourseTemplate_Id,
                            (stud, stand) => new { tem = stud, line = stand })
                            .Select(s => s.line).OrderBy(b => b.Part);
                }
                return data;
            }
            else
            {
                var data = _courseTemplateRepo.SelectWhere(x => x.IsActive && (x.DOW.Contains(curDay))
                                            && x.StartDate.Value.Date <= bookingDate.Date && bookingDate.Date <= x.EndDate.Value.Date)
                .Join(_courseTemplateLineRepo.SelectWhere(a => a.IsActive && a.C_Course_Id == courseId),
                        tem => tem.Id,
                        line => line.GF_CourseTemplate_Id,
                        (stud, stand) => new { tem = stud, line = stand })
                        .Select(s => s.line).OrderBy(b => b.Part);
                return data;
            }
        }

        public IEnumerable<GF_CourseTemplateLine> GetCourseTemplateLineByPromotion(Guid promotionId)
        {
            throw new NotImplementedException();
        }


        private string GetDow(DateTime bookingDate)
        {
            var curDay = bookingDate.DayOfWeek;
            if (curDay == DayOfWeek.Monday)
            {
                return "2";
            }
            if (curDay == DayOfWeek.Tuesday)
            {
                return "3";
            }
            if (curDay == DayOfWeek.Wednesday)
            {
                return "4";
            }
            if (curDay == DayOfWeek.Thursday)
            {
                return "5";
            }
            if (curDay == DayOfWeek.Friday)
            {
                return "6";
            }
            if (curDay == DayOfWeek.Saturday)
            {
                return "7";
            }
            return "1"; // sunday
        }

        public IEnumerable<CustomerGroup> GetCustomerGroup(Guid orgId)
        {
            var customerGroup = _customerGroupRepo.SelectWhere(x => x.C_Org_Id == orgId);
            if (customerGroup != null)
            {
                return customerGroup;
            }
            return null;
        }

        public CustomerGroup GetCustomerGroupByCode(string code)
        {
            var org = _customerGroupRepo.SelectWhere(x => x.Code == code).FirstOrDefault();
            return org;
        }

        public IEnumerable<BookingOtherType> GetBookingOtherType(Guid courseId)
        {
            var org = _courseRepo.SingleOrDefault(x => x.Id == courseId);
            if (org != null)
            {
                return _bookingOtherTypeRepo.SelectWhere(x => x.C_Org_Id == org.C_Org_Id);
            }
            return null;
        }

        public string CheckSelectedTeesheets(IEnumerable<SelectedTeeSheetTemp> selectedTeeSheet, string userId, Guid courseId, Guid orgId)
        {
            var bookedTeesheet = string.Empty;
            var lockedTeesheet = string.Empty;
            foreach (var item in selectedTeeSheet)
            {
                var teesshet = CheckTeesheetInSession(item, userId, courseId);
                if (!string.IsNullOrEmpty(teesshet))
                {
                    if (string.IsNullOrEmpty(bookedTeesheet))
                    {
                        bookedTeesheet = teesshet;
                    }
                    else
                    {
                        bookedTeesheet += "," + teesshet;
                    }
                }
                else
                {
                    var teesshetLock = CheckTeesheetlock(item, userId, courseId, orgId);
                    if (!string.IsNullOrEmpty(teesshetLock))
                    {
                        if (string.IsNullOrEmpty(lockedTeesheet))
                        {
                            lockedTeesheet = teesshetLock;
                        }
                        lockedTeesheet += "," + teesshetLock;
                    }
                }
            }
            var message = string.Format("16-{0}", bookedTeesheet);
            var message1 = string.Format("17-{0}", lockedTeesheet);
            if (!string.IsNullOrEmpty(bookedTeesheet) && string.IsNullOrEmpty(lockedTeesheet))
            {
                return message;
            }
            if (string.IsNullOrEmpty(bookedTeesheet) && !string.IsNullOrEmpty(lockedTeesheet))
            {
                return message1;
            }
            if (!string.IsNullOrEmpty(bookedTeesheet) && !string.IsNullOrEmpty(lockedTeesheet))
            {
                var message2 = bookedTeesheet + " " + lockedTeesheet;
                return string.Format("18-{0}", message2);
            }
            return string.Empty;
        }

        public int CheckTeesheet(DateTime selectedTeeSheet, string userId, Guid courseId, Guid orgId, DateTime endTeeSheet, int cardType)
        {
            var checkLockTeetime = CheckTeesheetlock(new SelectedTeeSheetTemp { Tee_Time = selectedTeeSheet, TeeTimeEnd = endTeeSheet }
                                                    , userId, courseId, orgId);
            if (!string.IsNullOrEmpty(checkLockTeetime))
            {
                return 0;
            }

            var bookingSession = _bookingSessionLineRepo.SelectWhere(x => x.DateId.Value.Date == selectedTeeSheet.Date
                                    && x.Tee_Time.Value.Hour == selectedTeeSheet.Hour
                                    && x.Tee_Time.Value.Minute == selectedTeeSheet.Minute
                                    && x.BookingSession.C_Course_Id == courseId
                                    && (x.Status != StatusBookingLine.booked.ToString() && x.Status != StatusBookingLine.cancel.ToString())
                                    && x.IsActive).OrderByDescending(o => o.CreatedDate);
            if (bookingSession.Any())
            {
                foreach (var session in bookingSession)
                {
                    if ((session.End_Time - DateTime.Now).Value.TotalMinutes >= 0)
                    {
                        return 0;
                    }
                }
            }
            var lineByDate = _bookingLineRepo.SelectWhere(x => x.DateId.Value.Date == selectedTeeSheet.Date
                                   && x.Booking.C_Course_Id == courseId
                                   && x.Booking.Status == StatusBookingLine.booked.ToString()
                                   && x.IsActive);
            var bookedTeesshet = lineByDate.Where(x => x.Tee_Time.Value.Hour == selectedTeeSheet.Hour
                                && x.Tee_Time.Value.Minute == selectedTeeSheet.Minute);

            if (bookedTeesshet.Count() == 0)
            {
                return (int)ValidNumPlayerBook.CurrentUser;
            }
            if (bookedTeesshet.Count() == 1)
            {
                var avaibleSlot = (int)ValidNumPlayerBook.CurrentUser - bookedTeesshet.Count();
                return avaibleSlot > 0 ? avaibleSlot : 0;
            }
            if (bookedTeesshet.Count() >= 2 && bookedTeesshet.Count() < 4)
            {
                var lineOfUser = lineByDate.Where(x => x.CreatedUser == userId);
                if (lineOfUser.Count() >= 1)
                {
                    var avaibleSlot = (int)ValidNumPlayerBook.CurrentUser - bookedTeesshet.Count();
                    return avaibleSlot > 0 ? avaibleSlot : 0;
                }
            }

            return 0;
        }

        public bool CheckNumberAvailableSlot(List<DateTime> tees, string userId, Guid courseId, Guid orgId, int? cardType)
        {
            var isHoliday = CheckDayHolidayByOrg(orgId, tees.FirstOrDefault().Date);
            var bookTee = tees.Count();
            var bookInDb = _bookingLineRepo.SelectWhere(x => x.DateId.Value.Date == tees.FirstOrDefault().Date
                                   && x.Booking.C_Course_Id == courseId
                                   && x.Booking.Status == StatusBookingLine.booked.ToString()
                                   && x.CreatedUser == userId
                                   && x.IsActive).GroupBy(g => g.Tee_Time);

            var bookedTees = bookInDb.Count();
            //if (totalTee > 0)
            //{
            var sameBookOfUser = bookInDb.Count(x => tees.Contains(x.Key.Value));
            var newTee = bookTee - sameBookOfUser;
            bookedTees = bookedTees - sameBookOfUser + newTee;
            var dayWeek = tees.FirstOrDefault().Date.DayOfWeek;
            if (dayWeek == DayOfWeek.Sunday || dayWeek == DayOfWeek.Saturday || isHoliday)
            {
                if (bookedTees > 1)
                {
                    return false;
                }
            }
            else
            {
                if (cardType.HasValue && cardType == (int)MemberCardType.member)
                {
                    if (bookedTees > 2)
                    {
                        return false;
                    }
                }
                else
                {
                    if (bookedTees > 1)
                    {
                        return false;
                    }
                }
            }
            //}
            return true;
        }


        public string CheckTeesheetlock(SelectedTeeSheetTemp tee, string userId, Guid courseId, Guid orgId)
        {
            var teeLocks = _teeSheetLockRepo.SelectWhere(x => x.StartDate.Date <= tee.Tee_Time.Date && x.EndDate.Date >= tee.Tee_Time.Date
                                                        && x.C_Org_Id == orgId && x.IsActive)
                                .Include(i => i.TeeSheetLockLines.Where(a => a.IsActive && a.C_Course_Id == courseId))
                                .OrderBy(x => x.StartDate);

            foreach (var teeLock in teeLocks)
            {
                var lines = teeLock.TeeSheetLockLines.Where(x => x.IsActive && x.C_Course_Id == courseId);

                if (lines != null)
                {
                    foreach (var line in lines)
                    {
                        var day = (int)tee.Tee_Time.DayOfWeek + 1;
                        var timeStart = line.StartTime.Split(':');
                        var start = new DateTime(teeLock.StartDate.Year, teeLock.StartDate.Month,
                            teeLock.StartDate.Day, Convert.ToInt32(timeStart[0]), Convert.ToInt32(timeStart[1]), 0);

                        var endStart = line.EndTime.Split(':');
                        var end = new DateTime(teeLock.EndDate.Year, teeLock.EndDate.Month,
                            teeLock.EndDate.Day, Convert.ToInt32(endStart[0]), Convert.ToInt32(endStart[1]), 0);

                        var isHoliday = false;
                        if (line.DOW.Contains("0"))
                        {
                            isHoliday = _holidayRepo.SelectWhere(x => x.IsActive && x.DateId.Date == tee.Tee_Time.Date
                                            && x.C_Org_Id == orgId).Count() >= 1 ? true : false;
                        }
                        var countDate = (teeLock.EndDate.Date - teeLock.StartDate.Date).TotalDays;
                        if (countDate > 0)
                        {
                            var currDate = start;
                            for (var i = 0; i <= countDate; i++)
                            {
                                var currEnd = new DateTime(currDate.Year, currDate.Month,
                                    currDate.Day, Convert.ToInt32(endStart[0]), Convert.ToInt32(endStart[1]), 0);
                                if (currDate <= tee.Tee_Time && tee.Tee_Time <= currEnd
                                                        && (line.DOW.Contains(day.ToString()) || (line.DOW.Contains("0") && isHoliday)))
                                {
                                    return tee.Tee_Time.ToString("HH:mm");
                                }
                                currDate = currDate.AddDays(1);
                            }
                        }
                        else
                        {
                            if (start <= tee.Tee_Time && tee.Tee_Time <= end
                                                        && (line.DOW.Contains(day.ToString()) || (line.DOW.Contains("0") && isHoliday)))
                            {
                                return tee.Tee_Time.ToString("HH:mm");
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        public string CheckTeesheetInSession(SelectedTeeSheetTemp selectedTeeSheet, string userId, Guid courseId)
        {
            var bookedTeesshetSession = _bookingSessionLineRepo.SelectWhere(x => x.DateId.Value.Date == selectedTeeSheet.Tee_Time.Date
                                    && x.Status == StatusBookingLine.open.ToString()
                                    && x.BookingSession.C_Course_Id == courseId
                                    && x.Tee_Time.Value.Hour == selectedTeeSheet.Tee_Time.Hour
                                    && x.Tee_Time.Value.Minute == selectedTeeSheet.Tee_Time.Minute
                                    && (x.Start_Time <= DateTime.Now && x.End_Time >= DateTime.Now)
                                    && x.IsActive).FirstOrDefault();

            if (bookedTeesshetSession != null)
            {
                return bookedTeesshetSession.Tee_Time.Value.ToString("HH:mm");
            }
            return string.Empty;

        }

        public async ValueTask<BookingSession> InsertBookingSession(BookingSession bookingsession)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    bookingsession = await _bookingSessionRepo.AddAsync(bookingsession);
                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                    return bookingsession;
                }
                catch (Exception e)
                {
                    using (LogContext.PushProperty("MethodName", "InsertBookingSession"))
                    {
                        _log.LogError(e.Message);
                    }
                    transaction.Rollback();
                    return bookingsession;
                }
            }
        }

        public IEnumerable<MemberCard> GetMemberCard(string userId)
        {
            var card = _memberCardRepo.SelectWhere(x => x.IsActive && !x.IsDelete && x.Golf_Effective_Date <= DateTime.Now
                        && (DateTime.Now <= x.Golf_Expire_Date || x.Golf_Expire_Date == null)
                        && x.Golf_IsActive && !x.Golf_IsLock)
                        .Include(i => i.MemberCardCourses)
                        .Include(i => i.Customer)
                        .Where(x => x.Customer.UserId == userId);

            return card;
        }

        public Customer GetCustomerByUserId(string userId)
        {
            return _customerRepo.SelectWhere(x => x.UserId == userId)
                .Include(i => i.MemberCards.Where(a => !a.IsDelete && a.IsActive)).ThenInclude(t => t.MemberCardCourses).FirstOrDefault();
        }

        public IEnumerable<BookingOtherType> GetBookingSpecialRequest(Guid c_Org_Id)
        {
            return _bookingOtherTypeRepo.SelectWhere(x => x.C_Org_Id == c_Org_Id);
        }

        public RespondData InsertBankInfo(UserBankInfo data)
        {
            try
            {
                _userBankInfoRepo.AddAsync(data);
                return new RespondData { IsSuccess = true, MsgCode = "08" };
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);

                }
                return new RespondData { IsSuccess = false, MsgCode = "09" };
            }
        }

        public IEnumerable<UserBankInfo> GetBankInfo(string userId)
        {
            return _userBankInfoRepo.SelectWhere(x => x.UserId == new Guid(userId) && x.IsActive);
        }

        public IEnumerable<PaymentType> GetPaymentType()
        {
            return _paymentTypeRepo.SelectWhere(x => x.IsActive);
        }

        public List<BookingOtherType> GetBookingSpecialRequests(Guid? C_Org_Id)
        {
            return _bookingOtherTypeRepo.SelectWhere(s => s.IsActive && s.C_Org_Id == C_Org_Id).OrderBy(c => c.CreatedDate).ToList();
        }
        public Organization GetOrganization(Guid? orgId)
        {
            return _organizationRepo.SingleOrDefault(s => s.IsActive && s.Id == orgId);
        }
        public Booking CancelBooking(Guid bookingId, string userId, string cancelId, string cancelDesc, bool isFromWeb)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var booking = _repo.SelectWhere(x => x.Id == bookingId
                                    && x.Status == StatusBookingLine.booked.ToString() && x.IsActive
                                    && x.BookingLines.Any(a => a.Tee_Time > DateTime.Now))
                        .Include(i => i.TransactionPayments).Include(i => i.AppUser)
                        .Include(i => i.BookingLines).ThenInclude(t => t.CustomerGroup)
                        .Include(o => o.Organization).Include(i => i.Course)
                        .Include(i => i.BookingSession).ThenInclude(t => t.BookingSessionLines)
                        .FirstOrDefault();
                    if (booking == null)
                    {
                        using (LogContext.PushProperty("MethodName", "CancelBooking"))
                        {
                            _log.LogError("Không tìm thấy booing " + bookingId);
                        }
                        return null;
                    }
                    else
                    {
                        var cancelResaon = _cancelReasonRepo.SelectWhere(x => x.IsActive && x.Code == Enums.CancelBookingType.PC.ToString()).FirstOrDefault();
                        booking.Status = StatusBookingLine.cancel.ToString();
                        booking.Cancel_Time = DateTime.Now;
                        booking.Cancel_User = userId;
                        if (!string.IsNullOrEmpty(cancelId))
                        {
                            booking.Cancel_Reason_Id = new Guid(cancelId);
                            booking.Cancel_Description = cancelDesc;
                        }
                        else
                        {
                            if (cancelResaon != null)
                            {
                                booking.Cancel_Reason_Id = cancelResaon.Id;
                            }
                        }
                        booking.UpdatedDate = DateTime.Now;
                        booking.UpdatedUser = userId;

                        _repo.Update(booking);

                        var bookingLine = booking.BookingLines.Where(x => x.IsActive);
                        foreach (var item in bookingLine)
                        {
                            item.BookingStatus = StatusBookingLine.cancel.ToString();
                            item.UpdatedUser = userId;
                            item.UpdatedDate = DateTime.Now;
                            _bookingLineRepo.Update(item);
                        }

                        booking.BookingLines = bookingLine.ToList();

                        if (booking.BookingSession != null)
                        {
                            booking.BookingSession.Status = StatusBookingLine.cancel.ToString();
                            booking.BookingSession.UpdatedDate = DateTime.Now;
                            booking.BookingSession.UpdatedUser = userId;
                            _bookingSessionRepo.Update(booking.BookingSession);
                            foreach (var tee in booking.BookingSession.BookingSessionLines)
                            {
                                tee.Status = StatusBookingLine.cancel.ToString();
                                tee.UpdatedUser = userId;
                                tee.UpdatedDate = DateTime.Now;
                                _bookingSessionLineRepo.Update(tee);
                            }
                        }
                        var isConnectSdk = Configuration.GetValue<bool>("IsConnectSdk");
                        if (isConnectSdk)
                        {
                            var isReturn = CheckReturnMoneyRuleWhenCancel(booking);
                            var payment = booking.TransactionPayments.Where(x => x.Status == StatusBookingLine.booked.ToString() && x.IsActive).FirstOrDefault();
                            if (payment != null)
                            {
                                //cancel tren mobile cancelId = null, trên web thì có giá trị
                                if (isFromWeb && !string.IsNullOrEmpty(cancelId))
                                {
                                    booking.Cancel_Reason_Id = new Guid(cancelId);
                                    booking.Cancel_Description = cancelDesc;
                                }
                                if (!isFromWeb)
                                {
                                    if (cancelResaon != null)
                                    {
                                        booking.Cancel_Reason_Id = cancelResaon.Id;
                                    }
                                    if (isReturn == 1)
                                    {
                                        payment.SdkRefundMoney = payment.TotalMoney / 2;
                                        payment.CancelStatus = ((int)CancelStatus.cancelRefund50).ToString();
                                    }
                                    if (isReturn == 2)
                                    {
                                        payment.SdkRefundMoney = payment.TotalMoney;
                                        payment.CancelStatus = ((int)CancelStatus.cancelRefund100).ToString();
                                    }
                                    if (isReturn == 0)
                                    {
                                        payment.SdkRefundMoney = 0;
                                        payment.CancelStatus = ((int)CancelStatus.notRefund).ToString();
                                    }
                                }
                                payment.Status = StatusBookingLine.cancel.ToString();
                                payment.CancelTime = DateTime.Now;
                                payment.CancelUer = userId;
                                _transactionPaymentRepo.Update(payment);
                            }
                        }
                    }
                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                    return booking;
                }
                catch (Exception e)
                {
                    using (LogContext.PushProperty("MethodName", "CancelBooking"))
                    {
                        _log.LogError(e.Message);
                        _log.LogError(e.StackTrace);
                    }
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public int CheckReturnMoneyRuleWhenCancel(Booking booking)
        {
            var timeCancelRule48 = GetTimeCancelRule48();
            var timeCancelRule24 = GetTimeCancelRule24();
            var lines = booking.BookingLines.OrderBy(x => x.Tee_Time).FirstOrDefault();
            if (lines != null)
            {
                var countTime = Math.Abs((DateTime.Now - lines.Tee_Time.Value).TotalHours);
                if (countTime >= Convert.ToDouble(timeCancelRule48.Value))
                {
                    return 2; // hoàn 100%
                }
                if (countTime > Convert.ToDouble(timeCancelRule24.Value) && countTime <= Convert.ToDouble(timeCancelRule48.Value))
                {
                    return 1; //hoàn 50%
                }
                if (countTime < Convert.ToDouble(timeCancelRule24.Value))
                {
                    return 0; // không cần hoàn
                }
            }
            return 0;
        }

        public BookingSession GetBookingSession(Guid id)
        {
            return _bookingSessionRepo.SingleOrDefault(x => x.Id == id && x.IsActive);
        }

        public CustomerGroup GetCustomerGroupId(string code)
        {
            return _customerGroupRepo.SelectWhere(x => x.Code == code).FirstOrDefault();
        }

        public async Task<Booking> InsertBooking(Booking booking)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var book = await _repo.AddAsync(booking);
                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                    return book;
                }
                catch (Exception e)
                {
                    using (LogContext.PushProperty("MethodName", "InsertBooking"))
                    {
                        _log.LogError(e.Message);
                        _log.LogError(e.StackTrace);
                    }
                    transaction.Rollback();
                    return booking;
                }
            }
        }

        public Booking GolfPaymentStep(PaymentData model)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var booking = _repo.SelectWhere(x => x.Id == model.BookingId && x.IsActive).Include(i => i.BookingLines)
                        .Include(i => i.Course).Include(i => i.Organization).Include(i => i.AppUser)
                        .Include(i => i.BookingSession).ThenInclude(t => t.BookingSessionLines).FirstOrDefault();
                    var status = StatusBookingLine.booked.ToString();
                    decimal nonRefundedFee = 0.0M;
                    decimal estimatePrice = 0.0M;
                    decimal seagolfPrice = 0.0M;
                    decimal totalEstimateFee = 0.0M;
                    if (booking != null)
                    {
                        booking.Status = status;
                        booking.BookingType = BookingButtonType.Payment.ToString();
                        booking.UpdatedUser = model.UserId;
                        booking.UpdatedDate = DateTime.Now;

                        if (booking.BookingSession != null)
                        {
                            booking.BookingSession.Status = status;
                            booking.BookingSession.UpdatedDate = DateTime.Now;
                            booking.BookingSession.UpdatedUser = model.UserId;
                            _bookingSessionRepo.Update(booking.BookingSession);
                            foreach (var tee in booking.BookingSession.BookingSessionLines)
                            {
                                tee.Status = status;
                                tee.UpdatedUser = model.UserId;
                                tee.UpdatedDate = DateTime.Now;
                                _bookingSessionLineRepo.Update(tee);
                            }
                        }
                        foreach (var tee in booking.BookingLines)
                        {
                            tee.BookingStatus = status;
                            tee.UpdatedUser = model.UserId;
                            tee.UpdatedDate = DateTime.Now;
                            if (tee.NonRefundedFee.HasValue)
                            {
                                nonRefundedFee = nonRefundedFee + tee.NonRefundedFee.Value;
                            }

                            if (tee.SeagolfPrice.HasValue)
                            {
                                if (tee.SeagolfPrice < tee.EstimatedPrice)
                                {
                                    totalEstimateFee = totalEstimateFee + tee.SeagolfPrice.Value;
                                }
                                else
                                {
                                    totalEstimateFee = totalEstimateFee + tee.EstimatedPrice.Value;
                                }
                            }
                            else
                            {
                                totalEstimateFee = totalEstimateFee + tee.EstimatedPrice.Value;
                            }
                            _bookingLineRepo.Update(tee);
                        }
                        booking.TotalEstimateFee = totalEstimateFee;
                        booking.NonRefundedFee = nonRefundedFee;
                        _repo.Update(booking);
                        if (Configuration.GetValue<bool>("IsConnectSdk"))
                        {
                            var org = _organizationRepo.SingleOrDefault(x => x.Id == booking.C_Org_Id);
                            var payment = new BookingTransactionPayment
                            {
                                BookingId = booking.Id,
                                Traceid = booking.BookingCode,
                                Ftid = model.FtId,
                                LinkCardAcctId = model.LinkCardAcctId,
                                SourceType = model.SourceType,
                                AccountID = model.AccountID,
                                Company = model.AccountID,
                                AccountType = model.AccountType,
                                EmbosingName = model.EmbosingName,
                                CardNumber = model.CardNumber,
                                CardProduct = model.CardProduct,
                                CardProductDesc = model.CardProductDesc,
                                CustomerId = model.CustomerId,
                                TotalMoney = nonRefundedFee,
                                UserId = new Guid(model.UserId),
                                OrgCode = org.Code,
                                OrgId = org.Id,
                                DatePayment = DateTime.Now,
                                Status = status,
                                IsActive = true,
                                CreatedDate = DateTime.Now,
                                CreatedUser = model.UserId
                            };
                            _transactionPaymentRepo.AddAsync(payment);
                        }

                        _unitOfWork.SaveChanges();
                        transaction.Commit();
                        return booking;
                    }
                    return null;
                }
                catch (Exception e)
                {
                    using (LogContext.PushProperty("MethodName", "GolfPaymentStep"))
                    {
                        _log.LogError(e.Message);
                        _log.LogError(e.StackTrace);
                    }
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public void InsertCancelBookingNotifyInQueue(string loginUser, Guid bookingId, string bookingCode, string userId)
        {
            try
            {
                var title = Configuration.GetValue<string>("NotifiCancelBookingTitle");
                var content = Configuration.GetValue<string>("AdCancelBooking");

                var cust = _customerRepo.SelectWhere(x => x.UserId == userId).FirstOrDefault();
                if (cust != null)
                {
                    var data = new NotificationQueue
                    {
                        SendTo = cust.FcmTokenDevice,
                        UserId = new Guid(loginUser),
                        SendDate = DateTime.Now.AddMinutes(2),
                        BookingId = bookingId,
                        IsSend = false,
                        IsActive = true,
                        NotificationType = FcmNotifiType.booking.ToString(),
                        CreatedDate = DateTime.Now,
                        CreatedUser = loginUser,
                        Title = title,
                        Content = string.Format(content, bookingCode),
                        TitleEn = Configuration.GetValue<string>("NotifiCancelBookingTitleEn"),
                        ContentEn = string.Format(Configuration.GetValue<string>("AdCancelBookingEn"), bookingCode)
                    };
                    InsertNotifyInQueue(data, loginUser);
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "InsertCancelBookingNotifyInQueue"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
            }
        }

        public void InsertBookingNotifyInQueue(DateTime firstTeeTime, string loginUser, string language, string courseName, Guid bookingId)
        {
            try
            {
                var title = Configuration.GetValue<string>("NotifiBookingTitle");
                var content = Configuration.GetValue<string>("NotifiBooking");

                var cust = _customerRepo.SelectWhere(x => x.UserId == loginUser).FirstOrDefault();
                if (cust != null)
                {
                    var data = new NotificationQueue
                    {
                        SendTo = cust.FcmTokenDevice,
                        UserId = new Guid(loginUser),
                        SendDate = firstTeeTime.AddHours(-24),
                        BookingId = bookingId,
                        IsSend = false,
                        IsActive = true,
                        NotificationType = FcmNotifiType.booking.ToString(),
                        CreatedDate = DateTime.Now,
                        CreatedUser = loginUser,
                        Title = title,
                        Content = string.Format(content, firstTeeTime.ToString("HH:mm"),
                        courseName, firstTeeTime.ToString("dd/MM/yyyy")),
                        TitleEn = Configuration.GetValue<string>("NotifiBookingTitleEn"),
                        ContentEn = string.Format(Configuration.GetValue<string>("NotifiBookingEn"), firstTeeTime.ToString("HH:mm"),
                        courseName, firstTeeTime.Date.ToString("dd/MM/yyyy"))
                    };
                    InsertNotifyInQueue(data, loginUser);
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "InsertBookingNotifyInQueue"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
            }
        }

        public void InsertNotifyInQueue(NotificationQueue model, string loginUser)
        {
            model.IsSend = false;
            model.IsActive = true;
            model.CreatedDate = DateTime.Now;
            model.CreatedUser = loginUser;

            if (!string.IsNullOrEmpty(model.SendTo))
            {
                _notificationQueueRepo.AddAsync(model);
            }
        }

        public void CancelBookingNotifyInQueue(Guid bookingId, string userId)
        {
            var notify = _notificationQueueRepo.SelectWhere(x => x.BookingId == bookingId
                            && x.UserId.ToString() == userId
                            && x.NotificationType == FcmNotifiType.booking.ToString()
                            && (!x.IsSend.HasValue || !x.IsSend.Value)
                            && x.IsActive.Value).FirstOrDefault();
            if (notify != null)
            {
                notify.IsActive = false;
                _notificationQueueRepo.UpdateByProperties(notify, new List<Expression<Func<NotificationQueue, object>>>()
                                {
                                    x => x.IsActive
                                });
                _unitOfWork.SaveChanges();
            }
        }

        public Booking GetBookingById(Guid id)
        {
            return _repo.SelectWhere(x => x.Id == id && x.IsActive)
                .Include(i => i.BookingLines).ThenInclude(t => t.CustomerGroup)
                .Include(i => i.Organization).Include(i => i.Course)
                .Include(i => i.BookingSpecialRequests).ThenInclude(t => t.BookingOtherType)
                .Include(i => i.AppUser).FirstOrDefault();
        }

        public async Task RemoveBookingLineAsync(IEnumerable<BookingLine> delBookingLine)
        {
            foreach (var item in delBookingLine)
            {
                var delIte = await _bookingLineRepo.GetByIdAsync(item.Id);
                if (delIte != null)
                {
                    _bookingLineRepo.Remove(delIte);
                }
            }
        }

        public async Task<M_Promotion> GetPromotionById(Guid m_Promotion_Id)
        {
            return await _promotionRepo.GetByIdAsync(m_Promotion_Id);
        }

        public IEnumerable<M_Promotion> GetSeagolfByCourse(Guid courseId, Guid? m_Promotion_Id)
        {
            return _promotionRepo.SelectWhere(x => x.PromotionCourse.Any(a => a.C_Course_Id == courseId)
                        && x.IsActive && x.End_Date >= DateTime.Now && x.Promotion_Type == PromotionType.seagolf.ToString()
                        && x.Id == m_Promotion_Id)
                .Include(i => i.PromotionCourse).Include(i => i.PromotionCustomerGroup).ThenInclude(t => t.CustomerGroup);
        }

        public IEnumerable<CustomerGroup> GetAllCustomerGroup()
        {
            return _customerGroupRepo.SelectWhere(x => x.IsActive);
        }

        public string GetCustomerGroupById(Guid? mB_CustomerGroup_Id)
        {
            return _customerGroupRepo.GetByIdAsync(mB_CustomerGroup_Id.Value).Result?.Name;
        }

        public AppUser GetAppUserById(string id)
        {
            return _appUserRepo.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<InTransactionDetails> GetInDataTransaction(TransactionFilterModel filter)
        {
            var orgId = new Guid(filter.UserOrgId);
            var query = _inTransactionDetailsRepo.SelectWhere(x => x.IsActive
                                            && x.InTransactionHeader.DateTrans.Date == filter.FindDate.Value.Date
                                            && x.InTransactionHeader.Status == TransBookStatus.inApprove.ToString()
                                            && x.OrganizationId == new Guid(filter.UserOrgId));
            return query.OrderBy(x => x.FT_Id);
        }

        public IEnumerable<BookingTransactionPayment> GetbookingPaymentTrans(TransactionFilterModel filter)
        {
            return _transactionPaymentRepo.SelectWhere(x => (x.DatePayment.Date == filter.FindDate.Value.Date
                        || (x.CancelTime.HasValue && x.CancelTime.Value.Date == filter.FindDate.Value.Date
                                && x.CancelStatus != ((int)CancelStatus.notRefund).ToString()))
                    && x.OrgId == new Guid(filter.UserOrgId)
                    && (x.Status == StatusBookingLine.booked.ToString() || x.Status == StatusBookingLine.cancel.ToString()
                        || x.Status == StatusBookingLine.close.ToString()))
                .Include(i => i.Booking).ThenInclude(t => t.AppUser)
                .Include(i => i.Booking).ThenInclude(t => t.Course)
                .Include(i => i.Booking).ThenInclude(t => t.BookingLines)
                .OrderBy(o => o.Traceid);
        }

        public async Task AddOutTransactionData(IEnumerable<OutTransactionDetails> outData, string userId, string orgId, DateTime findDate)
        {
            try
            {
                var guidOrg = new Guid(orgId);

                var org = _organizationRepo.SingleOrDefault(x => x.Id == guidOrg);
                var fileName = string.Format("OUT_SB_{0}_{1}.xlsx", org != null ? org.Code : orgId, findDate.ToString("yyyyMMdd"));

                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        // inactive dữ liệu out trong ngày
                        var headers = _outTransactionHeaderRepo.SelectWhere(x => x.DateTrans.Date == findDate.Date
                            && x.OrganizationId == guidOrg && x.IsActive)
                            .Include(x => x.OutTransactionDetails.Where(i => i.IsActive)).ToList();
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
                                        _outInTransactionDetailsRepo.Update(line);
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
                                //using (LogContext.PushProperty("MethodName", "AddOutTransactionData"))
                                //{
                                //    _log.LogInformation(JsonConvert.SerializeObject(logInActiveHeader));
                                //}
                                // await _transactionLogsRepo.AddAsync(logInActiveHeader);
                            }
                        }

                        var outHeader = new OutTransactionHeader
                        {
                            FileName = fileName,
                            DateTrans = findDate,
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
                        //using (LogContext.PushProperty("MethodName", "AddOutTransactionData"))
                        //{
                        //    _log.LogInformation(JsonConvert.SerializeObject(logHeader));
                        //}
                        //await _transactionLogsRepo.AddAsync(logHeader);

                        foreach (var item in outData)
                        {
                            item.Id = Guid.Empty;
                            item.IsActive = true;
                            if (string.IsNullOrEmpty(item.Rc_code) && string.IsNullOrEmpty(item.UserRc_code))
                            {
                                item.UserRc_code = "0";
                            }
                            if (item.OrganizationId == Guid.Empty)
                            {
                                item.OrganizationId = guidOrg;
                            }
                            item.OutTransactionHeaderId = outHeader.Id;
                            await _outInTransactionDetailsRepo.AddAsync(item);
                            var logdetail = new TransactionLogs()
                            {
                                CreatedDate = DateTime.Now,
                                CreatedUser = userId,
                                DateTrans = DateTime.Now,
                                FileName = "",
                                LogText = JsonConvert.SerializeObject(item, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
                                LogName = "Out Detail"
                            };
                            //using (LogContext.PushProperty("MethodName", "AddOutTransactionData"))
                            //{
                            //    _log.LogInformation(JsonConvert.SerializeObject(logdetail));
                            //}
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        using (LogContext.PushProperty("MethodName", "AddOutTransactionData"))
                        {
                            _log.LogError(ex.Message);
                            _log.LogError(ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", "AddOutTransactionData"))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
            }
        }

        public BookingTransactionPayment GetPaymentInfoByBookingId(Guid bookingId)
        {
            return _transactionPaymentRepo.SelectWhere(x => x.BookingId == bookingId).FirstOrDefault();
        }

        public Setting GetTimeCancelRule48()
        {
            return _settingRepo.SingleOrDefault(x => x.Code == "48hour_cancel_rule");
        }

        public Setting GetTimeCancelRule24()
        {
            return _settingRepo.SingleOrDefault(x => x.Code == "24hour_cancel_rule");
        }

        /// <summary>
        /// không trả ra appuser vì khác kiểu sễ map lỗi. để appuser = null
        /// </summary>
        /// <param name="code"></param>
        /// <param name="curOrgId"></param>
        /// <param name="bookedDate"></param>
        /// <returns></returns>
        public Booking GetBookingByCode(string code, string curOrgId, DateTime? bookedDate)
        {
            return _repo.SelectWhere(x => x.BookingCode == code
                            && x.IsActive
                            && x.Status == StatusBookingLine.booked.ToString())
                .Include(i => i.BookingLines).Include(i => i.Promotion).FirstOrDefault();
        }

        public BookingLine GetBookingLineById(Guid? bookingLineId)
        {
            return _bookingLineRepo.SingleOrDefault(x => x.Id == bookingLineId);
        }

        public Organization GetOrganizationById(Guid? id)
        {
            return _organizationRepo.SingleOrDefault(x => x.Id == id);
        }

        public BookingLine UpdateBookingLine(BookingLine bookingLine)
        {
            try
            {
                _bookingLineRepo.Update(bookingLine);
                return bookingLine;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public RespondData CancelTempBookingTeetime(Guid id)
        {
            var bookingSession = _bookingSessionRepo.SelectWhere(x => x.Id == id && x.IsActive)
                .Include(i => i.BookingSessionLines).FirstOrDefault();
            if (bookingSession != null)
            {
                var booking = _repo.SelectWhere(x => x.GF_Booking_Session_Id == bookingSession.Id && x.IsActive)
                    .Include(i => i.BookingLines).FirstOrDefault();

                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        if (booking != null && (booking.Status == StatusBookingLine.open.ToString()
                                     || booking.Status == StatusBookingLine.bookedTeetime.ToString()
                                     || booking.Status == StatusBookingLine.priced.ToString()))
                        {
                            booking.IsActive = false;
                            _repo.Update(booking);
                            foreach (var line in booking.BookingLines)
                            {
                                line.IsActive = false;
                                _bookingLineRepo.Update(line);
                            }
                        }

                        bookingSession.IsActive = false;
                        _bookingSessionRepo.Update(bookingSession);
                        foreach (var line in bookingSession.BookingSessionLines)
                        {
                            line.IsActive = false;
                            _bookingSessionLineRepo.Update(line);
                        }
                        transaction.Commit();
                        return new RespondData { IsSuccess = true };
                    }
                    catch (Exception e)
                    {
                        using (LogContext.PushProperty("MethodName", "CancelTempBookingTeetime"))
                        {
                            _log.LogError(e.Message);
                            _log.LogError(e.StackTrace);
                        }
                        transaction.Rollback();
                    }
                }
            }
            return new RespondData { IsSuccess = false };
        }

        public int GetLastFlight(Guid id, DateTime tee_Time)
        {
            var line = _bookingLineRepo.SelectWhere(x => x.Booking.C_Course_Id == id && x.Tee_Time.Value.Date == tee_Time.Date
                        && x.BookingStatus == StatusBookingLine.booked.ToString()
                        && x.Tee_Time.Value.Hour == tee_Time.Hour && x.Tee_Time.Value.Minute == tee_Time.Minute).Count();
            if (line > 0 && line < 4)
            {
                return line + 1;
            }
            else
            {
                return 1;
            }
        }
        public IEnumerable<BookingLine> GetLastBookingLine(Guid id, IEnumerable<DateTime> tees, DateTime dateId)
        {
            var line = _bookingLineRepo.SelectWhere(x => x.Booking.C_Course_Id == id && x.Tee_Time.Value.Date == dateId.Date
                        && x.BookingStatus == StatusBookingLine.booked.ToString()
                        && tees.Contains(x.Tee_Time.Value));
            return line;
        }
        public Booking RefreshBookingCode(PaymentData model, string bookingCode)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var booking = _repo.SelectWhere(x => x.Id == model.BookingId && x.IsActive).Include(i => i.BookingLines)
                                       .Include(i => i.Course).Include(i => i.Organization).FirstOrDefault();
                    decimal nonRefundedFee = 0.0M;
                    if (booking != null)
                    {
                        var oldCode = booking.BookingCode;
                        booking.BookingCode = bookingCode;
                        booking.UpdatedUser = model.UserId;
                        booking.UpdatedDate = DateTime.Now;
                        _repo.Update(booking);

                        var org = _organizationRepo.SingleOrDefault(x => x.Id == booking.C_Org_Id);
                        var payment = new BookingTransactionPayment
                        {
                            BookingId = booking.Id,
                            Traceid = oldCode,
                            LinkCardAcctId = model.LinkCardAcctId,
                            SourceType = model.SourceType,
                            AccountID = model.AccountID,
                            Company = model.AccountID,
                            AccountType = model.AccountType,
                            EmbosingName = model.EmbosingName,
                            CardNumber = model.CardNumber,
                            CardProduct = model.CardProduct,
                            CardProductDesc = model.CardProductDesc,
                            CustomerId = model.CustomerId,
                            TotalMoney = nonRefundedFee,
                            UserId = new Guid(model.UserId),
                            OrgCode = org.Code,
                            OrgId = org.Id,
                            DatePayment = DateTime.Now,
                            SdkDescription = model.Notes,
                            Status = StatusBookingLine.close.ToString(),
                            IsActive = false,
                            CreatedDate = DateTime.Now,
                            CreatedUser = model.UserId
                        };
                        _transactionPaymentRepo.AddAsync(payment);
                        transaction.Commit();
                        return booking;
                    }
                    return null;
                }
                catch (Exception e)
                {
                    using (LogContext.PushProperty("MethodName", "RefreshBookingCode"))
                    {
                        _log.LogError(e.Message);
                        _log.LogError(e.StackTrace);
                    }
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public Booking GetBookingByCode(string traceId)
        {
            return _repo.SelectWhere(x => x.BookingCode == traceId && x.IsActive).Include(i => i.BookingLines).Include(i => i.AppUser).FirstOrDefault();
        }

        public void SendEmailBookingToCourse(Booking booking, string userId)
        {
            var data = new NotificationQueue
            {
                UserId = new Guid(userId),
                BookingId = booking.Id,
                Title = "",
                Content = "",
                IsSend = false,
                IsActive = true,
                NotificationType = FcmNotifiType.SendEmailBookingCourse.ToString(),
                SendDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedUser = userId
            };

            _notificationQueueRepo.AddAsync(data);
            _unitOfWork.SaveChanges();
        }

        public void LockAccountCustomerDueNoShow(string key)
        {
            var user = _appUserRepo.SelectWhere(x => x.Id == key).FirstOrDefault();
            if (user != null)
            {
                if (user.LockStatus != (int)AccountStatus.noLockNomore)
                {
                    var lockHour = Configuration.GetSection("loginTime").GetValue<int>("Lock3Month");
                    user.LockoutEnd = DateTime.Now.AddMonths(lockHour);
                    user.LockStatus = (int)AccountStatus.moreNoShow;
                    _appUserRepo.Update(user);
                    var cust = _customerRepo.SelectWhere(x => x.UserId == user.Id).FirstOrDefault();
                    if (cust != null)
                    {
                        cust.IsActive = false;
                        cust.UpdatedDate = DateTime.Now;
                        cust.UpdatedUser = "LogAccountJobs";
                        _customerRepo.Update(cust);
                    }
                    _unitOfWork.SaveChanges();
                }
            }
        }

        public IEnumerable<BookingSpecialRequest> GetspecialRequest(Guid id)
        {
            return _bookingSpecialRequestRepo.SelectWhere(x => x.IsActive && x.GF_Booking_Id == id).Include(x => x.BookingOtherType);
        }

        public void ResetCountNoMoreShow(string key)
        {
            var user = _appUserRepo.SelectWhere(x => x.Id == key).FirstOrDefault();
            if (user != null)
            {
                user.LockStatus = (int)AccountStatus.open;
                _appUserRepo.Update(user);
                _unitOfWork.SaveChanges();
            }
        }

        public bool CheckFirstBookingForVoucher(Booking currBooking)
        {
            try
            {
                var voucherSettings = _settingRepo.SelectWhere(x => x.Code == "Kigr_voucher" || x.Code == "Lhgr_voucher"
                                    || x.Code == "Rtgr_voucher" || x.Code == "Dngr_voucher"
                                    || x.Code == Constants.fromDateVoucher || x.Code == Constants.toDateVoucher);
                var fromDate = voucherSettings.FirstOrDefault(x => x.Code == Constants.fromDateVoucher)?.Value;
                var toDate = voucherSettings.FirstOrDefault(x => x.Code == Constants.toDateVoucher)?.Value;
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    var fromDateValue = Convert.ToDateTime(fromDate);
                    var toDateVal = Convert.ToDateTime(toDate);
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    var toDateValue = toDateVal.Date + ts;
                    if (fromDateValue <= DateTime.Now && DateTime.Now <= toDateValue)
                    {
                        var kigr_voucher = voucherSettings.FirstOrDefault(x => x.Code == "Kigr_voucher")?.Value;
                        var lhgr_voucher = voucherSettings.FirstOrDefault(x => x.Code == "Lhgr_voucher")?.Value;
                        var rtgr_voucher = voucherSettings.FirstOrDefault(x => x.Code == "Rtgr_voucher")?.Value;
                        var dngr_voucher = voucherSettings.FirstOrDefault(x => x.Code == "Dngr_voucher")?.Value;

                        var bookings = _repo.SelectWhere(x => x.IsActive
                                            && (x.Status == StatusBookingLine.booked.ToString() || x.Status == StatusBookingLine.cancel.ToString())
                                            && x.CreatedDate >= fromDateValue
                                            && x.C_Org_Id == currBooking.C_Org_Id).Include(i => i.TransactionPayments).Include(i => i.Organization);

                        if ((currBooking.Organization.Code == Constants.KI && bookings.Count(x => x.IsVourcher200.HasValue && x.IsVourcher200.Value) < Convert.ToInt32(kigr_voucher))
                            || (currBooking.Organization.Code == Constants.LH && bookings.Count(x => x.IsVourcher200.HasValue && x.IsVourcher200.Value) < Convert.ToInt32(lhgr_voucher))
                            || (currBooking.Organization.Code == Constants.RT && bookings.Count(x => x.IsVourcher200.HasValue && x.IsVourcher200.Value) < Convert.ToInt32(rtgr_voucher))
                            || (currBooking.Organization.Code == Constants.DNG && bookings.Count(x => x.IsVourcher200.HasValue && x.IsVourcher200.Value) < Convert.ToInt32(dngr_voucher)))
                        {
                            var paymentCount = _transactionPaymentRepo.SelectWhere(x => x.IsActive
                                                && x.UserId == new Guid(currBooking.UserId)
                                                && (x.Status == Enums.StatusBookingLine.booked.ToString()
                                                || x.Status == Enums.StatusBookingLine.cancel.ToString()));
                            if (paymentCount.Count() == 1)
                            {
                                if (paymentCount.FirstOrDefault().Status == Enums.StatusBookingLine.booked.ToString())
                                {
                                    currBooking.IsVourcher200 = true;
                                    _repo.UpdateByProperties(currBooking, new List<Expression<Func<Booking, object>>>()
                                                                {
                                                                    x => x.IsVourcher200
                                                                });
                                    return true;
                                }
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public string GetEmailTitleCancelBooking(string langVn)
        {
            if (langVn == Constants.LangEn)
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.cancel_title_en).FirstOrDefault()?.Value;
            }
            else
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.cancel_title_vi).FirstOrDefault()?.Value;
            }
        }

        public string GetEmailTemplateCancelBooking(string langVn)
        {
            if (langVn == Constants.LangEn)
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.cancel_email_en).FirstOrDefault()?.Value;
            }
            else
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.cancel_email_vi).FirstOrDefault()?.Value;
            }
        }

        public BookingTransactionPayment SaveSbCancelReturn(Guid bookingId, string code, string description, string transactionNo)
        {
            var data = _transactionPaymentRepo.SelectWhere(x => x.IsActive && x.BookingId == bookingId).FirstOrDefault();

            if (data != null)
            {
                data.SdkRefundStatus = !string.IsNullOrEmpty(transactionNo); // nếu hoàn tiền không thành công thì transactionNo null
                data.TransactionNo = transactionNo;
                if (!string.IsNullOrEmpty(code) || !string.IsNullOrEmpty(description))
                {
                    data.SdkRefundCode = code + " -- " + description;
                }

                _transactionPaymentRepo.Update(data);
                return data;
            }
            else
            {
                throw new Exception("Không tìm thấy thông tin thanh toán - " + bookingId);
            }
        }


    }
}
