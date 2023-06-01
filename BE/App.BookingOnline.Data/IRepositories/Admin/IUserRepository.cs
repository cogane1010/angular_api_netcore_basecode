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
    public interface IUserRepository : IGridRepository<User, UserPagingModel>
    {
        User GetByUserName(string username);
        List<AspRole> GetRolesPermission(string Id);
        List<Menu> GetUserMenu(string userName);
        Task<User> AddUser(User entity);
        Task<User> UpdateUser(User entity);
        Task ChangePassword(ChangePasswordModel model);
        PagingResponseEntity<User> GetPagingUser(UserPagingModel pagingModel);
        User GetById(Guid id);
        IEnumerable<string> GetRoleUser(string userId);
    }
}