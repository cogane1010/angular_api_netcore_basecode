using App.BookingOnline.Data.Paging;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using App.Core.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface IMenuService : IBaseGridService<MenuDTO, MenuPagingModel>
    {
       
    }
}