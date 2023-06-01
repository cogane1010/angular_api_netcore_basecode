using AutoMapper;
using App.BookingOnline.Api.Validators;
using App.BookingOnline.Api.ViewModels;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.Core.Service;
using System;

namespace App.BookingOnline.Api.Controllers
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

        [HttpPost("GetPaging")]
        public virtual RespondData GetPaging(TPagingModel filter)
        {
            try
            {
                filter.UserId = UserId;
                filter.UserOrgId = CurOrgId;
                return Success(_service.GetPaging(filter));
            }
            catch(Exception e)
            {
                return Failure("",e.Message);
            }
           
        }


    }
}