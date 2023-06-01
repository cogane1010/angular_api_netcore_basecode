using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{ 
    public class TeeSheetLock : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public Organization Organization { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Guid C_LockReason_Id { get; set; }
        public LockReason LockReason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LockStatus { get; set; }
        public string LockType { get; set; }

        public List<TeeSheetLockLine> TeeSheetLockLines { get; set; }
    }

    public class TeeSheetLockLine : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public bool IsActive { get; set; }

        public Guid GF_TeeSheetLock_Id { get; set; }
        public TeeSheetLock TeeSheetLock { get; set; }
        public Guid C_Course_Id { get; set; }
        public Course Course { get; set; }
        public string Description { get; set; }
        public string DOW { get; set; }

        public int? StartTee { get; set; }
        public string FlightSeq { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime? StartTimeValue { get; set; }
        public DateTime? EndTimeValue { get; set; }

        [NotMapped]
        public bool? IsDeleted { get; set; }
    }

}