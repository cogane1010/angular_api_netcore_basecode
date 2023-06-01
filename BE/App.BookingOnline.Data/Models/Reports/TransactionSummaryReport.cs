using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BookingOnline.Data.Models.Reports
{
    public class TransactionSummaryReport : BaseEntity, IEntity
    {
        // Mã KH SB
        public string SeaBankCustomerCode { get; set; }

        // Họ tên khách
        public string CustomerName { get; set; }

        #region Tổng số tiền giao dịch trong Tháng 
        // Số tiền giao dịch thành công
        public decimal? MonthReceivedAmt { get; set; }

        // Số tiền giao dịch chưa về TK
        public decimal? MonthNotReceivedAmt { get; set; }

        // Tổng số tiền giao dịch
        public decimal? MonthTotalAmt { get; set; }
        #endregion

        #region Lũy kế đến hiện tại
        // Số tiền giao dịch thành công
        public decimal? ReceivedAmt { get; set; }

        // Số tiền giao dịch chưa về TK
        public decimal? NotReceivedAmt { get; set; }

        // Tổng số tiền giao dịch
        public decimal? TotalAmt { get; set; }
        #endregion

        public int TotalRow { get; set; }
    }

}
