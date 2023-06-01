using App.Core.Domain;
using System;

namespace App.BookingOnline.Data.Models
{
    public class TransactionLogs : BaseEntity, IEntity
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string LogName { get; set; }
        public string LogText { get; set; }
        public DateTime DateTrans { get; set; }
    }
}