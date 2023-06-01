using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Repositories;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Repositories
{
    public interface IMemberCardRepository : IGridRepository<MemberCard, MemberCardPagingModel>
    {
        void RemoveMemberCard(Guid mB_Customer_Id);
        CustomerGroup GetCusGroupByCode(Guid c_Org_Id, string golf_CusGroupCode);
    }
    public interface IMemberCardCourseRepository : IGridRepository<MemberCardCourse, MemberCardCoursePagingModel>
    {
        List<MemberCardCourse> GetByMemberCard(Guid memberCardId);
    }
}