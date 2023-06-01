using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Service.DTO.Common
{
    public class HomeDTO : IEntityDTO
    {
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public bool IsConnectSdk { get; set; } = false;
        public DateTime? UpdatedDate { get; set; }
        public Guid Id { get; set; }
        public CustomerDTO Customer { get; set; }
        public IEnumerable<PromotionDTO> Promotions { get; set; } = new List<PromotionDTO>();
        public IEnumerable<PromotionDTO> SeaGofl { get; set; } = new List<PromotionDTO>();
        public IEnumerable<CourseDTO> Courses { get; set; } = new List<CourseDTO>();
    }
}
