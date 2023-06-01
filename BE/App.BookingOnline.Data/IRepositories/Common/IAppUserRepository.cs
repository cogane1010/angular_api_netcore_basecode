using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Domain;
using App.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Data
{
    public interface IAppUserRepository : IBaseGridRepository<Customer, UserPagingModel>
    {
        ValueTask<IEnumerable<AspRole>> GetRolesExample();
        Task<string> GetByPhoneForLoginAsync(string phoneNumber);
        Task<RespondData> ForgotPasswordAsync(string email);
        Task<RespondData> ChangePassword(string email, string password, string oldPassword);
        Task<RespondData> CreateAccount(Customer entity, string password, List<MemberCard> listMemberCard);
        Task<RespondData> UpdateAccount(Customer entity, List<MemberCard> memberCards);
        Task<RespondData> RegisterMemberCard(MemberRequest model);
        RespondData GetInfoMemberCard(string userId);
        RespondData ListMemberCard(string userId);
        IEnumerable<Organization> GetAllOrg();
        RespondData GetAccountInfo(string userId);
        Customer GetCustomerByUserId(string userId);
        Task<SmsHistory> SaveOtp(string userId, string code, string mobilePhone);
        SmsHistory CheckOtpCode(SmsHistory entity);
        Course GetCourseByCode(string code);
        IEnumerable<Customer> GetAllCustomer();
        IEnumerable<MemberCard> GetMemberCardByCustId(Guid id);
        Course GetCourseById(Guid id);
        AppUser GetUserByEmailOrPhone(string mobilePhone, string email);
        IEnumerable<SmsHistory> CheckNumberOtpCode(string mobilePhone, string email);
        RespondData CheckCreateAccount(string Email, string PhoneNumber);
        bool CheckMemberCardByUser(string golf_CardNo, string orgCode);
    }
}