using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace App.BookingOnline.Service.DTO
{
    public class CourseDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public Guid C_Org_Id { get; set; }
        public string OrgCode { get; set; }
        public string OrganizationName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public string Full_IconUrl { get; set; }
        public string IconData { get; set; }
        public string ImageUrl { get; set; }
        public string Full_Image_Url { get; set; }
        public string ImageData { get; set; }
        public bool IsActive { get; set; }
        public int? OrderValue { get; set; }
        public int CodeInt { get; set; }
        [JsonIgnore]
        public string CreatedUser { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string UpdatedUser { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }
        public bool Selected { get; set; }
    }

}