using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.BookingOnline.Service.DTO
{

    public class OrganizationTypeDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class OrganizationDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid C_OrgType_Id { get; set; }
        public string OrganizationTypeName { get; set; }
        public bool IsActive { get; set; }
        public bool IsSummary { get; set; }
        public int? OrderValue { get; set; }
        public string PhoneSupport { get; set; }
        public string IconUrl { get; set; }
        public string ImageUrl { get; set; }
        public string EmailSupport { get; set; }
        public string TimeSupport { get; set; }
        public string NoteSupport { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public OrganizationInfoDTO OrganizationInfo { get; set; }

        public List<CourseDTO> Courses { get; set; }
    }


    public class OrganizationInfoDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public Guid C_Org_Id { get; set; }
        public string OrgName { get; set; }
        public string TaxCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string InvoiceName { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceBankAccount { get; set; }
        public string InvoiceBankName { get; set; }
        public string LogoUrl { get; set; }
        public string ShortAddress { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}