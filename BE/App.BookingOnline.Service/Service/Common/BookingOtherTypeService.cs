using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Paging;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.DTO;
using App.Core.Service;


namespace App.BookingOnline.Service
{
    public class BookingOtherTypeService : BaseGridService<BookingOtherTypeDTO, BookingOtherType, BookingOtherTypePagingModel, IBookingOtherTypeRepository>, IBookingOtherTypeService
    {
        public BookingOtherTypeService(IBookingOtherTypeRepository repo) : base(repo)
        {
        }
    }
}

