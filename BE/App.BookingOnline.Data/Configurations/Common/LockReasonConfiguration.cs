using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class LockReasonConfiguration : IEntityTypeConfiguration<LockReason>
    {
        public void Configure(EntityTypeBuilder<LockReason> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(m => m.Code)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(500);
            builder
                .Property(m => m.NameEn)
                .HasMaxLength(500);

            builder.HasOne(x => x.Organization)
              .WithMany(x => x.LockReasons)
              .HasForeignKey(x => x.C_Org_Id)
              .OnDelete(DeleteBehavior.Restrict);
            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);
            builder
                .ToTable("C_LockReason");
        }
    }

    
}