using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using App.Core.Service;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface ICustomerGroupService : IBaseGridService<CustomerGroupDTO, CustomerGroupPagingModel>
    {
       
    }

    public interface ICustomerGroupMappingService : IBaseGridService<CustomerGroupMappingDTO, CustomerGroupMappingPagingModel>
    {

    }



}