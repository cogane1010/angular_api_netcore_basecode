using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models
{

    public class UserBankInfo : BaseEntity, IEntity
    {
        public Guid UserId { get; set; }
        public Guid? C_PaymentType_Id { get; set; }
        public string FullName { get; set; }
        public string Start_Month { get; set; }
        public string Start_Year { get; set; }
        public string Expire_Month { get; set; }
        public string Expire_Year { get; set; }

        public string Cvv { get; set; }
        public bool IsActive { get; set; }

        public PaymentType PaymentType { get; set; }
    }

    

}