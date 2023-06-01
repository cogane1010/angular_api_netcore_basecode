using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{
    public class BookingLine : BaseEntity, IEntity
    {
        public Guid? C_Org_Id { get; set; }
        public Guid GF_Booking_Id { get; set; }
        public Guid? MB_CustomerGroup_Id { get; set; }
        public bool IsActive { get; set; }
        public string CardNo { get; set; }
        public DateTime? DateId { get; set; }
        public int? StartTee { get; set; }
        public DateTime? Tee_Time { get; set; }
        public int? Hole { get; set; }
        public int? FlightSeq { get; set; }
        public string Caddie_No { get; set; }
        public string CustomerFullName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string BookingStatus { get; set; }
        public decimal? Public_Price { get; set; }
        public decimal? Promotion_Price { get; set; }
        public decimal? EstimatedPrice { get; set; }
        public decimal? BuggyFee { get; set; }
        public decimal? SeagolfPrice { get; set; }
        public decimal? Net_Ammount { get; set; }
        public decimal? Discount_Value { get; set; }
        public decimal? Discount_Amount { get; set; }
        public decimal? Total_Amount { get; set; }
        public decimal? Deposit_Amount { get; set; }
        public decimal? NonRefundedFee { get; set; }
        public string Golf_Price_Description { get; set; }
        public string Golf_Promotion_Id { get; set; }
        public string Golf_Promotion_Name { get; set; }
        /// <summary>
        /// nếu có lên sân thì = true
        /// </summary>
        public bool Is_NoShow { get; set; }
        public DateTime? UpdateNoShow_Time { get; set; }
        public string UpdateNoShow_UserName { get; set; }
        public DateTime? TimeOnCoursePlayer { get; set; }
        public string CashierName { get; set; }
        public string GolfBag { get; set; }
        public string GolfDescription { get; set; }
        public string Part { get; set; }
        public int? TurnLength { get; set; }

        public Booking Booking { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
        public string Golf_CusGroupCode { get; set; }
    }

    public class BookStatByOrg
    {
        public DateTime? DateId { get; set; }
        public Guid C_Org_Id { get; set; }
        public string OrganizationName { get; set; }
        public int NumRows { get; set; }
        public int TotalBooking { get; set; }
        public int TotalNoReality { get; set; }
        public List<BookingLineStatistic> BookByCourse { get; set; }
    }
    public class BookingLineStatistic
    {
        public Guid? C_Course_Id { get; set; }
        public string CourseName { get; set; }
        public string Part { get; set; }
        public int NoBooking { get; set; }
        public int NoReality { get; set; }
        public string Type { get; set; }
        public int NumRows { get; set; }
        public List<BookStatByPart> BookStatByParts { get; set; }
    }

    public class BookStatByPart
    {
        public string PartName { get; set; }
        public int NoBooking { get; set; }
        public int NoReality { get; set; }

    }



}