using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Service.DTO
{
    public class TransactionNotApproveLineDTO
    {
        public DateTime TransDate { get; set; }
        public int NoOfRecord { get; set; }
        public OutTransactionHeaderDTO OutHeader { get; set; }
    }
}
