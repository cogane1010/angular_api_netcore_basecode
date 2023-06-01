using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Data.Repositories
{
    public interface IOrganizationTypeRepository : IGridRepository<OrganizationType, OrganizationTypePagingModel>
    {
       
    }
    public interface IOrganizationRepository : IGridRepository<Organization, OrganizationPagingModel>
    {
        Task<Organization> GetById(Guid id);
    }

    public interface IOrganizationInfoRepository : IGridRepository<OrganizationInfo, OrganizationInfoPagingModel>
    {

    }
}