using App.BookingOnline.Data.FilterModel;
using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Repositories;
using App.BookingOnline.Service.Base;
using App.BookingOnline.Service.DTO;
using App.Core.Domain;
using App.Core.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static App.Core.Enums;

namespace App.BookingOnline.Service
{
    public class HistoryBookingService : BaseGridDataService<BookingDTO, Booking, BookingFilterModel, IHistoryBookingRepository>
        , IHistoryBookingService
    {
        ICourseRepository _courseRepo;
        IBookingLineRepository _bookingLineRepo;
        ICancelBookingRepository _cancelBookingRepositoryRepo;
        IBookingRepository _bookingRepo;
        private readonly ILogger _log;
        public IConfiguration Configuration { get; }
        private const string keyCryp = "f3a54a33f297427183d7bb4d4530982c";

        public HistoryBookingService(IHistoryBookingRepository gridRepository, ICourseRepository courseRepo, IConfiguration configuration,
            IBookingLineRepository bookingLineRepo, ILogger<BookingRepository> logger, ICancelBookingRepository cancelBookingRepository
            , IBookingRepository bookingRepository) : base(gridRepository)
        {
            _courseRepo = courseRepo;
            _bookingLineRepo = bookingLineRepo;
            _log = logger;
            Configuration = configuration;
            _cancelBookingRepositoryRepo = cancelBookingRepository;
            _bookingRepo = bookingRepository;
        }

        public override BookingDTO GetById(Guid Id)
        {
            var entity = _gridRepository.SingleOrDefault(Id);
            var lines = entity.BookingLines;
            var spLines = entity.BookingSpecialRequests;
            entity.BookingLines = null;
            entity.BookingSpecialRequests = null;
            var dto = AutoMapperHelper.Map<Booking, BookingDTO>(entity);
            dto.BookingTeetime = AutoMapperHelper.Map<BookingLine, BookingLineDTO, List<BookingLine>, List<BookingLineDTO>>(lines);
            dto.BookingSpecialRequests = AutoMapperHelper.Map<BookingSpecialRequest, BookingSpecialRequestDTO, List<BookingSpecialRequest>, List<BookingSpecialRequestDTO>>(spLines);
            if (entity.C_Course_Id.HasValue)
            {
                var course = _courseRepo.GetByIdAsync(entity.C_Course_Id.Value).Result;
                dto.CourseName = course.Name;
            }

            var user = _cancelBookingRepositoryRepo.GetMemberCard(entity.UserId);
            if (user != null)
            {
                dto.CardFullName = user.FirstOrDefault().Golf_FullName;
                dto.CardMobilePhone = user.FirstOrDefault().Golf_Mobilephone;
                dto.CardEmail = user.FirstOrDefault().Golf_Email;
                dto.GolfCardNo = user.FirstOrDefault().Golf_CardNo;
            }

            return dto;
        }

        public override PagingResponseEntityDTO<BookingDTO> GetPaging(BookingFilterModel pagingModel)
        {
            var paging = _gridRepository.GetPaging(pagingModel);
            var list = new List<BookingDTO>();

            foreach (var item in paging.Data)
            {
                var book = new BookingDTO();
                book.Id = item.Id;
                book.UpdatedDate = item.UpdatedDate;
                book.BookingCode = item.BookingCode;
                book.NonRefundedFee = item.NonRefundedFee;
                book.TotalEstimateFee = item.TotalEstimateFee;
                book.CardFullName = item.AppUser.FullName;
                book.CardMobilePhone = item.AppUser.PhoneNumber;
                book.CardEmail = item.AppUser.Email;
                book.CourseName = item.Course.Name;
                book.CreatedDate = item.CreatedDate;
                book.OrgName = item.Organization?.Name;
                book.DateId = item.DateId.Value;
                book.Status = item?.Status;
                book.UserId = item.UserId;

                var enumStatus = EnumHelper<StatusBookingLine>.Parse(book.Status);
                book.StatusName = EnumHelper.GetDisplayName(enumStatus);

                if (item.BookingLines.Any())
                {
                    var i = 0;
                    foreach (var line in item.BookingLines)
                    {
                        if (i == 0)
                        {
                            book.TotalTeetimeDisplay = line.Tee_Time.Value.ToString("HH:mm");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(book.TotalTeetimeDisplay) && !book.TotalTeetimeDisplay.Contains(line.Tee_Time.Value.ToString("HH:mm")))
                            {
                                book.TotalTeetimeDisplay += " - " + line.Tee_Time.Value.ToString("HH:mm");
                            }
                        }
                        i++;
                    }
                    book.CountPlayers = item.BookingLines.Count();
                }

                if (item.BookingSpecialRequests.Any())
                {
                    foreach (var line in item.BookingSpecialRequests)
                    {
                        book.SpecialNameServices += line.BookingOtherType?.Name + " : ";
                    }
                    book.SpecialNameServices = book.SpecialNameServices.Remove(book.SpecialNameServices.Length - 1, 1);
                    book.CountPlayers = item.BookingLines.Count();
                }

                list.Add(book);
            }

            return new PagingResponseEntityDTO<BookingDTO>
            {
                Count = paging.Count,
                Data = list
            };
        }

        public IEnumerable<PaymentCompare> GetCheckPayment(BookingFilterModel filter)
        {
            throw new NotImplementedException();
        }
    }




}
