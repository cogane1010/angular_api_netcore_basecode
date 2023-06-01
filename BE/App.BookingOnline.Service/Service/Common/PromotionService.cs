using App.BookingOnline.Data;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.Core;
using App.Core.Domain;
using App.Core.Helper;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static App.Core.Enums;

namespace App.BookingOnline.Service.Service.Common
{
    public class PromotionService :
        BaseGridDataService<PromotionDTO, M_Promotion, PromotionFilterModel, IPromotionRepository>,
        IPromotionService
    {
        private readonly ILogger _log;
        private readonly IAppUserRepository appUserRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ISettingRepository _settingRepo;

        public PromotionService(IPromotionRepository gridRepository, IAppUserRepository appUserRepository,
            INotificationRepository notificationRepository, ILogger<PromotionService> logger, ISettingRepository settingRepo) : base(gridRepository)
        {
            this.appUserRepository = appUserRepository;
            _notificationRepository = notificationRepository;
            _log = logger;
            _settingRepo = settingRepo;
        }

        public async ValueTask<IEnumerable<CustomerGroup>> GetCustomerGroups(string userId)
        {
            return await _gridRepository.GetCustomerGroups(userId);
        }

        private string GetDow(PromotionDTO entityDTO)
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
        private PromotionDTO ConvertDow(PromotionDTO entityDto)
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
        public override async ValueTask<PromotionDTO> AddAsync(PromotionDTO entityDTO)
        {
            try
            {
                var folderName = string.Empty;
                var data = string.Empty;
                var pathToSave = string.Empty;
                var folderNameEn = string.Empty;
                var dataEn = string.Empty;
                var pathToSaveEn = string.Empty;
                entityDTO.DOW = GetDow(entityDTO);

                if (!string.IsNullOrEmpty(entityDTO.ImageData))
                {
                    var imageData = entityDTO.ImageData.Split(';');
                    var nameExtenstion = imageData[0].Split('/')[1];
                    var fileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "." + nameExtenstion;
                    folderName = Path.Combine("Assets\\Images\\PromotionImages", fileName);

                    data = imageData[1];
                    pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                }
                if (!string.IsNullOrEmpty(entityDTO.ImageDataEn))
                {
                    var imageData = entityDTO.ImageDataEn.Split(';');
                    var nameExtenstion = imageData[0].Split('/')[1];
                    var fileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "_en." + nameExtenstion;
                    folderNameEn = Path.Combine("Assets\\Images\\PromotionImages", fileName);

                    dataEn = imageData[1];
                    pathToSaveEn = Path.Combine(Directory.GetCurrentDirectory(), folderNameEn);
                }

                var entity = AutoMapperHelper.Map<PromotionDTO, M_Promotion>(entityDTO);
                entity.IsDeleted = false;
                var promotionCourse = new List<M_Promotion_Course>();
                var promotionCustom = new List<M_Promotion_CustomerGroup>();

                foreach (var courseItem in entityDTO.Course)
                {
                    var M_Promotion_Course = new M_Promotion_Course
                    {
                        C_Course_Id = courseItem.Id,
                        IsActive = true,
                        CreatedUser = entityDTO.CreatedUser,
                        CreatedDate = entityDTO.CreatedDate
                    };
                    promotionCourse.Add(M_Promotion_Course);
                }
                if (promotionCourse.Any())
                {
                    entity.PromotionCourse = promotionCourse;
                }

                foreach (var customerItem in entityDTO.CustomerGroups)
                {
                    var customerGroup = new M_Promotion_CustomerGroup
                    {
                        MB_CustomerGroup_Id = customerItem.Id,
                        C_Org_Id = customerItem.C_Org_Id,
                        IsActive = true,
                        CreatedUser = entityDTO.CreatedUser,
                        CreatedDate = entityDTO.CreatedDate
                    };
                    promotionCustom.Add(customerGroup);
                }

                if (promotionCustom.Any())
                {
                    entity.PromotionCustomerGroup = promotionCustom;
                }

                entity.Img_Url = folderName;
                entity.Img_Url_En = folderNameEn;
                var result = await _gridRepository.AddAsync(entity);
                if (result != null && (data != null && !string.IsNullOrEmpty(data)) && !string.IsNullOrEmpty(entityDTO.ImageData))
                {
                    var newBytes = Convert.FromBase64String(data.Split(',')[1]);
                    File.WriteAllBytes(pathToSave, newBytes);
                }
                if (result != null && (dataEn != null && !string.IsNullOrEmpty(dataEn)) && !string.IsNullOrEmpty(entityDTO.ImageDataEn))
                {
                    var newBytes = Convert.FromBase64String(dataEn.Split(',')[1]);
                    File.WriteAllBytes(pathToSaveEn, newBytes);
                }
                var configOut = new MapperConfiguration(c =>
                {
                    c.CreateMap<M_Promotion, PromotionDTO>()
                         .ForMember(d => d.PromotionCourses,
                         o => o.MapFrom(s => s.PromotionCourse))
                         .ForMember(d => d.PromotionCustomerGroup,
                         o => o.MapFrom(s => s.PromotionCustomerGroup));
                    c.CreateMap<M_Promotion_Course, PromotionCourseDTO>();
                    c.CreateMap<M_Promotion_CustomerGroup, PromotionCustomerGroupDTO>();
                    c.CreateMap<CustomerGroup, CustomerGroupDTO>();
                });
                var mapperOut = configOut.CreateMapper();
                var destOut = mapperOut.Map<M_Promotion, PromotionDTO>(result);

                if (result != null)
                {
                    var notificationType = FcmNotifiType.promotion.ToString();
                    if (result.Promotion_Type == Enums.PromotionType.seagolf.ToString())
                    {
                        notificationType = FcmNotifiType.seagolf.ToString();
                    }
                    var users = appUserRepository.GetAllCustomer().Where(x => !string.IsNullOrEmpty(x.FcmTokenDevice));
                    if (users.Any())
                    {
                        string promotionContent = _settingRepo.GetSetting("PromotionContent");
                        string promotionContentEn = _settingRepo.GetSetting("PromotionContentEn");
                        var dataNotificationQueue = new List<NotificationQueue>();
                        foreach (var user in users)
                        {
                            NotificationQueue notifiModel = new NotificationQueue
                            {
                                SendTo = user.FcmTokenDevice,
                                UserId = new Guid(user.UserId),
                                SendDate = DateTime.Now,
                                BookingId = result.Id,
                                IsSend = false,
                                IsActive = true,
                                NotificationType = notificationType,
                                CreatedDate = DateTime.Now,
                                CreatedUser = entityDTO.CreatedUser,
                                Title = result.Name,
                                Content = string.Format(promotionContent, string.Join(",", entityDTO.Course.Select(s => s.Name)), entityDTO.Name),
                                TitleEn = entity.NameEn,
                                ContentEn = string.Format(promotionContentEn, string.Join(",", entityDTO.Course.Select(s => s.Name)), entityDTO.NameEn),
                                Img_url = result.Img_Url
                            };
                            if (!string.IsNullOrEmpty(notifiModel.SendTo) && !string.IsNullOrEmpty(notifiModel.Title))
                            {
                                dataNotificationQueue.Add(notifiModel);
                            }
                        }
                        if (dataNotificationQueue.Any())
                        {
                            _notificationRepository.InsertNotificationQueue(dataNotificationQueue);
                        }
                    }
                }

                return destOut;
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                return await new ValueTask<PromotionDTO>();
            }

        }

        public override void Update(PromotionDTO entityDTO)
        {
            try
            {
                var folderName = string.Empty;
                var data = string.Empty;
                var pathToSave = string.Empty;
                var folderNameEn = string.Empty;
                var dataEn = string.Empty;
                var pathToSaveEn = string.Empty;
                entityDTO.DOW = GetDow(entityDTO);

                if (!string.IsNullOrEmpty(entityDTO.ImageData))
                {
                    var imageData = entityDTO.ImageData.Split(';');
                    var nameExtenstion = imageData[0].Split('/')[1];
                    if (string.IsNullOrEmpty(entityDTO.Img_Url) || !File.Exists(entityDTO.Img_Url))
                    {
                        var fileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "." + nameExtenstion;
                        folderName = Path.Combine("Assets\\Images\\PromotionImages", fileName);
                        data = imageData[1];
                        pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    }
                    else
                    {
                        data = imageData[1];
                        folderName = entityDTO.Img_Url;
                        pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    }
                }

                if (!string.IsNullOrEmpty(entityDTO.ImageDataEn))
                {
                    var imageData = entityDTO.ImageDataEn.Split(';');
                    var nameExtenstion = imageData[0].Split('/')[1];
                    if (string.IsNullOrEmpty(entityDTO.Img_Url_En) || !File.Exists(entityDTO.Img_Url_En))
                    {
                        var fileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "_en." + nameExtenstion;
                        folderNameEn = Path.Combine("Assets\\Images\\PromotionImages", fileName);
                        dataEn = imageData[1];
                        pathToSaveEn = Path.Combine(Directory.GetCurrentDirectory(), folderNameEn);
                    }
                    else
                    {
                        dataEn = imageData[1];
                        folderNameEn = entityDTO.Img_Url_En;
                        pathToSaveEn = Path.Combine(Directory.GetCurrentDirectory(), folderNameEn);
                    }
                }

                var tempPromotionCourses = entityDTO.PromotionCourses;
                var tempPromotionCustomerGroup = entityDTO.PromotionCustomerGroup;
                entityDTO.PromotionCourses = null;
                entityDTO.PromotionCustomerGroup = null;

                var entity = AutoMapperHelper.Map<PromotionDTO, M_Promotion>(entityDTO);
                var promotionCourse = new List<M_Promotion_Course>();
                var promotionCustom = new List<M_Promotion_CustomerGroup>();

                foreach (var courseItem in entityDTO.Course)
                {
                    var proCourse = _gridRepository.GetPromotionCourse(entityDTO.Id, courseItem.Id);
                    if (!proCourse.Any())
                    {
                        var M_Promotion_Course = new M_Promotion_Course
                        {
                            C_Course_Id = courseItem.Id,
                            IsActive = true,
                            CreatedUser = entityDTO.CreatedUser,
                            CreatedDate = entityDTO.CreatedDate
                        };
                        var isExisted = tempPromotionCourses?.Any(x => x.C_Course_Id == courseItem.Id
                        && x.M_Promotion_Id == entityDTO.Id);
                        if (!tempPromotionCourses.Any() || !isExisted.Value)
                        {
                            promotionCourse.Add(M_Promotion_Course);
                        }
                    }
                }
                _gridRepository.DeletePromotionCourse(entityDTO.Id, entityDTO.Course.Select(s => s.Id));
                if (promotionCourse.Any())
                {
                    entity.PromotionCourse = promotionCourse;
                }

                foreach (var customerItem in entityDTO.CustomerGroups)
                {
                    if (entityDTO.C_Org_Id == null)
                    {
                        entityDTO.C_Org_Id = Guid.Empty;
                    }
                    var cusGroup = _gridRepository.GetPromotionCustomerGroups(entityDTO.Id, entityDTO.C_Org_Id.Value, customerItem.Id);
                    if (!cusGroup.Any())
                    {
                        var customerGroup = new M_Promotion_CustomerGroup
                        {
                            MB_CustomerGroup_Id = customerItem.Id,
                            C_Org_Id = customerItem.C_Org_Id,
                            IsActive = true,
                            CreatedUser = entityDTO.CreatedUser,
                            CreatedDate = entityDTO.CreatedDate
                        };
                        var isExisted = tempPromotionCustomerGroup?.Any(x => x.MB_CustomerGroup_Id == customerItem.Id
                        && x.M_Promotion_Id == entityDTO.Id);
                        if (!tempPromotionCustomerGroup.Any() || !isExisted.Value)
                        {
                            promotionCustom.Add(customerGroup);
                        }
                    }
                }
                _gridRepository.DeletepromotionCustom(entityDTO.Id, entityDTO.CustomerGroups.Select(s => s.Id));
                if (promotionCustom.Any())
                {
                    entity.PromotionCustomerGroup = promotionCustom;
                }

                entity.Img_Url = folderName;
                entity.Img_Url_En = folderNameEn;
                _gridRepository.Update(entity);

                if (!string.IsNullOrEmpty(entityDTO.ImageData) && (data != null && !string.IsNullOrEmpty(data)))
                {
                    var newBytes = Convert.FromBase64String(data.Split(',')[1]);
                    File.WriteAllBytes(pathToSave, newBytes);
                }
                if (!string.IsNullOrEmpty(entityDTO.ImageDataEn) && (data != null && !string.IsNullOrEmpty(dataEn)))
                {
                    var newBytes = Convert.FromBase64String(dataEn.Split(',')[1]);
                    File.WriteAllBytes(pathToSaveEn, newBytes);
                }

                //var notificationType = FcmNotifiType.promotion.ToString();
                //if (entity.Promotion_Type == Enums.PromotionType.seagolf.ToString())
                //{
                //    notificationType = FcmNotifiType.seagolf.ToString();
                //}
                //var users = appUserRepository.GetAllCustomer().Where(x => !string.IsNullOrEmpty(x.FcmTokenDevice));
                //if (users.Any())
                //{
                //    string promotionContent = _settingRepo.GetSetting("PromotionContent");
                //    string promotionContentEn = _settingRepo.GetSetting("PromotionContentEn");
                //    var dataNotificationQueue = new List<NotificationQueue>();
                //    foreach (var user in users)
                //    {
                //        var notifiModel = new NotificationQueue
                //        {
                //            SendTo = user.FcmTokenDevice,
                //            UserId = new Guid(user.UserId),
                //            SendDate = entityDTO.Start_Date,
                //            BookingId = entity.Id,
                //            IsSend = false,
                //            IsActive = true,
                //            NotificationType = notificationType,
                //            CreatedDate = DateTime.Now,
                //            CreatedUser = entityDTO.CreatedUser,
                //            Title = entity.Name,
                //            Content = string.Format(promotionContent, string.Join(",", entityDTO.Course.Select(s => s.Name)), entityDTO.Name),
                //            TitleEn = entity.NameEn,
                //            ContentEn = string.Format(promotionContentEn, string.Join(",", entityDTO.Course.Select(s => s.Name)), entityDTO.NameEn),
                //            Img_url = entity.Img_Url
                //        };
                //        if (!string.IsNullOrEmpty(notifiModel.SendTo) && !string.IsNullOrEmpty(notifiModel.Title))
                //        {
                //            dataNotificationQueue.Add(notifiModel);
                //        }
                //    }
                //    if (dataNotificationQueue.Any())
                //    {
                //        _notificationRepository.InsertNotificationQueue(dataNotificationQueue);
                //    }
                //}
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                }
            }
        }

