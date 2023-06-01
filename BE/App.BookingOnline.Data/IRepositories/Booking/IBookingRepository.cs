using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Data.Models.Golf;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.Repositories
{
    public interface IBookingRepository : IBaseGridRepository<Booking, BookingFilterModel>
    {

        //IEnumerable<Booking> BookingHistory(BookingFilterModel filter);
        IEnumerable<Booking> BookingHistory(BookingFilterModel filter);
        IEnumerable<BookingLine> GetAllBookingLine();
        IEnumerable<BookingSpecialRequest> GetAllBookingSpecialRequest();
        IEnumerable<Booking> GetAllBooking();
        IEnumerable<GF_CourseTemplateLine> GetCourseTemplateLineByCourse(Guid courseId, Guid c_Org_Id, DateTime bookingDate);
        IEnumerable<GF_CourseTemplateLine> GetCourseTemplateLineByPromotion(Guid promotionId);
        Course GetCourseById(Guid courseId);
        List<M_Promotion> GetPromotionByBooking(Guid courseId, DateTime value, DateTime dateTime);
        // M_Promotion GetPromotionById(Guid promotionId);
        IEnumerable<CustomerGroup> GetCustomerGroup(Guid courseId);
        CustomerGroup GetCustomerGroupByCode(string code);
        IEnumerable<BookingOtherType> GetBookingOtherType(Guid courseId);
        string CheckSelectedTeesheets(IEnumerable<SelectedTeeSheetTemp> selectedTeeSheet, string userId, Guid courseId, Guid orgId);
        int CheckTeesheet(DateTime selectedTeeSheet, string userId, Guid courseId, Guid orgId, DateTime dateTime, int cardMemberType);
        ValueTask<BookingSession> InsertBookingSession(BookingSession bookingsession);
        IEnumerable<MemberCard> GetMemberCard(string userId);
        Customer GetCustomerByUserId(string userId);
        IEnumerable<BookingOtherType> GetBookingSpecialRequest(Guid c_Org_Id);
        Booking BookingHistoryDetail(Guid? bookingId, string userId);
        RespondData InsertBankInfo(UserBankInfo data);
        IEnumerable<UserBankInfo> GetBankInfo(string userId);
        IEnumerable<PaymentType> GetPaymentType();
        List<BookingOtherType> GetBookingSpecialRequests(Guid? C_Org_Id);
        Booking CancelBooking(Guid bookingId, string userId, string cancelId, string cancelDesc, bool isFromWeb);
        BookingSession GetBookingSession(Guid id);
        Organization GetOrganization(Guid? courseId);
        Task<Booking> InsertBooking(Booking booking);
        CustomerGroup GetCustomerGroupId(string value);
        Booking GolfPaymentStep(PaymentData model);
        void InsertBookingNotifyInQueue(DateTime firstTeeTime, string loginUser, string language, string courseName, Guid bookingId);
        void InsertNotifyInQueue(NotificationQueue model, string loginUser);
        void CancelBookingNotifyInQueue(Guid bookingId, string userId);
        Booking GetLastBooking();
        Booking GetBookingById(Guid id);
        Task RemoveBookingLineAsync(IEnumerable<BookingLine> delBookingLine);
        Task<M_Promotion> GetPromotionById(Guid m_Promotion_Id);
        IEnumerable<CustomerGroup> GetAllCustomerGroup();
        string GetCustomerGroupById(Guid? mB_CustomerGroup_Id);
        void InsertCancelBookingNotifyInQueue(string loginUser, Guid bookingId, string bookingCode, string userId);
        AppUser GetAppUserById(string id);
        IEnumerable<InTransactionDetails> GetInDataTransaction(TransactionFilterModel dateTrans);
        IEnumerable<BookingTransactionPayment> GetbookingPaymentTrans(TransactionFilterModel dateTrans);
        Task AddOutTransactionData(IEnumerable<OutTransactionDetails> outData, string userId, string orgId, DateTime findDate);
        BookingTransactionPayment GetPaymentInfoByBookingId(Guid bookingId);
        Setting GetTimeCancelRule48();
        Setting GetTimeCancelRule24();        
        int CheckReturnMoneyRuleWhenCancel(Booking booking);
        Booking GetBookingByCode(string code, string curOrgId, DateTime? bookedDate);
        BookingLine GetBookingLineById(Guid? bookingLineId);
        BookingLine UpdateBookingLine(BookingLine bookingLine);
        IEnumerable<M_Promotion> GetSeagolfByCourse(Guid courseId, Guid? m_Promotion_Id);
        Organization GetOrganizationById(Guid? id);
        IEnumerable<Booking> GetAllBookingByDate(DateTime date);
        RespondData CancelTempBookingTeetime(Guid id);
        int GetLastFlight(Guid id, DateTime tee_Time);
        IEnumerable<BookingLine> GetLastBookingLine(Guid id, IEnumerable<DateTime> tees, DateTime dateId);
        Booking RefreshBookingCode(PaymentData model, string bookingCode);
        Booking GetBookingByCode(string traceId);
        void SendEmailBookingToCourse(Booking booking, string userId);
        IEnumerable<Booking> GetAllBookingNoShow();
        void LockAccountCustomerDueNoShow(string key);
        bool CheckNumberAvailableSlot(List<DateTime> tees, string userId, Guid courseId, Guid orgId, int? cardType);
        IEnumerable<BookingSpecialRequest> GetspecialRequest(Guid id);
        void ResetCountNoMoreShow(string key);
        bool CheckFirstBookingForVoucher(Booking booking);
        string GetEmailTitleCancelBooking(string langVn);
        string GetEmailTemplateCancelBooking(string langVn);
        BookingTransactionPayment SaveSbCancelReturn(Guid bookingId, string code, string description, string transactionNo);
        bool CheckDayHolidayByOrg(Guid c_Org_Id, DateTime bookingDate);
    }
}
