using App.Core.Domain;
using System;

namespace App.BookingOnline.Data.Models
{

    public class LogDateFileOut : BaseEntity, IEntity
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string FileType { get; set; }
    }


}