using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using App.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.BookingOnline.Service
{
    public class BookingLineService : BaseGridDataService<BookingLineDTO, BookingLine, BookingLineFilterModel, IBookingLineRepository>, IBookingLineService
    {
        public BookingLineService(IBookingLineRepository gridRepository) : base(gridRepository)
        {

        }

        public List<BookStatByOrg> GetStats(BookingStatisticFilterModel filter)
        {
            return _gridRepository.GetStats(filter);
        }

        public IEnumerable<BookingLine> GetbyBookingId(Guid BookingId)
        {
            return _gridRepository.GetbyBookingId(BookingId);
        }

        public override PagingResponseEntityDTO<BookingLineDTO> GetPaging(BookingLineFilterModel filter)
        {
            var bookedData = _gridRepository.GetPagingBookedData(filter);
            var teesheet = _gridRepository.GetDataTeesheet(filter);

            var teetime = new List<BookingLineDTO>();
            if (!filter.IsSearch)
            {
                foreach (var item in teesheet)
                {
                    for(var a = 1; a <= 4; a++)
                    {
                        if (!string.IsNullOrEmpty(item.StartTime) && !string.IsNullOrEmpty(item.EndTime))
                        {
                            var start = new DateTime(filter.DateId.Value.Year, filter.DateId.Value.Month, filter.DateId.Value.Day,
                                Convert.ToInt32(item.StartTime.Split(':')[0]), Convert.ToInt32(item.StartTime.Split(':')[1]), 0, 0);

                            var end = new DateTime(filter.DateId.Value.Year, filter.DateId.Value.Month, filter.DateId.Value.Day,
                                Convert.ToInt32(item.EndTime.Split(':')[0]), Convert.ToInt32(item.EndTime.Split(':')[1]), 0, 0);

                            for (var i = 0; i < 10000; i += item.Interval.Value)
                            {
                                var time = start;
                                if (i != 0)
                                {
                                    time = start.AddMinutes(i);
                                }

                                if (time > end)
                                {
                                    break;
                                }

                                var line = new BookingLineDTO
                                {
                                    Tee_Time = time,
                                    TeeTimeEnd = time.AddMinutes(item.Interval.Value),
                                    Part = item.Part,
                                    FlightSeq = a
                                };

                                foreach (var bk in bookedData)
                                {
                                    if (bk.Tee_Time.Value.Hour == time.Hour
                                        && bk.Tee_Time.Value.Minute == time.Minute && a == bk.FlightSeq.Value)
                                    {
                                        line.Id = bk.Id;
                                        line.C_Org_Id = bk.C_Org_Id.Value;
                                        line.CustomerFullName = bk.CustomerFullName;
                                        line.FlightSeq = bk.FlightSeq.Value;
                                        line.BookingCode = bk.Booking.BookingCode;
                                        line.Hole = bk.Hole.Value;
                                        line.Telephone = bk.Booking.AppUser.PhoneNumber;
                                        line.CardNo = bk.CardNo;
                                        line.GF_Booking_Id = bk.GF_Booking_Id;
                                        line.Is_NoShow = bk.Is_NoShow;
                                        line.Description = bk.Description;
                                        var otherBooking = _gridRepository.GetOtherBooking(bk.GF_Booking_Id);
                                        if (otherBooking.Any())
                                        {
                                            line.Description = string.Join(",", otherBooking.Select(s => s.BookingOtherType?.Name));
                                        }
                                    }
                                }
                                teetime.Add(line);
                            }
                        }
                    }                    
                }
            }

            // add teetime nếu như đoạn trên chưa add duoc
            var ids = teetime.Select(s => s.Id).ToList();
            var missTeetime = bookedData.Where(x => !ids.Contains(x.Id));
            foreach (var bk in missTeetime)
            {
                var line = new BookingLineDTO
                {
                    Tee_Time = bk.Tee_Time.Value,
                    Part = bk.Part,
                    Id = bk.Id,
                    C_Org_Id = bk.C_Org_Id.Value,
                    CustomerFullName = bk.CustomerFullName,
                    FlightSeq = bk.FlightSeq.Value,
                    BookingCode = bk.Booking.BookingCode,
                    Hole = bk.Hole.Value,
                    Telephone = bk.Booking.AppUser.PhoneNumber,
                    CardNo = bk.CardNo,
                    GF_Booking_Id = bk.GF_Booking_Id
                };
                var otherBooking = _gridRepository.GetOtherBooking(bk.GF_Booking_Id);
                if (otherBooking.Any())
                {
                    line.Description = string.Join(",", otherBooking.Select(s => s.BookingOtherType?.Name));
                }
                teetime.Add(line);
            }
            teetime = teetime.OrderBy(o => o.Tee_Time).ToList();
            return new PagingResponseEntityDTO<BookingLineDTO>
            {
                Count = teetime.Count(),
                Data = teetime
            };
        }

        public IEnumerable<OrganizationDTO> GetOrgByUserId(string userId)
        {
            var data = _gridRepository.GetOrgByUserId(userId);
            var list = new List<OrganizationDTO>();
            foreach (var item in data)
            {
                var course = item.Courses;
                item.Courses = null;
                var result = AutoMapperHelper.Map<Organization, OrganizationDTO>(item);
                result.Courses = AutoMapperHelper.Map<Course, CourseDTO, List<Course>, List<CourseDTO>>(course);
                list.Add(result);
            }

            return list;
        }

        public void UpdateNoShow(BookingLineDTO entityDTO)
        {            
            _gridRepository.UpdateNoShow(entityDTO.Id, entityDTO.Is_NoShow,entityDTO.Caddie_No,entityDTO.Hole,entityDTO.Description);
        }
    }
}
