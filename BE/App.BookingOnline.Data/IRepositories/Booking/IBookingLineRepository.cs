using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Golf;
using App.Core.Repositories;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Repositories
{
    public interface IBookingLineRepository : IBaseGridRepository<BookingLine, BookingLineFilterModel>
    {
        List<BookStatByOrg> GetStats(BookingStatisticFilterModel filter);
        List<BookingLine> GetbyBookingId(Guid bookingId);
        IEnumerable<BookingLine> GetPagingBookedData(BookingLineFilterModel pagingModel);
        IEnumerable<BookingSpecialRequest> GetOtherBooking(Guid gF_Booking_Id);
        IEnumerable<Organization> GetOrgByUserId(string userId);
        IEnumerable<GF_CourseTemplateLine> GetDataTeesheet(BookingLineFilterModel pagingModel);
        void UpdateNoShow(Guid id, bool is_NoShow, string caddie_No, int hole, string description);
        GF_CourseTemplateLine GetCourseTemplateLineByTeetime(BookingLineFilterModel filter);
    }
}
