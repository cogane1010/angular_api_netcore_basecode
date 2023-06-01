using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class UserBankInfoConfiguration : IEntityTypeConfiguration<UserBankInfo>
    {
        public void Configure(EntityTypeBuilder<UserBankInfo> builder)
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
                .Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(500);
            builder
                .Property(m => m.Start_Month)
                .IsRequired().HasMaxLength(500);
            builder
                .Property(m => m.Start_Month)
                .IsRequired().HasMaxLength(500);
            builder
                .Property(m => m.Expire_Month)
                .IsRequired().HasMaxLength(500);
            builder
                .Property(m => m.Expire_Year)
                .IsRequired().HasMaxLength(500); ;

            builder
                .Property(m => m.Cvv)
                .HasMaxLength(10);

            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);

            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder.HasOne(x => x.PaymentType)
              .WithMany(x => x.UserBankInfos)
              .HasForeignKey(x => x.C_PaymentType_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("UserBankInfo");
        }
    }


}