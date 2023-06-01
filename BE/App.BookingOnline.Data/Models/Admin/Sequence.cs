using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.BookingOnline.Data.Models
{
    public class Sequence : BaseEntity, IEntity
    {
        public bool IsActive { get; set; }
        public string DocumentType { get; set; }
        public string SequenceType { get; set; }
        public string Prefix { get; set; }
        public int? MaxLength { get; set; }
        public IEnumerable<SequenceLine> SequenceLines { get; set; }

    }

    public class SequenceLine : BaseEntity, IEntity
    {
        public Guid App_Sequence_Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateId { get; set; }
        public int? MonthValue { get; set; }
        public int? YearValue { get; set; }
        public int SeqValue { get; set; }
        public Sequence Sequence { get; set; }
    }
}