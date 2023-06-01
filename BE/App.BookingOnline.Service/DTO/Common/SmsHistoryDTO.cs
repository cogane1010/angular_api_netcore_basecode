using App.Core.Domain;
using System;

namespace App.BookingOnline.Service.DTO.Common
{
    public class SmsHistoryDTO : IEntityDTO
    {
        public Guid? UserId { get; set; }
        public string Code { get; set; }
        public string Mobilephone { get; set; }
        public bool IsSend { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsExpire { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public int TimeLife { get; set; }
        public DateTime? SendDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid Id { get; set; }
    }

    public class RespondFirebase
    {
        public int Message_id { get; set; }
    }
}
