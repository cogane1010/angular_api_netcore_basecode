using App.BookingOnline.Data.Paging;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using App.Core.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface IRoleService : IGenericBaseGridService<AspRoleDTO, RolePagingModel, string>
    {
        List<MenuDTO> GetTreeMenuPermission();
    }
}