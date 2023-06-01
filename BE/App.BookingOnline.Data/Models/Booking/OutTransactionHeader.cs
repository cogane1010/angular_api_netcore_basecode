using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models
{
    public class OutTransactionHeader : BaseEntity, IEntity
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime DateTrans { get; set; }
        public string ApproverUserId { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveCooments { get; set; }
        public string Status { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<OutTransactionDetails> OutTransactionDetails { get; set; }
    }


    public class OutTransactionDetails : BaseEntity, IEntity
    {
        public Guid OutTransactionHeaderId { get; set; }
        public OutTransactionHeader OutTransactionHeader { get; set; }
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
        public string TraceId { get; set; }
        public string Payment_Detail { get; set; }
        public string Co_Code { get; set; }
        public string FT_Id { get; set; }
        public string Return_Acc { get; set; }
        public string Rc_code { get; set; }
        public string UserRc_code { get; set; }
        public bool IsActive { get; set; }
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
        // 01/06/2022
        public string CustomerId { get; set; }
        public string Charge_Type { get; set; }
        public decimal Charge_Amt { get; set; }
        public decimal Total_Amt { get; set; }
        public string RRN { get; set; }

        public int? StatusLine { get; set; }
        public DateTime? MoneyAtAcc { get; set; }
        public decimal? TotalMoneyAcc { get; set; }
        public string InMoneyUser { get; set; }
        public DateTime? MoneyChangeDate { get; set; }
        public string Refund_Ref { get; set; }
        public string Trans_type { get; set; }
        public decimal? Tien_hoan { get; set; }
    }

}