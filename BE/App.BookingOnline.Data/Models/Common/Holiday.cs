using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{ 
    public class Holiday : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateId { get; set; }
        public Organization Organization { get; set; }
    }


}