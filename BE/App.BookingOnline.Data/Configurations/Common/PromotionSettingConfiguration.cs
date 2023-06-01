using App.BookingOnline.Data.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class PromotionSettingConfiguration : IEntityTypeConfiguration<M_Promotion>
    {
        public void Configure(EntityTypeBuilder<M_Promotion> builder)
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
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(500);
            builder
                .Property(m => m.NameEn)
                .HasMaxLength(500);
            builder
                .Property(m => m.Description)
                .HasMaxLength(5000);
            builder
               .Property(m => m.DescriptionEn)
               .HasMaxLength(5000);
            builder
                .Property(m => m.PromotionCode)
                .HasMaxLength(250);
            builder
                .Property(m => m.Img_Url)
                .HasMaxLength(1000);
            builder
                .Property(m => m.DOW)
                .HasMaxLength(250);
            builder
                .Property(m => m.ApplyTime_From)
                .HasMaxLength(250);
            builder
                .Property(m => m.ApplyTime_To)
                .HasMaxLength(250);
            builder
                .Property(m => m.Promotion_Type)
                .HasMaxLength(250);
            builder
                .Property(m => m.Promotion_Formula)
                .HasMaxLength(500);
            builder
                .Property(m => m.CreatedUser)
                .HasMaxLength(250);
            builder
                .Property(m => m.UpdatedUser)
                .HasMaxLength(250);

            builder.HasOne(x => x.Organization)
              .WithMany(x => x.Promotion)
              .HasForeignKey(x => x.C_Org_Id)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.Restrict);
            builder
                .ToTable("M_Promotion");
        }
    }


}