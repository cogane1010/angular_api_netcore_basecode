import { NgModule } from '@angular/core'
import { CommonModule, DatePipe } from '@angular/common'
import { RouterModule, Routes } from '@angular/router'
import { ShareModule } from '../share/share.module'
import { OrganizationTypeComponent } from './components/organization-type/organization-type.component'
import { DialogOrganizationTypeDetailComponent } from './components/organization-type/dialog-organization-type-detail/dialog-organization-type-detail.component'
import { OrganizationComponent } from './components/organization/organization.component'
import { DialogOrganizationDetailComponent } from './components/organization/dialog-organization-detail/dialog-organization-detail.component'
import { PaymentTypeComponent } from './components/payment-type/payment-type.component'
import { DialogPaymentTypeDetailComponent } from './components/payment-type/dialog-payment-type-detail/dialog-payment-type-detail.component'
import { LockReasonComponent } from './components/lock-reason/lock-reason.component'
import { DialogLockReasonDetailComponent } from './components/lock-reason/dialog-lock-reason-detail/dialog-lock-reason-detail.component'
import { DialogCancelReasonDetailComponent } from './components/cancel-reason/dialog-cancel-reason-detail/dialog-cancel-reason-detail.component'
import { CancelReasonComponent } from './components/cancel-reason/cancel-reason.component'
import { BookingOtherTypeComponent } from './components/booking-other-type/booking-other-type.component'
import { DialogBookingOtherTypeDetailComponent } from './components/booking-other-type/dialog-booking-other-type-detail/dialog-booking-other-type-detail.component'
import { CourseComponent } from './components/course/course.component'
import { DialogCourseDetailComponent } from './components/course/dialog-course-detail/dialog-course-detail.component'
import { HolidayComponent } from './components/holiday/holiday.component'
import { DialogHolidayDetailComponent } from './components/holiday/dialog-holiday-detail/dialog-holiday-detail.component'
import { CustomerComponent } from './components/customer/customer-list/customer-list.component'
import { CustomerDetailComponent } from './components/customer/customer-detail/customer-detail.component'
import { DialogMemberCardListComponent } from './components/customer/dialog-member-card-list/dialog-member-card-list.component'
import { MemberRequestComponent } from './components/member-request/member-request.component'
import { TimingSettingComponent } from './components/timing-setting/timing-setting.component'
import { CourseTemplateDetailComponent } from './components/timing-setting/courseTemplate-detail/courseTemplate-detail.component'
import { TeeSheetLockComponent } from './components/tee-sheet-lock/tee-sheet-lock.component'
import { TeeSheetLockDetailComponent } from './components/tee-sheet-lock/tee-sheet-lock-detail/tee-sheet-lock-detail.component'
import { DialogTeeSheetLockLineComponent } from './components/tee-sheet-lock/dialog-tee-sheet-lock-line/dialog-tee-sheet-lock-line.component'
import { PromotionSettingListComponent } from './components/promotion-setting/promotion-setting-list/promotion-setting-list.component'
import { PendingChangesGuard } from '../admin/auth/auth.guard'
import { BookingListComponent } from './components/booking/booking-list/booking-list.component'
import { PromotionSettingAddComponent } from './components/promotion-setting/promotion-setting-add/promotion-setting-add.component'
import { DialogBookingSearchComponent } from './components/booking/dialog-booking-search/dialog-booking-search.component'
import { DialogBookingStatisticComponent } from './components/booking/dialog-booking-statistic/dialog-booking-statistic.component'
import { BookingDetailComponent } from './components/booking/booking-detail/booking-detail.component'
import { CustomerGroupListComponent } from './components/customer-group/customer-group-list/customer-group-list.component'
import { DialogCustomerGroupDetailComponent } from './components/customer-group/dialog-customer-group-detail/dialog-customer-group-detail.component'
import { PromotionSeagolfListComponent } from './components/promotion-seaGolf/promotion-seagolf-list/promotion-seagolf-list.component'
import { PromotionSeagolfAddComponent } from './components/promotion-seaGolf/promotion-seagolf-add/promotion-seagolf-add.component'
import { NotificationListComponent } from './components/notification/notification-list/notification-list.component'
import { NotificationAddComponent } from './components/notification/notification-add/notification-add.component'
import { DialogSendNotificattionComponent } from './components/notification/dialog-send-notificattion/dialog-send-notificattion.component'
import { SmsHistryComponent } from './components/sms-histry/sms-histry.component'
import { CancelReportComponent } from './components/reports/cancel-report/cancel-report.component'
import { CancelBookingAdminComponent } from './components/cancel-booking-admin/cancel-booking-admin.component'
import { CancelBookingAdminDialogComponent } from './components/cancel-booking-admin/cancel-booking-admin-dialog/cancel-booking-admin-dialog.component'
import { BookingHistoryListComponent } from './components/booking-history/booking-history-list/booking-history-list.component';
import { BookingTransCompareComponent } from './components/booking-trans-control/booking-trans-compare/booking-trans-compare.component';
import { BookingTransactionInApproveComponent } from './components/booking-trans-control/booking-trans-in-appr/booking-trans-in-appr.component';
import { TransactionInDetailsDialogComponent } from './components/booking-trans-control/booking-trans-in-appr/trans-details-dialog/trans-details-dialog.component'
import { BookingOutApprComponent } from './components/booking-trans-control/booking-out-appr/booking-out-appr.component';
import { BookingOutDetailComponent } from './components/booking-trans-control/booking-out-appr-detail/booking-out-detail/booking-out-detail.component';
import { BookingOutAllComponent } from './components/booking-trans-control/booking-out-all/booking-out-all.component';
import { BookingTransCompareSelectDateComponent } from './components/booking-trans-control/booking-trans-compare/booking-trans-compare-select-date/booking-trans-compare-select-date.component'
import { BookingOutApproveSelectDateComponent } from './components/booking-trans-control/booking-out-appr/booking-out-appr-select-date/booking-out-appr-select-date.component'
import { BookingOutEmailSettingComponent } from './components/booking-trans-control/booking-out-email-setting/booking-out-email-settingcomponent';
import { BookingConfirmMoneyComponent } from './components/booking-trans-control/booking-confirm-money/booking-confirm-money.component';
import { ConfirmMoneyDialogComponent } from './components/booking-trans-control/booking-confirm-money/confirm-money-dialog/confirm-money-dialog.component';
import { TransDateConfirmComponent } from './components/booking-trans-control/booking-confirm-money/trans-date-confirm/trans-date-confirm.component'
import { TransactionDetailReportComponent } from './components/reports/trans-detail-report/trans-detail-report.component'
import { NgxPrintModule } from 'ngx-print'
import { TransactionMonthlyReportComponent } from './components/reports/trans-monthly-report/trans-monthly-report.component'
import { TransactionSummaryReportComponent } from './components/reports/trans-summary-report/trans-summary-report.component'
import { PairsPipe } from '../common/pipe/PairsPipe.pipe';
import { RegistrationReportComponent } from './components/reports/registration-report/registration-report.component';
import { BookingReportComponent } from './components/reports/booking-report/booking-report.component';
import { FtpViewListComponent } from './components/booking-trans-control/ftp-view-list/ftp-view-list.component'

