using App.Core.Domain;
using System;

namespace App.BookingOnline.Service.DTO
{
    public class BookingLineDTO : IEntityDTO
    {
        public Guid C_Org_Id { get; set; }
        public Guid GF_Booking_Id { get; set; }
        public bool IsActive { get; set; }
        public string CardNo { get; set; }
        public string BookingCode { get; set; }
        public Guid? MB_CustomerGroup_Id { get; set; }
        public string CustomerGroupName { get; set; }
        public string CustomerGroupCode { get; set; }
        public int NumberPlayers { get; set; }
        public string Notes { get; set; }
        public string Telephone { get; set; }
        public DateTime DateId { get; set; }
        public int StartTee { get; set; }
        public DateTime Tee_Time { get; set; }
        public DateTime TeeTimeEnd { get; set; }
        public int Hole { get; set; }
        public int FlightSeq { get; set; }
        public string Caddie_No { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerOrderName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string BookingStatus { get; set; }
        public decimal? Public_Price { get; set; }
        public decimal? BuggyFee { get; set; }
        public decimal? Promotion_Price { get; set; }
        public decimal? EstimatedPrice { get; set; }
        public decimal? SeagolfPrice { get; set; }
        public decimal? Net_Ammount { get; set; }
        public decimal? Discount_Value { get; set; }
        public decimal? Discount_Amount { get; set; }
        public decimal? Total_Amount { get; set; }
        public decimal? Deposit_Amount { get; set; }
        public string Golf_Price_Description { get; set; }
        public string Golf_Promotion_Id { get; set; }
        public string Golf_Promotion_Name { get; set; }
        public string Golf_CusGroupCode { get; set; }
        public DateTime? TimeOnCoursePlayer { get; set; }
        public string CashierName { get; set; }
        public string GolfBag { get; set; }
        public Guid Id { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Is_NoShow { get; set; }
        public DateTime? UpdateNoShow_Time { get; set; }
        public string UpdateNoShow_UserName { get; set; }
        public string Part { get; set; }
        public int? TurnLength { get; set; }
        public bool? IsUpdateNoShow { get; set; }
        public DateTime? Cancel_Time { get; set; }
        public string Cancel_User { get; set; }
        public Guid? Cancel_Reason_Id { get; set; }
        public string CancelReasonName { get; set; }
        public string Cancel_Description { get; set; }
        public double? DurationPlay { get; set; }
        public decimal? NonRefundedFee { get; set; }
        public bool IsSelected { get; set; }
        public bool IsBooked { get; set; }
        public string TimeDisplay { get; set; }
        // public DateTime? Value { get; set; }
        //public int? Part { get; set; }
        public bool IsPromotion { get; set; }
        public int? AvailableFlightSeq { get; set; }

        public BookingDTO Booking { get; set; }
    }
}
