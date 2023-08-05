
export interface ApiEndpoint {
  apiEndpoint: string
  authority: string
  client: string
  notifyServer: string
}

export const Urls = {
  auth: {
    token: "/oauth/token",
    pingAuth: "/api/User/PingAuth"
  },
  user: {
    get: "/api/User/Get",
    getAll: "/api/User/GetAll",
    getPaging: "/api/User/GetPaging",
    addOrEdit: "/api/User/AddOrEdit",
    delete: "/api/User/Delete",
    getUserMenu: "/api/User/GetUserMenu",
    changePassword: "/api/user/ChangePassword",
    getUsers: "/api/user/getusers",
    resetPassword: "/api/user/resetpassword",
    resetPasswordCompany: "/api/user/resetPasswordByCompany",
    addOrEditUser: "/api/User/AddOrEditUser",
    getOrg: "/api/User/getOrg",
    roleUser: "/api/User/getRoleUser",
  },
  menu: {
    get: "/api/Menu/Get",
    getAll: "/api/Menu/GetAll",
    getPaging: "/api/Menu/GetPaging",
    addOrEdit: "/api/Menu/AddOrEdit",
    delete: "/api/Menu/Delete"
  },
  role: {
    get: "/api/Role/Get",
    getAll: "/api/Role/GetAll",
    getPaging: "/api/Role/GetPaging",
    addOrEdit: "/api/Role/AddOrEdit",
    delete: "/api/Role/Delete",
    getTreeMenuPermission: "/api/Role/GetTreeMenuPermission"
  },
  setting: {
    get: "/api/Setting/Get",
    getAll: "/api/Setting/GetAll",
    getPaging: "/api/Setting/GetPaging",
    addOrEdit: "/api/Setting/AddOrEdit",
    delete: "/api/Setting/Delete",
    getByCode: "/api/Setting/getByCode"
  },
  common: {
  },
  testApp: {
    sendMail: "/api/TestApp/SendMail",
    getServerTime: "/api/TestApp/GetServerTime",
    getTimeReturn: "/api/TestApp/GetTimeReturn"
  },
 
  organization: {
    get: "/api/Organization/Get",
    getAll: "/api/Organization/GetAll",
    getPaging: "/api/Organization/GetPaging",
    addOrEdit: "/api/Organization/AddOrEdit",
    delete: "/api/Organization/Delete"
  },

  organizationType: {
    get: "/api/organizationType/Get",
    getAll: "/api/organizationType/GetAll",
    getPaging: "/api/organizationType/GetPaging",
    addOrEdit: "/api/organizationType/AddOrEdit",
    delete: "/api/organizationType/Delete"
  },
  paymentType:  {
    get: "/api/PaymentType/Get",
    getAll: "/api/PaymentType/GetAll",
    getPaging: "/api/PaymentType/GetPaging",
    addOrEdit: "/api/PaymentType/AddOrEdit",
    delete: "/api/PaymentType/Delete"
  },
  lockReason: {
    get: "/api/lockReason/Get",
    getAll: "/api/lockReason/GetAll",
    getPaging: "/api/lockReason/GetPaging",
    addOrEdit: "/api/lockReason/AddOrEdit",
    delete: "/api/lockReason/Delete"
  },
  cancelReason: {
    get: "/api/CancelReason/Get",
    getAll: "/api/CancelReason/GetAll",
    getAllCancelReason: "/api/CancelReason/GetCancelReason",
    getPaging: "/api/CancelReason/GetPaging",
    addOrEdit: "/api/CancelReason/AddOrEdit",
    delete: "/api/CancelReason/Delete",
    cancelBookingsFromWeb: "/api/CancelReason/CancelBookingsFromWeb",
  },
  bookingOtherType: {
    get: "/api/bookingOtherType/Get",
    getAll: "/api/bookingOtherType/GetAll",
    getPaging: "/api/bookingOtherType/GetPaging",
    addOrEdit: "/api/bookingOtherType/AddOrEdit",
    delete: "/api/bookingOtherType/Delete"    
  },
  customerGroup: {
    get: "/api/customerGroup/Get",
    getAll: "/api/customerGroup/GetAll",
    getPaging: "/api/customerGroup/GetPaging",
    addOrEdit: "/api/customerGroup/AddOrEdit",
    delete: "/api/customerGroup/Delete"
  },
  course: {
    get: "/api/course/Get",
    getAll: "/api/course/GetAll",
    getAllByUser: "/api/course/GetCourseAllByUser",
    getPaging: "/api/course/GetPaging",
    addOrEdit: "/api/course/AddOrEdit",
    delete: "/api/course/Delete",
    getByOrg: "/api/course/getByOrg",
  },
  courseTemplate: {
    get: "/api/TimingSetting/Get",
    getAll: "/api/TimingSetting/GetAll",
    getPaging: "/api/TimingSetting/GetPaging",
    addOrEdit: "/api/TimingSetting/AddOrEdit",
    delete: "/api/TimingSetting/Delete"    
  },
  smsHistory: {
    get: "/api/SmsHistory/Get",
    getAll: "/api/SmsHistory/GetAll",
    getPaging: "/api/SmsHistory/GetPaging",
    addOrEdit: "/api/SmsHistory/AddOrEdit",
    delete: "/api/SmsHistory/Delete"    
  },
  reports: {
    get: "/api/Reports/Get",
    getAll: "/api/Reports/GetAll",
    getPaging: "/api/Reports/GetPaging",
    addOrEdit: "/api/Reports/AddOrEdit",
    getCancelPaging: "/api/Reports/GetPagingCancelReport",
    getRegistrationPaging: "/api/Reports/GetPagingRegistrationReport",
    getBookingPaging: "/api/Reports/GetPagingBookingReport",
    exportRBookingUrl: "/api/Reports/GetExportRBookingExcel",
    exportRegistrationUrl: "/api/Reports/GetExportRegistrationExcel"         
  },
  promotionSetting: {
    get: "/api/PromotionSetting/Get",
    getAll: "/api/PromotionSetting/GetAll",
    getPaging: "/api/PromotionSetting/GetPaging",
    addOrEdit: "/api/PromotionSetting/AddOrEdit",
    delete: "/api/PromotionSetting/Delete",
    getCustomers: "/api/PromotionSetting/GetCustomers",
    getAllCourse: "/api/PromotionSetting/getAllCourse",
    pushAllNotifi: "/api/SmsHistory/PushNotification",
  },
  notificationSetting: {
    get: "/api/Notification/Get",
    getAll: "/api/Notification/GetAll",
    getPaging: "/api/Notification/GetPaging",
    addOrEdit: "/api/Notification/AddOrEdit",
    delete: "/api/Notification/Delete",
    getCustomers: "/api/Notification/GetCustomers",
    getAllCourse: "/api/Notification/getAllCourse",
  },
  holiday: {
    get: "/api/holiday/Get",
    getAll: "/api/holiday/GetAll",
    getPaging: "/api/holiday/GetPaging",
    addOrEdit: "/api/holiday/AddOrEdit",
    delete: "/api/holiday/Delete"
  },
  customer: {
    get: "/api/customer/Get",
    getAll: "/api/customer/GetAll",
    getPaging: "/api/customer/GetPaging",
    addOrEdit: "/api/customer/AddOrEditCustomer",
    delete: "/api/customer/Delete",
    resetPassword: "/api/customer/resetPassword",
    uploadFile: "/api/customer/UploadFile",
    downloadFile: "/api/customer/DownloadFile",
    changeStatus: "/api/customer/changeStatus"
  },
  memberCardService: {
    get: "/api/memberCard/Get",
    getAll: "/api/memberCard/GetAll",
    getPaging: "/api/memberCard/GetPaging",
    addOrEdit: "/api/memberCard/AddOrEdit",
    delete: "/api/memberCard/Delete",
    search: "/api/memberCard/search",
    assign: "/api/memberCard/assign",
    reassign: "/api/memberCard/reassign",
    refresh: "/api/memberCard/refresh",
  },
  memberRequest: {
    get: "/api/memberRequest/Get",
    getAll: "/api/memberRequest/GetAll",
    getPaging: "/api/memberRequest/GetPaging",
    addOrEdit: "/api/memberRequest/AddOrEdit",
    delete: "/api/memberRequest/Delete",
    saveNote: "/api/memberRequest/saveNote"
  },
  teeSheetLock: {
    get: "/api/TeeSheetLock/Get",
    getAll: "/api/TeeSheetLock/GetAll",
    getPaging: "/api/TeeSheetLock/GetPaging",
    addOrEdit: "/api/TeeSheetLock/AddOrEdit",
    delete: "/api/TeeSheetLock/Delete",
  },
  teeSheetLockLine: {
    get: "/api/TeeSheetLockLine/Get",
    getAll: "/api/TeeSheetLockLine/GetAll",
    getPaging: "/api/TeeSheetLockLine/GetPaging",
    addOrEdit: "/api/TeeSheetLockLine/AddOrEdit",
    delete: "/api/TeeSheetLockLine/Delete",
  },

  booking: {
    get: "/api/booking/Get",
    getAll: "/api/booking/GetAll",    
    getPaging: "/api/booking/GetPaging",
    addOrEdit: "/api/booking/AddOrEdit",
    delete: "/api/booking/Delete",
    getPagingCancel: "/api/CancelBooking/GetPaging",
    getPagingHistory: "/api/HistoryBooking/GetPaging",
    getCheckPayment: "/api/HistoryBooking/GetCheckPayment"
  },
  bookingLine: {
    get: "/api/bookingLine/Get",
    getAll: "/api/bookingLine/GetAll",
    getPaging: "/api/bookingLine/GetPaging",
    addOrEdit: "/api/bookingLine/AddOrEdit",
    updateNoShow: "/api/bookingLine/UpdateNoShow",
    delete: "/api/bookingLine/Delete",
    getStats: "/api/bookingLine/getStats",
    getOrgByUserId: "/api/bookingLine/GetOrgByUserId",
  },
  Transaction: {
    get: "/api/Transaction/Get",
    getAll: "/api/Transaction/GetAll",
    getPaging: "/api/Transaction/GetPaging",
    addOrEdit: "/api/Transaction/AddOrEdit",
    delete: "/api/Transaction/Delete",
    getByOrg: "/api/Transaction/getByOrg",
    uploadFile: "/api/Transaction/uploadFile",
    seabankFileUrl: "/api/Transaction/getSeabankFile",
    getDetails: "/api/Transaction/getDetails",
    exportExcelFileOut: "/api/Transaction/exportExcelFileOut",
    exportExcelFileOutAll: "/api/Transaction/exportExcelFileOutAll",
    exportExcelConfirmMoney: "/api/Transaction/exportExcelConfirmMoney",
    getPagingOutFile: "/api/Transaction/GetPagingOutFile",
    getPagingOutFileAll: "/api/Transaction/getPagingOutFileAll",
    getDetailOutFile: "/api/Transaction/GetDetailOutFile",
    approveIn: "/api/Transaction/approveIn",
    checkTransSaveUrl: "/api/Transaction/checkSaveCompareTrans",
    approveOut: "/api/Transaction/approveOut",
    unApproveOut: "/api/Transaction/unApproveOut",
    getTransactionCompare: "/api/Transaction/getTransactionCompare",
    saveCompare: "/api/Transaction/saveCompare",
    sendEmailFileOut: "/api/Transaction/sendEmailFileOut",
    getNumOfDayNotCompare: "/api/Transaction/getNumOfDayNotCompare",
    getStatusCompareTrans: "/api/Transaction/getStatusCompareTrans",
    getNotCompareDate: "/api/Transaction/getNotCompareDate",
    getNumOfDayNotApprove: "/api/Transaction/getNumOfDayNotApprove",
    getNotApproveDate: "/api/Transaction/getNotApproveDate",
    getOutEmailSetting: "/api/Transaction/getOutEmailSetting",
    saveOutEmailSetting: "/api/Transaction/saveOutEmailSetting",
    testSendMail: "/api/Transaction/testSendMail",
    getDetailOutMoneyUrl: "/api/Transaction/getDetailOutMoney",
    cancelOutDataUrl: "/api/Transaction/CancelOutData",
    confirmMoneyUrl: "/api/Transaction/ConfirmMoneyData",
    cancelConfirmMoneyUrl: "/api/Transaction/CancelConfirmMoneyData",
    manualConfirmUrl:"/api/Transaction/ManualMoneyData",
    getNotConfirmMoneyDate: "/api/Transaction/getNotConfirmMoneyDate",
    getNotConfirmMoneyDateCountUrl: "/api/Transaction/GetNotConfirmMoneyDateCount",
    getNumOfDayNotConfirmMoneyUrl:"/api/Transaction/getNumOfDayNotConfirmMoney",
    getFtpTransPagingUrl: "/api/Transaction/getFtpTransPaging",
    uploadFtpExcelUrl: "/api/Transaction/uploadFtpExcel"
  },
  transactionDetailReport: {
    getReportData: "/api/TransactionDetailReport/GetReportData",
    exportExcel: "/api/TransactionDetailReport/exportExcel"  
  },
  transactionMonthlyReport: {
    getReportData: "/api/transactionMonthlyReport/GetReportData",
    exportExcel: "/api/transactionMonthlyReport/exportExcel"  
  },
  transactionSummaryReport: {
    getReportData: "/api/transactionSummaryReport/GetReportData",
    exportExcel: "/api/transactionSummaryReport/exportExcel"  
  }
}

