using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Service.DTO.Common
{
    public class PromotionCustomerGroupDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public Guid? C_Org_Id { get; set; }
        public Guid? M_Promotion_Id { get; set; }
        public Guid? MB_CustomerGroup_Id { get; set; }

        //public M_Promotion Promotion { get; set; }
        //public Organization Organization { get; set; }
        public CustomerGroupDTO CustomerGroup { get; set; }
    }
}
