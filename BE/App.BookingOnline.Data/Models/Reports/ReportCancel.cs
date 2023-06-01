using App.Core.Domain;
using System;

public class ReportCancelBooking : BaseEntity, IEntity
{
    public Guid Id { get; set; }
    public DateTime? Tee_Time { get; set; }
    public string BookingCode { get; set; }
    public string CourseName { get; set; }
    public string CustomerFullName { get; set; }
    public string Cancel_Description { get; set; }
    public string CancelName { get; set; }
    public DateTime? Cancel_Time { get; set; }
    public int TotalRow { get; set; }
    public int Totalplay { get; set; }
    public decimal? NonRefundedFee { get; set; }
    public decimal? DiffHour { get; set; }


}