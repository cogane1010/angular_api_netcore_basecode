using App.Core.Domain;
using System;

namespace App.BookingOnline.Data.Models
{
    public class FtpTransJobHistory : BaseEntity, IEntity
    {
        public DateTime DateRun { get; set; }
        public DateTime DateId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string SeabankFilePath { get; set; }
        public bool IsFtpFile { get; set; }
        public bool IsReadFile { get; set; }
        public bool IsInsertData { get; set; }             
        public string Status { get; set; } 
        public string UserId { get; set; }
        public bool IsActive { get; set; } = true;
        public string DescriptionError { get; set; }
       
    }


}