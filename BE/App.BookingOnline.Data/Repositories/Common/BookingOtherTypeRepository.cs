using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using App.Core.Helper;
using System;

namespace App.BookingOnline.Data.Repositories
{
    public class BookingOtherTypeRepository : GridRepository<BookingOtherType, BookingOtherTypePagingModel>, IBookingOtherTypeRepository
    {
        public BookingOtherTypeRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public override PagingResponseEntity<BookingOtherType> GetPaging(BookingOtherTypePagingModel pagingModel)
        {
            var query = this.dbSet.AsQueryable()
                            .Where(x => pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code))
                            .Where(x => pagingModel.Name.IsNullOrEmpty() || x.Name.Contains(pagingModel.Name))
                            .Include(x => x.Organization);
            var result = new PagingResponseEntity<BookingOtherType>
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