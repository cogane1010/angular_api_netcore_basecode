using App.Core.Domain;
using System;

namespace App.BookingOnline.Data.Models
{

    public class SmsHistory : BaseEntity, IEntity
    {
        public Guid? UserId { get; set; }
        public string Code { get; set; }
        public string Mobilephone { get; set; }
        public bool? IsSend { get; set; }
        public bool? IsCorrect { get; set; }
        public bool? IsExpire { get; set; }
        public string Type { get; set; }
        public int TimeLife { get; set; }
        public DateTime SendDate { get; set; }
    }


}