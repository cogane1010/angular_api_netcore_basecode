using App.BookingOnline.Data.Models;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.BookingOnline.Service.DTO
{
    public class TeeSheetLockDTO : IEntityDTO
    {
        public Guid C_Org_Id { get; set; }
        public string OrganizationName { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Guid C_LockReason_Id { get; set; }
        public string LockReasonName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LockStatus { get; set; }
        public string LockType { get; set; }
        public Guid Id { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<TeeSheetLockLineDTO> TeeSheetLockLines { get; set; }
    }

    public class TeeSheetLockLineDTO : IEntityDTO
    {
        public Guid C_Org_Id { get; set; }
        public bool IsActive { get; set; }

        public Guid? GF_TeeSheetLock_Id { get; set; }
        public Guid C_Course_Id { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string DOW { get; set; }

        public int? StartTee { get; set; }
        public string FlightSeq { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime? StartTimeValue { get; set; }
        public DateTime? EndTimeValue { get; set; }
        public Guid Id { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}