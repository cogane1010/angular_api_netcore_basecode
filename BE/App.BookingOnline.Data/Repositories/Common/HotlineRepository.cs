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
using App.BookingOnline.Data.Models.Common;

namespace App.BookingOnline.Data.Repositories
{
    public class HotlineRepository : GridRepository<Hotline, HotlinePagingModel>, IHotlineRepository
    {
        public HotlineRepository(BookingOnlineDbContext context)
            : base(context)
        { }


        public string Hotline(Guid Id)
        {
            var query = this.dbSet.AsQueryable()
                        .Where(x => x.Id == Id).FirstOrDefault();
            return query.PhoneNumber;

        }
    }

}