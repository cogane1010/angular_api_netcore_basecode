using App.Core.Domain;
using System;
using System.Text.Json.Serialization;

namespace App.BookingOnline.Service.DTO
{
    public class BookingSpecialRequestDTO : IEntityDTO
    {
        public Guid C_Org_Id { get; set; }
        public Guid GF_Booking_Id { get; set; }
        public bool IsActive { get; set; }
        public Guid C_Booking_OtherType_Id { get; set; }
        public string BookingOtherTypeName { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public Guid Id { get; set; }
        [JsonIgnore]
        public string CreatedUser { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string UpdatedUser { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }
    }
}
