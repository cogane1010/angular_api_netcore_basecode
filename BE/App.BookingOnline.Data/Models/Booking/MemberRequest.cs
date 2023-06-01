using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{ 
    public class MemberRequest : BaseEntity, IEntity
    {
        public DateTime Request_Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public Guid C_Org_Id { get; set; }
        public Organization Organization { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string Request_Status { get; set; }
        public DateTime? Responsed_Date { get; set; }
        public string Responsed_User { get; set; }
        public string Responsed_Description { get; set; }
        [NotMapped]
        public string Request_FullName { get; set; }
        [NotMapped]
        public string Request_MobilePhone { get; set; } 
    }

}