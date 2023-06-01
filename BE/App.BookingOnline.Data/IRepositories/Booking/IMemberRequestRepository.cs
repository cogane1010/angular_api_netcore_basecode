using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.Core.Repositories;


namespace App.BookingOnline.Data.Repositories
{
    public interface IMemberRequestRepository : IGridRepository<MemberRequest, MemberRequestPagingModel>
    {
    }
}