export const routes: Routes = [
    { path: 'organization-type', component: OrganizationTypeComponent },
    { path: 'organization', component: OrganizationComponent },
    { path: 'payment-type', component: PaymentTypeComponent },
    { path: 'lock-reason', component: LockReasonComponent },
    { path: 'cancel-reason', component: CancelReasonComponent },
    { path: 'booking-other-type', component: BookingOtherTypeComponent },
    { path: 'course', component: CourseComponent },
    { path: 'holiday', component: HolidayComponent },
    { path: 'customer-group', component: CustomerGroupListComponent },
    { path: 'customer', component: CustomerComponent },
    { path: 'customer/:id', component: CustomerDetailComponent },
    { path: 'member-request', component: MemberRequestComponent },
    { path: 'timing-setting', component: TimingSettingComponent },
    { path: 'timing-setting/:id', component: CourseTemplateDetailComponent },
    { path: 'teesheet-lock', component: TeeSheetLockComponent },
    { path: 'teesheet-lock/:id', component: TeeSheetLockDetailComponent },
    { path: 'booking-list', component: BookingListComponent },
    { path: 'booking-detail/:id', component: BookingDetailComponent },
    { path: 'promotion-setting', component: PromotionSettingListComponent },
    { path: 'promotion-setting/:id', component: PromotionSettingAddComponent },
    { path: 'promotion-seagolf', component: PromotionSeagolfListComponent },
    { path: 'promotion-seagolf/:id', component: PromotionSeagolfAddComponent },
    { path: 'notification', component: NotificationListComponent },
    { path: 'notification/:id', component: NotificationAddComponent },
    { path: 'smsHistry', component: SmsHistryComponent },
    { path: 'cancelReport', component: CancelReportComponent },
    { path: 'AdminCancel', component: CancelBookingAdminComponent },
    { path: 'history-booking', component: BookingHistoryListComponent },
    { path: 'booking-payment-check', component: BookingTransCompareComponent },
    { path: 'booking-trans-in-appr', component: BookingTransactionInApproveComponent },
    { path: 'booking-trans-out-appr', component: BookingOutApprComponent },
    { path: 'booking-trans-out-all', component: BookingOutAllComponent},
    { path: 'booking-out-email-cfg', component: BookingOutEmailSettingComponent},
    { path: 'booking-confirm-money', component: BookingConfirmMoneyComponent},
    { path: 'trans-detail-report', component: TransactionDetailReportComponent} ,
    { path: 'trans-monthly-report', component: TransactionMonthlyReportComponent} ,
    { path: 'trans-summary-report', component: TransactionSummaryReportComponent},
    { path: 'registration-report', component: RegistrationReportComponent},
    { path: 'booking-report', component: BookingReportComponent},
    { path: 'ftp-view-list', component: FtpViewListComponent} 
  ]

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ShareModule,
    NgxPrintModule
  ],
  declarations: [
    OrganizationTypeComponent,
    DialogOrganizationTypeDetailComponent,
    OrganizationComponent,
    DialogOrganizationDetailComponent,
    PaymentTypeComponent,
    DialogPaymentTypeDetailComponent,
    LockReasonComponent,
    DialogLockReasonDetailComponent,
    CancelReasonComponent,
    DialogCancelReasonDetailComponent,
    BookingOtherTypeComponent,
    DialogBookingOtherTypeDetailComponent,
    CourseComponent,
    DialogCourseDetailComponent,
    HolidayComponent,
    DialogHolidayDetailComponent,
    CustomerGroupListComponent,
    DialogCustomerGroupDetailComponent,
    CustomerComponent,
    CustomerDetailComponent,
    DialogMemberCardListComponent,
    MemberRequestComponent,
    TimingSettingComponent,
    CourseTemplateDetailComponent,
    TeeSheetLockComponent,
    TeeSheetLockDetailComponent,
    DialogTeeSheetLockLineComponent,
    BookingListComponent,
    BookingDetailComponent,
    DialogBookingStatisticComponent,
    DialogBookingSearchComponent,
    PromotionSettingListComponent,
    PromotionSettingAddComponent,
    PromotionSeagolfListComponent,
    PromotionSeagolfAddComponent,
    NotificationListComponent,
    NotificationAddComponent,
    DialogSendNotificattionComponent,
    SmsHistryComponent,
    CancelReportComponent,  
    CancelBookingAdminComponent,
    CancelBookingAdminDialogComponent,
    BookingHistoryListComponent,
    BookingTransCompareComponent,
    BookingTransactionInApproveComponent,
    TransactionInDetailsDialogComponent,
    BookingOutApprComponent,
    BookingOutDetailComponent,
    BookingOutAllComponent,
    BookingTransCompareSelectDateComponent,
    BookingOutApproveSelectDateComponent,
    BookingOutEmailSettingComponent,
    BookingConfirmMoneyComponent,
    ConfirmMoneyDialogComponent,
    TransDateConfirmComponent,
    TransactionDetailReportComponent,
    TransactionMonthlyReportComponent,
    TransactionSummaryReportComponent,
    RegistrationReportComponent,
    BookingReportComponent,
    FtpViewListComponent    
  ],
  providers:[DatePipe, PendingChangesGuard,PairsPipe]
})
export class BookingOnlineModule { }
