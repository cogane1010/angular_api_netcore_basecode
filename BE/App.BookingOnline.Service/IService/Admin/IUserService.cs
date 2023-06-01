using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using App.Core.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface IUserService : IBaseGridService<UserDTO, UserPagingModel>
    {
        IEnumerable<MenuDTO> GetUserMenu(string userName);
        Task<UserDTO> AddUser(UserDTO entityDTO);
        Task UpdateUser(UserDTO entityDTO);
        Task ChangePassword(ChangePasswordModel model);
        OrganizationDTO GetOrg(string username);
        PagingResponseEntity<User> GetPagingUser(UserPagingModel pagingModel);
        User GetById(Guid id);
        IEnumerable<string> GetRoleUser(string userId);
    }
}