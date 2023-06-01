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
    public class RoleService : GenericBaseGridService<AspRoleDTO, AspRole, RolePagingModel, IRoleRepository, string>, IRoleService
    {
        private IRoleMenuRepository _roleMenuRepo;
        public RoleService(IRoleRepository repo, IRoleMenuRepository roleMenuRepo) : base(repo)
        {
            _roleMenuRepo = roleMenuRepo;
        }

        public List<MenuDTO> GetTreeMenuPermission()
        {
            return GetTreeMenuPermission(Guid.Empty.ToString());
        }
        private List<MenuDTO> GetTreeMenuPermission(string Id)
        {

            var allMenuPermission = AutoMapperHelper.Map<Menu, MenuDTO, List<Menu>, List<MenuDTO>>(_repo.GetMenuPermisstion(new Guid(Id)));

            var rootMenu = allMenuPermission.FindAll(x => x.Level == -1);

            foreach (var root in rootMenu)
            {
                root.Sub = allMenuPermission.FindAll(x => x.ParentId == root.Id); // level 1
                foreach (var level1 in root.Sub)
                {
                    level1.Sub = allMenuPermission.FindAll(x => x.ParentId == level1.Id); // level 2

                    foreach (var level2 in level1.Sub)
                    {
                        level2.Sub = allMenuPermission.FindAll(x => x.ParentId == level2.Id); // level 3
                    }
                }
            }
            return rootMenu;
        }

        public override AspRoleDTO Get(string Id)
        {
            var result = base.Get(Id);
            result.TreeMenuPermission = GetTreeMenuPermission(Id);
            return result;
        }

        public override AspRoleDTO Add(AspRoleDTO entityDTO)
        {
            entityDTO.Id = Guid.NewGuid().ToString();
            var res=  base.Add(entityDTO);
            entityDTO.Id = res.Id;
            this.UpdateRoleMenuRef(entityDTO);
            return res;
        }

        public override void Update(AspRoleDTO entityDTO)
        {
            
            base.Update(entityDTO);
            this.UpdateRoleMenuRef(entityDTO);
        }


        private void UpdateRoleMenuRef(AspRoleDTO roleDTO)
        {

            _roleMenuRepo.RemoveRange(_roleMenuRepo.Find(x => x.AspRoleId == roleDTO.Id));

            var treeMenuPermisson = roleDTO.TreeMenuPermission;
            List<RoleMenu> roleMenuRef = new List<RoleMenu>();
            foreach (var rootMenu in treeMenuPermisson)
            {
                if (rootMenu.HasMenu)
                {
                    roleMenuRef.Add(new RoleMenu()
                    {
                        AspRoleId = roleDTO.Id,
                        MenuId = rootMenu.Id,
                        CreatedDate = roleDTO.CreatedDate
                    });
                }

                foreach (var level1 in rootMenu.Sub)
                {
                    if (level1.HasMenu)
                    {
                        roleMenuRef.Add(new RoleMenu()
                        {
                            AspRoleId = roleDTO.Id,
                            MenuId = level1.Id,
                            CreatedDate = roleDTO.CreatedDate
                        });
                    }

                    foreach (var level2 in level1.Sub)
                    {
                        if (level2.HasMenu)
                        {
                            roleMenuRef.Add(new RoleMenu()
                            {
                                AspRoleId = roleDTO.Id,
                                MenuId = level2.Id,
                                CreatedDate = roleDTO.CreatedDate
                            });
                        }

                        foreach (var level3 in level2.Sub)
                        {
                            if (level3.HasMenu)
                            {
                                roleMenuRef.Add(new RoleMenu()
                                {
                                    AspRoleId = roleDTO.Id,
                                    MenuId = level3.Id,
                                    CreatedDate = roleDTO.CreatedDate
                                });
                            }
                        }
                    }
                }
            }

            _roleMenuRepo.AddRangeAsync(roleMenuRef);
        }
    }
}
