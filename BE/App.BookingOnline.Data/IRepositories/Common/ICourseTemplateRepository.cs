using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Models.Golf;
using App.Core.Repositories;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.IRepositories.Common
{
    public interface ICourseTemplateRepository : IBaseGridRepository<GF_CourseTemplate, CourseTemplateFilterModel>
    {
        IEnumerable<GF_CourseTemplateLine> GetCourseTemplatelineByCourseTemplateId(Guid Id);
        bool CheckCourseTemplateCode(string code, Guid? id);
    }
}
