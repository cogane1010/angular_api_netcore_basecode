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
    public class OrganizationTypeRepository : GridRepository<OrganizationType, OrganizationTypePagingModel>, IOrganizationTypeRepository
    {
        public OrganizationTypeRepository(BookingOnlineDbContext context)
            : base(context)
        { }
        public override PagingResponseEntity<OrganizationType> GetPaging(OrganizationTypePagingModel pagingModel)
        {
            var query = this.dbSet.AsQueryable()
                            .Where(x => pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code))
                            .Where(x => pagingModel.Name.IsNullOrEmpty() || x.Name.Contains(pagingModel.Name));
            var result = new PagingResponseEntity<OrganizationType>
            {
                Data = query.OrderBy(x => x.CreatedDate)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList(),
                Count = query.Count()
            };

            return result;
        }
        
    }

    public class OrganizationRepository : GridRepository<Organization, OrganizationPagingModel>, IOrganizationRepository
    {
        public OrganizationRepository(BookingOnlineDbContext context)
            : base(context)
        { }

        public Task<Organization> GetById(Guid id)
        {
            var result = this.dbSet.Where(x => x.Id == id)
                 .Include(x => x.OrganizationType)
                 .Include(x => x.OrganizationInfos).FirstOrDefaultAsync();
            return result;
        }

        public override PagingResponseEntity<Organization> GetPaging(OrganizationPagingModel pagingModel)
        {
            var query = this.dbSet.AsQueryable()
                            .Where(x => pagingModel.Code.IsNullOrEmpty() || x.Code.Contains(pagingModel.Code))
                            .Where(x => pagingModel.Name.IsNullOrEmpty() || x.Code.Contains(pagingModel.Name))
                            .Include(x => x.OrganizationType).Include(x => x.OrganizationInfos);
            var result = new PagingResponseEntity<Organization>
            {
                Data = query.OrderBy(x => x.CreatedDate)
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).ToList(),
                Count = query.Count()
            };
            
            return result;
        }


        public override async Task<IEnumerable<Organization>> GetAllAsync()
        {
            return await Context.Set<Organization>().Include(x => x.Courses).ToListAsync();
        }

    }

    public class OrganizationInfoRepository : GridRepository<OrganizationInfo, OrganizationInfoPagingModel>, IOrganizationInfoRepository
    {
        public OrganizationInfoRepository(BookingOnlineDbContext context)
            : base(context)
        { }
    }
}