using App.BookingOnline.Data.Models.Common;
using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models
{
    public class BookingSession : BaseEntity, IEntity
    {
        public Guid? C_Org_Id { get; set; }
        public Guid? C_Course_Id { get; set; }
        public Guid? M_Promotion_Id { get; set; } // Cho Promotion

        public bool IsActive { get; set; }
        public string BookingCode { get; set; }
        public string Device_Id { get; set; }
        public string UserId { get; set; }
        public DateTime? DateId { get; set; }
        public DateTime? Start_Time { get; set; }
        public DateTime? End_Time { get; set; }

        public DateTime? Tee_Time { get; set; }
        public string Status { get; set; }
        public string BookingType { get; set; }


        public M_Promotion M_Promotion { get; set; }
        public Course Course { get; set; }
        public Organization Organization { get; set; }
        public IEnumerable<BookingSessionLine> BookingSessionLines { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }

    }


}