using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using App.Core.Helper;
using System;
using App.BookingOnline.Data.Identity;
using App.Core;

namespace App.BookingOnline.Data.Repositories
{
    public class CourseRepository : BaseGridRepository<Course, CoursePagingModel>, ICourseRepository
    {
        protected readonly BookingOnlineDbContext _context;
        private readonly IBaseRepository<Organization> _organizationRepo;
        private readonly IBaseRepository<AppUser> _userRepo;
        private readonly IBaseRepository<Course> _courseRepo;
        public CourseRepository(IUnitOfWork unitOfWork, BookingOnlineDbContext context)
            : base(unitOfWork)
        {
            _organizationRepo = _unitOfWork.GetDataRepository<Organization>();
            _userRepo = _unitOfWork.GetDataRepository<AppUser>();
            _courseRepo = _unitOfWork.GetDataRepository<Course>();
        }

        public override PagingResponseEntity<Course> GetPaging(CoursePagingModel pagingModel)
        {
            var query = _repo.SelectWhere(x => pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code)
                            && (pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code))
                            && (pagingModel.Name.IsNullOrEmpty() || x.Code.Contains(pagingModel.Name)))
                            .Include(x => x.Organization);
            var result = new PagingResponseEntity<Course>
            {
                Data = query.OrderBy(x => x.CreatedDate)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList(),
                Count = query.Count()
            };

            return result;
        }

        public IEnumerable<Course> GetCourseByOrg(Guid OrgId)
        {
            return _repo.SelectWhere(x => x.C_Org_Id == OrgId).ToList();
        }

        public IEnumerable<Course> GetCourseAllByUser(string userId)
        {
            var org = _userRepo.SelectWhere(a => a.Id == userId)
                            .Join(_organizationRepo.SelectWhere(x => x.IsActive).Include(i => i.Courses), pro => pro.C_Org_Id, cou => cou.Id,
                (pro, cou) => new { pro, cou }).Select(s => new Organization
                {
                    Id = s.cou.Id,
                    Name = s.cou.Name,
                    Code = s.cou.Code,
                    C_OrgType_Id = s.cou.C_OrgType_Id,
                    OrderValue = s.cou.OrderValue,
                    IsActive = s.cou.IsActive,
                    IsSummary = s.cou.IsSummary,
                    Courses = s.cou.Courses
                }).FirstOrDefault();

            if (org.Code == Constants.BrgCode)
            {
                return _courseRepo.SelectWhere(x => x.IsActive);
            }
            else
            {
                return _courseRepo.SelectWhere(x => x.IsActive && x.C_Org_Id == org.Id);
            }
        }
    }




}