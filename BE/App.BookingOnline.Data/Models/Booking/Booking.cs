using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models.Common;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{
    public class Booking : BaseEntity, IEntity
    {
        public Guid? M_Promotion_Id { get; set; } // Cho Promotion
        public string PromotionName { get; set; }
        public Guid? C_Course_Id { get; set; }
        public Guid C_Org_Id { get; set; }
        public Guid GF_Booking_Session_Id { get; set; }
        public Guid? Cancel_Reason_Id { get; set; }
        public int? BookingOrder { get; set; }
        public bool IsActive { get; set; }
        [Index(IsUnique = true)]
        public string BookingCode { get; set; }
        public string Device_Id { get; set; }
        public string UserId { get; set; }
        public DateTime? DateId { get; set; }
        public DateTime? Start_Time { get; set; }
        public DateTime? End_Time { get; set; }
        public DateTime? Tee_Time { get; set; }
        public string Status { get; set; }
        public string BookingType { get; set; }
        public decimal? TotalEstimateFee { get; set; }
        public decimal? NonRefundedFee { get; set; }
        public string Confirm_User { get; set; }
        public DateTime? Confirm_Date { get; set; }
        public string Confirm_Description { get; set; }
        public string Confirm_Type { get; set; }
        public Guid? PaymentTypeId { get; set; }
        public string PaymentSbType { get; set; }
        public string PaymentSbId { get; set; }
        public DateTime? Payment_Time { get; set; }
        public DateTime? Cancel_Time { get; set; }
        public string Cancel_User { get; set; }
        public bool? IsVourcher200 { get; set; }
        public string Cancel_Description { get; set; }
        public bool Is_NoShow { get; set; }
        public DateTime? UpdateNoShow_Time { get; set; }
        public string UpdateNoShow_UserName { get; set; }

        public string BookedCardNo { get; set; }
        public string MemberType { get; set; }
        public string TeeTimeDisplay { get; set; }
        public string AccountFullName { get; set; }
        public string AccountName { get; set; }

        public PaymentType PaymentType { get; set; }
        public CancelReason CancelReason { get; set; }
        public BookingSession BookingSession { get; set; }
        public M_Promotion Promotion { get; set; }
        public AppUser AppUser { get; set; }
        public List<BookingLine> BookingLines { get; set; }
        public List<BookingSpecialRequest> BookingSpecialRequests { get; set; }
        public List<BookingTransactionPayment> TransactionPayments { get; set; }
        public Course Course { get; set; }
        public Organization Organization { get; set; }

    }


}