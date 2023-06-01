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

    public class GenericBaseGridController<TEntityDTO, TPagingModel, TBaseGridService, TKey> 
        : GenericBaseDataController<TEntityDTO, TBaseGridService, TKey>
       where TKey : IEquatable<TKey>
       where TEntityDTO : class, IGenericEntityDTO<TKey>
       where TPagingModel : class, IPagingModel
       where TBaseGridService : class, IGenericBaseGridService<TEntityDTO, TPagingModel, TKey>

    {
        protected TBaseGridService _service;

        public GenericBaseGridController(TBaseGridService service) : base(service)
        {
            _service = service;
        }

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