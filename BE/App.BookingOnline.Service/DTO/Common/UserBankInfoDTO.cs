using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;

namespace App.BookingOnline.Service.DTO.Common
{
    public class UserBankInfoDTO : IEntityDTO
    {
        public Guid? UserId { get; set; }
        public Guid? C_PaymentType_Id { get; set; }    
        public string CardNo { get; set; }
        public string FullName { get; set; }
        public string TokenResource { get; set; } 
        public bool IsActive { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid Id { get; set; }
    }
}
