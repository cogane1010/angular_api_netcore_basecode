using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.Repositories
{
    public interface ICustomerRepository : IGridRepository<Customer, CustomerPagingModel>
    {
        ValueTask<Customer> AddCustomer(Customer entity);

        ValueTask<Customer> UpdateCustomer(Customer entity);

        Task<Customer> GetByIdAsyncExtend(Guid id);

        Task<Customer> GetByUserId(string UserId);


        Task ResetPassword(Customer entity);
        AppUser GetAspUserById(string userId);
        void UpdateLockAspUser(string userId, int? stausInt);
    }
}