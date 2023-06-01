using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace App.BookingOnline.Service.DTO
{
    public class UserDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MobilePhone { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserId { get; set; }
        public IEnumerable<AspRoleDTO> Roles { get; set; }
        public bool HasRole { get; set; }
        public Guid? C_Org_Id { get; set; }
        public string OrganizationName { get; set; }

    }

}