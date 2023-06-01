using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace App.BookingOnline.Data.Repositories
{
    public class MemberCardRepository : GridRepository<MemberCard, MemberCardPagingModel>, IMemberCardRepository
    {
        public MemberCardRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public CustomerGroup GetCusGroupByCode(Guid c_Org_Id, string golf_CusGroupCode)
        {           
            return Context.Set<CustomerGroup>().Where(x => x.C_Org_Id == c_Org_Id && x.Code == golf_CusGroupCode).FirstOrDefault();
        }

        public void RemoveMemberCard(Guid mB_Customer_Id)
        {
            throw new NotImplementedException();
        }
    }

    public class MemberCardCourseRepository : GridRepository<MemberCardCourse, MemberCardCoursePagingModel>, IMemberCardCourseRepository
    {
        public MemberCardCourseRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public List<MemberCardCourse> GetByMemberCard(Guid memberCardId)
        {
            return this.dbSet.Where(x => x.MC_MemberCard_Id == memberCardId).AsNoTracking().ToList();
        }
    }




}