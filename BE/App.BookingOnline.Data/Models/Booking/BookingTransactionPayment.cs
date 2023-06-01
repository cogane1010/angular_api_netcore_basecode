using App.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{
    public class BookingTransactionPayment : BaseEntity, IEntity
    {
        public bool IsActive { get; set; }
        public string Traceid { get; set; }
        public string Ftid { get; set; }
        public string LinkCardAcctId { get; set; }
        public string SourceType { get; set; }
        public string AccountID { get; set; }
        public string Company { get; set; }
        public string AccountType { get; set; }
        public string EmbosingName { get; set; }
        public string CardNumber { get; set; }
        public string CardProduct { get; set; }
        public string CardProductDesc { get; set; }
        public string CustomerId { get; set; }
        public string DebitAccount { get; set; }
        public decimal TotalMoney { get; set; }
        public Guid BookingId { get; set; }
        public Guid? OrgId { get; set; }
        public Organization Organization { get; set; }
        public string OrgCode { get; set; }
        public Guid UserId { get; set; }
        public DateTime DatePayment { get; set; }
        public Booking Booking { get; set; }
        public string Status { get; set; }
        public DateTime? CancelTime { get; set; }
        public string CancelUer { get; set; }
        public string CancelStatus { get; set; }
        public string CancelDescription { get; set; }
        public DateTime? InMoneyDate { get; set; }
        public decimal? TotalMoneyAcc { get; set; }
        public decimal? OutMoneyAcc { get; set; }
        public string OutMoneyUser { get; set; }
        public DateTime? OutMoneyDate { get; set; }
        public string InMoneyUser { get; set; }
        public DateTime? MoneyChangeDate { get; set; }
        public bool? SdkRefundStatus { get; set; }
        public string SdkRefundCode { get; set; }
        public string SdkDescription { get; set; }
        public string TransactionNo { get; set; }
        public decimal? SdkRefundMoney { get; set; }


        [NotMapped]
        public string CustomerName { get; set; }
        [NotMapped]
        public string DebitAcc { get; set; }
        [NotMapped]
        public string CreditAcc { get; set; }
        [NotMapped]
        public string BookingCode { get; set; }
        [NotMapped]
        public int? StatusLine { get; set; }
        [NotMapped]
        public int StatusOrder { get; set; }
        [NotMapped]
        public string TotalTeetimeDisplay { get; set; }
        [NotMapped]
        public int PlayerCount { get; set; }
        [NotMapped]
        public string CardFullName { get; set; }
        [NotMapped]
        public string UserRc_code { get; set; }
        [NotMapped]
        public string Trans_type { get; set; }
        [NotMapped]
        public DateTime? MoneyAtAcc { get; set; }
    }


}