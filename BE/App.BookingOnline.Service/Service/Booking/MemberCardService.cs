using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Booking;
using App.Core;
using App.Core.Helper;
using App.Core.Service;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;

namespace App.BookingOnline.Service
{
    public class MemberCardService : BaseGridService<MemberCardDTO, MemberCard, MemberCardPagingModel, IMemberCardRepository>, IMemberCardService
    {
        IMemberCardCourseRepository _memberCardCourseRepo;
        private readonly ILogger _log;
        public IConfiguration Configuration { get; }
        public MemberCardService(IMemberCardRepository repo, IMemberCardCourseRepository memberCardCourseRepo
            , IConfiguration configuration, ILogger<MemberCardService> logger) : base(repo)
        {
            _memberCardCourseRepo = memberCardCourseRepo;
            Configuration = configuration;
            _log = logger;
        }

        public MemberCardDTO AddExtend(MemberCardDTO memberCardDTO)
        {
            var memberCardCourseDTOs = memberCardDTO.CoursesMemberCard;
            memberCardDTO.IsDelete = false;
            memberCardDTO.IsActive = true;
            var memberCard = base.Add(memberCardDTO);

            foreach (var memberCardCourseDTO in memberCardCourseDTOs)
            {
                memberCardCourseDTO.CreatedUser = memberCardDTO.CreatedUser;
                memberCardCourseDTO.MC_MemberCard_Id = memberCard.Id;
                var cusGroup = _repo.GetCusGroupByCode(memberCardDTO.C_Org_Id, memberCardCourseDTO.BK_CusGroupCode);
                if (cusGroup != null)
                {
                    memberCardCourseDTO.MB_CustomerGroup_Id = cusGroup.Id;
                }
                //var memberCardCourseEntity = AutoMapperHelper.Map<MemberCardCourseDTO, MemberCardCourse>(memberCardCourseDTO);
                //var result = _memberCardCourseRepo.AddAsync(memberCardCourseEntity);
            }
            var memberCardCourses = AutoMapperHelper.Map<MemberCardCourseDTO, MemberCardCourse, List<MemberCardCourseDTO>, List<MemberCardCourse>>(memberCardCourseDTOs);

            _memberCardCourseRepo.AddRangeAsync(memberCardCourses).Wait();

            memberCard.CoursesMemberCard = memberCardCourseDTOs;
            return memberCard;

        }
        public void UpdateExtend(MemberCardDTO memberCardExtendDTO)
        {
            var memberCardCourseDTOs = memberCardExtendDTO.CoursesMemberCard;

            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<MemberCardDTO, MemberCard>();
                c.CreateMap<MemberCardCourseDTO, MemberCardCourse>();
                c.CreateMap<OrganizationDTO, Organization>();
                c.CreateMap<CourseDTO, Course>();
            });
            var mapper = config.CreateMapper();

            var memberCardDTO = mapper.Map<MemberCardDTO, MemberCard>(memberCardExtendDTO);
            _repo.Update(memberCardDTO);

            var old_MemberCardCourses = _memberCardCourseRepo.GetByMemberCard(memberCardDTO.Id);

