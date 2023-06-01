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
    public class LockReasonService : BaseGridService<LockReasonDTO, LockReason, LockReasonPagingModel, ILockReasonRepository>, ILockReasonService
    {
        public LockReasonService(ILockReasonRepository repo) : base(repo)
        {
        }

    }
}

