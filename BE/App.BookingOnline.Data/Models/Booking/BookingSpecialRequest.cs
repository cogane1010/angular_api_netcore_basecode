using App.Core.Domain;
using System;

namespace App.BookingOnline.Data.Models
{
    public class BookingSpecialRequest : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public Guid GF_Booking_Id { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Guid? C_Booking_OtherType_Id { get; set; }
        public BookingOtherType BookingOtherType { get; set; }
        public Organization Organization { get; set; }
        public Booking Booking { get; set; }
    }


}