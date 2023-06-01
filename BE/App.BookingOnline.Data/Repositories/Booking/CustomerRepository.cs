using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Helper;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using App.BookingOnline.Data.Identity;
using App.Core;
using Microsoft.Extensions.Configuration;
using static App.Core.Enums;

namespace App.BookingOnline.Data.Repositories
{
    public class CustomerRepository : GridRepository<Customer, CustomerPagingModel>, ICustomerRepository
    {
        public IConfiguration Configuration { get; }
        public CustomerRepository(BookingOnlineDbContext context, IConfiguration configuration)
            : base(context)
        {
            Configuration = configuration;
        }

        public override PagingResponseEntity<Customer> GetPaging(CustomerPagingModel pagingModel)
        {

            var query = this.dbSet.Include(x => x.MemberCards)
                            .AsQueryable()
                            .Where(x => pagingModel.CustomerCode.IsNullOrEmpty() || x.CustomerCode.Contains(pagingModel.CustomerCode))
                            .Where(x => pagingModel.Name.IsNullOrEmpty() || x.LowerFullName.Contains(pagingModel.Name))
                            .Where(x => pagingModel.Email.IsNullOrEmpty() || x.Email.Contains(pagingModel.Email))
                            .Where(x => pagingModel.MobilePhone.IsNullOrEmpty() || x.MobilePhone.Contains(pagingModel.MobilePhone))
                            .Where(x => pagingModel.DOB == null || x.DOB == pagingModel.DOB)
                            .Where(x => string.IsNullOrEmpty(pagingModel.MemberCard) || x.MemberCards.Any(y => y.Golf_CardNo.ToLower() == pagingModel.MemberCard.ToLower() && !y.IsDelete))
                            .Where(x => !pagingModel.IsActive.HasValue || x.IsActive == pagingModel.IsActive.Value);

            var customers = query
                            .OrderBy(x => x.CreatedDate)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList();
            foreach (Customer customer in customers)
            {
                if (customer.MemberCards != null)
                {
                    customer.Golf_CardNo = String.Join(", ", customer.MemberCards.Where(x => !x.IsDelete).Select(x => x.Golf_CardNo));
                }

            }
            var result = new PagingResponseEntity<Customer>
            {
                Data = customers,
                Count = query.Count()
            };

            return result;
        }

        public AppUser GetAspUserById(string userId)
        {
            return Context.Set<AppUser>().Where(x => x.Id == userId).FirstOrDefault();
        }

        public Task<Customer> GetByIdAsyncExtend(Guid id)
        {
            var res = this.dbSet
                         .Include(x => x.MemberCards).ThenInclude(x => x.MemberCardCourses).ThenInclude(x => x.CustomerGroup)
                         .Include(x => x.MemberCards).ThenInclude(x => x.MemberCardCourses).ThenInclude(x => x.Course)
                         .Include(x => x.MemberCards).ThenInclude(x => x.Organization)
                         .Include(x => x.MemberCards).ThenInclude(x => x.CustomerGroup)
                         .Where(x => x.Id == id).FirstOrDefaultAsync();
            return res;
        }

        private void CheckValid(Customer entity)
        {
            var data = this.dbSet.Where(x => (x.Email == entity.Email || x.MobilePhone == entity.MobilePhone) && x.Id != entity.Id).ToList();
            if (data.Count > 0)
            {
                if (data.Find(x => x.Email == entity.Email) != null)
                {
                    throw new Exception("Email đã tồn tại");
                }
                else
                {
                    throw new Exception("Số điện thoại đã tồn tại");
                }
            }

        }

        public async ValueTask<Customer> UpdateCustomer(Customer entity)
        {
            entity.LowerFullName = entity.FullName.ToLower();
            this.CheckValid(entity);
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
                            //context.Database.Migrate();
                            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                            var user = await _userManager.FindByIdAsync(entity.UserId);
                            user.Name = entity.FullName;
                            user.Email = entity.Email;
                            user.PhoneNumber = entity.MobilePhone;

                            await _userManager.UpdateAsync(user);

                        }
                    }
                    transaction.Commit();
                    base.Update(entity);
                    return entity;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }

        }

        public async ValueTask<Customer> AddCustomer(Customer entity)
        {
            entity.LowerFullName = entity.FullName.ToLower();
            this.CheckValid(entity);
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
                            var user = new AppUser
                            {
                                UserName = entity.CustomerCode + '.' + Guid.NewGuid().ToString("n").Substring(0, 8),
                                Name = entity.FullName,
                                Email = entity.Email,
                                PhoneNumber = entity.MobilePhone,
                            };
                            await _userManager.CreateAsync(user, "Brg@123456");
                            await _userManager.AddToRoleAsync(user, Constants.Customer);
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("userName", user.UserName));
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("name", user.Name));
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("email", user.Email));
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("phone", user.PhoneNumber));
                            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("role", Constants.Customer));
                            var created_user = await _userManager.FindByNameAsync(user.UserName);
                            entity.UserId = created_user.Id.ToString();
                        }
                    }
                    transaction.Commit();
                    return await base.AddAsync(entity);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }

            }
        }

        public async Task ResetPassword(Customer entity)
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
                            //context.Database.Migrate();
                            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                            var user = await _userManager.FindByIdAsync(entity.UserId);
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var changePassRes = await _userManager.ResetPasswordAsync(user, token, "Brg@123456");

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

        public Task<Customer> GetByUserId(string UserId)
        {
            var res = this.dbSet.Include(x => x.MemberCards).ThenInclude(x => x.MemberCardCourses)
                                .Where(x => x.UserId == UserId).FirstOrDefaultAsync();
            return res;
        }

        public void UpdateLockAspUser(string userId, int? stausInt)
        {
            var user = Context.Set<AppUser>().Where(x => x.Id == userId).FirstOrDefault();
            if (user != null)
            {
                if (stausInt != (int)Enums.AccountStatus.locked)
                {
                    user.LockoutEnd = null;
                    if (user.LockStatus == (int)AccountStatus.moreNoShow)
                    {
                        user.LockStatus = (int)AccountStatus.noLockNomore;
                    }
                    else
                    {
                        user.LockStatus = (int)AccountStatus.open;
                    }
                }
                else
                {
                    var lockHour = Configuration.GetSection("loginTime").GetValue<int>("LockHour");
                    user.LockoutEnd = DateTime.Now.AddYears(lockHour);
                    user.LockStatus = (int)Enums.AccountStatus.locked;
                }
                Context.Set<AppUser>().Update(user);
                Context.SaveChanges();
            }
        }
    }

}