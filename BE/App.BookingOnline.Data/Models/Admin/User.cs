using App.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{
    public class User : BaseEntity, IEntity
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public Guid? C_Org_Id { get; set; }

        public Organization Organization { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public string Password { get; set; }

    }

    public class UserRole : BaseEntity, IEntity
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }

    public class ChangePasswordModel
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string RetypeNewPassword { get; set; }
    }
}