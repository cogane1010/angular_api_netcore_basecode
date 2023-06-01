using App.Core.Domain;
using System;

namespace App.BookingOnline.Data.Models.Reports
{
    public class TransactionDetailReportResult
    {
        public PagingResponseEntityDTO<TransactionDetailReportDTO> PagingData { get; set; }
        public decimal? TransAmt { get; set; }
        public decimal? CusCancelReturnBf24HAmt { get; set; }
        public decimal? NotReceivedAmt { get; set; }
        public decimal? TotalReceived { get; set; }

    }

    public class TransactionDetailReportDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }

        #region Data
        // Mã KH SB
        public string SeaBankCustomerCode { get; set; }

        // Tên KH SB
        public string SeaBankCustomerName { get; set; }

        // Mã đặt chỗ
        public string BookingCode { get; set; }

        // Sân Golf
        public string GolfOrgCode { get; set; }

        // Ngày đặt
        public DateTime? BookingDate { get; set; }

        // TK ghi co
        public string CreditAccount { get; set; }

        // CARD.type
        public string card_type { get; set; }

        // số card masking
        public string CardMaskingNo { get; set; }

        // Số TK debit
        public string DebitAccountNo { get; set; }

        // Nội dung giao dịch
        public string TransDesciption { get; set; }

        // Ngày giao dịch
        public DateTime? TransDate { get; set; }

        // Số tiền phát sinh
        public decimal? TransAmt { get; set; }

        // Hoàn tiền do khách hủy GD trước 24h
        public decimal? CusCancelReturnBf24HAmt { get; set; }

        // Số tiền chưa về
        public decimal? NotReceivedAmt { get; set; }

        // Tổng số tiền đã về
        public decimal? TotalReceived { get; set; }
        #endregion
    }
}
