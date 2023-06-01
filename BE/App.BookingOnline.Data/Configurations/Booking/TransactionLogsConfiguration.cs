using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class TransactionLogsConfiguration : IEntityTypeConfiguration<TransactionLogs>
    {
        public void Configure(EntityTypeBuilder<TransactionLogs> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
                .Property(m => m.FileName)
                .IsRequired()
                .HasMaxLength(250);

            builder
                .Property(m => m.FilePath)
                .HasMaxLength(2500);

            builder
                .Property(m => m.LogName)
                .HasMaxLength(250);

            builder
                .Property(m => m.LogText)
                .HasMaxLength(5000);

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
                .ToTable("SB_TransactionLogs");
        }
    }

}