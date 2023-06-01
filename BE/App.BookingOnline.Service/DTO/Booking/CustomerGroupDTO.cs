using App.Core.Domain;
using System;
using System.Text.Json.Serialization;

namespace App.BookingOnline.Service.DTO
{
    public class CustomerGroupDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public Guid C_Org_Id { get; set; }
        public string OrganizationName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int? OrderValue { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public string CreatedUser { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string UpdatedUser { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }

    }

    public class CustomerGroupMappingDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public Guid C_Org_Id { get; set; }
        public Guid MB_CustomerGroup_Id { get; set; }
        public string Golf_Code { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public string CreatedUser { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string UpdatedUser { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }

    }
}