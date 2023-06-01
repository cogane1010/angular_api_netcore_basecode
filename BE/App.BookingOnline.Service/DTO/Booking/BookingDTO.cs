using App.BookingOnline.Service.DTO.Common;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace App.BookingOnline.Service.DTO
{
    public class BookingDTO : IEntityDTO
    {
        public Guid GF_Booking_Session_Id { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
        public string BookingCode { get; set; }
        public string Device_Id { get; set; }
        public string UserId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DateId { get; set; }
        public string DateDisplay { get; set; }
        public string CourseName { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public Guid? M_Promotion_Id { get; set; }
        public string PromotionName { get; set; }
        public string Confirm_User { get; set; }
        public string PaymentSbType { get; set; }
        public string PaymentSbId { get; set; }
        public DateTime? Payment_Time { get; set; }
        public Guid? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public bool? IsVourcher200 { get; set; }
        public DateTime? Cancel_Time { get; set; }
        public string Cancel_User { get; set; }
        public string Cancel_UserName { get; set; }
        public Guid? Cancel_Reason_Id { get; set; }
        public string CancelReasonName { get; set; }
        public string Cancel_Description { get; set; }
        public decimal? Tien_hoan { get; set; }
        public string ButtonType { get; set; }
        public int Golfers { get; set; }
        public string language { get; set; }
        public List<DateTime> BookingDates { get; set; }
        public decimal? TotalEstimateFee { get; set; }
        public decimal? NonRefundedFee { get; set; }
        public int? CountPlayers { get; set; }
        public int? ShowUpCount { get; set; }
        public string OrgName { get; set; }

        #region Payment SB Info
        public string Traceid { get; set; }
        public string Ftid { get; set; }
        public string LinkCardAcctId { get; set; }
        public string DebitAccount { get; set; }
        public bool IsPaymentSuccess { get; set; } // trang thái = true nếu như sdk thanh toán thành công
        public string SbOrgCode { get; set; }
        public bool IsRefundMoney { get; set; } = false;
        public decimal? TotalRefund { get; set; }
        public bool IsConnectSdk { get; set; } = false;
        #endregion

        #region Customer Info
        public string CustomerCode { get; set; }
        public string CardFullName { get; set; }
        public string CardEmail { get; set; }
        public string CardMobilePhone { get; set; }
        public string GolfCardNo { get; set; }
        public string CardCourseName { get; set; }
        public int? CardMemberType { get; set; }
        public string CardMemberName { get; set; }
        public string TotalTeetimeDisplay { get; set; }
        public string SpecialNameServices { get; set; }
        public string NotificationType { get; set; }
        #endregion

        public Guid Id { get; set; }


        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string UpdatedUser { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }

        public CourseDTO Courses { get; set; }
        public PromotionDTO SeletedPromotion { get; set; }
        public List<PromotionDTO> Promotions { get; set; }
        public List<BookingLineDTO> BookingTeetime { get; set; }
        //public List<TeeTime> TeeTimes { get; set; }
        public List<BookingSpecialRequestDTO> BookingSpecialRequests { get; set; }
        public List<CustomerGroupDTO> MemberCardTypes { get; set; }
        public Guid? OrgId { get; set; }
    }

    /// <summary>
    /// tham so chuyen vao o booking
    /// </summary>
    public class BookingTeeTime
    {
        public Guid? CourseId { get; set; }
        public Guid? OrgId { get; set; }
        public Guid? PromotionId { get; set; }
        public DateTime? BookingDate { get; set; }
        public string UserId { get; set; }
        public string CourseCode { get; set; }
        public string OrgCode { get; set; }
        public string Golf_CusGroupCode { get; set; }
        public string BK_CusGroupCode { get; set; }
        public string CustomerFullName { get; set; }
        public string CardNo { get; set; }
        public DateTime? BookingTime { get; set; }
        public int Hole { get; set; }
    }

    public class MemberCardTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class CheckCardDto
    {
        public string CourseCode { get; set; }
        public string OrgCode { get; set; }
        public List<CheckCardTeetime> CheckCardTeetimes { get; set; }
    }

    public class CheckCardTeetime
    {
        public string MsgCode { get; set; }
        public List<string> MsgParams { get; set; }
        public string CardNo { get; set; }
        public string CustomerFullName { get; set; }
        public DateTime TeeTime { get; set; }
        public string Golf_CusGroupCode { get; set; }
        public string CustomerGroupCode { get; set; }
    }

    public class SbCancelReturn
    {
        public Guid? BookingId { get; set; }
        public string BookingCode { get; set; }
        public string TransactionNo { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

    }

    public class GolfBagDto
    {
        public Guid? BookingId { get; set; }
        public Guid? BookingLineId { get; set; }
        public string Cashier { get; set; }
        public DateTime? TimeOnCoursePlayer { get; set; }
        public string GolfBag { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
    }
}
