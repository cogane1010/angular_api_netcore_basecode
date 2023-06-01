using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using System.Linq;
using App.Core.Helper;
using System;
using Microsoft.EntityFrameworkCore;

namespace App.BookingOnline.Data.Repositories
{
    public class MemberRequestRepository : GridRepository<MemberRequest, MemberRequestPagingModel>, IMemberRequestRepository
    {
        public MemberRequestRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public override PagingResponseEntity<MemberRequest> GetPaging(MemberRequestPagingModel pagingModel)
        {
            var query = this.dbSet.Where(x => pagingModel.FullName.IsNullOrEmpty() || x.FullName.Contains(pagingModel.FullName))
                                .Where(x => pagingModel.MobilePhone.IsNullOrEmpty() || x.MobilePhone.Contains(pagingModel.MobilePhone))
                                .Where(x => pagingModel.Email == null || x.Email.Contains(pagingModel.Email))
                                .Where(x => !pagingModel.C_Org_Id.HasValue || pagingModel.C_Org_Id.Value == Guid.Empty || x.C_Org_Id == pagingModel.C_Org_Id);
            if (pagingModel.Request_Date.HasValue)
            {
                DateTime stDate = pagingModel.Request_Date.Value;
                DateTime endDate = pagingModel.Request_Date.Value.AddDays(1);
                query = query.Where(x => stDate <= x.Request_Date && x.Request_Date < endDate);
            }
            query = query.Include(x => x.Organization);

            var customerQuery = this.Context.Set<Customer>()
                .Where(x => pagingModel.Request_FullName.IsNullOrEmpty() || x.FullName.Contains(pagingModel.Request_FullName))
                .Where(x => pagingModel.Request_MobilePhone.IsNullOrEmpty() || x.MobilePhone.Contains(pagingModel.Request_MobilePhone));


            var join = query.Join(customerQuery, x => x.UserId, y => y.UserId,
                  (query, customerQuery) => new { query, customerQuery })
                  .Select(x => new MemberRequest
                  {
                      Id = x.query.Id,
                      CreatedDate = x.query.CreatedDate,
                      CreatedUser = x.query.CreatedUser,
                      UpdatedDate = x.query.UpdatedDate,
                      UpdatedUser = x.query.UpdatedUser,
                      Request_Date = x.query.Request_Date,
                      FirstName = x.query.FirstName,
                      LastName = x.query.LastName,
                      FullName = x.query.FullName,
                      MobilePhone = x.query.MobilePhone,
                      Email = x.query.Email,
                      C_Org_Id = x.query.C_Org_Id,
                      Organization = x.query.Organization,
                      Description = x.query.Description,
                      UserId = x.query.UserId,
                      Request_Status = x.query.Request_Status,
                      Responsed_Date = x.query.Responsed_Date,
                      Responsed_User = x.query.Responsed_User,
                      Responsed_Description = x.query.Responsed_Description,
                      Request_FullName = x.customerQuery.FullName,
                      Request_MobilePhone = x.customerQuery.MobilePhone,
                  });


            var data = join.OrderBy(x => x.CreatedDate)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList();



            var result = new PagingResponseEntity<MemberRequest>
            {
                Data = data,
                Count = join.Count()
            };
            return result;
        }
    }



}