using App.Core.Domain;
using System;

namespace App.BookingOnline.Data.Models.Common
{
    public class M_Promotion_Course : BaseEntity, IEntity
    {
        public bool IsActive { get; set; }
        public Guid? M_Promotion_Id { get; set; }
        public Guid? C_Course_Id { get; set; }

        public M_Promotion Promotion { get; set; }
        public Course Course { get; set; }

    }
}
