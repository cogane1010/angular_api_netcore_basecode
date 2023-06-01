using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
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

            builder.Property(x => x.BookingCode)
                .HasMaxLength(50);
            builder.Property(x => x.Device_Id)
                .HasMaxLength(50);
            builder.Property(x => x.UserId)
                .HasMaxLength(450)
                .IsRequired();
            builder
                .Property(m => m.DateId)
                .HasColumnType("date");

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.BookingType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Confirm_User)
                .HasMaxLength(50);

            builder.Property(x => x.Confirm_Type)
                .HasMaxLength(50);
            builder.Property(x => x.PromotionName)
                .HasMaxLength(1000);

            builder.Property(x => x.BookedCardNo)
                .HasMaxLength(500);
            builder.Property(x => x.MemberType)
                .HasMaxLength(500);
            builder.Property(x => x.TeeTimeDisplay)
                .HasMaxLength(500);
            builder.Property(x => x.AccountName)
                .HasMaxLength(200);
            builder.Property(x => x.AccountFullName)
                .HasMaxLength(500);

            builder.Property(x => x.PaymentSbId)
                .HasMaxLength(1000);
            builder.Property(x => x.PaymentSbType)
                .HasMaxLength(500);

            builder.Property(x => x.Confirm_Description)
              .HasMaxLength(2000);
            builder.Property(x => x.Cancel_User)
               .HasMaxLength(50);
            builder.Property(x => x.Cancel_Description)
                .HasMaxLength(2000);
            builder.Property(x => x.UpdateNoShow_UserName)
              .HasMaxLength(50);

            builder.HasOne(x => x.CancelReason)
               .WithMany(x => x.Bookings)
               .HasForeignKey(x => x.Cancel_Reason_Id)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Promotion)
              .WithMany(x => x.Bookings)
              .HasForeignKey(x => x.M_Promotion_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
              .WithMany(x => x.Bookings)
              .HasForeignKey(x => x.C_Course_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
              .WithMany(x => x.Bookings)
              .HasForeignKey(x => x.C_Org_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BookingSession)
               .WithMany(x => x.Bookings)
               .HasForeignKey(x => x.GF_Booking_Session_Id)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AppUser)
              .WithMany(x => x.Bookings)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentType)
              .WithMany(x => x.Bookings)
              .HasForeignKey(x => x.PaymentTypeId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("GF_Booking");
        }
    }
}
