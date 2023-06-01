using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class BookingSessionConfiguration : IEntityTypeConfiguration<BookingSession>
    {
        public void Configure(EntityTypeBuilder<BookingSession> builder)
        {
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
                .HasMaxLength(50)
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

            builder.HasOne(x => x.Organization)
                .WithMany(x => x.BookingSessions)
                .HasForeignKey(x => x.C_Org_Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.BookingSessions)
                .HasForeignKey(x => x.C_Course_Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.M_Promotion)
                .WithMany(x => x.BookingSessions)
                .HasForeignKey(x => x.M_Promotion_Id)
                .OnDelete(DeleteBehavior.Restrict);

            

            builder
                .ToTable("GF_BookingSession");
        }
    }



}