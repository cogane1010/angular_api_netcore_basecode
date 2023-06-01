using App.BookingOnline.Data.FilterModel;
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
    public interface ICancelBookingRepository : IBaseGridRepository<Booking, BookingFilterModel>
    {

        //IEnumerable<Booking> BookingHistory(BookingFilterModel filter);
        IEnumerable<Booking> BookingHistory(BookingFilterModel filter);
        IEnumerable<BookingLine> GetAllBookingLine();
        IEnumerable<BookingSpecialRequest> GetAllBookingSpecialRequest();
        IEnumerable<Booking> GetAllBooking();
        IEnumerable<GF_CourseTemplateLine> GetCourseTemplateLineByCourse(Guid courseId, DateTime bookingDate);
        IEnumerable<GF_CourseTemplateLine> GetCourseTemplateLineByPromotion(Guid promotionId);
        Course GetCourseById(Guid courseId);
        List<M_Promotion> GetPromotionByBooking(Guid courseId, DateTime value, DateTime dateTime);
        // M_Promotion GetPromotionById(Guid promotionId);
        IEnumerable<CustomerGroup> GetCustomerGroup(Guid courseId);
        CustomerGroup GetCustomerGroupByCode(string code);
        IEnumerable<BookingOtherType> GetBookingOtherType(Guid courseId);
        string CheckSelectedTeesheets(IEnumerable<SelectedTeeSheetTemp> selectedTeeSheet, string userId);
        int CheckTeesheet(DateTime selectedTeeSheet, string userId, Guid courseId);
        ValueTask<BookingSession> InsertBookingSession(BookingSession bookingsession);
        IEnumerable<MemberCard> GetMemberCard(string userId);
        Customer GetCustomerById(string userId);
        IEnumerable<BookingOtherType> GetBookingSpecialRequest(Guid c_Org_Id);
        Booking BookingHistoryDetail(Guid? bookingId, string userId);
        RespondData InsertBankInfo(UserBankInfo data);
        UserBankInfo GetBankInfo(string userId);
        IEnumerable<PaymentType> GetPaymentType();
        List<BookingOtherType> GetBookingSpecialRequests(Guid? C_Org_Id);
        RespondData CancelBooking(Guid bookingId, string userId);
        BookingSession GetBookingSession(Guid id);
        Organization GetOrganization(Guid? courseId);
        Task<Booking> InsertBooking(Booking booking);
        CustomerGroup GetCustomerGroupId(string value);
        Booking GolfPaymentStep(Guid id, string userId, bool isSuccess, string PaymentSbId, DateTime paymentTime, string paymentSbType);
        void InsertBookingNotifyInQueue(DateTime firstTeeTime, string loginUser, string language, string courseName, Guid bookingId);
        void InsertNotifyInQueue(NotificationQueue model, string loginUser);
        void CancelBookingNotifyInQueue(Guid bookingId, string userId);
        Booking GetLastBooking();
        Booking GetBookingById(Guid id);
        void RemoveBookingLine(IEnumerable<BookingLine> delBookingLine);
        Task<M_Promotion> GetPromotionById(Guid m_Promotion_Id);
        IEnumerable<CustomerGroup> GetAllCustomerGroup();
        string GetCustomerGroupById(Guid? mB_CustomerGroup_Id);
    }
}
