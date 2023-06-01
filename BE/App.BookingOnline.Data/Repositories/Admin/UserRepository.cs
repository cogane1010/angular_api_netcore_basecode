using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Helper;
using Microsoft.Extensions.DependencyInjection;
using App.BookingOnline.Data.Identity;
using Microsoft.AspNetCore.Identity;
using App.Core;
using System;

namespace App.BookingOnline.Data.Repositories
{
    public class UserRepository : GridRepository<User, UserPagingModel>, IUserRepository
    {

        public UserRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public PagingResponseEntity<User> GetPagingUser(UserPagingModel pagingModel)
        {
            var users = Context.Set<AppUser>();
            var organization = Context.Set<Organization>();
            var customer = Context.Set<Customer>();
            var roles = Context.Set<AspRole>();
            var roleUser = Context.Set<AspUserRole>();

            var subtables = users.Join(organization, rd => rd.C_Org_Id, rs => rs.Id, (users, organization)
                   => new { users.Id, users, organization })
                   .Join(roleUser, rd => rd.users.Id, rs => rs.UserId, (users, roleUser)
                   => new { users.Id, users, roleUser })
                   .Join(roles, rd => rd.roleUser.RoleId, rs => rs.Id, (users, roles)
                   => new { users.Id, users, roles })
                   .Where(x => string.IsNullOrEmpty(pagingModel.Name) || x.users.users.users.UserName.Contains(pagingModel.Name))
                   .Where(x => string.IsNullOrEmpty(pagingModel.OrgId) || x.users.users.users.C_Org_Id == new Guid(pagingModel.OrgId)).ToList()
                   .GroupBy(
                           p => p.Id,
                           p => p.users,
                           (key, g) => new { PersonId = key, enti = g.FirstOrDefault() })
              .Select(s => new User
              {
                  Id = new Guid(s.PersonId),
                  UserId = s.PersonId,
                  Email = s.enti.users.users.Email,
                  FullName = s.enti.users.users.FullName,
                  UserName = s.enti.users.users.UserName,
                  C_Org_Id = s.enti.users.users.C_Org_Id,
                  Organization = s.enti.users.organization,
                  IsActive = s.enti.users.users.IsActive
              }).ToList();

            foreach (var it in subtables)
            {
                var userRole = roleUser.Where(x => x.UserId == it.Id.ToString()).Select(s => s.RoleId);
                var roleName = roles.Where(x => userRole.Any(s => s == x.Id)).Select(s => s.DisplayName);
                it.RoleName = string.Join(", ", roleName);
            }
            var result = new PagingResponseEntity<User>
            {
                Data = subtables.OrderBy(x => x.UserName)
                           .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                           .Take(pagingModel.PageSize).ToList(),
                Count = subtables.Count()
            };
            return result;

        }

        public List<AspRole> GetRolesPermission(string Id)
        {
            string conString = this.Context.Database.GetDbConnection().ConnectionString;
            using (var conn = new SqlConnection(conString))
            {
                var p = new DynamicParameters();
                p.Add("Id", Id);
                var res = conn.ExecuteProcedure<AspRole>("App_User_getRolesPermisstion", p);
                return res.Item1.ToList();
            }
        }

        public List<Menu> GetUserMenu(string userName)
        {
            //string conString = this.Context.Database.GetDbConnection().ConnectionString;
            //using (var conn = new SqlConnection(conString))
            //{
            //    var p = new DynamicParameters();
            //    p.Add("userName", userName);
            //    var res = conn.ExecuteProcedure<Menu>("App_User_getUserMenu", p);
            //    return res.Item1.ToList();
            //}
            var menu = Context.Set<Menu>();
            var roleMenu = Context.Set<RoleMenu>();
            var subtables = menu.Join(roleMenu, rd => rd.Id, rs => rs.MenuId, (menuDetail, roleMenu)
                => new { menuDetail.Id, menuDetail, roleMenu });
            var appUser = Context.Set<AppUser>();
            var aspUserRole = Context.Set<AspUserRole>();

            var menuList = subtables.Join(aspUserRole, rd => rd.roleMenu.AspRoleId, rs => rs.RoleId, (menuDetail, aspUserRole)
                => new { menuDetail.Id, menuDetail, aspUserRole })
                .Join(appUser, rd => rd.aspUserRole.UserId, rs => rs.Id, (menuDetail, appUser)
                => new { menuDetail.Id, menuDetail, appUser }).Where(x => x.appUser.UserName == userName)
                .Select(s => new Menu
                {
                    Id = s.Id,
                    ParentId = s.menuDetail.menuDetail.menuDetail.ParentId,
                    Level = s.menuDetail.menuDetail.menuDetail.Level,
                    Name = s.menuDetail.menuDetail.menuDetail.Name,
                    Icon = s.menuDetail.menuDetail.menuDetail.Icon,
                    Url = s.menuDetail.menuDetail.menuDetail.Url,
                    DisplayOrder = s.menuDetail.menuDetail.menuDetail.DisplayOrder
                }).OrderBy(o => o.DisplayOrder).ToList();
            List<Menu> menus = new List<Menu>();
            foreach (var item in menuList)
            {
                if (!menus.Select(s => s.Id).Contains(item.Id))
                {
                    menus.Add(item);
                }
            }
            return menus;
        }

