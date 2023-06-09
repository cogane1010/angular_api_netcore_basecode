
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using App.Core.Service;
using System;

namespace App.BookingOnline.AppApi.Controllers
{
    public class BaseDataController<TEntityDTO, TBaseDataService> : BaseApiController
        where TEntityDTO : class, IEntityDTO
        where TBaseDataService : class, IBaseDataService<TEntityDTO>
    {
        protected TBaseDataService _service;

        public BaseDataController(TBaseDataService service)
        {
            _service = service;
        }


        [HttpPost("Get")]
        public virtual RespondData Get(Guid Id)
        {
            try
            {
                return Success(_service.Get(Id));
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

        [HttpPost("GetAll")]
        public virtual RespondData GetAll()
        {
            try
            {
                return Success(_service.GetAll());
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("AddOrEdit")]
        public virtual RespondData AddOrEdit(TEntityDTO entityDTO)
        {
            try
            {
                if (entityDTO.Id == null || entityDTO.Id == Guid.Empty)
                {
                    entityDTO.CreatedDate = DateTime.Now;
                    //entityDTO.CreatedUser = this.UserName;
                    var entity = _service.Add(entityDTO);
                    entityDTO.Id = entity.Id;
                }
                else
                {
                    entityDTO.UpdatedDate = DateTime.Now;
                    //entityDTO.UpdatedUser = this.UserName;
                    _service.Update(entityDTO);
                }
                return Success(entityDTO.Id);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
           
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("Delete")]
        public RespondData Delete(Guid Id)
        {
            try
            {
                _service.Delete(Id);
                return Success(null);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
            
        }
    }

}