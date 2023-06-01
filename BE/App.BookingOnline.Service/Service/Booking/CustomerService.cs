using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.Core;
using App.Core.Domain;
using App.Core.Helper;
using App.Core.Service;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public class CustomerService : BaseGridService<CustomerDTO, Customer, CustomerPagingModel, ICustomerRepository>, ICustomerService
    {
        ISequenceRepository _seqRepo;
        IUploadFileRepository _uploadFileRepo;
        private string fileUrl = string.Empty;
        public IConfiguration Configuration { get; }
        public CustomerService(ICustomerRepository repo, ISequenceRepository seqRepo, IUploadFileRepository uploadFileRepo
            , IConfiguration configuration) : base(repo)
        {
            _seqRepo = seqRepo;
            _uploadFileRepo = uploadFileRepo;
            Configuration = configuration;
            fileUrl = Configuration.GetSection("fileUpload").GetValue<string>("fileUrl");
        }

        public CustomerDTO GetExtend(Guid Id)
        {
            var entity = _repo.GetByIdAsyncExtend(Id).Result;

            if (entity == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }
            var membercards = entity.MemberCards.Where(x => !x.IsDelete);
            entity.MemberCards = null;
            var dto = AutoMapperHelper.Map<Customer, CustomerDTO>(entity);

            var isLock = _repo.GetAspUserById(entity.UserId);
            if (isLock.LockoutEnd != null || isLock.LockoutEnd.HasValue)
            {
                dto.StausInt = (int)Enums.AccountStatus.locked;
                dto.IsActive = false;
            }
            else
            {
                if (dto.IsActive)
                {
                    dto.StausInt = (int)Enums.AccountStatus.active;
                }
                else
                {
                    dto.StausInt = (int)Enums.AccountStatus.inActive;
                }
            }

            if (membercards != null && membercards.Any())
            {
                dto.MemberCards = new List<MemberCardDTO>();
                foreach (var memberCard in membercards)
                {
                    //var mcDTO = AutoMapperHelper.Map<MemberCard, MemberCardDTO>(memberCard);
                    var config = new MapperConfiguration(c =>
                    {
                        c.CreateMap<MemberCard, MemberCardDTO>();
                        c.CreateMap<Organization, OrganizationDTO>();
                        c.CreateMap<Course, CourseDTO>();
                    });
                    var mapper = config.CreateMapper();
                    var mcDTO = mapper.Map<MemberCard, MemberCardDTO>(memberCard);

                    mcDTO.OrgName = memberCard.Organization?.Name;
                    mcDTO.CoursesMemberCard = new List<MemberCardCourseDTO>();
                    foreach (var cour in memberCard.MemberCardCourses)
                    {
                        var mcDTOExtends = AutoMapperHelper.Map<MemberCardCourse, MemberCardCourseDTO>(cour);
                        mcDTOExtends.CourseName = cour.Course?.Name;
                        mcDTO.CoursesMemberCard.Add(mcDTOExtends);
                    }
                    dto.MemberCards.Add(mcDTO);
                }
            }

            if (entity != null && !string.IsNullOrEmpty(entity.Img_Url))
            {

                var url = fileUrl + entity.Img_Url;
                if (File.Exists(url))
                {
                    Byte[] bytes = File.ReadAllBytes(url);
                    String fileData = Convert.ToBase64String(bytes);
                    var filext = url.Split('.')[1];
                    var typeName = string.Format("data:image/{0};base64,", filext);
                    dto.ImageData = typeName + fileData;
                }
            }

            return dto;

        }

        public async ValueTask<CustomerDTO> AddCustomer(CustomerDTO entityDTO)
        {
            var folderName = string.Empty;
            var data = string.Empty;
            var pathToSave = string.Empty;
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<CustomerDTO, Customer>();
                c.CreateMap<MemberCardDTO, MemberCard>();
                c.CreateMap<OrganizationDTO, Organization>();
                c.CreateMap<CourseDTO, Course>();
            });
            var mapper = config.CreateMapper();
            var entity = mapper.Map<CustomerDTO, Customer>(entityDTO);

            if (!string.IsNullOrEmpty(entityDTO.ImageData))
            {
                var imageData = entityDTO.ImageData.Split(';');
                var nameExtenstion = imageData[0].Split('/')[1];
                var fileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "." + nameExtenstion;
                entity.Img_Url = "\\" + fileName;
                data = imageData[1];
                pathToSave = Path.Combine(fileUrl, fileName);
            }

            entity.CustomerCode = _seqRepo.GetCode("CUSTOMERCODE", SequenceModes.Year);
            if (entityDTO.StausInt == (int)Enums.AccountStatus.active)
            {
                entity.IsActive = true;
            }
            else
            {
                entity.IsActive = false;
                _repo.UpdateLockAspUser(entityDTO.UserId, entityDTO.StausInt);
            }

            var res = await _repo.AddCustomer(entity);

            if (res != null && (data != null && !string.IsNullOrEmpty(data)) && !string.IsNullOrEmpty(entityDTO.ImageData))
            {
                var newBytes = Convert.FromBase64String(data.Split(',')[1]);
                File.WriteAllBytes(pathToSave, newBytes);
            }
            return AutoMapperHelper.Map<Customer, CustomerDTO>(res);
        }


        public async Task UpdateCustomer(CustomerDTO entityDTO)
        {
            var folderName = string.Empty;
            var data = string.Empty;
            var pathToSave = string.Empty;
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<CustomerDTO, Customer>();
                c.CreateMap<MemberCardDTO, MemberCard>();
                c.CreateMap<OrganizationDTO, Organization>();
                c.CreateMap<CourseDTO, Course>();
            });
            var mapper = config.CreateMapper();
            var entity = mapper.Map<CustomerDTO, Customer>(entityDTO);

            if (!string.IsNullOrEmpty(entityDTO.ImageData))
            {
                var imageData = entityDTO.ImageData.Split(';');
                var nameExtenstion = imageData[0].Split('/')[1];
                if (string.IsNullOrEmpty(entityDTO.Img_Url) || !File.Exists(entityDTO.Img_Url))
                {
                    var fileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "." + nameExtenstion;
                    data = imageData[1];
                    pathToSave = Path.Combine(fileUrl, fileName);
                    entity.Img_Url = "\\" + fileName;
                }
                else
                {
                    data = imageData[1];
                    folderName = entityDTO.Img_Url;
                    pathToSave = Path.Combine(fileUrl, folderName);
                }
            }

            if (entityDTO.StausInt == (int)Enums.AccountStatus.active)
            {
                entity.IsActive = true;
                _repo.UpdateLockAspUser(entityDTO.UserId, entityDTO.StausInt);
            }
            else
            {
                entity.IsActive = false;
                _repo.UpdateLockAspUser(entityDTO.UserId, entityDTO.StausInt);
            }

            await _repo.UpdateCustomer(entity);

            if (!string.IsNullOrEmpty(entityDTO.ImageData) && (data != null && !string.IsNullOrEmpty(data)))
            {
                var newBytes = Convert.FromBase64String(data.Split(',')[1]);
                File.WriteAllBytes(pathToSave, newBytes);
            }
        }

        public async Task ResetPassword(CustomerDTO entityDTO)
        {
            entityDTO.MemberCards = null;
            var entity = AutoMapperHelper.Map<CustomerDTO, Customer>(entityDTO);
            await _repo.ResetPassword(entity);
        }

        public void ChangeStatus(CustomerDTO entityDTO)
        {
            entityDTO.IsActive = !entityDTO.IsActive;
            if (entityDTO.IsActive)
            {
                _repo.UpdateLockAspUser(entityDTO.UserId, (int)Enums.AccountStatus.active);
            }
            else
            {
                _repo.UpdateLockAspUser(entityDTO.UserId, (int)Enums.AccountStatus.locked);
            }
            entityDTO.MemberCards = null;
            var entity = AutoMapperHelper.Map<CustomerDTO, Customer>(entityDTO);
            _repo.UpdateByProperties(entity, new List<Expression<Func<Customer, object>>>() {
                { x => x.IsActive }
            });
        }

        public UploadFile SaveUploadFile(UploadFileDTO dtoFile)
        {
            var entity = AutoMapperHelper.Map<UploadFileDTO, UploadFile>(dtoFile);
            var res = _uploadFileRepo.AddAsync(entity);
            return res.Result;
        }

        public UploadFileDTO GetFile(UploadFileDTO dtoFile)
        {
            var entity = _uploadFileRepo.GetByIdAsync(dtoFile.Id);
            var dto = AutoMapperHelper.Map<UploadFile, UploadFileDTO>(entity.Result);
            return dto;
        }

        public override PagingResponseEntityDTO<CustomerDTO> GetPaging(CustomerPagingModel pagingModel)
        {
            var paging = _repo.GetPaging(pagingModel);
            var listData = new List<CustomerDTO>();
            foreach (var item in paging.Data)
            {
                var member = item.MemberCards;
                item.MemberCards = null;
                var customer = AutoMapperHelper.Map<Customer, CustomerDTO>(item);
                customer.MemberCards = AutoMapperHelper.Map<MemberCard, MemberCardDTO, IEnumerable<MemberCard>, IEnumerable<MemberCardDTO>>(member).ToList();

                //var isLock = _repo.GetAspUserById(customer.UserId);
                //if(pagingModel.UserId == "1e817f18-9162-4fe8-9d54-9311d49efc49")
                //{
                //    var a = 1;
                //}
                if (customer.IsActive == false)
                {
                    customer.StausInt = (int)Enums.AccountStatus.locked;
                    //customer.IsActive = false;
                }
                else
                {
                    customer.StausInt = (int)Enums.AccountStatus.active;
                    //customer.IsActive = true;
                }

                listData.Add(customer);
            }
            return new PagingResponseEntityDTO<CustomerDTO>
            {
                Count = paging.Count,
                Data = listData
            };
        }
    }
}

