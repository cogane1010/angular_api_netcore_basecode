using App.BookingOnline.Data.Models;
using App.BookingOnline.Data.Models.Common;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class ContactSupportConfiguration : IEntityTypeConfiguration<ContactSupport>
    {
        public void Configure(EntityTypeBuilder<ContactSupport> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");

            builder
              .Property(m => m.CourseCode)
              .HasMaxLength(50)
              .IsRequired();

            builder
            .Property(m => m.CourseName)
            .HasMaxLength(250)
            .IsRequired();

            builder
             .Property(m => m.PhoneNumber)
             .HasMaxLength(50)
             .IsRequired();

            builder
              .Property(m => m.Email)
              .HasMaxLength(50);

            builder
           .Property(m => m.Note)
           .HasMaxLength(50);

            builder
             .Property(m => m.Status);

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
                .ToTable("C_ContactSupport");
        }
    }


}