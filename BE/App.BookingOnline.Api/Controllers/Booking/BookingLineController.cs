using App.BookingOnline.Api.Controllers;
using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static App.Core.Enums;

namespace App.BookingOnline.WebApi.Controllers
{
    public class BookingLineController : GridController<BookingLineDTO, BookingLineFilterModel, IBookingLineService>
    {
        public BookingLineController(IBookingLineService service) : base(service)
        {
        }


        [HttpPost("GetPaging")]
        public override RespondData GetPaging(BookingLineFilterModel filter)
        {
            try
            {
                filter.UserName = UserName;
                filter.UserOrgId = CurOrgId;
                filter.UserId = UserId;

                if (!Roles.Contains(RoleEnums.admin.ToString()))
                {
                    filter.C_Org_Id = new Guid(CurOrgId);
                }
                return Success(_service.GetPaging(filter));
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }

        public override async ValueTask<RespondData> AddOrEdit(BookingLineDTO entityDTO)
        {
            try
            {
                if (entityDTO.Id == null || entityDTO.Id == Guid.Empty)
                {
                    var message = "Bạn không có quyền tạo mới!";
                    return Failure(message);
                }
                else
                {
                    entityDTO.UpdatedDate = DateTime.Now;
                    entityDTO.UpdatedUser = this.UserName;
                    if (entityDTO.IsUpdateNoShow.Value)
                    {
                        entityDTO.UpdateNoShow_Time = DateTime.Now;
                        entityDTO.UpdateNoShow_UserName = this.UserName;
                    }
                    _service.Update(entityDTO);
                }
                return Success(entityDTO.Id);
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }

        }

        [HttpPost("UpdateNoShow")]
        public async ValueTask<RespondData> UpdateNoShow(BookingLineDTO entityDTO)
        {
            try
            {
                if (entityDTO.Id == null || entityDTO.Id == Guid.Empty)
                {
                    var message = "Bạn không có quyền tạo mới!";
                    return Failure(message);
                }
                else
                {
                    entityDTO.UpdatedDate = DateTime.Now;
                    entityDTO.UpdatedUser = this.UserName;
                    if (entityDTO.IsUpdateNoShow.Value)
                    {
                        entityDTO.UpdateNoShow_Time = DateTime.Now;
                        entityDTO.UpdateNoShow_UserName = this.UserName;
                    }
                    else
                    {
                        entityDTO.UpdateNoShow_Time = null;
                        entityDTO.UpdateNoShow_UserName = null;
                    }
                    _service.UpdateNoShow(entityDTO);
                }
                return Success(entityDTO.Id);
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }

        public override RespondData Delete(Guid Id)
        {
            var message = "Bạn không có quyền xóa!";
            return Failure(message);
        }


        [HttpPost("GetStats")]
        public virtual RespondData GetStats(BookingStatisticFilterModel filter)
        {
            try
            {
                return Success(_service.GetStats(filter));
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }

        [HttpPost("GetOrgByUserId")]
        public RespondData GetOrgByUserId()
        {
            try
            {
                return Success(_service.GetOrgByUserId(UserId));
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }
    }
}
