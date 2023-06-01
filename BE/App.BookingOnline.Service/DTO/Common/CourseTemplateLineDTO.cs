using App.Core.Domain;
using System;

namespace App.BookingOnline.Service.DTO.Common
{
    public class CourseTemplateLineDTO : IEntityDTO
    {
        public bool IsActive { get; set; }
        public Guid C_Org_Id { get; set; }
        public string OrgName { get; set; }
        public Guid C_Course_Id { get; set; }
        public int? StartTee { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? Interval { get; set; }
        public int? TurnLength { get; set; }
        public int? Hole { get; set; }
        public string FlightSeq { get; set; }
        public int? MaxTurnLength { get; set; }
        public string Part { get; set; }
        public Guid Id { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
