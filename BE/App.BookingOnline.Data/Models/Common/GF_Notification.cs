using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Data.Models.Common
{
    public class GF_Notification : BaseEntity, IEntity
    {
        public Guid? C_Org_Id { get; set; }
        public bool IsActive { get; set; }
        public string Message_Type { get; set; }
        public string Message_Title { get; set; }
        public string Message_Content { get; set; }
        public string Message_TitleEn { get; set; }
        public string Message_ContentEn { get; set; }
        public string Code { get; set; }
        public int? Status { get; set; }
        public DateTime? Sent_Date { get; set; }
        public string Sent_User { get; set; }
        public string Img_Url { get; set; }

        public Organization Organization { get; set; }
    }
}
