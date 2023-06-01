using App.BookingOnline.Data.Models;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service.DTO.Common
{
    public class GoflBrgCardDTO
    {
        public bool IsCardBrg { get; set; }
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
        public string? Golf_Address { get; set; }
        public DateTime? Golf_DOB { get; set; }
        public string Golf_Email { get; set; }
        public DateTime? Golf_Effective_Date { get; set; }
        public DateTime? Golf_Expire_Date { get; set; }
        public string Golf_CardStatus { get; set; }
        public bool Golf_IsActive { get; set; }
        public bool Golf_IsLock { get; set; }
        public Guid? MB_CustomerGroup_Id { get; set; }

        public List<MemberCardCourseDTO> MemberCardCourses { get; set; }
    }
}
