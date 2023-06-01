using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{
    
    public class LockReason : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public bool IsActive { get; set; }
        public Organization Organization { get; set; }
        public IEnumerable<TeeSheetLock> TeeSheetLocks { get; set; }
    }


}