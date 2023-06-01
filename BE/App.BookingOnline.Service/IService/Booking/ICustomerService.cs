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
    public interface ICustomerService : IBaseGridService<CustomerDTO, CustomerPagingModel>
    {
        CustomerDTO GetExtend(Guid Id);
        ValueTask<CustomerDTO> AddCustomer(CustomerDTO entityDTO);
        Task UpdateCustomer(CustomerDTO entityDTO);
        Task ResetPassword(CustomerDTO entityDTO);
        void ChangeStatus(CustomerDTO entityDTO);
        UploadFile SaveUploadFile(UploadFileDTO dtoFile);
        UploadFileDTO GetFile(UploadFileDTO dtoFile);

    }

   
}