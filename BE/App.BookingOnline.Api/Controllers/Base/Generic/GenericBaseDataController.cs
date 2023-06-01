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
    public class GenericBaseDataController<TEntityDTO, TBaseDataService, TKey> : BaseApiController
        where TKey : IEquatable<TKey>
        where TEntityDTO : class, IGenericEntityDTO<TKey>
        where TBaseDataService : class, IGenericBaseDataService<TEntityDTO, TKey>
    {
        protected TBaseDataService _service;

        public GenericBaseDataController(TBaseDataService service)
        {
            _service = service;
        }

        [HttpPost("Get")]
        public virtual RespondData Get(TKey Id)
        {
            try
            {
                return Success(_service.Get(Id));
            }
            catch(Exception e)
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

        [HttpPost("AddOrEdit")]
        public virtual RespondData AddOrEdit(TEntityDTO entityDTO)
        {
            try
            {
                if (entityDTO.Id == null)
                {
                    var entity = _service.Add(entityDTO);
                    
                    entityDTO.Id = entity.Id;
                }
                else
                {
                    _service.Update(entityDTO);
                }
                return Success(entityDTO.Id);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
           
        }

        [HttpPost("Delete")]
        public RespondData Delete(TKey Id)
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