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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public class MemberRequestService : BaseGridService<MemberRequestDTO, MemberRequest, MemberRequestPagingModel, IMemberRequestRepository>, IMemberRequestService
    {
        public MemberRequestService(IMemberRequestRepository repo) : base(repo)
        {

        }
        public void SaveNote(MemberRequestDTO memberRequestDTO)
        {
            var memberRequest = AutoMapperHelper.Map<MemberRequestDTO, MemberRequest>(memberRequestDTO);
            _repo.UpdateByProperties(memberRequest, new List<Expression<Func<MemberRequest, object>>>() {
                { x => x.Description },
                { x => x.UpdatedDate },
                { x => x.UpdatedUser }
            });
        }
    }
}