        public async Task<User> AddUser(User entity)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var services = new ServiceCollection();
                    string conString = this.Context.Database.GetDbConnection().ConnectionString;
                    services.AddDbContext<BookingOnlineDbContext>(options =>
                       options.UseSqlServer(conString));

                    services.AddIdentity<AppUser, AspRole>()
                        .AddEntityFrameworkStores<BookingOnlineDbContext>()
                        .AddDefaultTokenProviders();
                    services.AddLogging();

                    using (var serviceProvider = services.BuildServiceProvider())
                    {
                        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                        {
                            var context = scope.ServiceProvider.GetService<BookingOnlineDbContext>();
                            context.Database.Migrate();
                            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                            var user = new AppUser { UserName = entity.UserName, FullName = entity.FullName, Name = entity.FullName, Email = entity.Email, C_Org_Id = entity.C_Org_Id, IsActive = entity.IsActive };
                            await _userManager.CreateAsync(user, entity.Password);
                            //await _userManager.AddToRoleAsync(user, Constant.Employee);
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("name", user.Name));
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", Constants.Employee));
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("org", user.C_Org_Id.Value.ToString()));
                            var created_user = await _userManager.FindByNameAsync(user.UserName);
                            entity.UserId = created_user.Id.ToString();
                        }
                    }

                    //var res = Context.AddAsync(entity);
                    Context.SaveChanges();
                    transaction.Commit();
                    return entity;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public async Task<User> UpdateUser(User entity)
        {

            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var services = new ServiceCollection();
                    string conString = this.Context.Database.GetDbConnection().ConnectionString;
                    services.AddDbContext<BookingOnlineDbContext>(options =>
                       options.UseSqlServer(conString));

                    services.AddIdentity<AppUser, AspRole>()
                        .AddEntityFrameworkStores<BookingOnlineDbContext>()
                        .AddDefaultTokenProviders();
                    services.AddLogging();

                    using (var serviceProvider = services.BuildServiceProvider())
                    {
                        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                        {
                            var context = scope.ServiceProvider.GetService<BookingOnlineDbContext>();
                            // context.Database.Migrate();
                            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                            var user = await _userManager.FindByIdAsync(entity.UserId);
                            user.C_Org_Id = entity.C_Org_Id;
                            user.FullName = entity.FullName;
                            user.IsActive = entity.IsActive;
                            await _userManager.UpdateAsync(user);

                            if (!string.IsNullOrEmpty(entity.Password))
                            {
                                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                                var changePassRes = await _userManager.ResetPasswordAsync(user, token, entity.Password);
                                if (!changePassRes.Succeeded)
                                {
                                    throw new Exception("Không thay đổi được mật khẩu. Mật khẩu có ít nhất 8 ký tự và có ký tự đặt biệt, chữ hoa, chứ thường và số");
                                }
                            }
                        }
                    }

                    transaction.Commit();
                    //base.Update(entity);
                    return entity;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public async Task ChangePassword(ChangePasswordModel model)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    if (model.NewPassword != model.RetypeNewPassword)
                    {
                        throw new Exception("Retype password không khớp");
                    }

                    var services = new ServiceCollection();
                    string conString = this.Context.Database.GetDbConnection().ConnectionString;
                    services.AddDbContext<BookingOnlineDbContext>(options =>
                       options.UseSqlServer(conString));

                    services.AddIdentity<AppUser, AspRole>()
                        .AddEntityFrameworkStores<BookingOnlineDbContext>()
                        .AddDefaultTokenProviders();
                    services.AddLogging();

                    using (var serviceProvider = services.BuildServiceProvider())
                    {
                        using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                        {
                            var context = scope.ServiceProvider.GetService<BookingOnlineDbContext>();
                            context.Database.Migrate();
                            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                            var user = await _userManager.FindByNameAsync(model.UserName);
                            var checkPassRes = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                            if (!checkPassRes)
                            {
                                throw new Exception("Password không đúng");
                            }
                            var changePassRes = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                            if (!changePassRes.Succeeded)
                            {
                                throw new Exception(string.Join(", ", changePassRes.Errors));
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }

        }

        public User GetByUserName(string username)
        {
            return this.dbSet.Where(x => x.UserName == username).Include(x => x.Organization).FirstOrDefault();
        }

        public User GetById(Guid id)
        {
            var users = Context.Set<AppUser>();
            var organization = Context.Set<Organization>();
            var data = users.Join(organization, rd => rd.C_Org_Id, rs => rs.Id, (users, organization)
               => new { users.Id, users, organization }).Where(x => x.users.Id == id.ToString())
               .Select(s => new User
               {
                   Id = new Guid(s.users.Id),
                   UserId = s.users.Id,
                   Email = s.users.Email,
                   FullName = s.users.FullName,
                   UserName = s.users.UserName,
                   C_Org_Id = s.users.C_Org_Id,
                   Organization = s.organization,
                   IsActive = s.users.IsActive
               }).FirstOrDefault();
            return data;
        }

        public IEnumerable<string> GetRoleUser(string userId)
        {
            var users = Context.Set<AspUserRole>().Where(x => x.UserId == userId).Select(s => s.RoleId);

            return users;
        }
    }
}