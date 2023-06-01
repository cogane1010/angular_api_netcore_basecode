using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class LogDateFileOutConfiguration : IEntityTypeConfiguration<LogDateFileOut>
    {
        public void Configure(EntityTypeBuilder<LogDateFileOut> builder)
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
                .Property(m => m.Status)
                .IsRequired()
                .HasMaxLength(100);
            builder
               .Property(m => m.FileType)
               .IsRequired()
               .HasMaxLength(100);
            builder
                .ToTable("LogDateFileOut");
        }
    }


}