using App.BookingOnline.Data.Models.Common;
using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models
{
    public class CustomerGroup : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public Organization Organization { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int? OrderValue { get; set; }
        public bool IsActive { get; set; }

        public List<CustomerGroupMapping> CustomerGroupMappings { get; set; }
        public List<MemberCard> MemberCards { get; set; }
        public List<MemberCardCourse> MemberCardCourses { get; set; }
        public List<M_Promotion_CustomerGroup> PromotionCustomerGroup { get; set; }
        public List<BookingLine> BookingLines { get; set; }
    }


    public class CustomerGroupMapping : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public Guid MB_CustomerGroup_Id { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
        public string Golf_Code { get; set; }
        public bool IsActive { get; set; }
    }


}