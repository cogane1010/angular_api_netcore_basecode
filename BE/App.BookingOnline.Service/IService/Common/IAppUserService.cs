using App.BookingOnline.Data.Identity;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.BookingOnline.Service.DTO.Common;
using App.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BookingOnline.Service
{
    public interface IAppUserService : IBaseGridDataService<CustomerDTO, UserPagingModel>
    {
        Task<string> GetByPhoneAsync(string phoneNumber);
        Task<RespondData> ForgotPasswordAsync(string email);
        Task<RespondData> ChangePassword(string email, string password, string oldPassword);
        Task<RespondData> CreateAccount(CustomerDTO model);
        Task<RespondData> UpdateAccount(CustomerDTO model);
        Task<RespondData> RegisterMemberCard(MemberRequestDTO model);
        RespondData GetInfoMemberCard(string userId);
        RespondData ListMemberCard(string userId);
        RespondData GetAllOrg();
        RespondData GetAccountInfo(string userId);
        Task<RespondData> GetOtpCode(string userId, string lang, string mobilePhone, string email);
        RespondData CheckOtpCode(SmsHistoryDTO model);
        RespondData GetuploadFolder();
        RespondData CheckMemberCard(string cardNumber, string fullName);
        RespondData CheckGoflBrgCard(string cardNumber, string fullname);
        RespondData CheckCreateAccount(CustomerDTO model);
        RespondData SearchGoflBrgCard(string cardNumber, string fullname, string orgCode);
    }
}