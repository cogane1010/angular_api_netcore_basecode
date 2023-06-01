
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace App.BookingOnline.Api.Controllers
{

    public class CourseController : GridController<CourseDTO, CoursePagingModel, ICourseService>
    {
        public CourseController(ICourseService service) : base(service)
        {
        }

        [HttpPost("GetByOrg")]
        public virtual RespondData GetByOrg(Guid OrgId)
        {
            try
            {
                return Success(_service.GetCourseByOrg(OrgId));
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }

        }

        [HttpPost("GetCourseAllByUser")]
        public virtual RespondData GetCourseAllByUser()
        {
            try
            {
                return Success(_service.GetCourseAllByUser(UserId));
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }

        }
    }



}