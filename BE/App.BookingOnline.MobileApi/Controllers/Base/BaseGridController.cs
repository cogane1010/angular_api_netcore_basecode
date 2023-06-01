using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using App.Core.Service;
using System;

namespace App.BookingOnline.AppApi.Controllers
{

    public class BaseGridController<TEntityDTO, TPagingModel, TBaseGridService> : BaseDataController<TEntityDTO, TBaseGridService>
       where TEntityDTO : class, IEntityDTO
       where TPagingModel : class, IPagingModel
       where TBaseGridService : class, IBaseGridService<TEntityDTO, TPagingModel>

    {
        protected TBaseGridService _service;

        public BaseGridController(TBaseGridService service) : base(service)
        {
            _service = service;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("GetPaging")]
        public virtual RespondData GetPaging(TPagingModel filter)
        {
            try
            {
                return Success(_service.GetPaging(filter));
            }
            catch(Exception e)
            {
                return Failure("",e.Message);
            }
           
        }


    }
}