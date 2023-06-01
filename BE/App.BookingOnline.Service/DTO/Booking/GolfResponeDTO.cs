using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service.DTO.Booking
{
    public class GolfRespone
    {
        public string ErrorCode { get; set; }
        public string InfoMessage { get; set; }
        public List<string> MsgParams { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class GolfPriceBookingRespone : GolfRespone
    {
        public GolfPriceBooking Data { get; set; }
    }
    public class GolfPriceBooking
    {
        public decimal? PublicPrice { get; set; }
        public decimal? PromotionPrice { get; set; }
        public decimal? BuggyFee { get; set; }
        public string Public_CustmerGroup_Code { get; set; }
        public string Promotion_CustmerGroup_Code { get; set; }
        public string PublicPrice_Name { get; set; }
        public string PromotionPrice_Name { get; set; }
        public string PublicPrice_Id { get; set; }
        public string PublicPriceVersion_Id { get; set; }
        public string PromotionPrice_Id { get; set; }
    }

    public class GolfMemberBookingRespone : GolfRespone
    {
        public List<CheckCardTeetime> Data { get; set; }
    }

    public class GolfMemberBooking
    {
        public string ErrorCode { get; set; }
        public string Golf_CusGroupCode { get; set; }
        public string BK_CusGroupCode { get; set; }
        public string UsedInCourse { get; set; }

    }

    public class GolfMemberRespone : GolfRespone
    {
        public List<BookOnlineMemberCard> Data { get; set; }
    }

    public class BookOnlineMemberCard
    {
        public string Golf_MemberCardId { get; set; }
        public string Golf_IDNo { get; set; }
        public string Golf_CardNo { get; set; }
        public string Golf_FullName { get; set; }
        public string Golf_Mobilephone { get; set; }
        public string Golf_Address { get; set; }
        public string Golf_DOB { get; set; }
        public string Golf_Email { get; set; }
        public string Golf_CardStatus { get; set; }
        public string Golf_Effective_Date { get; set; }
        public string Golf_Expire_Date { get; set; }
        public bool Golf_IsActive { get; set; }
        public bool Golf_IsLock { get; set; }
        public string C_Org_Id { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string Golf_CardType { get; set; }

        public List<BookOnlineMemberCardCourse> MemberCardCourses { get; set; }
    }

    public class BookOnlineMemberCardCourse
    {
        public string C_Course_Id { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Description { get; set; }
        public string MB_CustomerGroup_Id { get; set; }
        public string Golf_CusGroupCode { get; set; }
        public string Golf_CusGroupName { get; set; }
        public string BK_CusGroupCode { get; set; }
    }
}
