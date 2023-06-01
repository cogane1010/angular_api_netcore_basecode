using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.BookingOnline.Service.DTO
{
    public class HotlineDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    
    }

}