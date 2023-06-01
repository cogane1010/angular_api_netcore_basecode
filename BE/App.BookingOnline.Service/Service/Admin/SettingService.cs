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
    public class SettingService : BaseGridService<SettingDTO, Setting, SettingPagingModel, ISettingRepository>, ISettingService
    {
        public SettingService(ISettingRepository repo) : base(repo)
        {

        }

        public SettingDTO GetByCode(string code)
        {
            var val = _repo.GetSetting(code);
            return new SettingDTO()
            {
                Code = code,
                Value = val
            };
        }

        public override PagingResponseEntityDTO<SettingDTO> GetPaging(SettingPagingModel pagingModel)
        {

            return base.GetPaging(pagingModel);
        }


    }
}
