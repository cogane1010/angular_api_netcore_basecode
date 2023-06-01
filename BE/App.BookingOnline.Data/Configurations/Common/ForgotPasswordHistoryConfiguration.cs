using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class ForgotPasswordHistoryHistoryConfiguration : IEntityTypeConfiguration<ForgotPassworkHistory>
    {
        public void Configure(EntityTypeBuilder<ForgotPassworkHistory> builder)
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
              .Property(m => m.UserId)
              .HasMaxLength(500);

            builder
                .Property(m => m.Email)
                .HasMaxLength(500);
            builder
                .Property(m => m.Telephone)
                .HasMaxLength(500);

            builder
                .ToTable("ForgotPasswordHistory");
        }
    }


}