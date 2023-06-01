using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class TeeSheetLockConfiguration : IEntityTypeConfiguration<TeeSheetLock>
    {
        public void Configure(EntityTypeBuilder<TeeSheetLock> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");

            builder
               .Property(m => m.Description)
               .HasMaxLength(255);
            builder
              .Property(m => m.StartDate)
              .HasColumnType("date")
              .IsRequired();
            builder
             .Property(m => m.EndDate)
             .HasColumnType("date")
             .IsRequired();
            builder
               .Property(m => m.LockStatus)
               .HasMaxLength(50)
               .IsRequired();
            builder
               .Property(m => m.LockType)
               .HasMaxLength(50);

            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");

            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);
            builder.HasOne(x => x.Organization)
              .WithMany(x => x.TeeSheetLocks)
              .HasForeignKey(x => x.C_Org_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LockReason)
             .WithMany(x => x.TeeSheetLocks)
             .HasForeignKey(x => x.C_LockReason_Id)
             .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("GF_TeeSheetLock");
        }
    }


    public class TeeSheetLockLineConfiguration : IEntityTypeConfiguration<TeeSheetLockLine>
    {
        public void Configure(EntityTypeBuilder<TeeSheetLockLine> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
              .Property(m => m.Description)
              .HasMaxLength(255);
            builder
              .Property(m => m.DOW)
              .HasMaxLength(20);
            builder
             .Property(m => m.FlightSeq)
             .HasMaxLength(10);
            builder
              .Property(m => m.StartTime)
              .HasMaxLength(5);
            builder
              .Property(m => m.EndTime)
              .HasMaxLength(5);

            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);


            builder.HasOne(x => x.TeeSheetLock)
             .WithMany(x => x.TeeSheetLockLines)
             .HasForeignKey(x => x.GF_TeeSheetLock_Id)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Course)
              .WithMany(x => x.TeeSheetLockLines)
              .HasForeignKey(x => x.C_Course_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("GF_TeeSheetLockLine");
        }
    }

}