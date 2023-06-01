using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class SequenceConfiguration : IEntityTypeConfiguration<Sequence>
    {
        public void Configure(EntityTypeBuilder<Sequence> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder.Property(m => m.DocumentType).HasMaxLength(50);
            builder.Property(m => m.SequenceType).HasMaxLength(50);
            builder.Property(m => m.Prefix).HasMaxLength(50);
            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);
            builder.HasData(new Sequence
            {
                Id = new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                CreatedDate = DateTime.Now,
                CreatedUser = "admin",
                DocumentType = "CUSTOMERCODE",
                Prefix = "GA",
                MaxLength = 6,
                IsActive = true,
            });
            builder
                .ToTable("App_Sequence");
        }
    }


    public class SequenceLineConfiguration : IEntityTypeConfiguration<SequenceLine>
    {
        public void Configure(EntityTypeBuilder<SequenceLine> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder.Property(m => m.DateId).HasColumnType("date");

            builder.HasOne(x => x.Sequence)
                .WithMany(x => x.SequenceLines)
                .HasForeignKey(x => x.App_Sequence_Id);
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);
            builder.HasData(new SequenceLine
            {
                Id = new Guid("59bfc647-ef93-4af4-aaf0-4c49272a975b"),
                App_Sequence_Id = new Guid("efb6f443-337c-4f7e-afa9-328bec063f21"),
                CreatedDate = DateTime.Now,
                CreatedUser = "admin",
                YearValue = DateTime.Now.Year,
                SeqValue = 0,
                IsActive = true,
            });
            builder
                .ToTable("App_SequenceLine");
        }
    }
}