using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Service.DTO.Common
{
    public class PromotionCourseDTO : IEntityDTO
    {
        public bool IsActive { get; set; }
        public Guid? M_Promotion_Id { get; set; }
        public Guid? C_Course_Id { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public Guid Id { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
