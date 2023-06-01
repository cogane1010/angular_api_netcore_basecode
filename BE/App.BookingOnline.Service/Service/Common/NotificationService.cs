using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.BookingOnline.Service.IService.Common;
using App.Core;
using App.Core.Domain;
using App.Core.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace App.BookingOnline.Service.Service.Common
{
    public class NotificationService :
        BaseGridDataService<NotificationDTO, GF_Notification, NotificationFilterModel, INotificationRepository>,
        INotificationService
    {
        private IPromotionService _promotionService;
        private IBookingService _bookingService;
        public IConfiguration Configuration { get; }
        private string backendUrl;
        private readonly ILogger _log;

        public NotificationService(INotificationRepository gridRepository, IPromotionService promotionService
            , IBookingService bookingService, IConfiguration configuration
            , ILogger<NotificationService> logger) : base(gridRepository)
        {
            _log = logger;
            _promotionService = promotionService;
            _bookingService = bookingService;
            Configuration = configuration;
            backendUrl = Configuration.GetSection("urlData").GetValue<string>("BackendUrl") + "/";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                backendUrl = Configuration.GetSection("urlData").GetValue<string>("BackendUrlPro") + "/";
            }
        }

        public override async ValueTask<NotificationDTO> AddAsync(NotificationDTO entityDTO)
        {
            var checkCode = _gridRepository.GetNotificationByCode(entityDTO.Code);
            if (checkCode != null)
            {
                throw new Exception("Mã thông báo đã tồn tại");
            }
            try
            {
                var folderName = string.Empty;
                var data = string.Empty;
                var pathToSave = string.Empty;
                if (!string.IsNullOrEmpty(entityDTO.ImageData))
                {
                    var imageData = entityDTO.ImageData.Split(';');
                    var nameExtenstion = imageData[0].Split('/')[1];
                    var fileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "." + nameExtenstion;
                    folderName = Path.Combine("Assets\\Images\\NotifiImages", fileName);

                    data = imageData[1];
                    pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                }
                var entity = AutoMapperHelper.Map<NotificationDTO, GF_Notification>(entityDTO);

                entity.Img_Url = folderName;
                entity.Status = 0;
                var result = await _gridRepository.AddAsync(entity);
                if (result != null && (data != null && !string.IsNullOrEmpty(data)) && !string.IsNullOrEmpty(entityDTO.ImageData))
                {
                    var newBytes = Convert.FromBase64String(data.Split(',')[1]);
                    File.WriteAllBytes(pathToSave, newBytes);
                }
                var outEntity = AutoMapperHelper.Map<GF_Notification, NotificationDTO>(result);
                return outEntity;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public override void Update(NotificationDTO entityDTO)
        {
            var checkCode = _gridRepository.GetNotificationByCode(entityDTO.Code);
            if (checkCode != null)
            {
                throw new Exception("Mã thông báo đã tồn tại");
            }
            try
            {
                var folderName = string.Empty;
                var data = string.Empty;
                var pathToSave = string.Empty;
                if (!string.IsNullOrEmpty(entityDTO.ImageData))
                {
                    var imageData = entityDTO.ImageData.Split(';');
                    var nameExtenstion = imageData[0].Split('/')[1];
                    data = imageData[1];
                    if (string.IsNullOrEmpty(entityDTO.Img_Url) || !File.Exists(entityDTO.Img_Url))
                    {
                        var fileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-fff") + "." + nameExtenstion;
                        folderName = Path.Combine("Assets\\Images\\NotifiImages", fileName);
                        pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    }
                    else
                    {
                        folderName = entityDTO.Img_Url;
                        pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    }
                }

                var entity = AutoMapperHelper.Map<NotificationDTO, GF_Notification>(entityDTO);
                entity.Img_Url = folderName;

                _gridRepository.Update(entity);
                if (!string.IsNullOrEmpty(entityDTO.ImageData) && (data != null && !string.IsNullOrEmpty(data)))
                {
                    var newBytes = Convert.FromBase64String(data.Split(',')[1]);
                    File.WriteAllBytes(pathToSave, newBytes);
                }
            }
            catch (Exception e)
            {
                using (LogContext.PushProperty("MethodName", System.Reflection.MethodBase.GetCurrentMethod().Name))
                {
                    _log.LogError(e.Message);
                    _log.LogError(e.StackTrace);
                }
                throw;
            }
        }

        /// <summary>
        /// this function used in web
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override NotificationDTO GetById(Guid Id)
        {
            try
            {
                var data = _gridRepository.SingleOrDefault(Id);
                var mapperOut = AutoMapperHelper.Map<GF_Notification, NotificationDTO>(data);

                if (data != null && !string.IsNullOrEmpty(data.Img_Url))
                {
                    if (File.Exists(data.Img_Url))
                    {
                        Byte[] bytes = File.ReadAllBytes(data.Img_Url);
                        String fileData = Convert.ToBase64String(bytes);
                        var filext = data.Img_Url.Split('.')[1];
                        var typeName = string.Format("data:image/{0};base64,", filext);
                        mapperOut.ImageData = typeName + fileData;
                        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), data.Img_Url);
                        mapperOut.Img_FullUrl = pathToSave;
                    }
                }
                return mapperOut;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //public override NotificationDTO GetById(Guid Id)
        //{
        //    var result = _gridRepository.SingleOrDefault(Id);
        //    return AutoMapperHelper.Map<Notification, NotificationDTO>(result);
        //}

        /// <summary>
        /// this function used in mobile app
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RespondData> GetAllNotificationByUserAsync(string userId)
        {
            var result = await _gridRepository.GetAllNotificationByUser(userId);
            //var data = AutoMapperHelper.Map<NotificationQueue, NotificationDTO, List<NotificationQueue>, List<NotificationDTO>>(result.ToList());
            return new RespondData { IsSuccess = true, Data = result };
        }

        /// <summary>
        /// this function used in mobile app
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public RespondData GetNotificationById(Guid id, string userId)
        {
            var data = _gridRepository.GetNotificationById(id, userId);
            if (data != null)
            {
                if (data.NotificationType == Enums.FcmNotifiType.promotion.ToString() || data.NotificationType == Enums.FcmNotifiType.seagolf.ToString())
                {
                    var promotion = _promotionService.GetById(data.BookingId.Value);
                    promotion.Full_Image_Url = backendUrl + promotion.Img_Url;
                    promotion.Full_Image_Url = promotion.Full_Image_Url.Replace("\\", "/");
                    promotion.Full_Image_Url_En = backendUrl + promotion.Img_Url_En;
                    promotion.Full_Image_Url_En = promotion.Full_Image_Url_En.Replace("\\", "/");
                    if (promotion.Promotion_Type == Enums.PromotionType.seagolf.ToString())
                    {
                        promotion.NotificationType = Enums.FcmNotifiType.seagolf.ToString();
                    }
                    else
                    {
                        promotion.NotificationType = Enums.FcmNotifiType.promotion.ToString();
                    }
                    if (promotion == null || !promotion.IsActive || promotion.End_Date < DateTime.Now)
                    {
                        return new RespondData { IsSuccess = false, MsgCode = "55" };
                    }

                    return new RespondData { IsSuccess = true, Data = promotion };
                }
                if (data.NotificationType == Enums.FcmNotifiType.booking.ToString())
                {
                    var filter = new BookingFilterModel()
                    {
                        BookingId = data.BookingId,
                        UserId = data.UserId.ToString()
                    };
                    var booking = _bookingService.BookingHistoryDetail(filter);
                    BookingDTO convertBooking = (BookingDTO)booking.Data;
                    convertBooking.NotificationType = Enums.FcmNotifiType.booking.ToString();
                    return new RespondData { IsSuccess = booking.IsSuccess, Data = convertBooking };
                }
                if (data.NotificationType == Enums.FcmNotifiType.notifiAll.ToString())
                {
                    data.Full_Image_Url = backendUrl + data.Img_url;
                    data.Full_Image_Url = data.Full_Image_Url.Replace("\\", "/");
                    data.Name = data.Title;
                    data.NameEn = data.TitleEn;
                    data.Description = data.Content;
                    data.DescriptionEn = data.ContentEn;
                    return new RespondData { IsSuccess = true, Data = data };
                }                
                return new RespondData { IsSuccess = true, Data = data };
            }
            else
            {
                return new RespondData { IsSuccess = false, MsgCode = "44" };
            }
        }

        public async Task<RespondData> GetNumberOpenNotiByUser(string userId)
        {
            int data = _gridRepository.GetNumberOpenNotiByUser(userId);
            return new RespondData { IsSuccess = true, Data = new NumberOpenNoti { Badge = data } };
        }

        public async Task<RespondData> UpdateNumberOpenNotiByUser(string userId)
        {
            RespondData data = _gridRepository.UpdateNumberOpenNotiByUser(userId);
            return data;
        }
    }

    public class NumberOpenNoti
    {
        public int Badge { get; set; }
    }
}
