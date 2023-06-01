using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.Core.Helper;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service
{
    public class CourseService : BaseGridDataService<CourseDTO, Course, CoursePagingModel, ICourseRepository>, ICourseService
    {
        public CourseService(ICourseRepository repo) : base(repo)
        {
        }

        public object GetCourseAllByUser(string userId)
        {
            return AutoMapperHelper.Map<Course, CourseDTO, IEnumerable<Course>, IEnumerable<CourseDTO>>(_gridRepository.GetCourseAllByUser(userId));
        }

        public IEnumerable<CourseDTO> GetCourseByOrg(Guid OrgId)
        {
            return AutoMapperHelper.Map<Course, CourseDTO, IEnumerable<Course>, IEnumerable<CourseDTO>>(_gridRepository.GetCourseByOrg(OrgId));
        }
    }
}

