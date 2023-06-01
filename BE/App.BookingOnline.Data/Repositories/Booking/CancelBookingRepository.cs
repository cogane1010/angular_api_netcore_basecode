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

namespace App.BookingOnline.Data.Repositories
{
    public class CancelBookingRepository : BaseGridRepository<Booking, BookingFilterModel>, ICancelBookingRepository
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
        public IConfiguration Configuration { get; }

        public CancelBookingRepository(IUnitOfWork unitOfWork, BookingOnlineDbContext context,
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
        }

        public override PagingResponseEntity<Booking> GetPaging(BookingFilterModel filter)
        {
            var teetime = string.IsNullOrEmpty(filter.Teetime) ? null : filter.Teetime.Split(':');
            var data = _repo.SelectWhere(x => x.IsActive
                         && (string.IsNullOrEmpty(filter.BookingCode) || x.BookingCode == filter.BookingCode)
                         && (string.IsNullOrEmpty(filter.Fullname) || x.AppUser.FullName.Contains(filter.Fullname))
                         && (string.IsNullOrEmpty(filter.PhoneNumber) || x.AppUser.PhoneNumber.Contains(filter.PhoneNumber))                         
                         && (string.IsNullOrEmpty(filter.Email) || x.AppUser.Email.Contains(filter.Email))
                         && ((string.IsNullOrEmpty(filter.Status) && x.Status == StatusBookingLine.booked.ToString()) || x.Status == filter.Status)
                         && (filter.DateCreated == null || x.CreatedDate.Date == filter.DateCreated.Value.Date)
                         && (filter.BookingDate == null || x.DateId.Value.Date == filter.BookingDate.Value.Date)
                         && (filter.C_Org_Id == null || x.C_Org_Id == filter.C_Org_Id)
                         && (filter.C_Course_Id == null || x.C_Course_Id == filter.C_Course_Id )
                         && (string.IsNullOrEmpty(filter.Teetime) || (x.BookingLines.Any(b => b.Tee_Time.Value.Hour == Convert.ToInt32(teetime[0])
                                                                    && b.Tee_Time.Value.Minute == Convert.ToInt32(teetime[1]))))
                         && (!filter.NumberPlayers.HasValue || x.BookingLines.Count == filter.NumberPlayers))
                .OrderByDescending(o => o.DateId)
                .Include(i => i.Course).ThenInclude(t => t.Organization)
                .Include(i => i.BookingLines).Include(i => i.AppUser)
                .Include(i => i.BookingSpecialRequests).ThenInclude(t => t.BookingOtherType);

            var result = new PagingResponseEntity<Booking>
            {
                Data = data.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize).ToList()
            };
            result.Count = data.Count();
            return result;
        }

        public Booking GetLastBooking()
        {
            var entity = _repo.SelectWhere(x => x.IsActive && x.DateId.Value.Month == DateTime.Now.Month)
                .OrderByDescending(o => o.BookingOrder).FirstOrDefault();
            return entity;
        }

        public override Booking SingleOrDefault(Guid id)
        {
            var entity = _repo.AsQueryable()
                .Include(x => x.BookingSession).ThenInclude(x => x.Course)
                .Include(x => x.BookingSpecialRequests).ThenInclude(x => x.BookingOtherType)
                .Include(x => x.BookingLines).ThenInclude(x => x.CustomerGroup)
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
                .OrderByDescending(d => d.UpdatedDate);

            return querry;
        }

        public Booking BookingHistoryDetail(Guid? bookingId, string userId)
        {
            var data = _repo.SelectWhere(x => x.UserId == userId && x.Id == bookingId).Include(i => i.BookingLines)
                .Include(a => a.BookingSpecialRequests).FirstOrDefault();
            return data;
        }

        public IEnumerable<GF_CourseTemplateLine> GetCourseTemplateLineByCourse(Guid courseId, DateTime bookingDate)
        {
            //var now = DateTime.Now;
            var curDay = GetDow(bookingDate);

            var data = _courseTemplateRepo.SelectWhere(x => x.IsActive && x.DOW.Contains(curDay)
                                            && x.StartDate <= bookingDate && bookingDate <= x.EndDate)
                .Join(_courseTemplateLineRepo.SelectWhere(a => a.IsActive && a.C_Course_Id == courseId),
                        tem => tem.Id,
                        line => line.GF_CourseTemplate_Id,
                        (stud, stand) => new { tem = stud, line = stand })
                        .Select(s => s.line).OrderBy(b => b.Part);

            return data;
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
            return "1";
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

        public string CheckSelectedTeesheets(IEnumerable<SelectedTeeSheetTemp> selectedTeeSheet, string userId)
        {
            var bookedTeesheet = string.Empty;
            var lockedTeesheet = string.Empty;
            foreach (var item in selectedTeeSheet)
            {
                var teesshet = CheckTeesheetInSession(item, userId);
                if (!string.IsNullOrEmpty(teesshet))
                {
                    if (string.IsNullOrEmpty(bookedTeesheet))
                    {
                        bookedTeesheet = teesshet;
                    }
                    bookedTeesheet += "," + teesshet;
                }
                else
                {
                    var teesshetLock = CheckTeesheetlock(item, userId);
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

        public int CheckTeesheet(DateTime selectedTeeSheet, string userId, Guid courseId)
        {
            var bookingSession = _bookingSessionLineRepo.SelectWhere(x => x.DateId.Value.Date == selectedTeeSheet.Date.Date
                                    && x.Tee_Time.Value.Hour == selectedTeeSheet.Hour
                                    && x.Tee_Time.Value.Minute == selectedTeeSheet.Minute
                                    && x.BookingSession.C_Course_Id == courseId
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
            var bookedTeesshet = _bookingLineRepo.SelectWhere(x => x.DateId == selectedTeeSheet.Date
                                    && x.Tee_Time.Value.Hour == selectedTeeSheet.Hour
                                    && x.Tee_Time.Value.Minute == selectedTeeSheet.Minute
                                    && x.Booking.C_Course_Id == courseId
                                    && x.IsActive);

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
                var bycurrUser = bookedTeesshet.Where(x => x.CreatedUser == userId);
                if (bycurrUser.Count() >= 1)
                {
                    var avaibleSlot = (int)ValidNumPlayerBook.CurrentUser - bookedTeesshet.Count();
                    return avaibleSlot > 0 ? avaibleSlot : 0;
                }
            }
            return 0;
        }

        public string CheckTeesheetlock(SelectedTeeSheetTemp selectedTeeSheet, string userId)
        {
            var bookedTeesshetSession = _teeSheetLockLineRepo.SelectWhere(x =>
                                  (x.StartTimeValue <= selectedTeeSheet.Tee_Time && x.EndTimeValue >= selectedTeeSheet.Tee_Time)
                                | (x.StartTimeValue <= selectedTeeSheet.TeeTimeEnd && x.EndTimeValue >= selectedTeeSheet.TeeTimeEnd))
                .FirstOrDefault();

            if (bookedTeesshetSession != null)
            {
                return selectedTeeSheet.Tee_Time.Hour + ":" + selectedTeeSheet.Tee_Time.Minute;

            }
            return string.Empty;

        }

        public string CheckTeesheetInSession(SelectedTeeSheetTemp selectedTeeSheet, string userId)
        {
            var bookedTeesshetSession = _bookingSessionLineRepo.SelectWhere(x => x.DateId == selectedTeeSheet.Tee_Time.Date
                                    && x.Status == StatusBookingLine.open.ToString()
                                    && x.Tee_Time.Value.Hour == selectedTeeSheet.Tee_Time.Hour && x.IsActive
                                    && x.Tee_Time.Value.Minute == selectedTeeSheet.Tee_Time.Minute
                                    && (x.Start_Time <= DateTime.Now || x.End_Time >= DateTime.Now)
                                    && x.IsActive).FirstOrDefault();

            if (bookedTeesshetSession != null)
            {
                return bookedTeesshetSession.Tee_Time.Value.Hour + ":" + bookedTeesshetSession.Tee_Time.Value.Minute;

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
                    using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
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
                .Join(_customerRepo.SelectWhere(x => x.IsActive && x.UserId == userId),
                        tem => tem.MB_Customer_Id,
                        line => line.Id,
                        (stud, stand) => new { card = stud, line = stand })
                        .Select(s => new MemberCard
                        {
                            Id = s.card.Id,
                            IsActive = s.card.IsActive,
                            IsDelete = s.card.IsDelete,
                            Golf_Effective_Date = s.card.Golf_Effective_Date,
                            Golf_Expire_Date = s.card.Golf_Expire_Date,
                            Golf_CardNo = s.card.Golf_CardNo,
                            Golf_CardStatus = s.card.Golf_CardStatus,
                            Golf_MemberCardId = s.card.Golf_MemberCardId,
                            Golf_FullName = s.card.Golf_FullName,
                            Golf_Mobilephone = s.card.Golf_Mobilephone,
                            Golf_Email = s.card.Golf_Email,
                            Golf_DOB = s.card.Golf_DOB,
                            MemberCardCourses = s.card.MemberCardCourses
                        });
            return card;
        }

        public Customer GetCustomerById(string userId)
        {
            return _customerRepo.SelectWhere(x => x.UserId == userId).Include(i => i.MemberCards).FirstOrDefault();
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

        public UserBankInfo GetBankInfo(string userId)
        {
            return _userBankInfoRepo.SingleOrDefault(x => x.UserId == new Guid(userId) && x.IsActive);
        }

        public IEnumerable<PaymentType> GetPaymentType()
        {
            return _paymentTypeRepo.SelectWhere(x => x.IsActive);
        }

        public List<BookingOtherType> GetBookingSpecialRequests(Guid? C_Org_Id)
        {
            return _bookingOtherTypeRepo.SelectWhere(s => s.IsActive && s.C_Org_Id == C_Org_Id).ToList();
        }
        public Organization GetOrganization(Guid? courseId)
        {
            return _organizationRepo.SingleOrDefault(s => s.IsActive && s.Id == courseId);
        }
        public RespondData CancelBooking(Guid bookingId, string userId)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var booking = _repo.SingleOrDefault(x => x.Id == bookingId && x.UserId == userId && x.Status == StatusBookingLine.booked.ToString());
                    if (booking == null)
                    {
                        return new RespondData { IsSuccess = false, MsgCode = "29" };
                    }
                    else
                    {
                        var cancelResaon = _cancelReasonRepo.SelectWhere(x => x.IsActive && x.Code == Enums.CancelBookingType.PC.ToString()).FirstOrDefault();
                        booking.Status = StatusBookingLine.cancel.ToString();
                        booking.Cancel_Time = DateTime.Now;
                        booking.Cancel_User = userId;
                        if (cancelResaon != null)
                        {
                            booking.Cancel_Reason_Id = cancelResaon.Id;
                        }

                        booking.UpdatedDate = DateTime.Now;
                        booking.UpdatedUser = userId;

                        _repo.Update(booking);

                        var bookingLine = _bookingLineRepo.SelectWhere(x => x.GF_Booking_Id == booking.Id
                                            && x.BookingStatus == StatusBookingLine.booked.ToString()
                                            && x.IsActive);
                        foreach (var item in bookingLine)
                        {
                            item.BookingStatus = StatusBookingLine.cancel.ToString();
                            item.UpdatedUser = userId;
                            item.UpdatedDate = DateTime.Now;
                            _bookingLineRepo.Update(item);
                        }
                        var bookingSession = _bookingSessionLineRepo.SelectWhere(x => x.BookingSessionsId == booking.GF_Booking_Session_Id && x.IsActive);
                        foreach (var item in bookingSession)
                        {
                            item.Status = StatusBookingLine.cancel.ToString();
                            item.UpdatedUser = userId;
                            item.UpdatedDate = DateTime.Now;
                            _bookingSessionLineRepo.Update(item);
                        }
                    }
                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                    return new RespondData { IsSuccess = true };
                }
                catch (Exception e)
                {
                    using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                    {
                        _log.LogError(e.Message);
                    }
                    transaction.Rollback();
                    return new RespondData { IsSuccess = false, MsgCode = "30" };
                }
            }
        }

        public BookingSession GetBookingSession(Guid id)
        {
            return _bookingSessionRepo.SingleOrDefault(x => x.Id == id && x.IsActive);
        }

        public CustomerGroup GetCustomerGroupId(string code)
        {
            return _customerGroupRepo.SingleOrDefault(x => x.Code == code);
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
                    using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                    {
                        _log.LogError(e.Message);
                    }
                    transaction.Rollback();
                    return booking;
                }
            }
        }

        public Booking GolfPaymentStep(Guid id, string userId, bool isSuccess, string PaymentSbId, DateTime paymentTime, string paymentSbType)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var booking = _repo.SingleOrDefault(x => x.Id == id);
                    var status = booking.Status = Enums.StatusBookingLine.close.ToString();
                    if (isSuccess)
                    {
                        status = Enums.StatusBookingLine.booked.ToString();
                    }
                    if (booking != null)
                    {
                        booking.Payment_Time = paymentTime;
                        booking.PaymentSbId = PaymentSbId;
                        booking.PaymentSbType = paymentSbType;
                        booking.BookingType = BookingButtonType.Payment.ToString();
                        booking.Status = status;
                        booking.UpdatedUser = userId;
                        booking.UpdatedDate = DateTime.Now;
                        _repo.Update(booking);

                        var teetimes = _bookingLineRepo.SelectWhere(x => x.GF_Booking_Id == booking.Id && x.IsActive);
                        foreach (var tee in teetimes)
                        {
                            booking.Status = status;
                            booking.UpdatedUser = userId;
                            booking.UpdatedDate = DateTime.Now;
                            _bookingLineRepo.Update(tee);
                        }
                        _unitOfWork.SaveChanges();
                        transaction.Commit();
                        return booking;
                    }
                    return null;
                }
                catch (Exception e)
                {
                    using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                    {
                        _log.LogError(e.Message);
                    }
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public void InsertBookingNotifyInQueue(DateTime firstTeeTime, string loginUser, string language, string courseName, Guid bookingId)
        {
            try
            {
                var title = Configuration.GetValue<string>("NotifiBookingTitle");
                var content = Configuration.GetValue<string>("NotifiBooking");
                //if (language == Constants.LangEn)
                //{
                //    title = Configuration.GetValue<string>("NotifiBookingTitleEn");
                //    content = Configuration.GetValue<string>("NotifiBookingEn");
                //}

                var cust = _customerRepo.SingleOrDefault(x => x.UserId == loginUser);
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
                        Content = string.Format(content, firstTeeTime.Hour + ":" + firstTeeTime.Minute,
                        courseName, firstTeeTime.Date),
                        TitleEn = Configuration.GetValue<string>("NotifiBookingTitleEn"),
                        ContentEn = string.Format(Configuration.GetValue<string>("NotifiBookingEn"), firstTeeTime.Hour + ":" + firstTeeTime.Minute,
                        courseName, firstTeeTime.Date)
                    };
                    InsertNotifyInQueue(data, loginUser);
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                }
            }

        }

        public void InsertNotifyInQueue(NotificationQueue model, string loginUser)
        {
            var data = new NotificationQueue
            {
                SendTo = model.SendTo,
                UserId = model.UserId,
                SendDate = model.SendDate,
                BookingId = model.BookingId,
                IsSend = false,
                IsActive = true,
                NotificationType = model.NotificationType,
                CreatedDate = DateTime.Now,
                CreatedUser = loginUser,
                Title = model.Title,
                Content = model.Content,
                TitleEn = model.TitleEn,
                ContentEn = model.ContentEn
            };
            if (!string.IsNullOrEmpty(data.SendTo))
            {
                _notificationQueueRepo.AddAsync(data);
            }
        }

        public void CancelBookingNotifyInQueue(Guid bookingId, string userId)
        {
            var notify = _notificationQueueRepo.SingleOrDefault(x => x.BookingId == bookingId
                            && x.UserId.ToString() == userId);
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
            return _repo.SelectWhere(x => x.Id == id).Include(i => i.BookingLines).FirstOrDefault();
        }

        public void RemoveBookingLine(IEnumerable<BookingLine> delBookingLine)
        {
            foreach (var item in delBookingLine)
            {
                var delIte = _bookingLineRepo.GetByIdAsync(item.Id);
                if (delIte != null)
                {
                    _bookingLineRepo.Remove(item);
                }
            }
        }

        public async Task<M_Promotion> GetPromotionById(Guid m_Promotion_Id)
        {
            return await _promotionRepo.GetByIdAsync(m_Promotion_Id);
        }

        public IEnumerable<CustomerGroup> GetAllCustomerGroup()
        {
            return _customerGroupRepo.GetAll();
        }

        public string GetCustomerGroupById(Guid? mB_CustomerGroup_Id)
        {
            return _customerGroupRepo.GetByIdAsync(mB_CustomerGroup_Id.Value).Result?.Name;
        }


    }
}
