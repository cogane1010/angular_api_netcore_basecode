using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using Microsoft.AspNetCore.Mvc;
using App.Core.Domain;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using App.Core.Helper;

namespace App.BookingOnline.Api.Controllers
{

    public class MemberRequestController : BaseGridController<MemberRequestDTO, MemberRequestPagingModel, IMemberRequestService>
    {

        public MemberRequestController(IMemberRequestService service) : base(service)
        {

        }
        [HttpPost("SaveNote")]
        public virtual RespondData SaveNote(MemberRequestDTO memberRequestDTO)
        {
            try
            {
                memberRequestDTO.UpdatedDate = DateTime.Now;
                memberRequestDTO.UpdatedUser = UserName;
                _service.SaveNote(memberRequestDTO);
                return Success();
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }

        }

        public override RespondData AddOrEdit(MemberRequestDTO entityDTO)
        {
            try
            {
                if (entityDTO.Id == null || entityDTO.Id == Guid.Empty)
                {
                    return Failure("Không được tạo mới");
                }
                else
                {
                    entityDTO.UpdatedDate = DateTime.Now;
                    entityDTO.UpdatedUser = this.UserName;
                    _service.Update(entityDTO);
                }
                return Success(entityDTO.Id);
            }
            catch (Exception e)
            {
                return Failure("",e.Message);
            }
        }

        public override RespondData Delete(Guid Id)
        {
            return Failure("Không được xóa");
        }
    }
   

}