using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class InTransactionHeaderConfiguration : IEntityTypeConfiguration<InTransactionHeader>
    {
        public void Configure(EntityTypeBuilder<InTransactionHeader> builder)
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
               .Property(m => m.Status)
               .HasMaxLength(250);

            builder
                .Property(m => m.FilePath)
                .HasMaxLength(2500);

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
                .Property(m => m.ApproverUserId)
                .HasMaxLength(250);

            builder
                .Property(m => m.ApproveCooments)
                .HasMaxLength(2500);


            builder
                .ToTable("SB_InTransactionHeader");
        }
    }

    public class InTransactionDetailsConfiguration : IEntityTypeConfiguration<InTransactionDetails>
    {
        public void Configure(EntityTypeBuilder<InTransactionDetails> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
                .Property(m => m.Phone_Number)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.OrgCode)
                .HasMaxLength(50);

            builder
               .Property(m => m.OrgName)
               .HasMaxLength(255);

            builder
               .Property(m => m.SourceType)
               .HasMaxLength(50);

            builder
              .Property(m => m.PAN_NUMBER)
              .HasMaxLength(100);

            builder
             .Property(m => m.CustomerName)
             .HasMaxLength(255);

            builder
             .Property(m => m.DebitAcc)
             .HasMaxLength(50);

            builder
             .Property(m => m.CreditAcc)
             .HasMaxLength(50);

            builder
             .Property(m => m.TraceId)
             .HasMaxLength(50);

            builder
             .Property(m => m.Payment_Detail)
             .HasMaxLength(1000);

            builder
             .Property(m => m.Co_Code)
             .HasMaxLength(50);

            builder
             .Property(m => m.FT_Id)
             .HasMaxLength(50);

            builder
             .Property(m => m.Return_Acc)
             .HasMaxLength(50);
            builder
             .Property(m => m.Trans_type)
             .HasMaxLength(50);
            builder
             .Property(m => m.Refund_Ref)
             .HasMaxLength(500);
            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder.HasOne(x => x.InTransactionHeader)
               .WithMany(x => x.InTransactionDetails)
               .HasForeignKey(x => x.InTransactionHeaderId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
              .WithMany(x => x.InTransactionDetails)
              .HasForeignKey(x => x.OrganizationId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("SB_InTransactionDetails");
        }
    }
}