using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Golf;
using App.Core.Domain;
using App.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.Repositories.Common
{
    public class CourseTemplateRepository : BaseGridRepository<GF_CourseTemplate, CourseTemplateFilterModel>, ICourseTemplateRepository
    {
        private readonly IBaseRepository<GF_CourseTemplateLine> _courseTemplateLineRepo;
        private readonly IBaseRepository<Organization> _orgRepo;

        public CourseTemplateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _courseTemplateLineRepo = _unitOfWork.GetDataRepository<GF_CourseTemplateLine>();
            _orgRepo = _unitOfWork.GetDataRepository<Organization>();
        }

        public override async ValueTask<GF_CourseTemplate> AddAsync(GF_CourseTemplate entity)
        {
            var org = _orgRepo.SingleOrDefault(x => x.Id == entity.C_Org_Id);
            var orgName = org != null ? org.Name : string.Empty;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (entity.GF_CourseTemplateLines.Any())
                    {
                        foreach (var item in entity.GF_CourseTemplateLines)
                        {
                            if (item.C_Course_Id != Guid.Empty)
                            {
                                item.C_Org_Id = entity.C_Org_Id;
                                item.StartTee = 1; //only allow booking starttee =1
                                item.OrgName = orgName;
                                item.CreatedUser = entity.CreatedUser;
                                item.CreatedDate = entity.CreatedDate;
                                item.IsActive = true;
                            }
                        }
                    }
                    entity.OrgName = orgName;
                    var res = await _repo.AddAsync(entity);
                    _unitOfWork.SaveChanges();

                    transaction.Commit();
                    return res;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }

            return new GF_CourseTemplate();
        }

        public IEnumerable<GF_CourseTemplateLine> GetCourseTemplatelineByCourseTemplateId(Guid Id)
        {
            var result = _courseTemplateLineRepo.SelectWhere(x => x.GF_CourseTemplate_Id == Id);
            return result;
        }

        public override async void Update(GF_CourseTemplate entity)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    //var obj = _repo.SelectWhere(x => x.Id == entity.Id).FirstOrDefault();
                    //var ids = new List<Guid>();

                    var childitem = _courseTemplateLineRepo.SelectWhere(x => x.GF_CourseTemplate_Id == entity.Id
                   && !entity.GF_CourseTemplateLines.Select(s => s.Id).Contains(x.Id)).Select(s => s.Id).ToList();

                    if (childitem.Any())
                    {
                        //var deleted = _courseTemplateLineRepo.SelectWhere(x => x.GF_CourseTemplate_Id == entity.Id
                        //&& childitem.Contains(x.Id));
                        foreach (var de in childitem)
                        {
                            _courseTemplateLineRepo.Delete(de);
                        }
                    }
                    if (entity.GF_CourseTemplateLines.Any())
                    {
                        foreach (var item in entity.GF_CourseTemplateLines)
                        {
                            if (item.C_Course_Id != Guid.Empty)
                            {
                                //ids.Add(item.Id);
                                item.StartTee = 1; //only allow booking starttee =1
                                item.C_Org_Id = entity.C_Org_Id;
                                item.CreatedUser = entity.UpdatedUser;
                                item.CreatedDate = entity.UpdatedDate.Value;
                                item.UpdatedUser = entity.UpdatedUser;
                                item.UpdatedDate = entity.UpdatedDate;
                                item.IsActive = true;
                            }
                        }
                    }

                    _repo.Update(entity);
                    _unitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }
        }

        public override PagingResponseEntity<GF_CourseTemplate> GetPaging(CourseTemplateFilterModel pagingModel)
        {
            var data = _repo.SelectWhere(x => (x.IsActive == pagingModel.Status || pagingModel.Status == null)
                && (string.IsNullOrEmpty(pagingModel.Code) || x.Code.Contains(pagingModel.Code))
                && (string.IsNullOrEmpty(pagingModel.Name) || x.Name.Contains(pagingModel.Name))
                && (pagingModel.OrgId == null || pagingModel.OrgId == Guid.Empty || x.C_Org_Id == pagingModel.OrgId));
            var result = new PagingResponseEntity<GF_CourseTemplate>
            {
                Data = data.Skip(pagingModel.PageIndex * pagingModel.PageSize).Take(pagingModel.PageSize)
            };
            result.Count = data.Count();
            return result;
        }

        public bool CheckCourseTemplateCode(string code, Guid? id)
        {
            if (id != Guid.Empty)
            {
                var timeSetting = _repo.SelectWhereNoTracking(x => x.Id == id && x.Code == code).FirstOrDefault();
                if (timeSetting != null)
                {
                    return false;
                }
            }
            var time = _repo.SelectWhereNoTracking(x => x.Code == code).FirstOrDefault();
            if (time != null)
            {
                return true;
            }

            return false;
        }
    }
}
