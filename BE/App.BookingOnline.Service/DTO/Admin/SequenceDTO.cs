using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.BookingOnline.Service.DTO
{
    public class SequenceDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string DocumentType { get; set; }
        public string SequenceType { get; set; }
        public string Prefix { get; set; }
        public int? MaxLength { get; set; }
        public IEnumerable<SequenceLineDTO> SequenceLines { get; set; }

        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class SequenceLineDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public Guid App_Sequence_Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateId { get; set; }
        public int? MonthValue { get; set; }
        public int? YearValue { get; set; }
        public int SeqValue { get; set; }

        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public SequenceDTO Sequence { get; set; }
    }
}