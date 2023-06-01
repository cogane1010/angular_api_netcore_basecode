using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.Core.Service;

namespace App.BookingOnline.Service.IService.Common
{
    public interface ICourseTemplateService : IBaseGridDataService<CourseTemplateDTO, CourseTemplateFilterModel>
    {

    }
}
