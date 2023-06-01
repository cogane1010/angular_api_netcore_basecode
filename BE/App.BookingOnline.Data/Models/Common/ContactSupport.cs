using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{ 
    public class ContactSupport : BaseEntity, IEntity
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }

        public Guid? CourseId { get; set; }
        public Course Course { get; set; }

    }


}