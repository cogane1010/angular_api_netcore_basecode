using App.Core.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{

    public class NotificationQueue : BaseEntity, IEntity
    {
        public Guid? UserId { get; set; }
        public Guid? BookingId { get; set; }
        public string NotificationType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string TitleEn { get; set; }
        public string ContentEn { get; set; }
        public string LinkId { get; set; }
        public string Img_url { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsSend { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSuccess { get; set; }
        public string SendTo { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        [NotMapped]
        public string Full_Image_Url { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string NameEn { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        public string DescriptionEn { get; set; }
    }


}