using App.BookingOnline.Service;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Data.Paging;
using Microsoft.AspNetCore.Mvc;
using App.Core.Domain;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using App.Core.Helper;
using Newtonsoft.Json;
using System.Linq;

namespace App.BookingOnline.Api.Controllers
{

    public class MemberCardController : BaseGridController<MemberCardDTO, MemberCardPagingModel, IMemberCardService>
    {

        private readonly IAppUserService _userService;
        public MemberCardController(IMemberCardService service, IAppUserService userService) : base(service)
        {
            _userService = userService;
        }

        [HttpPost("Search")]
        public async Task<RespondData> Search(MemberCardPagingModel pagingModel)
        {
            try
            {
                // Get cau hinh => Call sang pm golf  => xu ly data
                // 
                var respondData = _userService.SearchGoflBrgCard(pagingModel.Golf_CardNo, pagingModel.Golf_FullName, pagingModel.OrgCode);
                if (respondData.IsSuccess)
                {
                    //if (!string.IsNullOrEmpty(pagingModel.OrgCode))
                    //{
                    //    respondData = respondData.Data.Where(x => x.OrgCode == pagingModel.OrgCode);
                    //}
                    //var res = new PagingResponseEntityDTO<MemberCardDTO>()
                    //{
                    //    Count = respondData.Count(),
                    //    Data = respondData
                    //};

                    return Success(respondData.Data);
                }
                else
                {
                    return Failure("", respondData.Message);
                }               
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }

        [HttpPost("Assign")]
        public RespondData Assign(MemberCardDTO memberCard)
        {
            try
            {
                memberCard.CreatedUser = UserId;
                var memberCardId = _service.GetIdByCardNo(memberCard.Golf_CardNo);
                if (memberCardId == null)
                {
                    _service.AddExtend(memberCard);
                }
                else
                {
                    if (memberCardId.C_Org_Id == memberCard.C_Org_Id)
                    {
                        return Failure("", "Thẻ này đã được add vào tài khoản khác");
                    }
                    else
                    {
                        _service.AddExtend(memberCard);
                    }
                }

                return Success();
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }


        [HttpPost("Reassign")]
        public RespondData Reassign(MemberCardDTO memberCard)
        {
            try
            {
                memberCard.IsDelete = true;
                _service.Reassign(memberCard);
                return Success();
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }
        }


        [HttpPost("Refresh")]
        public async Task<RespondData> Refresh(MemberCardDTO memberCard)
        {
            try
            {               
                memberCard = _service.Refresh(memberCard);

                return Success(memberCard);
            }
            catch (Exception e)
            {
                return Failure("", e.Message);
            }

        }


    }


}