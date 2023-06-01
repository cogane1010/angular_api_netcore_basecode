using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Data.Models.Reports
{
    public class TransactionMonthlyReport : BaseEntity, IEntity
    {
        // Mã KH SB
        public string SeaBankCustomerCode { get; set; }

        // Họ tên khách
        public string CustomerName { get; set; }

        // Sân Golf
        public string GolfCourseCode { get; set; }

        // Số tiền chưa về kỳ trước chuyển sang - 9
        public decimal? NotReceivedAmtAfterPrevMonth { get; set; }

        // Số tiền khách book trong tháng - 10
        public decimal? MonthBookedAmt { get; set; }

        // Số tiền hoàn trả của giao dịch kỳ trước - 11
        public decimal? MonthTransAmtForPrevMonths { get; set; }

        // Số tiền về của giao dịch trong tháng - 12
        public decimal? MonthTransAmt { get; set; }

        // Tổng số tiền về trong kỳ - 13 = (11 + 12)
        public decimal? TotalMonthAmt { get; set; }

        // Số dư tiền chưa về của kỳ trước (lũy kế) - 14 = 9 - 11
        public decimal? RemainNotReceivedAmtAfterPrevMonth { get; set; }

        // Số tiền chưa về p/s trong tháng - 15 = 10 - 12
        public decimal? MonthNotReceivedAmt { get; set; }

        // Tổng số tiền chưa về đến kỳ này 16 = 14 + 15
        public decimal? TotalNotReceivedAmt { get; set; }

        public int TotalRow { get; set; }
    }

}
