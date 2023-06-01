using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Data.Models.Common
{
    public class M_Promotion_CustomerGroup : BaseEntity, IEntity
    {
        public bool IsActive { get; set; }
        public Guid? C_Org_Id { get; set; }
        public Guid? M_Promotion_Id { get; set; }
        public Guid? MB_CustomerGroup_Id { get; set; }

        public M_Promotion Promotion { get; set; }
        public Organization Organization { get; set; }
        public CustomerGroup CustomerGroup { get; set; }
    }
}
