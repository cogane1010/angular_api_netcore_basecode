using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO.Common;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace App.BookingOnline.WebApi.Controllers.Common
{
    [ApiController]
    public class PromotionSettingController : GridController<PromotionDTO, PromotionFilterModel, IPromotionService>
    {
        private readonly ICourseService _courseService;

        public PromotionSettingController(IPromotionService service, ICourseService courseService) : base(service)
        {
            _courseService = courseService;
        }

        [HttpPost("GetCustomers")]
        public async Task<RespondData> GetCustomers()
        {
            try
            {
                var data = await _service.GetCustomerGroups(UserId);
                return Success(data);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
        }

        [HttpPost("GetCourse")]
        public RespondData GetCourse()
        {
            try
            {
                var data = _courseService.GetAll();
                return Success(data);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
        }

        [HttpPost("GetAllCourse")]
        public RespondData GetAllCourse()
        {
            try
            {
                var data = _courseService.GetAll();
                return Success(data);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("Delete")]
        public override RespondData Delete(Guid Id)
        {
            try
            {
                _service.Delete(Id);
                return Success(null);
            }
            catch (Exception e)
            {
                return Failure("", "Có khách hàng đang dùng mã khuyến mại này. Nên không xóa được", "");
            }

        }



        //[HttpPost("AddOrEdit")]
        //public async ValueTask<RespondData> AddOrEditWithImage()
        //{
        //    try
        //    {
        //        var file = Request.Form.Files[0];
        //        var folderName = Path.Combine("Assets", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        if (file.Length > 0)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            var fullPath = Path.Combine(pathToSave, fileName);
        //            var dbPath = Path.Combine(folderName, fileName);
        //            //using (var stream = new FileStream(fullPath, FileMode.Create))
        //            //{
        //            //    file.CopyTo(stream);
        //            //}
        //        }

        //        //if (entityDTO.Id == null || entityDTO.Id == Guid.Empty)
        //        //{
        //        //    entityDTO.CreatedDate = DateTime.Now;
        //        //    entityDTO.CreatedUser = this.UserName;
        //        //    var entity = await _service.AddAsync(entityDTO);

        //        //    entityDTO.Id = entity.Id;
        //        //}
        //        //else
        //        //{
        //        //    entityDTO.UpdatedDate = DateTime.Now;
        //        //    entityDTO.UpdatedUser = this.UserName;
        //        //    _service.Update(entityDTO);
        //        //}
        //        return Success("ok");
        //    }
        //    catch (Exception e)
        //    {
        //        return Failure("",e.Message);
        //    }

        //}
    }
}
