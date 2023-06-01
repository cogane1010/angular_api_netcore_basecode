using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.IService.Common;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace App.BookingOnline.WebApi.Controllers.Common
{
    public class TimingSettingController : GridController<CourseTemplateDTO, CourseTemplateFilterModel, ICourseTemplateService>
    {
        public TimingSettingController(ICourseTemplateService service) : base(service)
        {
        }

        [HttpPost("AddOrEdit")]
        public async override ValueTask<RespondData> AddOrEdit(CourseTemplateDTO entityDTO)
        {
            try
            {
                if (entityDTO.Id == null || entityDTO.Id == Guid.Empty)
                {
                    entityDTO.CreatedDate = DateTime.Now;
                    entityDTO.CreatedUser = this.UserName;
                    var entity = await _service.AddAsync(entityDTO);

                    return Success(entity);
                }
                else
                {
                    entityDTO.UpdatedDate = DateTime.Now;
                    entityDTO.UpdatedUser = this.UserName;
                    _service.Update(entityDTO);
                }
                return Success(entityDTO);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }
    }
}
