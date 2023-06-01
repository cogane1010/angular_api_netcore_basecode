using App.BookingOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
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
                .Property(m => m.PhoneSupport)
                .HasMaxLength(250);
            builder
                .Property(m => m.IconUrl)
                .HasMaxLength(1000);
            builder
                .Property(m => m.ImageUrl)
                .HasMaxLength(1000);
            builder
                .Property(m => m.EmailSupport)
                .HasMaxLength(250);
            builder
                .Property(m => m.NoteSupport)
                .HasMaxLength(2000);
            builder
                .Property(m => m.TimeSupport)
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

            builder.HasOne(x => x.Organization)
              .WithMany(x => x.Courses)
              .HasForeignKey(x => x.C_Org_Id)
              .OnDelete(DeleteBehavior.Restrict);
            builder
                .ToTable("C_Course");
        }
    }


}