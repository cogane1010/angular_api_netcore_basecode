import { GridFilterModel } from '../../common/model'
export class OrganizationTypeFilterModel extends GridFilterModel {
  name?: string
  code?: string
}

export class OrganizationFilterModel extends GridFilterModel {
  name?: string
  code?: string
}

export class CustomerGroupFilterModel extends GridFilterModel {
  name?: string
  code?: string
}

export class PaymentTypeFilterModel extends GridFilterModel {
  name?: string
  code?: string
}

export class LockReasonFilterModel extends GridFilterModel {
  name?: string
  code?: string
}

export class CancelReasonFilterModel extends GridFilterModel {
  name?: string
  code?: string
}

export class BookingOtherTypeFilterModel extends GridFilterModel {
  name?: string
  code?: string
}


export class CourseFilterModel extends GridFilterModel {
  name?: string
  code?: string
}


export class CourseTemplateFilterModel extends GridFilterModel {
  name?: string
  code?: string
  orgId?: string
  status?: boolean
}
export class SmsFilterModel extends GridFilterModel {
  timeFrom?: Date
  timeTo?: Date
  mobilephone?: string
  email?: string
  cardNo?: string
  fullName?: string
  c_Org_Id?: string
  customerCode?: string
  status?: string
}
export class PromotionSettingFilterModel extends GridFilterModel {
  name?: string
  code?: string
  orgId?: string
  isActive?: boolean
  isHot?: boolean
  promotionType?: string
}

export class CancelBookingFilterModel extends GridFilterModel {
  fullname?: string
  phoneNumber?: string
  email?: string
  c_Org_Id?: string
  c_Course_Id?: string
  isActive?: boolean
  teetime?: string
  numberPlayers?: number
  status?: string
  dateCreated?: Date
  bookingDate?: Date
  bookingCode?:string
  createdFrom?:Date
  createdTo?:Date
  bookingTo?:Date
  bookingFrom?:Date
}

export class NotificationFilterModel extends GridFilterModel {
  messange_title?: string
  code?: string 
  name?: string
  status?: string
  sendDate?: string
  sendUser?: string
}

export class HolidayFilterModel extends GridFilterModel {
  name?: string
  code?: string
}

export class CustomerFilterModel extends GridFilterModel {
  customerCode: string
  name: string
  dob: Date
  email: string
  mobilePhone: string
  memberCard: string
  isActive: boolean
}



export class MemberCardFilterModel extends GridFilterModel {
  golf_FullName: string
  golf_CardNo: string
  c_Org_Id: string
  orgCode: string
}

export class MemberRequestFilterModel extends GridFilterModel {
  fullName: string
  mobilePhone: string
  email: string
  request_Date: Date
  c_Org_Id: string
  request_FullName: string
  request_MobilePhone: string
}

export class TeeSheetLockFilterModel extends GridFilterModel {
  c_Org_Id: string
  c_LockReason_Id: string
  description: string
  isActive: boolean
}


export class TeeSheetLockLineFilterModel extends GridFilterModel {
  c_Org_Id: string
}



export class BookingFilterModel extends GridFilterModel {
  dateId: Date
  c_Org_Id: string
  c_Course_Id: string
  partId: string
}


export class BookingLineFilterModel extends GridFilterModel {
  dateId: Date
  c_Org_Id: string
  c_Course_Id: string
  part: string
  search: string
  isSearch: boolean = false
}


export class BookingStatisticFilterModel {
  dateId: Date
  c_Org_Id: string
}

export class TransactionDetailReportFilterModel extends GridFilterModel {
  fromDate: Date
  toDate: Date
}

export class TransactionMonthlyReportFilterModel extends GridFilterModel {
  filterDate: Date
}

export class TransactionSummaryReportFilterModel extends GridFilterModel {
  fromDate: Date
  toDate: Date
}