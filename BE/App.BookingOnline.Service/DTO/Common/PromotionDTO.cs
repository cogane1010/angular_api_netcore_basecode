using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service.DTO.Common
{
    public class PromotionDTO : IEntityDTO
    {
        public Guid? C_Org_Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string NotificationType { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
        public string PromotionCode { get; set; }
        public string Img_Url { get; set; } = string.Empty;
        public string Img_Url_En { get; set; } = string.Empty;
        public string Full_Image_Url { get; set; }
        public string Full_Image_Url_En { get; set; }
        public string Img_Url_mobile { get; set; }
        public string Full_Image_Url_mobile { get; set; }
        public string ImageData { get; set; }
        public string ImageDataEn { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string DOW { get; set; }
        public string ApplyTime_From { get; set; }
        public string ApplyTime_To { get; set; }
        public string Promotion_Type { get; set; }
        public int Priotity { get; set; }
        public bool IsHotPromotion { get; set; }
        public string Promotion_Formula { get; set; }
        public int? Promotion_Value { get; set; }
        public int? PublicPrice_Percent { get; set; }
        public int? PromotionPrice_Percent { get; set; }
        public Guid Id { get; set; }
        public string CustomerGroupNames { get; set; }
        public IEnumerable<string> CustomerGroupList { get; set; }
        public string CourseNames { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool AppliedDate0 { get; set; }
        public bool AppliedDate1 { get; set; }
        public bool AppliedDate2 { get; set; }
        public bool AppliedDate3 { get; set; }
        public bool AppliedDate4 { get; set; }
        public bool AppliedDate5 { get; set; }
        public bool AppliedDate6 { get; set; }
        public bool AppliedDate7 { get; set; }

        public IEnumerable<CourseDTO> Course { get; set; }
        public IEnumerable<CustomerGroupDTO> CustomerGroups { get; set; }

        public List<PromotionCourseDTO> PromotionCourses { get; set; }
        public IEnumerable<PromotionCustomerGroupDTO> PromotionCustomerGroup { get; set; }
    }
}
