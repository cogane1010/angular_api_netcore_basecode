using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Repositories;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Repositories
{
    public interface ICourseRepository : IBaseGridRepository<Course, CoursePagingModel>
    {
        IEnumerable<Course> GetCourseByOrg(Guid OrgId);
        IEnumerable<Course> GetCourseAllByUser(string userId);
    }
}