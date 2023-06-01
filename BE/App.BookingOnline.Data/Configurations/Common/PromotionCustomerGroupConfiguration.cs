using App.BookingOnline.Data.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BookingOnline.Data.Configurations
{
    public class PromotionCustomerGroupConfiguration : IEntityTypeConfiguration<M_Promotion_CustomerGroup>
    {
        public void Configure(EntityTypeBuilder<M_Promotion_CustomerGroup> builder)
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
             .WithMany(x => x.PromotionCustomerGroup)
             .HasForeignKey(x => x.M_Promotion_Id)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Organization)
             .WithMany(x => x.PromotionCustomerGroup)
             .HasForeignKey(x => x.C_Org_Id)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CustomerGroup)
             .WithMany(x => x.PromotionCustomerGroup)
             .HasForeignKey(x => x.MB_CustomerGroup_Id)
             .OnDelete(DeleteBehavior.Restrict);

            builder
                .ToTable("M_Promotion_CustomerGroup");
        }
    }


}