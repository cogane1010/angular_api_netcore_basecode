using System;

namespace App.BookingOnline.Data.Models
{
    public class PaymentData
    {
        public Guid? BookingId { get; set; }
        public string BookingCode { get; set; }
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
        public bool IsPaymentSuccess { get; set; }
        public string Notes { get; set; }
        public string FtId { get; set; }
        public string UserId { get; set; }
        public bool IsConnectSdk { get; set; }
    }
}
