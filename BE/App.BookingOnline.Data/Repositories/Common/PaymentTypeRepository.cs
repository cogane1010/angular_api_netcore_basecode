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
    public class PaymentTypeRepository : GridRepository<PaymentType, PaymentTypePagingModel>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public override PagingResponseEntity<PaymentType> GetPaging(PaymentTypePagingModel pagingModel)
        {
            var query = this.dbSet.AsQueryable()
                            .Where(x => pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code))
                            .Where(x => pagingModel.Name.IsNullOrEmpty() || x.Name.Contains(pagingModel.Name))
                            .Include(x => x.Organization);
            var result = new PagingResponseEntity<PaymentType>
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