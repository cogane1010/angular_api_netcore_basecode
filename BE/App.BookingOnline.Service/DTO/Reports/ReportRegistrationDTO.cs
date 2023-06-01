using App.Core.Domain;
using System;

namespace App.BookingOnline.Data.Models.Reports
{
    public class ReportRegistrationDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public DateTime? Tee_Time { get; set; }
        public string BookingCode { get; set; }
        public string CourseName { get; set; }
        public string FullName { get; set; }
        public string Cancel_Description { get; set; }
        public string CancelName { get; set; }
        public DateTime? Cancel_Time { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int TotalRow { get; set; }
    }
}
