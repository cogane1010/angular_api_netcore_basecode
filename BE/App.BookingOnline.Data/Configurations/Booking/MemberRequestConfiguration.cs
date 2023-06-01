using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class MemberRequestConfiguration : IEntityTypeConfiguration<MemberRequest>
    {
        public void Configure(EntityTypeBuilder<MemberRequest> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(m => m.Request_Date)
                .IsRequired()
                .HasColumnType("datetime");
            builder
                .Property(m => m.FirstName)
                .HasMaxLength(255);
            builder
                .Property(m => m.LastName)
                .HasMaxLength(255);
            builder
                .Property(m => m.FullName)
                .HasMaxLength(255);
            builder
                .Property(m => m.MobilePhone)
                .IsRequired()
                .HasMaxLength(255);
            builder
                .Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder
               .Property(m => m.Description)
               .HasMaxLength(2000);
            builder
               .Property(m => m.UserId)
               .HasMaxLength(50)
               .IsRequired();

            builder
               .Property(m => m.Request_Status)
               .IsRequired()
               .HasMaxLength(50);

            builder
               .Property(m => m.Responsed_Date)
               .HasColumnType("date");

            builder
              .Property(m => m.Responsed_User)
              .HasMaxLength(255);

            builder
              .Property(m => m.Responsed_Description)
              .HasMaxLength(255);
            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");

            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder.HasOne(x => x.Organization)
              .WithMany(x => x.MemberRequests)
              .HasForeignKey(x => x.C_Org_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("MB_MemberRequest");
        }
    }

}