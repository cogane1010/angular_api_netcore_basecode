using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Helper;
using System;

namespace App.BookingOnline.Data.Repositories
{
    public class ContactSupportRepository : GridRepository<ContactSupport, ContactSupportPagingModel>, IContactSupportRepository
    {
        public ContactSupportRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public override PagingResponseEntity<ContactSupport> GetPaging(ContactSupportPagingModel pagingModel)
        {
            var query = this.dbSet.AsQueryable()
                            .Where(x => pagingModel.CourseCode.IsNullOrEmpty() || x.CourseCode.Contains(pagingModel.CourseCode))
                            .Where(x => pagingModel.PhoneNumber.IsNullOrEmpty() || x.PhoneNumber.Contains(pagingModel.PhoneNumber))
                            .Include(x => x.Course);
            var result = new PagingResponseEntity<ContactSupport>
            {
                Data = query.OrderBy(x => x.CreatedDate)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList(),
                Count = query.Count()
            };

            return result;
        }
    }

    

    
}