            foreach (var memberCardCourseDTO in memberCardCourseDTOs)
            {
                var memberCardCourse = AutoMapperHelper.Map<MemberCardCourseDTO, MemberCardCourse>(memberCardCourseDTO);
                var old = old_MemberCardCourses.Find(x => x.C_Course_Id == memberCardCourse.C_Course_Id);
                old_MemberCardCourses.Remove(old);
                memberCardCourse.MC_MemberCard_Id = memberCardDTO.Id;
                if (old != null)
                {
                    memberCardCourse.Id = old.Id;
                    memberCardCourse.UpdatedDate = DateTime.Now;
                    memberCardCourse.UpdatedUser = memberCardDTO.UpdatedUser;
                    _memberCardCourseRepo.Update(memberCardCourse);
                }
                else
                {
                    memberCardCourse.CreatedDate = DateTime.Now;
                    memberCardCourse.CreatedUser = memberCardDTO.UpdatedUser;
                    _memberCardCourseRepo.AddAsync(memberCardCourse);
                }
            }
            foreach (var old_mc in old_MemberCardCourses)
            {
                _memberCardCourseRepo.Delete(old_mc.Id);
            }
        }

        public MemberCard GetIdByCardNo(string CardNo)
        {
            var mc = _repo.Find(x => x.Golf_CardNo == CardNo && !x.IsDelete).FirstOrDefault();
            if (mc != null)
            {
                return mc;
            }
            else
            {
                return null;
            }
        }

        public void Reassign(MemberCardDTO memberCard)
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<MemberCardDTO, MemberCard>();
                c.CreateMap<OrganizationDTO, Organization>();
                c.CreateMap<CourseDTO, Course>();
            });
            var mapper = config.CreateMapper();
            var entity = mapper.Map<MemberCardDTO, MemberCard>(memberCard);
            //var entity = AutoMapperHelper.Map<MemberCardDTO, MemberCard>(memberCard);
            _repo.UpdateByProperties(entity, new List<Expression<Func<MemberCard, object>>>() {
                { x => x.IsDelete}
            });
        }

        public MemberCardDTO Refresh(MemberCardDTO entityDTO)
        {
            // check thẻ
            var membercards = CheckGoflBrgCard(entityDTO.Golf_CardNo);
            if (membercards != null && membercards.Any())
            {
                if (entityDTO.Golf_FullName != membercards.FirstOrDefault().Golf_FullName)
                {
                    entityDTO.IsDelete = true;
                    Reassign(entityDTO);
                    throw new Exception("thẻ đã chuyển nhượng");
                }
                var item = membercards.FirstOrDefault(x => x.C_Org_Id == entityDTO.C_Org_Id);
                if (item != null)
                {
                    entityDTO.Golf_CardNo = item.Golf_CardNo;
                    entityDTO.Golf_Email = item.Golf_Email;
                    entityDTO.Golf_Mobilephone = item.Golf_Mobilephone;
                    entityDTO.Golf_CardStatus = item.Golf_CardStatus;
                    entityDTO.Golf_FullName = item.Golf_FullName;
                    entityDTO.Golf_DOB = item.Golf_DOB;
                    entityDTO.Golf_Address = item.Golf_Address;
                    entityDTO.Golf_IDNo = item.Golf_IDNo;
                    entityDTO.Golf_IsLock = item.Golf_IsLock;
                    entityDTO.Golf_IsActive = item.Golf_IsActive;
                    entityDTO.Golf_Effective_Date = item.Golf_Effective_Date;
                    entityDTO.Golf_Expire_Date = item.Golf_Expire_Date;
                    entityDTO.Golf_MemberCardId = item.Golf_MemberCardId;
                    entityDTO.CoursesMemberCard = item.CoursesMemberCard;
                    entityDTO.IsActive = true;
                }

                //entityDTO.MemberCardExtendCourses = null;

                this.UpdateExtend(entityDTO);
            }

            return entityDTO;
        }

        public IEnumerable<MemberCardDTO> CheckGoflBrgCard(string cardNumber, string fullname = "")
        {
            string baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApi");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                baseUrl = Configuration.GetSection("urlData").GetValue<string>("GolfAccountApiPro");
            }
            string checkmemberUrl = Configuration.GetSection("urlData").GetValue<string>("Checkcard");

            IRestClient client = new RestClient(baseUrl);
            IRestRequest request = new RestRequest(checkmemberUrl, Method.POST);
            request.AddHeader("Accept", "*/*");
            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            request.AddParameter("CardNo", cardNumber, ParameterType.GetOrPost);

            try
            {
                var memberCard = new List<MemberCardDTO>();
                IRestResponse<GolfMemberRespone> response = client.Execute<GolfMemberRespone>(request);
                if (response.IsSuccessful)
                {
                    var weatherForecast = JsonSerializer.Deserialize<GolfMemberRespone>(response.Content);
                    if (!weatherForecast.IsSuccess)
                    {
                        throw new Exception(weatherForecast.InfoMessage);
                    }
                    foreach (var item in weatherForecast.Data)
                    {
                        var card = AutoMapperHelper.Map<BookOnlineMemberCard, MemberCardDTO>(item);
                        card.CoursesMemberCard = AutoMapperHelper.Map<BookOnlineMemberCardCourse, MemberCardCourseDTO,
                            List<BookOnlineMemberCardCourse>, List<MemberCardCourseDTO>>(item.MemberCardCourses);

                        memberCard.Add(card);
                    }

                    return memberCard;
                }
                else
                {
                    throw new Exception("Không tìm thấy thẻ");
                }
            }
            catch (Exception e)
            {
                _log.LogError(Constants.GolfApiError + "-CheckGoflBrgCard-" + e.Message);
                _log.LogError(e.StackTrace);
                throw new Exception("Không tìm thấy thẻ");
            }
        }

    }
    public class MemberCardCourseService : BaseGridService<MemberCardCourseDTO, MemberCardCourse, MemberCardCoursePagingModel, IMemberCardCourseRepository>, IMemberCardCourseService
    {
        public MemberCardCourseService(IMemberCardCourseRepository repo) : base(repo)
        {

        }
    }
}

