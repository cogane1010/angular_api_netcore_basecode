using App.BookingOnline.Service.DTO.Common;
using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service.DTO
{
    public class CourseTemplateDTO : IEntityDTO
    {
        public bool IsActive { get; set; }
        public Guid C_Org_Id { get; set; }
        public string OrgName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string DOW { get; set; }
        public Guid Id { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool AppliedDate0 { get; set; }
        public bool AppliedDate1 { get; set; }
        public bool AppliedDate2 { get; set; }
        public bool AppliedDate3 { get; set; }
        public bool AppliedDate4 { get; set; }
        public bool AppliedDate5 { get; set; }
        public bool AppliedDate6 { get; set; }
        public bool AppliedDate7 { get; set; }

        public IEnumerable<CourseTemplateLineDTO> CourseTemplateLine { get; set; }
    }
}
