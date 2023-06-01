using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models.Golf;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.BookingOnline.Service.IService.Common;
using App.Core.Helper;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.BookingOnline.Service.Service.Common
{
    public class CourseTemplateService :
        BaseGridDataService<CourseTemplateDTO, GF_CourseTemplate, CourseTemplateFilterModel, ICourseTemplateRepository>,
        ICourseTemplateService
    {
        public CourseTemplateService(ICourseTemplateRepository gridRepository) : base(gridRepository)
        {

        }

        public override async ValueTask<CourseTemplateDTO> AddAsync(CourseTemplateDTO entityDTO)
        {
            var checkCode = _gridRepository.CheckCourseTemplateCode(entityDTO.Code,Guid.Empty);
            if (checkCode)
            {
                throw new Exception("Mã code đã tồn tại");
            }
            entityDTO.DOW = GetDow(entityDTO);
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<CourseTemplateDTO, GF_CourseTemplate>()
                     .ForMember(d => d.GF_CourseTemplateLines,
                     o => o.MapFrom(s => s.CourseTemplateLine.Where(x => x.C_Course_Id != Guid.Empty)));
                c.CreateMap<CourseTemplateLineDTO, GF_CourseTemplateLine>();
            });
            var mapper = config.CreateMapper();
            var destinations = mapper.Map<CourseTemplateDTO, GF_CourseTemplate>(entityDTO);
            var result = await _gridRepository.AddAsync(destinations);

            var configOut = new MapperConfiguration(c =>
            {
                c.CreateMap<GF_CourseTemplate, CourseTemplateDTO>()
                     .ForMember(d => d.CourseTemplateLine,
                     o => o.MapFrom(s => s.GF_CourseTemplateLines));
                c.CreateMap<GF_CourseTemplateLine, CourseTemplateLineDTO>();
            });
            var mapperOut = configOut.CreateMapper();
            var destOut = mapperOut.Map<GF_CourseTemplate, CourseTemplateDTO>(result);

            return ConvertDow(destOut);
        }

        private CourseTemplateDTO ConvertDow(CourseTemplateDTO entityDto)
        {
            if (!string.IsNullOrEmpty(entityDto.DOW))
            {
                var splitDow = entityDto.DOW.Split(',');
                foreach (var i in splitDow)
                {
                    if (i == "0")
                    {
                        entityDto.AppliedDate0 = true;
                    }
                    if (i == "1")
                    {
                        entityDto.AppliedDate1 = true;
                    }
                    if (i == "2")
                    {
                        entityDto.AppliedDate2 = true;
                    }
                    if (i == "3")
                    {
                        entityDto.AppliedDate3 = true;
                    }
                    if (i == "4")
                    {
                        entityDto.AppliedDate4 = true;
                    }
                    if (i == "5")
                    {
                        entityDto.AppliedDate5 = true;
                    }
                    if (i == "6")
                    {
                        entityDto.AppliedDate6 = true;
                    }
                    if (i == "7")
                    {
                        entityDto.AppliedDate7 = true;
                    }
                }
            }
            return entityDto;
        }
        private string GetDow(CourseTemplateDTO entityDTO)
        {
            var dow = string.Empty;
            if (entityDTO.AppliedDate0)
            {
                dow += string.IsNullOrEmpty(dow) ? "0" : ",0";
            }
            if (entityDTO.AppliedDate1)
            {
                dow += string.IsNullOrEmpty(dow) ? "1" : ",1";
            }
            if (entityDTO.AppliedDate2)
            {
                dow += string.IsNullOrEmpty(dow) ? "2" : ",2";
            }
            if (entityDTO.AppliedDate3)
            {
                dow += string.IsNullOrEmpty(dow) ? "3" : ",3";
            }
            if (entityDTO.AppliedDate4)
            {
                dow += string.IsNullOrEmpty(dow) ? "4" : ",4";
            }
            if (entityDTO.AppliedDate5)
            {
                dow += string.IsNullOrEmpty(dow) ? "5" : ",5";
            }
            if (entityDTO.AppliedDate6)
            {
                dow += string.IsNullOrEmpty(dow) ? "6" : ",6";
            }
            if (entityDTO.AppliedDate7)
            {
                dow += string.IsNullOrEmpty(dow) ? "7" : ",7";
            }

            return dow;
        }

        public override async void Update(CourseTemplateDTO entityDTO)
        {
            var checkCode = _gridRepository.CheckCourseTemplateCode(entityDTO.Code, entityDTO.Id);
            if (checkCode)
            {
                throw new Exception("Mã code đã tồn tại");
            }
            entityDTO.DOW = GetDow(entityDTO);

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<CourseTemplateDTO, GF_CourseTemplate>()
                     .ForMember(d => d.GF_CourseTemplateLines,
                     o => o.MapFrom(s => s.CourseTemplateLine.Where(x => x.C_Course_Id != Guid.Empty)));
                c.CreateMap<CourseTemplateLineDTO, GF_CourseTemplateLine>()
                .AfterMap((s, d) => d.StartTee = 1)
                .AfterMap((s, d) => d.IsActive = true)
                .AfterMap((s, d) => d.C_Org_Id = entityDTO.C_Org_Id);
            });
            var mapper = config.CreateMapper();
            var destinations = mapper.Map<CourseTemplateDTO, GF_CourseTemplate>(entityDTO);
            _gridRepository.Update(destinations);
        }

        public override CourseTemplateDTO GetById(Guid Id)
        {
            try
            {
                var data = _gridRepository.SingleOrDefault(Id);
                var child = _gridRepository.GetCourseTemplatelineByCourseTemplateId(Id);

                if (child.Any())
                {
                    data.GF_CourseTemplateLines = child.ToList();
                }

                var configOut = new MapperConfiguration(c =>
                {
                    c.CreateMap<GF_CourseTemplate, CourseTemplateDTO>()
                         .ForMember(d => d.CourseTemplateLine,
                         o => o.MapFrom(s => s.GF_CourseTemplateLines));
                    c.CreateMap<GF_CourseTemplateLine, CourseTemplateLineDTO>();
                });
                var mapperOut = configOut.CreateMapper();
                var destOut = mapperOut.Map<GF_CourseTemplate, CourseTemplateDTO>(data);

                return ConvertDow(destOut);
            }
            catch (Exception e)
            {

            }
            return new CourseTemplateDTO();
        }
    }
}
