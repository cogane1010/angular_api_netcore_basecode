using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models.Common
{
    public class M_Promotion : BaseEntity, IEntity
    {
        public Guid? C_Org_Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public string PromotionCode { get; set; }
        public string Img_Url { get; set; }
        public string Img_Url_En { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string DOW { get; set; }
        public string ApplyTime_From { get; set; }
        public string ApplyTime_To { get; set; }
        public string Promotion_Type { get; set; }
        public int Priotity { get; set; }
        public bool IsHotPromotion { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Promotion_Formula { get; set; }
        public int? Promotion_Value { get; set; }
        public int? PublicPrice_Percent { get; set; }
        public int? PromotionPrice_Percent { get; set; }

        public Organization Organization { get; set; }
        public IEnumerable<M_Promotion_Course> PromotionCourse { get; set; }
        public IEnumerable<M_Promotion_CustomerGroup> PromotionCustomerGroup { get; set; }
        public IEnumerable<BookingSession> BookingSessions { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
