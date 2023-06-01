using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.BookingOnline.Service.DTO
{
    public class BookingOtherTypeDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public Guid C_Org_Id { get; set; }
        public string OrganizationName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool IsActive { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}