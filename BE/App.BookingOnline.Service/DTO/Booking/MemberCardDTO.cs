using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service.DTO
{
    public class MemberCardDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public Guid MB_Customer_Id { get; set; }
        public Guid C_Org_Id { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string OrgMemberName { get; set; }
        public string Golf_CardType { get; set; }
        public Guid? MB_CustomerGroup_Id { get; set; }
        public string CustomerGroupName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Golf_MemberCardId { get; set; }
        public string Golf_IDNo { get; set; }
        public string Golf_CardNo { get; set; }
        
        public string Golf_FullName { get; set; }
        public string Golf_Mobilephone { get; set; }
        public string Golf_Email { get; set; }
        public string Golf_Address { get; set; }
        public DateTime? Golf_DOB { get; set; }
        public DateTime? Golf_Effective_Date { get; set; }
        public DateTime? Golf_Expire_Date { get; set; }
        public string Golf_CardStatus { get; set; }
        public bool Golf_IsActive { get; set; }
        public bool Golf_IsLock { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string InforMessage { get; set; }
        public OrganizationDTO Organization { get; set; }
        public List<MemberCardCourseDTO> CoursesMemberCard { get; set; } = new List<MemberCardCourseDTO>();
    }

    public class MemberCardMobile
    {
        public Guid Id { get; set; }
        public string Golf_MemberCardId { get; set; }
        public bool Golf_IsActive { get; set; }
        public string DateValid { get; set; }
        public string GolfCourse { get; set; }
    }

    public class MemberCardExtendDTO : MemberCardDTO
    {
        public List<MemberCardCourseDTO> MemberCardExtendCourses { get; set; }
    }

    public class MemberCardCourseDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public Guid MC_MemberCard_Id { get; set; }
        public Guid? MB_CustomerGroup_Id { get; set; }
        public string CustomerGroupName { get; set; }
        public Guid? C_Course_Id { get; set; }
        public string CourseName { get; set; }
        public string Golf_CusGroupCode { get; set; }
        public string Golf_CusGroupName { get; set; }
        public string BK_CusGroupCode { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Description { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}