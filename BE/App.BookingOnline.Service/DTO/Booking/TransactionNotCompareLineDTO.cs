using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Service.DTO
{
    public class TransactionNotCompareLineDTO
    {
        public DateTime TransDate { get; set; }
        public int NoOfRecord { get; set; }
        public IEnumerable<PaymentCompare> PaymentsCompare { get; set; }
    }
}
