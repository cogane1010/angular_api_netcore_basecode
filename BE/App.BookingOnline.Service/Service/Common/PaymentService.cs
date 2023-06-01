using App.BookingOnline.Data;
using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.Core;
using App.Core.Domain;
using App.Core.Helper;
using App.Core.Service;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service
{
    public class PaymentTypeService : BaseGridService<PaymentTypeDTO, PaymentType, PaymentTypePagingModel, IPaymentTypeRepository>, IPaymentTypeService
    {
        public PaymentTypeService(IPaymentTypeRepository repo) : base(repo)
        {
        }
    }
}

