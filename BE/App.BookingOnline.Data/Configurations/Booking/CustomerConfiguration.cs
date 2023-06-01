using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
                .Property(m => m.CustomerCode)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(m => m.FirstName)
                .HasMaxLength(250);
            builder
                .Property(m => m.LastName)
                .HasMaxLength(250);
            builder
                .Property(m => m.Img_Url)
                .HasMaxLength(1000);
            builder
                .Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(250);
            builder
               .Property(m => m.LowerFullName)
               .IsRequired()
               .HasMaxLength(250);
            builder
               .Property(m => m.Email)
               .IsRequired()
               .HasMaxLength(250);
            builder
               .Property(m => m.FcmTokenDevice)
               .HasMaxLength(3000);
            builder
               .Property(m => m.Languague)
               .HasMaxLength(100);
            builder
               .Property(m => m.Appversion)
               .HasMaxLength(100);
            builder
               .Property(m => m.VnVersion)
               .HasMaxLength(100);
            builder
               .Property(m => m.EnVersion)
               .HasMaxLength(100);
            builder
               .Property(m => m.MobilePhone)
               .IsRequired()
               .HasMaxLength(250);

            builder
                .Property(m => m.DOB)
                .HasColumnType("date");

            builder
               .Property(m => m.UserId)
               .IsRequired()
               .HasMaxLength(250);
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
                .ToTable("MB_Customer");
        }
    }


}