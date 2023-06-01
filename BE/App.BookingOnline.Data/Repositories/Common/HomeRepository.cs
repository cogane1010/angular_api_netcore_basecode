using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.MailService;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.Repositories.Common
{
    public class HomeRepository : BaseGridRepository<Customer, UserPagingModel>, IHomeRepository
    {
        private readonly IBaseRepository<AspRole> _roleRepo;
        private readonly IBaseRepository<AppUser> _userRepo;
        private readonly IBaseRepository<AspUserRole> _userInRoleRepo;
        private readonly IBaseRepository<UserClaim> _userClaimRepo;
        private readonly IBaseRepository<UserLogin> _userLoginRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMailService _mailService;
        private readonly IBaseRepository<Customer> _customerRepo;
        private readonly IBaseRepository<CustomerGroup> _customerGroupRepo;
        private readonly IBaseRepository<Course> _courseRepo;
        private readonly IBaseRepository<Organization> _orgRepo;
        private readonly IBaseRepository<M_Promotion> _promotionRepo;
        private readonly IBaseRepository<MemberCard> _memberCardRepo;
        private readonly ILogger _log;
        private readonly IConfiguration _config;

        public HomeRepository(IUnitOfWork unitOfWork, IMailService mailService, IConfiguration config, UserManager<AppUser> userManager
            , ILogger<HomeRepository> logger) : base(unitOfWork)
        {
            _mailService = mailService;
            _userManager = userManager;
            _roleRepo = _unitOfWork.GetDataRepository<AspRole>();
            _userRepo = _unitOfWork.GetDataRepository<AppUser>();
            _userInRoleRepo = _unitOfWork.GetDataRepository<AspUserRole>();
            _userClaimRepo = _unitOfWork.GetDataRepository<UserClaim>();
            _userLoginRepo = _unitOfWork.GetDataRepository<UserLogin>();
            _customerRepo = _unitOfWork.GetDataRepository<Customer>();
            _customerGroupRepo = _unitOfWork.GetDataRepository<CustomerGroup>();
            _courseRepo = _unitOfWork.GetDataRepository<Course>();
            _orgRepo = _unitOfWork.GetDataRepository<Organization>();
            _promotionRepo = _unitOfWork.GetDataRepository<M_Promotion>();
            _memberCardRepo = _unitOfWork.GetDataRepository<MemberCard>();
            _log = logger;
            _config = config;
        }

        public async Task<Customer> GetCustomer(string userId)
        {
            return _repo.SelectWhereNoTracking(x => x.UserId == userId).FirstOrDefault();
        }

        public async Task<IEnumerable<M_Promotion>> GetPromotion(string userId)
        {
            var promotion = _promotionRepo.SelectWhere(x => x.IsActive && x.End_Date > DateTime.Now)
                .Include(i => i.PromotionCourse).ThenInclude(t => t.Course)
                .Include(i => i.PromotionCustomerGroup).ThenInclude(i => i.CustomerGroup);
            return promotion;
        }

        public async Task<IEnumerable<Course>> GetCourses(string userId)
        {
            var course = _courseRepo.SelectWhere(x => x.IsActive).OrderBy(o => o.OrderValue);
            return course;
        }

        public RespondData GetMemberCard(string userId)
        {
            var customer = _customerRepo.SelectWhere(x => x.UserId == userId).FirstOrDefault();
            if (customer != null)
            {
                var card = _memberCardRepo.SelectWhere(x => x.MB_Customer_Id == customer.Id
                            && x.IsActive && !x.IsDelete).Include(i => i.MemberCardCourses).Include(o => o.Organization);
                foreach (var item in card)
                {
                    if (item.MemberCardCourses != null)
                    {
                        foreach (var gou in item.MemberCardCourses)
                        {
                            gou.CustomerGroup = null;
                        }
                    }
                }
                return new RespondData { IsSuccess = true, Data = card };
            }
            return new RespondData { IsSuccess = false, MsgCode = "48" };
        }

        public void SettingLanguage(Customer customer)
        {
            try
            {
                _customerRepo.UpdateByProperties(customer, new List<Expression<Func<Customer, object>>>()
                                {
                                    x => x.Languague,
                                    x => x.UpdatedDate,
                                    x => x.UpdatedUser
                                });
                _unitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                }
            }
        }

        public void InsertFcmTokenDevice(Customer customer)
        {
            try
            {
                _customerRepo.UpdateByProperties(customer, new List<Expression<Func<Customer, object>>>()
                                {
                                    x => x.FcmTokenDevice,
                                    x => x.Languague,
                                    x => x.UpdatedDate,
                                    x => x.UpdatedUser
                                });
                _unitOfWork.SaveChanges();
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                }
            }
        }

        public IEnumerable<Organization> GetContactAllOrg()
        {
            return _orgRepo.SelectWhere(x => x.IsActive);
        }

        public bool GetStatusMessageVnByUser(string userId)
        {
            var cust = _customerRepo.SelectWhere(x => x.UserId == userId).FirstOrDefault();
            if (cust != null)
            {
                return cust.IsUpdateErrCode ? cust.IsUpdateErrCode : false;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateStatusMessageVnByUser(string userId, bool v)
        {
            var cust = _customerRepo.SelectWhere(x => x.UserId == userId).FirstOrDefault();
            if (cust != null)
            {
                cust.IsUpdateErrCode = v;
            }
            _customerRepo.UpdateByProperties(cust, new List<Expression<Func<Customer, object>>>()
                                {
                                    x => x.IsUpdateErrCode
                                });
            _unitOfWork.SaveChanges();
            return true;
        }

        public RespondData UpdateAppUserVersion(string version, string userId)
        {
            try
            {
                var cust = _customerRepo.SelectWhere(x => x.UserId == userId).FirstOrDefault();
                if (cust != null)
                {
                    cust.Appversion = version;
                    _customerRepo.UpdateByProperties(cust, new List<Expression<Func<Customer, object>>>()
                                {
                                    x => x.Appversion
                                });
                    _unitOfWork.SaveChanges();
                }
            }
            catch(Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
            }                 
            return new RespondData { IsSuccess = true };
        }
        
    }
}
