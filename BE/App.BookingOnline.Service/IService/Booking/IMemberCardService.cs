using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using App.Core.Service;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface IMemberCardService : IBaseGridService<MemberCardDTO, MemberCardPagingModel>
    {
        MemberCard GetIdByCardNo(string CardNo);
        void Reassign(MemberCardDTO memberCard);
        MemberCardDTO AddExtend(MemberCardDTO entityDTO);
        void UpdateExtend(MemberCardDTO entityDTO);
        MemberCardDTO Refresh(MemberCardDTO entityDTO);
    }

    public interface IMemberCardCourseService : IBaseGridService<MemberCardCourseDTO, MemberCardCoursePagingModel>
    {

    }



}