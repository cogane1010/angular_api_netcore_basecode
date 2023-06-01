using App.BookingOnline.Data;
using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.Core;
using App.Core.Domain;
using App.Core.Helper;
using App.Core.Service;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service
{
    public class OrganizationTypeService : BaseGridService<OrganizationTypeDTO, OrganizationType, OrganizationTypePagingModel, IOrganizationTypeRepository>, IOrganizationTypeService
    {
        public OrganizationTypeService(IOrganizationTypeRepository repo) : base(repo)
        {
        }
    }
    public class OrganizationService : BaseGridService<OrganizationDTO, Organization, OrganizationPagingModel, IOrganizationRepository>, IOrganizationService
    {
        IOrganizationInfoRepository _repoInfo;
        public OrganizationService(IOrganizationRepository repo, IOrganizationInfoRepository repoInfo) : base(repo)
        {
            _repoInfo = repoInfo;
        }


        public override OrganizationDTO Get(Guid Id)
        {
            var result = _repo.GetById(Id).Result;
            var dto = AutoMapperHelper.Map<Organization, OrganizationDTO>(result);
            dto.OrganizationTypeName = result.OrganizationType.Name;
            dto.OrganizationInfo = AutoMapperHelper.Map<OrganizationInfo, OrganizationInfoDTO>(result.OrganizationInfos.Find(x => true));
            return dto;
        }
        
        public override OrganizationDTO Add(OrganizationDTO entityDTO)
        {
            var infoDTO = entityDTO.OrganizationInfo;
            entityDTO.OrganizationInfo = null;
            var res = base.Add(entityDTO);

            if (infoDTO != null)
            {
                var info = AutoMapperHelper.Map<OrganizationInfoDTO, OrganizationInfo>(infoDTO);
                if (infoDTO.Id == null || infoDTO.Id == Guid.Empty)
                {
                    info.CreatedDate = DateTime.Now;
                    info.CreatedUser = entityDTO.UpdatedUser;
                    info.C_Org_Id = res.Id;
                    _repoInfo.AddAsync(info);
                }
                else
                {
                    info.UpdatedDate = DateTime.Now;
                    info.UpdatedUser = entityDTO.UpdatedUser;
                    info.C_Org_Id = res.Id;
                    _repoInfo.Update(info);
                }
            }

            return res;
        }

        public override PagingResponseEntityDTO<OrganizationDTO> GetPaging(OrganizationPagingModel pagingModel)
        {
            var paging = _repo.GetPaging(pagingModel);
            List<OrganizationDTO> dtos = new List<OrganizationDTO>();
            foreach (var dt in paging.Data)
            {
                var dto = AutoMapperHelper.Map<Organization, OrganizationDTO>(dt);
                dto.OrganizationInfo = AutoMapperHelper.Map<OrganizationInfo, OrganizationInfoDTO>(dt.OrganizationInfos.Find(x => true));
                dtos.Add(dto);
            }
            var result = new PagingResponseEntityDTO<OrganizationDTO>
            {
                Count = paging.Count,
                Data = dtos
            };

            return result;
        }

        public override void Update(OrganizationDTO entityDTO)
        {
           
            if (entityDTO.OrganizationInfo != null)
            {
                var info = AutoMapperHelper.Map<OrganizationInfoDTO, OrganizationInfo>(entityDTO.OrganizationInfo);
                if (entityDTO.OrganizationInfo.Id == null || entityDTO.OrganizationInfo.Id == Guid.Empty)
                {
                    info.CreatedDate = DateTime.Now;
                    info.CreatedUser = entityDTO.UpdatedUser;
                    _repoInfo.AddAsync(info);
                }
                else
                {
                    info.UpdatedDate = DateTime.Now;
                    info.UpdatedUser = entityDTO.UpdatedUser;
                    _repoInfo.Update(info);
                }
            }
            entityDTO.OrganizationInfo = null; 
            base.Update(entityDTO);

        }
        public override void Delete(Guid Id)
        {
            base.Delete(Id);
        }

        public override IEnumerable<OrganizationDTO> GetAll()
        {
            var data = _repo.GetAllAsync().Result;
            List<OrganizationDTO> listOrg = new List<OrganizationDTO>();
            if (data != null)
            {
                foreach(Organization org in data)
                {
                    var courses = org.Courses;
                    org.Courses = null;
                    var orgDTO = AutoMapperHelper.Map<Organization, OrganizationDTO>(org);
                    orgDTO.Courses = AutoMapperHelper.Map<Course, CourseDTO, List<Course>, List<CourseDTO>>(courses);
                    listOrg.Add(orgDTO);
                }  
            }
            return listOrg;
            
        }


    }
    public class OrganizationInfoService : BaseGridService<OrganizationInfoDTO, OrganizationInfo, OrganizationInfoPagingModel, IOrganizationInfoRepository>, IOrganizationInfoService
    {
        public OrganizationInfoService(IOrganizationInfoRepository repo) : base(repo)
        {
        }
    }
}
