using App.BookingOnline.Data.Configurations;
using App.BookingOnline.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using App.BookingOnline.Data.Configurations.Golf;

namespace App.BookingOnline.Data
{
    public class BookingOnlineDbContext : IdentityDbContext<AppUser, AspRole, string, Identity.UserClaim, AspUserRole, UserLogin, RoleClaim, UserToken>
    {
        public BookingOnlineDbContext(DbContextOptions<BookingOnlineDbContext> options)
            : base(options)
        {
        }

        public BookingOnlineDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
            builder.Entity<AspRole>().Property(x => x.CreatedDate).HasDefaultValueSql("GETDATE()");
            builder.Entity<AspRole>().Property(x => x.CreatedUser).HasMaxLength(255);
            builder.Entity<AspRole>().Property(x => x.UpdatedUser).HasMaxLength(255);
            builder.Entity<AspRole>().Property(x => x.Description).HasMaxLength(255);
            builder.Entity<AspRole>().Property(x => x.DisplayName).HasMaxLength(255);

            #region admin
            builder
                .ApplyConfiguration(new MenuConfiguration());
            builder
                .ApplyConfiguration(new SettingConfiguration());
            //builder
            //    .ApplyConfiguration(new RoleConfiguration());
            builder
                .ApplyConfiguration(new RoleMenuConfiguration());
            builder
               .ApplyConfiguration(new UserConfiguration());
            //builder
            //   .ApplyConfiguration(new UserRoleConfiguration());
            builder
              .ApplyConfiguration(new SequenceConfiguration());
            builder
              .ApplyConfiguration(new SequenceLineConfiguration());

            builder
             .ApplyConfiguration(new UploadFileConfiguration());
            #endregion

            #region Common
            builder
               .ApplyConfiguration(new OrganizationTypeConfiguration());
            builder
               .ApplyConfiguration(new OrganizationConfiguration());
            builder
               .ApplyConfiguration(new OrganizationInfoConfiguration());
            builder
               .ApplyConfiguration(new PaymentTypeConfiguration());
            builder
               .ApplyConfiguration(new LockReasonConfiguration());
            builder
               .ApplyConfiguration(new CancelReasonConfiguration());
            builder
               .ApplyConfiguration(new HotlineConfiguration());
            builder
               .ApplyConfiguration(new BookingOtherTypeConfiguration());
            builder
               .ApplyConfiguration(new CourseConfiguration());
            builder
               .ApplyConfiguration(new HolidayConfiguration());
            builder
             .ApplyConfiguration(new GF_CourseTemplateConfiguration());
            builder
                .ApplyConfiguration(new GF_CourseTemplateLineConfiguration());
            builder
                .ApplyConfiguration(new NotificationConfiguration());
            builder
                .ApplyConfiguration(new PromotionSettingConfiguration());
            builder
                .ApplyConfiguration(new PromotionCourseConfiguration());
            builder
                .ApplyConfiguration(new PromotionCustomerGroupConfiguration());
            builder
             .ApplyConfiguration(new ContactSupportConfiguration());
            builder
             .ApplyConfiguration(new SmsHistoryConfiguration());
            builder
             .ApplyConfiguration(new UserBankInfoConfiguration());
            builder
             .ApplyConfiguration(new NotificationQueueConfiguration());
            builder
             .ApplyConfiguration(new ForgotPasswordHistoryHistoryConfiguration());
            builder
             .ApplyConfiguration(new LogDateFileOutConfiguration());
            builder
             .ApplyConfiguration(new FtpTransJobHistoryConfiguration());
            #endregion

            #region Booking
            builder
               .ApplyConfiguration(new CustomerConfiguration());
            builder
              .ApplyConfiguration(new CustomerGroupConfiguration());
            builder
              .ApplyConfiguration(new CustomerGroupMappingConfiguration());
            builder
             .ApplyConfiguration(new MemberCardConfiguration());
            builder
                .ApplyConfiguration(new MemberCardCourseConfiguration());
            builder
                .ApplyConfiguration(new MemberRequestConfiguration());

            builder
               .ApplyConfiguration(new TeeSheetLockConfiguration());

            builder
               .ApplyConfiguration(new TeeSheetLockLineConfiguration());

            builder
               .ApplyConfiguration(new BookingSessionConfiguration());

            builder
               .ApplyConfiguration(new BookingConfiguration());

            builder
               .ApplyConfiguration(new BookingLineConfiguration());
            builder
              .ApplyConfiguration(new BookingSpecialRequestConfiguration());

            builder
              .ApplyConfiguration(new OutTransactionHeaderConfiguration());
            builder
              .ApplyConfiguration(new OutTransactionDetailsConfiguration());
            builder
              .ApplyConfiguration(new InTransactionHeaderConfiguration());
            builder
              .ApplyConfiguration(new InTransactionDetailsConfiguration());
            builder
              .ApplyConfiguration(new BookingTransactionPaymentConfiguration());
            builder
              .ApplyConfiguration(new TransactionLogsConfiguration());
            #endregion

            #region Seed Data Identity Roles  
            builder.Entity<AspRole>().HasData(new AspRole
            {
                Id = "672db3b8-c436-49bd-8172-bdb6ad6d6148",
                Name = Core.Constants.Admin,
                NormalizedName = Core.Constants.Admin.ToUpper(),
                DisplayName = "Admin",
                IsActive = true,
                Protected = true
            });
            builder.Entity<AspRole>().HasData(new AspRole
            {
                Id = "db29c853-03ea-4328-9553-83676192aeed",
                Name = Core.Constants.Employee,
                NormalizedName = Core.Constants.Employee.ToUpper(),
                DisplayName = "Nhân viên",
                IsActive = true,
                Protected = true
            });
            builder.Entity<AspRole>().HasData(new AspRole
            {
                Id = "3e1ce2a6-e835-41ff-ab54-11dc1e60e839",
                Name = Core.Constants.Customer,
                NormalizedName = Core.Constants.Customer.ToUpper(),
                DisplayName = "Khách hàng",
                IsActive = true,
                Protected = true
            });
            #endregion
        }
    }
}
