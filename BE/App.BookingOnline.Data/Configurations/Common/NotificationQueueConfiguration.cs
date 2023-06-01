using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class NotificationQueueConfiguration : IEntityTypeConfiguration<NotificationQueue>
    {
        public void Configure(EntityTypeBuilder<NotificationQueue> builder)
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
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(500);
            builder
              .Property(m => m.TitleEn)
              .HasMaxLength(500);
            builder
                .Property(m => m.LinkId)
                .HasMaxLength(500);
            builder
                .Property(m => m.UserId)
                .HasMaxLength(500);
            builder
                .Property(m => m.Img_url)
                .HasMaxLength(1000);
            builder
               .Property(m => m.NotificationType)
               .HasMaxLength(100);

            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);

            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder
                .ToTable("NotificationQueue");
        }
    }


}