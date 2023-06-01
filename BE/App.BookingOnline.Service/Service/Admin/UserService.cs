using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using App.Core.Helper;
using App.Core.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public class UserService : BaseGridService<UserDTO, User, UserPagingModel, IUserRepository>, IUserService
    {
        private IUserRoleRepository _userRoleRepo;
        public UserService(IUserRepository repo, IUserRoleRepository userRoleRepo) : base(repo)
        {
            _userRoleRepo = userRoleRepo;
        }
        private void UpdateUserRoleRef(UserDTO userDTO)
        {
            _userRoleRepo.RemoveRange(_userRoleRepo.Where(x => x.UserId == userDTO.UserId));

            List<AspUserRole> userRoleRef = new List<AspUserRole>();
            foreach (var userRole in userDTO.Roles)
            {
                if (userRole.HasRole)
                {
                    userRoleRef.Add(new AspUserRole()
                    {
                        UserId = userDTO.UserId,
                        RoleId = userRole.Id
                    });
                }
            }

            _userRoleRepo.AddRange(userRoleRef);
        }


        public override UserDTO Add(UserDTO entityDTO)
        {

            var res = base.Add(entityDTO);
            entityDTO.Id = res.Id;
            this.UpdateUserRoleRef(entityDTO);
            return res;
        }

        public override void Update(UserDTO entityDTO)
        {

            base.Update(entityDTO);
            this.UpdateUserRoleRef(entityDTO);
        }


        public override UserDTO Get(Guid Id)
        {
            var result = base.Get(Id);
            result.Roles = GetRolesPermission(result.UserId);
            return result;
        }
        private List<AspRoleDTO> GetRolesPermission(string Id)
        {
            return AutoMapperHelper.Map<AspRole, AspRoleDTO, List<AspRole>, List<AspRoleDTO>>(_repo.GetRolesPermission(Id));
        }

        public IEnumerable<MenuDTO> GetUserMenu(string userName)
        {

            var listMenu = AutoMapperHelper.Map<Menu, MenuDTO, List<Menu>, List<MenuDTO>>(_repo.GetUserMenu(userName));
            if (listMenu.Count > 0)
            {
                var rootMenu = listMenu.Find(x => x.Level == -1);

                rootMenu.Sub = listMenu.FindAll(x => x.ParentId == rootMenu.Id); // level 1

                foreach (var level1 in rootMenu.Sub)
                {
                    level1.Level = 1;
                    level1.Sub = listMenu.FindAll(x => x.ParentId == level1.Id);

                    foreach (var level2 in level1.Sub)
                    {
                        level2.Level = 2;
                        level2.Sub = listMenu.FindAll(x => x.ParentId == level2.Id);

                        foreach (var level3 in level2.Sub)
                        {
                            level3.Level = 3;
                        }
                    }
                }

                return rootMenu.Sub;
            }
            else return new List<MenuDTO>();
        }

        public async Task<UserDTO> AddUser(UserDTO entityDTO)
        {
            var entity = AutoMapperHelper.Map<UserDTO, User>(entityDTO);
            var res = await _repo.AddUser(entity);
            entityDTO.Id = res.Id;
            entityDTO.UserId = res.UserId;
            this.UpdateUserRoleRef(entityDTO);
            return entityDTO;

        }

        public async Task UpdateUser(UserDTO entityDTO)
        {
            var entity = AutoMapperHelper.Map<UserDTO, User>(entityDTO);
            await _repo.UpdateUser(entity);
            this.UpdateUserRoleRef(entityDTO);
        }

        public async Task ChangePassword(ChangePasswordModel model)
        {
            await _repo.ChangePassword(model);
        }

        public OrganizationDTO GetOrg(string username)
        {
            var user = _repo.GetByUserName(username);
            return AutoMapperHelper.Map<Organization, OrganizationDTO>(user.Organization);

        }

        public PagingResponseEntity<User> GetPagingUser(UserPagingModel pagingModel)
        {
            return _repo.GetPagingUser(pagingModel);
        }

        public User GetById(Guid id)
        {
            return _repo.GetById(id);
        }

        public IEnumerable<string> GetRoleUser(string userId)
        {
            IEnumerable<string> data = _repo.GetRoleUser(userId);
            return data;
        }
    }
}
