using App.Core.Domain;
using System;

namespace App.BookingOnline.Service.DTO
{
    public class BookingSessionLineDTO : IEntityDTO
    {
        public Guid? BookingSessionsId { get; set; }
        public bool IsActive { get; set; }
        public string BookingCode { get; set; }
        public string Device_Id { get; set; }
        public string UserId { get; set; }
        public DateTime? DateId { get; set; }
        public DateTime? Start_Time { get; set; }
        public DateTime? End_Time { get; set; }
        public DateTime? Tee_Time { get; set; }
        public string Status { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid Id { get; set; }
    }
}
