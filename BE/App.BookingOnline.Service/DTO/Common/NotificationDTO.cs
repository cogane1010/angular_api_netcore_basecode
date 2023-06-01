using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;

namespace App.BookingOnline.Service.DTO.Common
{
    public class NotificationDTO : IEntityDTO
    {
        public Guid? C_Org_Id { get; set; }
        public bool IsActive { get; set; }
        public string NotificationType { get; set; }
        public string Message_Type { get; set; }
        public string Message_Title { get; set; }
        public string Message_Content { get; set; }
        public string Message_TitleEn { get; set; }
        public string Message_ContentEn { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string SendTo { get; set; }
        public DateTime? Sent_Date { get; set; }
        public string Sent_User { get; set; }
        public string Img_Url { get; set; }
        public string Img_FullUrl { get; set; }
        public string ImageData { get; set; }
        public Organization Organization { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid Id { get; set; }
        public string lang { get; set; }
    }

    public class Notification
    {
        public string body { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string click_action { get; set; }
        public string icon { get; set; }
        public string image { get; set; }
        public string id { get; set; }
    }
}
