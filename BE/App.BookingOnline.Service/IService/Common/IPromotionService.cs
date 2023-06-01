using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface IPromotionService : IBaseGridDataService<PromotionDTO, PromotionFilterModel>
    {
        ValueTask<IEnumerable<CustomerGroup>> GetCustomerGroups(string userId);
    }
}