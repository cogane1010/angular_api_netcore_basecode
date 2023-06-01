using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class BookingSpecialRequestConfiguration : IEntityTypeConfiguration<BookingSpecialRequest>
    {
        public void Configure(EntityTypeBuilder<BookingSpecialRequest> builder)
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
                .HasMaxLength(50);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(50);
            builder.Property(x => x.Description)
             .HasMaxLength(2000);

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.BookingSpecialRequests)
                .HasForeignKey(x => x.GF_Booking_Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BookingOtherType)
               .WithMany(x => x.BookingSpecialRequests)
               .HasForeignKey(x => x.C_Booking_OtherType_Id)
               .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Organization)
               .WithMany(x => x.BookingSpecialRequests)
               .HasForeignKey(x => x.C_Org_Id)
               .OnDelete(DeleteBehavior.Restrict);

           

   
            builder
                .ToTable("GF_Booking_SpecialRequest");
        }
    }
}