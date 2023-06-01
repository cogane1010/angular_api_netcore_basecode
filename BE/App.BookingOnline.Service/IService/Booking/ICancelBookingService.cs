using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using System.Collections.Generic;

namespace App.BookingOnline.Service
{
    public interface ICancelBookingService : IBaseGridDataService<BookingDTO, BookingFilterModel>
    {
        
    }
}
