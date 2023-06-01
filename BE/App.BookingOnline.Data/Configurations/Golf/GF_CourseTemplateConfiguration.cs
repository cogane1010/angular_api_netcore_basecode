using App.BookingOnline.Data.Models.Golf;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace App.BookingOnline.Data.Configurations.Golf
{
    public class GF_CourseTemplateConfiguration : IEntityTypeConfiguration<GF_CourseTemplate>
    {
        public void Configure(EntityTypeBuilder<GF_CourseTemplate> builder)
        {
            builder
                .HasKey(m => m.Id);
            builder
                .Property(m => m.Id)
                .HasDefaultValueSql("NEWID()");
            builder
                .Property(m => m.IsActive)
                .IsRequired();
            builder
               .Property(m => m.CreatedUser)
               .HasMaxLength(500)
               .IsRequired();
            builder
               .Property(m => m.UpdatedUser)
               .HasMaxLength(500);
            builder
               .Property(m => m.CreatedDate)
               .IsRequired().HasDefaultValueSql("GETDATE()"); ;
            builder
               .Property(m => m.Code)
               .HasMaxLength(50);
            builder
               .Property(m => m.OrgName)
               .HasMaxLength(250);
            builder
               .Property(m => m.Name)
               .HasMaxLength(250);
            builder
               .Property(m => m.DOW)
               .HasMaxLength(50);
            builder
               .Property(m => m.Status)
               .HasMaxLength(50);

            builder.HasOne(x => x.Organization)
             .WithMany(x => x.GF_CourseTemplates)
             .HasForeignKey(x => x.C_Org_Id);

            builder
                .ToTable("GF_CourseTemplate");
                

            //builder.HasData(new GF_CourseTemplate
            //{
            //    Id = new Guid("53db74b8-599c-48b2-af06-dafb6c666a38"),
            //    Name = "CourseTemplate",
            //    CreatedUser = "admin",
            //    CreatedDate = DateTime.Now,
            //    IsActive = true,                
            //});
        }
    }
}
