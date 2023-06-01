using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models
{
    public class BookingOtherType : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool IsActive { get; set; }

        public Organization Organization { get; set; }

        public IEnumerable<BookingSpecialRequest> BookingSpecialRequests { get; set; }
    }
}