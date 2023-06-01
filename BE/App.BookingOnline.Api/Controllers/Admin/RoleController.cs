using AutoMapper;
using App.BookingOnline.Api.Validators;
using App.BookingOnline.Api.ViewModels;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using System;

namespace App.BookingOnline.Api.Controllers
{

    public class RoleController : GenericBaseGridController<AspRoleDTO, RolePagingModel, IRoleService, string>
    {
        public RoleController(IRoleService service) : base(service)
        {
        }

        public override RespondData AddOrEdit(AspRoleDTO entityDTO)
        {
            return base.AddOrEdit(entityDTO);
        }

        [HttpPost("GetTreeMenuPermission")]
        public RespondData GetTreeMenuPermission()
        {
            try
            {
                return Success(_service.GetTreeMenuPermission());
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
            
        }
    }
}