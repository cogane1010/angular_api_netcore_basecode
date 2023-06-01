using App.BookingOnline.Data.Models.Common;
using App.BookingOnline.Data.Models.Golf;
using App.Core.Domain;
using System;
using System.Collections.Generic;

namespace App.BookingOnline.Data.Models
{
    public class OrganizationType : BaseEntity, IEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public IEnumerable<Organization> Organizations { get; set; }
    }

    public class Organization : BaseEntity, IEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid C_OrgType_Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsSummary { get; set; }
        public int? OrderValue { get; set; }
        public string PhoneSupport { get; set; }
        public string IconUrl { get; set; }
        public string ImageUrl { get; set; }
        public string EmailSupport { get; set; }
        public string TimeSupport { get; set; }
        public string NoteSupport { get; set; }

        #region Relation
        public OrganizationType OrganizationType { get; set; }
        public List<OrganizationInfo> OrganizationInfos { get; set; }
        public List<BookingTransactionPayment> TransactionPayments { get; set; }
        public List<User> Users { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        public List<LockReason> LockReasons { get; set; }
        public List<CancelReason> CancelReasons { get; set; }
        public List<BookingOtherType> BookingOtherTypes { get; set; }
        public List<Course> Courses { get; set; }
        public List<Holiday> Holidays { get; set; }
        public List<CustomerGroup> CustomerGroups { get; set; }
        public IEnumerable<MemberCard> MemberCards { get; set; }
        public IEnumerable<OutTransactionHeader> OutTransactionHeaders { get; set; }
        public IEnumerable<OutTransactionDetails> OutTransactionDetails { get; set; }
        public IEnumerable<InTransactionDetails> InTransactionDetails { get; set; }
        public IEnumerable<MemberRequest> MemberRequests { get; set; }
        public IEnumerable<GF_CourseTemplate> GF_CourseTemplates { get; set; }
        public IEnumerable<TeeSheetLock> TeeSheetLocks { get; set; }
        public IEnumerable<M_Promotion> Promotion { get; set; }
        public IEnumerable<M_Promotion_CustomerGroup> PromotionCustomerGroup { get; set; }
        public IEnumerable<GF_Notification> Notification { get; set; }
        public IEnumerable<BookingSession> BookingSessions { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
        public IEnumerable<BookingSpecialRequest> BookingSpecialRequests { get; set; }
        #endregion
    }


    public class OrganizationInfo : BaseEntity, IEntity
    {
        public Guid C_Org_Id { get; set; }
        public string TaxCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string InvoiceName { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceBankAccount { get; set; }
        public string InvoiceBankName { get; set; }
        public string LogoUrl { get; set; }
        public string ShortAddress { get; set; }

        public Organization Organization { get; set; }
    }


}