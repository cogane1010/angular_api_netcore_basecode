using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class BookingTransactionPaymentConfiguration : IEntityTypeConfiguration<BookingTransactionPayment>
    {
        public void Configure(EntityTypeBuilder<BookingTransactionPayment> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
                .Property(m => m.Traceid)
                .HasMaxLength(1000);

            builder
                .Property(m => m.OrgCode)
                .HasMaxLength(250);
            builder
               .Property(m => m.LinkCardAcctId)
               .HasMaxLength(1000);
            builder
              .Property(m => m.SourceType)
              .HasMaxLength(1000);
            builder
              .Property(m => m.AccountID)
              .HasMaxLength(1000);
            builder
              .Property(m => m.Company)
              .HasMaxLength(1000);
            builder
              .Property(m => m.AccountType)
              .HasMaxLength(1000);
            builder
              .Property(m => m.EmbosingName)
              .HasMaxLength(1000);
            builder
              .Property(m => m.CardNumber)
              .HasMaxLength(1000);
            builder
              .Property(m => m.CardProduct)
              .HasMaxLength(1000);
            builder
              .Property(m => m.CardProductDesc)
              .HasMaxLength(1000);
            builder
              .Property(m => m.CustomerId)
              .HasMaxLength(1000);
            builder
              .Property(m => m.Ftid)
              .HasMaxLength(2000);
            builder
              .Property(m => m.DebitAccount)
              .HasMaxLength(2000);
            builder
             .Property(m => m.CancelDescription)
             .HasMaxLength(2000);
            builder
            .Property(m => m.CancelStatus)
            .HasMaxLength(100);
            builder
            .Property(m => m.CancelUer)
            .HasMaxLength(1000);
            builder
            .Property(m => m.Status)
            .HasMaxLength(100);
            builder
            .Property(m => m.InMoneyUser)
            .HasMaxLength(500);
            builder
             .Property(m => m.SdkDescription)
             .HasMaxLength(2000);
            builder
            .Property(m => m.TransactionNo)
            .HasMaxLength(200);
            builder
             .Property(m => m.SdkRefundStatus)
             .HasMaxLength(100);
            builder
             .Property(m => m.SdkRefundCode)
             .HasMaxLength(100);
            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder.HasOne(x => x.Booking)
               .WithMany(x => x.TransactionPayments)
               .HasForeignKey(x => x.BookingId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
               .WithMany(x => x.TransactionPayments)
               .HasForeignKey(x => x.OrgId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("SB_BookingTransactionPayment");
        }
    }


}