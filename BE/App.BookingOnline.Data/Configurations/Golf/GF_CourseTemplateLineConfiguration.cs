using App.BookingOnline.Data.Models.Golf;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations.Golf
{
    public class GF_CourseTemplateLineConfiguration : IEntityTypeConfiguration<GF_CourseTemplateLine>
    {
        public void Configure(EntityTypeBuilder<GF_CourseTemplateLine> builder)
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
               .IsRequired().HasDefaultValueSql("GETDATE()");
            builder
               .Property(m => m.Part)
               .IsRequired();
            builder
              .Property(m => m.OrgName)
              .HasMaxLength(250);
            builder
               .Property(m => m.GF_CourseTemplate_Id)
               .IsRequired();

            //builder.HasOne(x => x.Organization)
            // .WithMany(x => x.GF_CourseTemplateLine)
            // .HasForeignKey(x => x.C_Org_Id);
            builder.HasOne(x => x.Course)
             .WithMany(x => x.GF_CourseTemplateLine)
             .HasForeignKey(x => x.C_Course_Id);
            builder.HasOne(x => x.GF_CourseTemplate)
             .WithMany(x => x.GF_CourseTemplateLines)
             .HasForeignKey(x => x.GF_CourseTemplate_Id);

            builder
                .ToTable("GF_CourseTemplateLine");

            //builder.HasData(new GF_CourseTemplateLine
            //{
            //    Id = Guid.NewGuid(),
            //    GF_CourseTemplate_Id = new Guid("53db74b8-599c-48b2-af06-dafb6c666a38"),
            //    CreatedUser = "admin",
            //    CreatedDate = DateTime.Now,
            //    IsActive = true,
            //    Part = "part"
            //});
        }
    }
}
