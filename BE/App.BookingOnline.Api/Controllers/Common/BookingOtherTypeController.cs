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
using Microsoft.AspNetCore.Identity;
using App.BookingOnline.Data.Identity;
using System;
using App.BookingOnline.Data.Models;

namespace App.BookingOnline.Api.Controllers
{

    public class BookingOtherTypeController : BaseGridController<BookingOtherTypeDTO, BookingOtherTypePagingModel, IBookingOtherTypeService>
    {
        public BookingOtherTypeController(IBookingOtherTypeService service) : base(service)
        {
        }
    }


    
}