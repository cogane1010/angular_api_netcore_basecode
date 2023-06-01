
using App.Core.Domain;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using System;
using Microsoft.AspNetCore.Mvc;

namespace App.BookingOnline.AppApi.Controllers
{
    public class HotlineController : BaseGridController<HotlineDTO, HotlinePagingModel, IHotlineService>
    {
        public HotlineController(IHotlineService service) : base(service)
        {
        }

        /// <summary>
        /// Lấy số điện thoại hotline
        /// </summary>
        /// <returns>số điện thoại</returns>
        [HttpGet]
        public RespondData Hotline()
        {
            try
            {
                return Success(_service.Hotline(Guid.Parse("c8a63280-5af0-45e9-987d-4519d611c30f")));
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
        }
    }
}