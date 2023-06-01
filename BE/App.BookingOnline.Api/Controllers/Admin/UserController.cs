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

    public class UserController : BaseGridController<UserDTO, UserPagingModel, IUserService>
    {
        public UserController(IUserService service) : base(service)
        {
        }


        [HttpPost("AddOrEditUser")]
        public async Task<RespondData> AddOrEditUser(UserDTO entityDTO)
        {
            try
            {
                UserDTO res = entityDTO;
                if (entityDTO.Id == null || entityDTO.Id == Guid.Empty)
                {
                    entityDTO.CreatedDate = DateTime.Now;
                    entityDTO.CreatedUser = UserName;
                    res = await _service.AddUser(entityDTO);

                }
                else
                {
                    entityDTO.UpdatedDate = DateTime.Now;
                    entityDTO.UpdatedUser = UserName;
                    await _service.UpdateUser(entityDTO);

                }

                return Success(res);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

        [HttpPost("GetUserMenu")]
        public RespondData GetUserMenu()
        {
            try
            {
                return Success(_service.GetUserMenu(UserName));
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
        }


        [HttpPost("ChangePassword")]
        public async Task<RespondData> ChangePassword(ChangePasswordModel model)
        {
            model.UserName = UserName;
            try
            {
                await _service.ChangePassword(model);
                return Success();
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }


        }

        [HttpPost("GetOrg")]
        public async Task<RespondData> GetOrg()
        {
            try
            {
                var res = _service.GetOrg(UserName);

                return Success(res);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

        public override RespondData GetPaging(UserPagingModel filter)
        {
            try
            {
                return Success(_service.GetPagingUser(filter));
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

        [HttpPost("GetRoleUser")]
        public RespondData GetRoleUser(string userId)
        {
            try
            {
                return Success(_service.GetRoleUser(userId));
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }

        }

        [HttpPost("Get")]
        public override RespondData Get(Guid Id)
        {
            try
            {
                return Success(_service.GetById(Id));
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

    }
}