using AutoMapper;
using App.BookingOnline.Api.Validators;
using App.BookingOnline.Api.ViewModels;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using App.Core;
using System;

namespace App.BookingOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "adminOrEmployee")]
    public class GenericBaseApiController : ControllerBase
    {

        protected ClaimsIdentity ClaimsIdentity => User.Identity as ClaimsIdentity;

        protected string UserName => ClaimsIdentity.FindFirst("name")?.Value;
        protected string UserId => ClaimsIdentity.FindFirst("id")?.Value;
        protected RespondData Success(object Data = null)
        {
            return new RespondData { IsSuccess = true, Data = Data };
        }

        protected RespondData Failure(string message = "", object Data = null)
        {
            return new RespondData { IsSuccess = false, Message = message, Data = Data };
        }

    }


}