using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.BookingOnline.Service.DTO
{
    public class ContactSupportDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }

        public Guid? CourseId { get; set; }
   
        public bool IsActive { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}