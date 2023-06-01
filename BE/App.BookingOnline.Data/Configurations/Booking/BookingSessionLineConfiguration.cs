using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class BookingSessionLineConfiguration : IEntityTypeConfiguration<BookingSessionLine>
    {
        public void Configure(EntityTypeBuilder<BookingSessionLine> builder)
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


            builder.HasOne(x => x.BookingSession)
                .WithMany(x => x.BookingSessionLines)
                .HasForeignKey(x => x.BookingSessionsId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .ToTable("GF_BookingSessionLine");
        }
    }



}