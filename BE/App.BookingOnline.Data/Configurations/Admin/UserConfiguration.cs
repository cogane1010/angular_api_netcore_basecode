using App.BookingOnline.Data.Models;
using App.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
               .Property(m => m.UserId)
               .HasMaxLength(50);

            builder
                .Property(m => m.UserName)
                .IsRequired()
                .HasMaxLength(250);
            builder
               .Property(m => m.FullName)
               .IsRequired()
               .HasMaxLength(250);
            builder
               .Property(m => m.Email)
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
            builder.HasOne(x => x.Organization)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.C_Org_Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("App_User");

            builder.HasData(new User
            {
                Id = new Guid("e45a2214-25aa-4f42-8d91-5aee48ab1c20"),
                UserName = "admin",
                FullName = "Admin",
                IsActive= true,
                Email = "admin@brggroup.vn",
            });
        }
    }
}