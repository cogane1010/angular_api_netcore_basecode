using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using App.Core.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public class MenuService : BaseGridService<MenuDTO, Menu, MenuPagingModel, IMenuRepository>,IMenuService
    {
        public MenuService(IMenuRepository repo) : base(repo)
        {
            
        }

        

    }
}
