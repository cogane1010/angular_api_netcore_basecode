using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models.Golf
{
    public class GF_CourseTemplate : BaseEntity, IEntity
    {
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }        
        public Guid C_Org_Id { get; set; }
        public string OrgName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string DOW { get; set; }

        public Organization Organization { get; set; }

        public IEnumerable<GF_CourseTemplateLine> GF_CourseTemplateLines { get; set; }
    }
}
