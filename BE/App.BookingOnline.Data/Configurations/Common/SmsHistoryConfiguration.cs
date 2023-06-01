using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class SmsHistoryConfiguration : IEntityTypeConfiguration<SmsHistory>
    {
        public void Configure(EntityTypeBuilder<SmsHistory> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");

            builder
               .Property(m => m.CreatedUser)
               .HasMaxLength(500)
               .IsRequired();

            builder
               .Property(m => m.UpdatedUser)
               .HasMaxLength(500);

            builder
                .Property(m => m.Code)
                .IsRequired()
                .HasMaxLength(500);
            builder
              .Property(m => m.Mobilephone)
              .HasMaxLength(500);
            builder
                .Property(m => m.UserId)
                .HasMaxLength(500);

            builder
               .Property(m => m.SendDate)
               .IsRequired();

            builder
               .Property(m => m.Type)
               .HasMaxLength(100);

            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);

            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder
                .ToTable("SmsHistory");
        }
    }


}