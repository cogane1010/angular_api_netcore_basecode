using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service
{
    public interface IBookingLineService : IBaseGridDataService<BookingLineDTO, BookingLineFilterModel>
    {
        List<BookStatByOrg> GetStats(BookingStatisticFilterModel filter);
        IEnumerable<BookingLine> GetbyBookingId(Guid BookingId);
        IEnumerable<OrganizationDTO> GetOrgByUserId(string userId);
        void UpdateNoShow(BookingLineDTO entityDTO);
    }
}
