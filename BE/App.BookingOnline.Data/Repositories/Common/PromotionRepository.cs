using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.Repositories.Common
{
    public class PromotionRepository : BaseGridRepository<M_Promotion, PromotionFilterModel>, IPromotionRepository
    {
        private readonly IBaseRepository<AspRole> _roleRepo;
        private readonly IBaseRepository<AppUser> _userRepo;
        private readonly IBaseRepository<AspUserRole> _userInRoleRepo;
        private readonly IBaseRepository<UserClaim> _userClaimRepo;
        private readonly IBaseRepository<UserLogin> _userLoginRepo;
        private readonly IBaseRepository<CustomerGroup> _customerGroupRepo;
        private readonly IBaseRepository<M_Promotion_CustomerGroup> _promotion_CustomerGroupRepo;
        private readonly IBaseRepository<M_Promotion_Course> _promotion_CourseRepo;
        private readonly IBaseRepository<Course> _courseRepo;
        private readonly IBaseRepository<Customer> _customerRepo;
        private readonly ILogger _log;

        public PromotionRepository(ILogger<PromotionRepository> logger, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _log = logger;
            _roleRepo = _unitOfWork.GetDataRepository<AspRole>();
            _userRepo = _unitOfWork.GetDataRepository<AppUser>();
            _userInRoleRepo = _unitOfWork.GetDataRepository<AspUserRole>();
            _userClaimRepo = _unitOfWork.GetDataRepository<UserClaim>();
            _userLoginRepo = _unitOfWork.GetDataRepository<UserLogin>();
            _customerGroupRepo = _unitOfWork.GetDataRepository<CustomerGroup>();
            _promotion_CustomerGroupRepo = _unitOfWork.GetDataRepository<M_Promotion_CustomerGroup>();
            _promotion_CourseRepo = _unitOfWork.GetDataRepository<M_Promotion_Course>();
            _courseRepo = _unitOfWork.GetDataRepository<Course>();
            _customerRepo = _unitOfWork.GetDataRepository<Customer>();
        }

        public async ValueTask<IEnumerable<AspRole>> GetRolesExample()
        {
            try
            {
                var user = _userRepo.GetAllAsync().Result;
                var userInRole = _userInRoleRepo.GetAllAsync().Result;
                var roles = _roleRepo.GetAllAsync().Result;
                var userClaim = _userClaimRepo.GetAllAsync().Result;
                var userLogin = _userLoginRepo.GetAllAsync().Result;
            }
            catch (Exception e)
            {

            }


            return await _roleRepo.GetAllAsync();
        }

        public async ValueTask<IEnumerable<CustomerGroup>> GetCustomerGroups(string userId)
        {
            var cust = _userRepo.SelectWhere(x => x.Id == userId).FirstOrDefault();
            return _customerGroupRepo.SelectWhere(x => x.IsActive && x.C_Org_Id == cust.C_Org_Id);
        }

        public IEnumerable<M_Promotion_CustomerGroup> GetPromotionCustomerGroups(Guid promotionId, Guid orgId, Guid customerGroupId)
        {
            return _promotion_CustomerGroupRepo.SelectWhere(x => x.M_Promotion_Id == promotionId
            && (x.C_Org_Id == orgId || x.C_Org_Id == Guid.Empty) && x.MB_CustomerGroup_Id == customerGroupId);
        }

        public IEnumerable<M_Promotion_Course> GetPromotionCourse(Guid promotionId, Guid CourseId)
        {
            return _promotion_CourseRepo.SelectWhere(x => x.M_Promotion_Id == promotionId
            && x.C_Course_Id == CourseId);
        }

        public void DeletePromotionCourse(Guid id, IEnumerable<Guid> courseId)
        {
            var deList = _promotion_CourseRepo.SelectWhere(x => x.M_Promotion_Id == id
            && !courseId.Contains(x.C_Course_Id.Value));
            if (deList.Any())
            {
                _promotion_CourseRepo.RemoveRange(deList);
            }
        }

        public void DeletepromotionCustom(Guid id, IEnumerable<Guid> cusGroups)
        {
            var deList = _promotion_CustomerGroupRepo.SelectWhere(x => x.M_Promotion_Id == id
            && !cusGroups.Contains(x.MB_CustomerGroup_Id.Value));
            if (deList.Any())
            {
                _promotion_CustomerGroupRepo.RemoveRange(deList);
            }
        }

        public IEnumerable<M_Promotion_Course> GetPromotionCourse(Guid id)
        {
            return _promotion_CourseRepo.SelectWhere(x => x.M_Promotion_Id == id).Include(i => i.Course);
        }

        public IEnumerable<M_Promotion_CustomerGroup> GetPromotionCusGroup(Guid id)
        {
            return _promotion_CustomerGroupRepo.SelectWhere(x => x.M_Promotion_Id == id);
        }

        public string GetPromotionCusGroupName(Guid id)
        {
            var result = _promotion_CustomerGroupRepo.GetAll().Where(x => x.M_Promotion_Id.Value == id)
                            .Join(_customerGroupRepo.GetAll(), pro => pro.MB_CustomerGroup_Id, cou => cou.Id,
                (pro, cou) => new { pro, cou }).Select(s => s.cou.Name);
            if (result.Any())
            {
                return string.Join(", ", result);
            }
            return string.Empty;
        }

        public string GetPromotionCourseName(Guid id)
        {
            var result = _promotion_CourseRepo.GetAll().Where(x => x.M_Promotion_Id.Value == id)
                            .Join(_courseRepo.GetAll(), pro => pro.C_Course_Id, cou => cou.Id,
                (pro, cou) => new { pro, cou }).Select(s => s.cou.Name);
            if (result.Any())
            {
                return string.Join(", ", result);
            }
            return string.Empty;
        }

        public override void Delete(Guid id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var promotion = _repo.SingleOrDefault(x => x.Id == id);
                    if (promotion != null)
                    {
                        var promotionCourse = _promotion_CourseRepo.SelectWhere(x => x.M_Promotion_Id == id);
                        if (promotionCourse.Any())
                        {
                            foreach (var item in promotionCourse)
                            {
                                item.IsActive = false;
                                _promotion_CourseRepo.Update(item);
                            }
                        }
                        var promotionCustomerGroup = _promotion_CustomerGroupRepo.SelectWhere(x => x.M_Promotion_Id == id);
                        if (promotionCustomerGroup.Any())
                        {
                            foreach (var item in promotionCustomerGroup)
                            {
                                item.IsActive = false;
                                _promotion_CustomerGroupRepo.Update(item);
                            }
                        }
                        promotion.IsDeleted = true;
                        promotion.IsActive = false;
                        _repo.Update(promotion);
                    }

                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                    {
                        _log.LogError(e.Message);
                    }
                    transaction.Rollback();
                }
            }
        }

        public IEnumerable<M_Promotion_Course> GetAllPromotionCourse()
        {
            return _promotion_CourseRepo.GetAll();
        }

        public IEnumerable<M_Promotion_CustomerGroup> GetAllPromotionCusGroup()
        {
            return _promotion_CustomerGroupRepo.GetAll();
        }

        public PagingResponseEntity<M_Promotion> GetPagingData(PromotionFilterModel pagingModel)
        {
            var proCo = _promotion_CourseRepo.SelectWhere(x => x.C_Course_Id == pagingModel.CourseId)
                .Select(s => s.M_Promotion_Id).Distinct();

            var result = _repo.SelectWhere(x => (x.PromotionCode == pagingModel.Code || string.IsNullOrEmpty(pagingModel.Code))
                        && (x.Promotion_Type == pagingModel.PromotionType)
                        && (!x.IsDeleted)
                        && (x.Name == pagingModel.Name || string.IsNullOrEmpty(pagingModel.Name))
                        && (x.IsHotPromotion == pagingModel.IsHot || pagingModel.IsHot == null)
                        && (x.IsActive == pagingModel.IsActive || pagingModel.IsActive == null)
                        && (proCo.Contains(x.Id) || !proCo.Any()))
                        .OrderByDescending(o => o.CreatedDate);
            //&& (proCuGroup.Contains(x.Id) || !proCuGroup.Any()))
            //.Skip(pagingModel.PageIndex * pagingModel.PageSize)
            //.Take(pagingModel.PageSize);
            return new PagingResponseEntity<M_Promotion>
            {
                Count = result.Count(),
                Data = result.Skip(pagingModel.PageIndex * pagingModel.PageSize).Take(pagingModel.PageSize)
            };
        }



        //public override PagingResponseEntity<M_Promotion> GetPaging(PromotionFilterModel pagingModel)
        //{
        //    var result = _repo.GetAll().Join(_promotion_CourseRepo.GetAll(), pro => pro.Id, cou => cou.Id,
        //        (pro, cou) => new { pro, cou })
        //        .Join(_promotion_CustomerGroupRepo.GetAll(), pcc => pcc.pro.Id, cuu => cuu.M_Promotion_Id,
        //        (pcc, cuu) => new { pcc, cuu }).Select(s => new );

        //    var data = new PagingResponseEntity<M_Promotion>
        //    {
        //        Data = _repo.GetAllAsync().Result
        //                    .Skip(pagingModel.PageIndex * pagingModel.PageSize)
        //                    .Take(pagingModel.PageSize).ToList()
        //    };
        //    data.Count = data.Data.Count();
        //    return data;
        //}
    }
}
