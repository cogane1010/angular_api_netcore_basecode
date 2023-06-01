using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class BookingLineConfiguration : IEntityTypeConfiguration<BookingLine>
    {
        public void Configure(EntityTypeBuilder<BookingLine> builder)
        {
            builder
                .HasKey(m => m.Id);
            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);
            builder.Property(x => x.CardNo)
               .HasMaxLength(50);
            builder
                .Property(m => m.DateId)
                .HasColumnType("date");

            builder.Property(x => x.Caddie_No)
                .HasMaxLength(50);

            builder.Property(x => x.CustomerFullName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.MobilePhone)
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .HasMaxLength(255);

            builder.Property(x => x.Description)
              .HasMaxLength(2000);
            builder.Property(x => x.Golf_CusGroupCode)
              .HasMaxLength(100);
            builder.Property(x => x.CashierName)
              .HasMaxLength(2000);
            builder.Property(x => x.GolfBag)
              .HasMaxLength(2000);

            builder.Property(x => x.BookingStatus)
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(x => x.Part)
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(x => x.UpdateNoShow_UserName)
              .HasMaxLength(50);

            builder.Property(x => x.Public_Price)
               .HasPrecision(18, 2);
            builder.Property(x => x.Promotion_Price)
               .HasPrecision(18, 2);
            builder.Property(x => x.Net_Ammount)
               .HasPrecision(18, 2);
            builder.Property(x => x.Discount_Amount)
               .HasPrecision(18, 2);
            builder.Property(x => x.Discount_Value)
              .HasPrecision(18, 2);
            builder.Property(x => x.Total_Amount)
               .HasPrecision(18, 2);
            builder.Property(x => x.Deposit_Amount)
               .HasPrecision(18, 2);

            builder.Property(x => x.Golf_Price_Description)
                .HasMaxLength(2000);
            builder.Property(x => x.Golf_Promotion_Id)
               .HasMaxLength(50);
            builder.Property(x => x.Golf_Promotion_Name)
               .HasMaxLength(50);

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.BookingLines)
                .HasForeignKey(x => x.GF_Booking_Id)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(x => x.CustomerGroup)
               .WithMany(x => x.BookingLines)
               .HasForeignKey(x => x.MB_CustomerGroup_Id)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("GF_BookingLine");
        }
    }
}