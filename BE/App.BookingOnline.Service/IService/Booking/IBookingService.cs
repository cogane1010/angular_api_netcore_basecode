using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface IBookingService : IBaseGridDataService<BookingDTO, BookingFilterModel>
    {
        IEnumerable<BookingDTO> BookingHistory(BookingFilterModel filter);
        RespondData GetBookingTeetimeData(BookingTeeTime model);
        RespondData GetCustomerGroup(Guid orgId);
        RespondData GetBookingOtherType(Guid courseId);
        object GetBookingData(BookingDTO model);
        string CheckSelectedTeesheet(List<BookingLineDTO> selectedTeeSheet, string userId, Guid courseId, Guid orgId);
        RespondData CancelBooking(Guid bookingId, string userId, bool isFromWeb, string bookingCode = "", string loginUser = "", string cancelId = "", string cancelDesc = "");
        ValueTask<BookingDTO> InsertBookingSession(BookingDTO model);
        RespondData BookingHistoryDetail(BookingFilterModel model);
        RespondData GetBookingByCode(string code, string curOrgId, DateTime? bookedDate);
        RespondData InsertBankInfo(UserBankInfoDTO model);
        RespondData GetBankInfo(string userId);
        RespondData GetPaymentType(string userId);
        List<BookingSpecialRequestDTO> GetBookingSpecialRequests(Guid? C_Org_Id);
        RespondData SaveSbCancelReturn(SbCancelReturn model);
        List<CustomerGroupDTO> GetCustomerGroupData(Guid orgId);
        RespondData SaveTimePlayerOnBoard(GolfBagDto model);
        RespondData CheckMemberCard(BookingTeeTime input);
        RespondData GetPriceGolfPlayer(ref BookingDTO model);
        Task<RespondData> InssertGolfPlayerStep(BookingDTO model);
        Task<RespondData> PriceCheckStep(BookingDTO model);
        RespondData CancelGolfBag(GolfBagDto model);
        RespondData GolfPaymentStep(PaymentData model);
        object PaymentStep(BookingDTO model, string userId);
        RespondData CheckAllMemberCards(CheckCardDto input);
        RespondData CancelTempBookingTeetime(Guid id);
        RespondData RefreshBookingCode(PaymentData model);
        RespondData CheckRuleCancelBooking(Guid bookingId, string userId, bool v);
        void LockAccountCustomerDueNoShow();
        bool CheckNumberAvailableSlot(List<DateTime> tees, string userId, Guid courseId, Guid orgId, int? cardType);
        bool CheckNumberFlightHoliday(DateTime dateId, Guid c_Org_Id, int lines);
    }
}
