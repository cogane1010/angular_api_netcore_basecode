using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Data.Models.Golf;
using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models
{
    public class Course : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? OrderValue { get; set; }
        public int CodeInt { get; set; }
        public string PhoneSupport { get; set; }
        public string IconUrl { get; set; }
        public string ImageUrl { get; set; }
        public string EmailSupport { get; set; }
        public string TimeSupport { get; set; }
        public string NoteSupport { get; set; }
        public Organization Organization { get; set; }
        public IEnumerable<MemberCardCourse> MemberCardCourses { get; set; }
        public IEnumerable<GF_CourseTemplateLine> GF_CourseTemplateLine { get; set; }
        public IEnumerable<TeeSheetLockLine> TeeSheetLockLines { get; set; }
        public IEnumerable<M_Promotion_Course> PromotionCourse { get; set; }
        public IEnumerable<BookingSession> BookingSessions { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }


}