using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using System;

namespace App.BookingOnline.Api.Controllers
{

    public class SettingController : BaseGridController<SettingDTO, SettingPagingModel, ISettingService>
    {
        public SettingController(ISettingService service) : base(service)
        {
        }

        [HttpPost("getByCode")]

        public RespondData GetByCode(string code)
        {
            try
            {
                return Success(_service.GetByCode(code));
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }
    }
}