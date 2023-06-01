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
using Microsoft.AspNetCore.Authorization;

namespace App.BookingOnline.Api.Controllers
{
    public class MenuController : BaseGridController<MenuDTO, MenuPagingModel, IMenuService>
    {
        public MenuController(IMenuService service) : base(service)
        {
        }
    }
}