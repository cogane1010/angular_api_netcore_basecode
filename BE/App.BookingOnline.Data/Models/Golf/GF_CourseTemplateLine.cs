using App.Core.Domain;
using System;

namespace App.BookingOnline.Data.Models.Golf
{
    public class GF_CourseTemplateLine : BaseEntity, IEntity
    {
        public bool IsActive { get; set; }

        public Guid C_Org_Id { get; set; }
        public string OrgName { get; set; }

        public Guid C_Course_Id { get; set; }

        public Guid GF_CourseTemplate_Id { get; set; }

        public int? StartTee { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int? Interval { get; set; }

        public int? TurnLength { get; set; }

        public int? Hole { get; set; }

        public string FlightSeq { get; set; }

        public int? MaxTurnLength { get; set; }

        public string Part { get; set; }        

        public Course Course { get; set; }

        public GF_CourseTemplate GF_CourseTemplate { get; set; }
    }
}