        public override PromotionDTO GetById(Guid Id)
        {
            try
            {
                var data = _gridRepository.SingleOrDefault(Id);
                var course = _gridRepository.GetPromotionCourse(Id);
                var cusGroup = _gridRepository.GetPromotionCusGroup(Id);

                if (course.Any())
                {
                    data.PromotionCourse = course.ToList();
                }
                if (cusGroup.Any())
                {
                    data.PromotionCustomerGroup = cusGroup.ToList();
                }

                var configOut = new MapperConfiguration(c =>
                {
                    c.CreateMap<M_Promotion, PromotionDTO>()
                         .ForMember(d => d.PromotionCourses,
                         o => o.MapFrom(s => s.PromotionCourse))
                         .ForMember(d => d.PromotionCustomerGroup,
                         o => o.MapFrom(s => s.PromotionCustomerGroup));
                    c.CreateMap<M_Promotion_Course, PromotionCourseDTO>();
                    c.CreateMap<M_Promotion_CustomerGroup, PromotionCustomerGroupDTO>();
                    c.CreateMap<CustomerGroup, CustomerGroupDTO>();
                });
                var mapperOut = configOut.CreateMapper();
                var destOut = mapperOut.Map<M_Promotion, PromotionDTO>(data);

                if (data != null && !string.IsNullOrEmpty(data.Img_Url))
                {
                    if (File.Exists(data.Img_Url))
                    {
                        Byte[] bytes = File.ReadAllBytes(data.Img_Url);
                        String fileData = Convert.ToBase64String(bytes);
                        var filext = data.Img_Url.Split('.')[1];
                        var typeName = string.Format("data:image/{0};base64,", filext);
                        destOut.ImageData = typeName + fileData;
                    }
                }
                if (data != null && !string.IsNullOrEmpty(data.Img_Url_En))
                {
                    if (File.Exists(data.Img_Url_En))
                    {
                        Byte[] bytes = File.ReadAllBytes(data.Img_Url_En);
                        String fileData = Convert.ToBase64String(bytes);
                        var filext = data.Img_Url.Split('.')[1];
                        var typeName = string.Format("data:image/{0};base64,", filext);
                        destOut.ImageDataEn = typeName + fileData;
                    }
                }
                destOut = ConvertDow(destOut);
                return destOut;
            }
            catch (Exception e)
            {
                return new PromotionDTO();
            }
        }

        public override PagingResponseEntityDTO<PromotionDTO> GetPaging(PromotionFilterModel pagingModel)
        {
            var paging = _gridRepository.GetPagingData(pagingModel);
            var data = paging.Data.Select(s => new PromotionDTO
            {
                Id = s.Id,
                PromotionCode = s.PromotionCode,
                Name = s.Name,
                Start_Date = s.Start_Date,
                End_Date = s.End_Date,
                IsActive = s.IsActive,
                DOW = s.DOW,
                IsHotPromotion = s.IsHotPromotion,
                CustomerGroupNames = _gridRepository.GetPromotionCusGroupName(s.Id),
                CourseNames = _gridRepository.GetPromotionCourseName(s.Id)
            });

            return new PagingResponseEntityDTO<PromotionDTO>
            {
                Count = paging.Count,
                Data = data
            };
        }
    }
}
