using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models
{
    public class ForgotPassworkHistory : BaseEntity, IEntity
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? LockTimeEnd { get; set; }
        public bool IsActive { get; set; }
    }



}