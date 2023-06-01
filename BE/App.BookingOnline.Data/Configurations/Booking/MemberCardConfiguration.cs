using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class MemberCardConfiguration : IEntityTypeConfiguration<MemberCard>
    {
        public void Configure(EntityTypeBuilder<MemberCard> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(m => m.Golf_MemberCardId)
                .HasMaxLength(50);
            builder
                .Property(m => m.Golf_IDNo)
                .HasMaxLength(50);
            builder
                .Property(m => m.Golf_CardNo)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .Property(m => m.Golf_FullName)
                .IsRequired()
                .HasMaxLength(255);
            builder
                .Property(m => m.Golf_Mobilephone)
                .HasMaxLength(255);
            builder
                .Property(m => m.Golf_Email)
                .HasMaxLength(255);
            
            builder
                .Property(m => m.Golf_Address)
                .HasMaxLength(500);
            builder
                .Property(m => m.Golf_CardType)
                .HasMaxLength(500);
            builder
               .Property(m => m.Golf_Effective_Date)
               .HasColumnType("datetime");
            builder
               .Property(m => m.Golf_Expire_Date)
               .HasColumnType("datetime");
            builder
               .Property(m => m.Golf_DOB)
               .HasColumnType("date");
            builder
              .Property(m => m.Golf_CardStatus)
              .IsRequired()
              .HasMaxLength(50);
            builder
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");

            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder.HasOne(x => x.Customer)
               .WithMany(x => x.MemberCards)
               .HasForeignKey(x => x.MB_Customer_Id)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
              .WithMany(x => x.MemberCards)
              .HasForeignKey(x => x.C_Org_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CustomerGroup)
             .WithMany(x => x.MemberCards)
             .HasForeignKey(x => x.MB_CustomerGroup_Id)
             .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("MB_MemberCard");
        }
    }



    public class MemberCardCourseConfiguration : IEntityTypeConfiguration<MemberCardCourse>
    {
        public void Configure(EntityTypeBuilder<MemberCardCourse> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
               .Property(m => m.Description)
               .HasMaxLength(2000);
            builder
               .Property(m => m.GolfCode)
               .HasMaxLength(200);
            builder
               .Property(m => m.GolfName)
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
            builder.HasOne(x => x.MemberCard)
              .WithMany(x => x.MemberCardCourses)
              .HasForeignKey(x => x.MC_MemberCard_Id);

            builder.HasOne(x => x.CustomerGroup)
              .WithMany(x => x.MemberCardCourses)
              .HasForeignKey(x => x.MB_CustomerGroup_Id);

            builder.HasOne(x => x.Course)
              .WithMany(x => x.MemberCardCourses)
              .HasForeignKey(x => x.C_Course_Id);

            builder
               .ToTable("MB_MemberCard_Course");


        }
    }

}