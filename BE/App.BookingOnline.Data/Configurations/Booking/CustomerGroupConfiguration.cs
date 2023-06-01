using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class CustomerGroupConfiguration : IEntityTypeConfiguration<CustomerGroup>
    {
        public void Configure(EntityTypeBuilder<CustomerGroup> builder)
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
                .Property(m => m.CreatedDate)
                .IsRequired().HasDefaultValueSql("GETDATE()");
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(500);
            builder
                .Property(m => m.NameEn)
                .HasMaxLength(500);
            builder.HasOne(x => x.Organization)
               .WithMany(x => x.CustomerGroups)
               .HasForeignKey(x => x.C_Org_Id)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("MB_CustomerGroup");
        }
    }


    public class CustomerGroupMappingConfiguration : IEntityTypeConfiguration<CustomerGroupMapping>
    {
        public void Configure(EntityTypeBuilder<CustomerGroupMapping> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
                .Property(m => m.Golf_Code)
                .IsRequired()
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

            builder.HasOne(x => x.CustomerGroup)
              .WithMany(x => x.CustomerGroupMappings)
              .HasForeignKey(x => x.MB_CustomerGroup_Id);

            builder
                .ToTable("MB_CustomerGroup_Mapping");
        }
    }


}