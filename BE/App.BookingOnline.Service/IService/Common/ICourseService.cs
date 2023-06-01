using App.BookingOnline.Data.Paging;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service
{
    public interface ICourseService : IBaseGridDataService<CourseDTO, CoursePagingModel>
    {
        IEnumerable<CourseDTO> GetCourseByOrg(Guid OrgId);
        object GetCourseAllByUser(string userId);
    }

   
}