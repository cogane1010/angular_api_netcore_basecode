using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Data
{
    public interface IHomeRepository : IBaseGridRepository<Customer, UserPagingModel>
    {
        Task<Customer> GetCustomer(string userId);
        Task<IEnumerable<M_Promotion>> GetPromotion(string userId);
        Task<IEnumerable<Course>> GetCourses(string userId);
        RespondData GetMemberCard(string userId);
        void SettingLanguage(Customer customer);
        void InsertFcmTokenDevice(Customer customer);
        IEnumerable<Organization> GetContactAllOrg();
        bool GetStatusMessageVnByUser(string userId);
        bool UpdateStatusMessageVnByUser(string userId, bool v);
        RespondData UpdateAppUserVersion(string version, string userId);       
    }
}