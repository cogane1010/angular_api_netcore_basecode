using System;

namespace App.BookingOnline.Service.DTO.Common
{
    public class OrganizationInfoDTO
    {
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
    }
}
