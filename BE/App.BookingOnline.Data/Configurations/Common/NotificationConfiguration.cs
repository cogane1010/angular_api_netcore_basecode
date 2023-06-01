using App.BookingOnline.Data.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<GF_Notification>
    {
        public void Configure(EntityTypeBuilder<GF_Notification> builder)
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
                .HasMaxLength(250);
            builder
                .Property(m => m.Message_Title)
                .HasMaxLength(100);
            builder
                .Property(m => m.Message_Type)
                .HasMaxLength(100);
            builder
                .Property(m => m.Message_TitleEn)
                .HasMaxLength(100);
           
            builder
                .Property(m => m.Img_Url)
                .HasMaxLength(4000);

            builder.HasOne(x => x.Organization)
              .WithMany(x => x.Notification)
              .HasForeignKey(x => x.C_Org_Id)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);
            builder
                .ToTable("GF_Notification");
        }
    }


}