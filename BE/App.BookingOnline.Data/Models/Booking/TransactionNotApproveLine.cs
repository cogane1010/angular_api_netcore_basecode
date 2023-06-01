using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Data.Models
{
    public class TransactionNotApproveLine
    {
        public DateTime TransDate { get; set; }
        public int NoOfRecord { get; set; }
    }
}
