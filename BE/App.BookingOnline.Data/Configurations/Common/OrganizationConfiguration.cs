using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class OrganizationTypeConfiguration : IEntityTypeConfiguration<OrganizationType>
    {
        public void Configure(EntityTypeBuilder<OrganizationType> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(m => m.Code)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder
                .ToTable("C_OrgType");
        }
    }

    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(m => m.Code)
                .IsRequired()
                .HasMaxLength(50);
            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(250);
            builder
                .Property(m => m.IsActive)
                .IsRequired();
            builder
                .Property(m => m.IsSummary)
                .IsRequired();
            builder
                .Property(m => m.C_OrgType_Id)
                .IsRequired();
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
               .Property(m => m.PhoneSupport)
               .HasMaxLength(50);
            builder
               .Property(m => m.IconUrl)
               .HasMaxLength(1000);
            builder
               .Property(m => m.ImageUrl)
               .HasMaxLength(1000);
            builder
               .Property(m => m.EmailSupport)
               .HasMaxLength(200);
            builder
               .Property(m => m.TimeSupport)
               .HasMaxLength(1000);
            builder
               .Property(m => m.NoteSupport)
               .HasMaxLength(4000);
            builder.HasOne(x => x.OrganizationType)
              .WithMany(x => x.Organizations)
              .HasForeignKey(x => x.C_OrgType_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("C_Org");
            

    
        }
    }
    public class OrganizationInfoConfiguration : IEntityTypeConfiguration<OrganizationInfo>
    {
        public void Configure(EntityTypeBuilder<OrganizationInfo> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
                .Property(m => m.TaxCode)
                .HasMaxLength(50);
            builder
                .Property(m => m.Telephone)
                .HasMaxLength(50);
            builder
                .Property(m => m.Fax)
                .HasMaxLength(50);
            builder
              .Property(m => m.Website)
              .HasMaxLength(250);
            builder
                .Property(m => m.Email)
                .HasMaxLength(250);
            builder
                .Property(m => m.InvoiceName)
                .HasMaxLength(500);
            builder
                .Property(m => m.InvoiceAddress)
                .HasMaxLength(500);
            builder
                .Property(m => m.InvoiceBankAccount)
                .HasMaxLength(50);
            builder
               .Property(m => m.InvoiceBankName)
               .HasMaxLength(250);
            builder
              .Property(m => m.LogoUrl)
              .HasMaxLength(250);

            builder
              .Property(m => m.ShortAddress)
              .HasMaxLength(250);
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
               .WithMany(x => x.OrganizationInfos)
               .HasForeignKey(x => x.C_Org_Id);
            builder
                .ToTable("C_OrgInfo");

        }
    }
}