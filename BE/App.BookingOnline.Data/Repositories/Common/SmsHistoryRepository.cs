using App.BookingOnline.Data.FilterModel.Common;
using App.BookingOnline.Data.IRepositories.Common;
using App.BookingOnline.Data.Models;
using App.Core;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.BookingOnline.Data.Repositories.Common
{
    public class SmsHistoryRepository : BaseGridRepository<SmsHistory, SmsHistoryFilterModel>, ISmsHistoryRepository
    {
        private readonly IBaseRepository<Setting> _settingRepo;
        private readonly IBaseRepository<Organization> _organizationRepo;
        public SmsHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _settingRepo = _unitOfWork.GetDataRepository<Setting>();
            _organizationRepo = _unitOfWork.GetDataRepository<Organization>();
        }

        public object GetAllUser()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> GetEmailCourse(Guid orgId)
        {
            var org = _organizationRepo.SelectWhere(x => x.IsActive && x.Id == orgId).FirstOrDefault();
            if (org != null && org.Code == Constants.RT)
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.RubyTree_Email).Select(s => s.Value);
            }
            if (org != null && org.Code == Constants.KI)
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.KI_Email).Select(s => s.Value);
            }
            if (org != null && org.Code == Constants.DNG)
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.DN_Email).Select(s => s.Value);
            }
            if (org != null && org.Code == Constants.LH)
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.Legend_Email).Select(s => s.Value);
            }
            return null;
        }

        public Setting GetEmailTemplateBooking(string lang)
        {
            if (lang == Constants.LangEn)
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.BookingEmailTemplate_en).FirstOrDefault();
            }
            else
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.BookingEmailTemplate_vi).FirstOrDefault();
            }
        }

        public Setting GetEmailTitleBooking(string lang)
        {
            if (lang == Constants.LangEn)
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.BookingEmailTemplate_title_en).FirstOrDefault();
            }
            else
            {
                return _settingRepo.SelectWhere(x => x.Code == Constants.BookingEmailTemplate_title_vi).FirstOrDefault();
            }
        }

        public override PagingResponseEntity<SmsHistory> GetPaging(SmsHistoryFilterModel pagingModel)
        {
            var paggingData = GetPagingData(pagingModel);

            var data = new PagingResponseEntity<SmsHistory>
            {
                Data = paggingData
            };
            data.Count = paggingData.Count();

            return data;
        }

        public IEnumerable<SmsHistory> GetPagingData(SmsHistoryFilterModel pagingModel)
        {
            var result = _repo.SelectWhere(x => (x.Mobilephone == pagingModel.Mobilephone || string.IsNullOrEmpty(pagingModel.Mobilephone))
            && (x.SendDate >= pagingModel.TimeFrom || pagingModel.TimeFrom == null)
            && (x.SendDate <= pagingModel.TimeTo || pagingModel.TimeTo == null)
            )
                            .Skip(pagingModel.PageIndex * pagingModel.PageSize)
                            .Take(pagingModel.PageSize).OrderByDescending(o => o.CreatedDate);

            return result;
        }


    }
}
