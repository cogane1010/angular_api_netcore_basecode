using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class FtpTransJobHistoryConfiguration : IEntityTypeConfiguration<FtpTransJobHistory>
    {
        public void Configure(EntityTypeBuilder<FtpTransJobHistory> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
                .Property(m => m.SeabankFilePath)
                .IsRequired()
                .HasMaxLength(500);
            builder
                .Property(m => m.FileName)
                .IsRequired()
                .HasMaxLength(500);
            builder
                .Property(m => m.FilePath)
                .IsRequired()
                .HasMaxLength(3000);
            builder
               .Property(m => m.UserId)               
               .HasMaxLength(100);
            builder
               .Property(m => m.Status)
               .IsRequired()
               .HasMaxLength(50);
            builder
                .Property(m => m.DescriptionError)                
                .HasMaxLength(4000);
            builder.Property(m => m.DateRun)
                .HasColumnType("datetime");
            builder.Property(m => m.DateId)
                .HasColumnType("datetime");
            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder
                .ToTable("J_FtpTransJobHistory");
        }
    }


}