using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models
{
    public class MemberCard : BaseEntity, IEntity
    {
        public Guid MB_Customer_Id { get; set; }
        public Customer Customer { get; set; }
        public Guid C_Org_Id { get; set; }
        public Organization Organization { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Golf_MemberCardId { get; set; }
        public string Golf_IDNo { get; set; }
        public string Golf_CardNo { get; set; }
        public string Golf_FullName { get; set; }
        public string Golf_Mobilephone { get; set; }
        public string Golf_Address { get; set; }
        public DateTime? Golf_DOB { get; set; }
        public string Golf_Email { get; set; }
        public string Golf_CardType { get; set; }
        public DateTime? Golf_Effective_Date { get; set; }
        public DateTime? Golf_Expire_Date { get; set; }
        public string Golf_CardStatus { get; set; }
        public bool Golf_IsActive { get; set; }
        public bool Golf_IsLock { get; set; }
        public Guid? MB_CustomerGroup_Id { get; set; }
        public CustomerGroup CustomerGroup { get; set; }

        public List<MemberCardCourse> MemberCardCourses { get; set; }
    }

    public class MemberCardCourse : BaseEntity, IEntity
    {
        public Guid MC_MemberCard_Id { get; set; }
        public MemberCard MemberCard { get; set; }
        public Guid? MB_CustomerGroup_Id { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
        public Guid? C_Course_Id { get; set; }
        public Course Course { get; set; }
        public string GolfCode { get; set; }
        public string GolfName { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Description { get; set; }
    }

}