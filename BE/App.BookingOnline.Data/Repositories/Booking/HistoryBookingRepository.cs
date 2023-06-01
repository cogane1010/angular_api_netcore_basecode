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
    public class HistoryBookingRepository : BaseGridRepository<Booking, BookingFilterModel>, IHistoryBookingRepository
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

        public HistoryBookingRepository(IUnitOfWork unitOfWork, BookingOnlineDbContext context,
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
                        && ((string.IsNullOrEmpty(filter.Status) && (x.Status == StatusBookingLine.booked.ToString() 
                                || x.Status == StatusBookingLine.cancel.ToString())) || x.Status == filter.Status)
                        && (filter.CreatedFrom == null || x.CreatedDate.Date >= filter.CreatedFrom.Value.Date)
                        && (filter.CreatedTo == null || x.CreatedDate.Date <= filter.CreatedTo.Value.Date)
                        && (filter.BookingFrom == null || x.DateId.Value.Date >= filter.BookingFrom.Value.Date)
                        && (filter.BookingTo == null || x.DateId.Value.Date <= filter.BookingTo.Value.Date)
                        && (filter.C_Org_Id == null || x.C_Org_Id == filter.C_Org_Id)
                        && (filter.C_Course_Id == null || x.C_Course_Id == filter.C_Course_Id)
                        && (string.IsNullOrEmpty(filter.Teetime) || (x.BookingLines.Any(b => b.Tee_Time.Value.Hour == Convert.ToInt32(teetime[0])
                                                                    && b.Tee_Time.Value.Minute == Convert.ToInt32(teetime[1]))))
                        && (!filter.NumberPlayers.HasValue || x.BookingLines.Count == filter.NumberPlayers))
                .OrderByDescending(o => o.DateId)
                .Include(i => i.Course).ThenInclude(t => t.Organization)
                .Include(i => i.BookingLines).Include(i => i.AppUser)
                .Include(i => i.BookingSpecialRequests).ThenInclude(t => t.BookingOtherType);


            var result = new PagingResponseEntity<Booking>
            {
                Data = data.Skip(filter.PageIndex * filter.PageSize)
                            .Take(filter.PageSize).ToList()
            };
            result.Count = data.Count();
            return result;
        }






    }
}
