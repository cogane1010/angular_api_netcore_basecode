using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Service.DTO
{
    public class OutTransactionHeaderDTO : IEntityDTO
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime DateTrans { get; set; }
        public string Status { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid Id { get; set; }
        public string CreatedUserName { get; set; }
        public string UpdatedUserUserName { get; set; }
        public string ApproverUserId { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveCooments { get; set; }
        public string ApproverUserName { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<OutTransactionDetailDTO> OutTransactionDetailDTO { get; set; } = new List<OutTransactionDetailDTO>();
    }

    public class OutTransactionDetailDTO : IEntityDTO
    {
        public Guid OutTransactionHeaderId { get; set; }       
        public string Phone_Number { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string SourceType { get; set; }
        public string PAN_NUMBER { get; set; }
        public string CustomerName { get; set; }
        public string DebitAcc { get; set; }
        public string CreditAcc { get; set; }
        public DateTime Trans_Date { get; set; }
        public DateTime Trans_Time { get; set; }
        public decimal Trans_Amt { get; set; }
        public string Refund_Ref { get; set; }
        public string Trans_type { get; set; }
        public decimal? Tien_hoan { get; set; }
        public string TraceId { get; set; }
        public string Payment_Detail { get; set; }
        public string Co_Code { get; set; }
        public string FT_Id { get; set; }
        public string Return_Acc { get; set; }
        public string Rc_code { get; set; }
        public string UserRc_code { get; set; }
        public string CustomerId { get; set; }
        public Guid OrganizationId { get; set; }
        public OrganizationDTO Organization { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid Id { get; set; }
        public string Trans_Id { get; set; }
        public int? StatusLine { get; set; }
        public string InMoneyUser { get; set; }
        public decimal TotalMoneyAcc { get; set; }
        public DateTime? MoneyAtAcc { get; set; }
        public bool IsActive { get; set; }
        public Data.Models.Booking Booking { get; set; }
    }

    public class NotConfirmMoneyList
    {
        public string Id { get; set; }
        public DateTime? DateId { get; set; }
        public int NumberNotConfirm { get; set; }
        public decimal TotalMoney { get; set; }
    }
}
