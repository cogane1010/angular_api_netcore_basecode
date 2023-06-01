using App.BookingOnline.Data.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class PromotionCourseConfiguration : IEntityTypeConfiguration<M_Promotion_Course>
    {
        public void Configure(EntityTypeBuilder<M_Promotion_Course> builder)
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

            builder.HasOne(x => x.Promotion)
              .WithMany(x => x.PromotionCourse)
              .HasForeignKey(x => x.M_Promotion_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
              .WithMany(x => x.PromotionCourse)
              .HasForeignKey(x => x.C_Course_Id)
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("M_Promotion_Course");
        }
    }


}