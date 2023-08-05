import { GridFilterModel } from "src/app/common/model";

export class OrganizationType {
    id?: string;
    code?: string;
    name?: string;
}


export class Organization {
    id?: string;
    code: string;
    name: string;
    c_OrgType_Id: string;
    isActive: boolean;
    isSummary: boolean;
    organizationTypeName: string;
    organizationInfo: OrganizationInfo;
    phoneSupport: string;
    iconUrl: string;
    imageUrl: string;
    emailSupport: string;
    timeSupport: string;
    noteSupport: string;
    courses?: Course[]
}

export class OrganizationInfo {
    id?: string;
    c_Org_Id: string;
    taxCode: string;
    telephone: string;
    fax: string;
    website: string;
    email: string;
    invoiceName: string;
    invoiceAddress: string;
    invoiceBankAccount: string;
    invoiceBankName: string;
    logoUrl: string;
    shortAddress: string;
}

export class PaymentType {
    id?: string;
    c_Org_Id: string;
    OrganizationName: string;
    name: string;
    nameEn: string;
    code: string;
    isActive: boolean;
}

export class LockReason {
    id?: string;
    c_Org_Id: string;
    organizationName: string;
    name: string;
    nameEn: string;
    code: string;
    isActive: boolean;
}

export class CancelReason {
    id?: string;
    c_Org_Id: string;
    organizationName: string;
    name: string;
    nameEn: string;
    code: string;
    isActive: boolean;
}

export class BookingOtherType {
    id?: string;
    c_Org_Id: string;
    organizationName: string;
    name: string;
    nameEn: string;
    code: string;
    isActive: boolean;
}

export class Course {
    id?: string;
    c_Org_Id?: string;
    organizationName?: string;
    name?: string;
    code?: string;
    isActive?: boolean;
    orderValue?: number
    selected?: boolean
}


export class CourseTemplate {
    id?: string;
    c_Org_Id?: string;
    C_Course_Id?: string;
    orgName?: string;
    dow?: string;
    name?: string;
    code?: string;
    appliedDate?: number;
    startDate?: Date;
    endDate?: Date;
    isActive?: boolean = true;
    appliedDate0?: boolean = false;
    appliedDate1?: boolean = false;
    appliedDate2?: boolean = false;
    appliedDate3?: boolean = false;
    appliedDate4?: boolean = false;
    appliedDate5?: boolean = false;
    appliedDate6?: boolean = false;
    appliedDate7?: boolean = false;
    courseTemplateLine?: CourseTemplateLine[];
}

export class SmsHistory {
    sendDate?: string;
    mobilephone?: string;
    type?: string;
}

export class GF_Notification {
    id?: string
    c_Org_Id?: number
    status?: number
    created_Date?: Date
    created_User?: string
    code?: string
    message_Type?: string
    message_Title?: string
    message_Content?: string
    message_TitleEn?: string
    message_ContentEn?: string
    sent_Date?: Date
    sent_User?: string
    img_Url?: string
    img_FullUrl?: string
    imageData?: string
    serverKey?: string
}

export class FirebaseBody {
    data?: BodyData
    to?: string
}

export class BodyData {
    id?: string
    title?: string
    body?: string
    imgUrl?: string
}

export class M_Promotion {
    id?: string
    c_Org_Id?: number
    isActive?: boolean
    created_Date?: Date
    Created_User?: string
    code?: string
    name?: string
    nameEn?: string
    description?: string
    promotionCode?: string
    start_Date?: Date
    end_Date?: Date
    dOW?: string
    applyTime_From?: string
    applyTime_To?: string
    promotion_Type?: string
    priotity?: number
    isHotPromotion?: boolean
    promotion_Formula?: string
    promotion_Value?: number
    publicPrice_Percent?: number
    promotionPrice_Percent?: number
    course?: Course[]
    customerGroups?: CustomerGroup[]
    imageData?: string
    img_Url?: string
    imageDataEn?: string
    img_Url_En?: string
    promotionCourses?: PromotionCourse[] = []
    promotionCustomerGroup?: PromotionCustomerGroup[] = []
    customerGroupNames?: string
    courseNames?: string
    appliedDate0?: boolean = false;
    appliedDate1?: boolean = false;
    appliedDate2?: boolean = false;
    appliedDate3?: boolean = false;
    appliedDate4?: boolean = false;
    appliedDate5?: boolean = false;
    appliedDate6?: boolean = false;
    appliedDate7?: boolean = false;
}

export class PromotionCourse {
    id?: string;
    m_Promotion_Id?: string;
    c_Course_Id?: string;
    name?: string;
    code?: string;
    isActive?: boolean;
    orderValue?: number
    selected?: boolean
}

export class PromotionCustomerGroup {
    id?: string;
    m_Promotion_Id?: string;
    c_Org_Id?: string;
    mB_CustomerGroup_Id?: string;
    isActive?: boolean;
    orderValue?: number
    selected?: boolean
}

export class CourseTemplateLine {
    id?: string;
    c_Org_Id?: string;
    organizationName?: string;
    name?: string;
    code?: string;
    c_Course_Id?: string;
    gf_CourseTemplate_Id?: string;
    startTee?: number = 1;
    startTime?: string;
    endTime?: string;
    interval?: number;
    turnLength?: number;
    hole?: number;
    flightSeq?: string;
    maxTurnLength?: number;
    part?: string;
}

export class Holiday {
    id?: string;
    c_Org_Id: string;
    OrganizationName: string;
    name: string;
    code: string;
    isActive: boolean;
    dateId: Date;
}

export class CustomerGroup {
    id?: string;
    c_Org_Id?: string
    organizationName?: string
    code?: string
    name?: string
    nameEn?: string
    orderValue?: number
    isActive?: boolean
    selected?: boolean
}


export class Customer {
    id?: string;
    customerCode: string;
    firstName: string;
    lastName: string;
    fullName: string;
    email: string;
    mobilePhone: string;
    total_Golf_CardNo: string;
    total_Course: string;
    total_Org: string;
    dob: Date;
    createdDate: Date;
    imageData?: string
    img_Url?: string
    gender: number;
    isMember: boolean;
    isActive: boolean;
    stausInt: number
    userId: string;
    avatarFileId: string;
    golf_CardNo: string;
    mB_Customer_Id: string;
    memberCards?: MemberCard[]

}


export class MemberCard {
    id?: string;
    mB_Customer_Id?: string
    c_Org_Id?: string
    orgName?: string;
    mB_CustomerGroup_Id?: string
    customerGroupName?: string
    isActive?: boolean
    isDelete?: boolean
    golf_MemberCardId?: string
    golf_IDNo?: string
    golf_Address?: string
    golf_CardNo?: string
    golf_FullName?: string
    golf_Mobilephone?: string
    golf_Email?: string
    golf_DOB: Date
    golf_Effective_Date?: Date
    golf_Expire_Date?: Date
    golf_CardStatus?: string
    golf_IsActive?: boolean
    golf_IsLock?: boolean
    coursesMemberCard?: MemberCardCourse[]
}

export class MemberCardCourse {
    id?: string;
    mC_MemberCard_Id?: string
    mB_CustomerGroup_Id?: string
    customerGroupName?: string
    c_Course_Id?: string
    validFrom?: Date
    validTo?: Date
    description?: string
    courseName?: string
}


export class MemberRequest {
    id?: string;
    request_Date?: Date
    firstName?: string
    lastName?: string
    fullName?: string
    mobilePhone?: string
    email?: string
    c_Org_Id?: string
    description?: string
    request_Status?: string
    responsed_Date?: Date
    responsed_User?: string
    responsed_Description?: string
    request_FullName?: string
    request_MobilePhone?: string
}

export class TeeSheetLock {
    id?: string;
    c_Org_Id: string
    organizationName: string
    isActive: boolean
    description: string
    c_LockReason_Id: string
    startDate: Date
    endDate: Date
    lockStatus: string
    lockType: string
    teeSheetLockLines?: TeeSheetLockLine[]
}


export class Roles {
    id?: string
    name: string
    selected: boolean
}

export class TeeSheetLockLine {
    id?: string;
    c_Org_Id: string
    isActive: boolean

    gF_TeeSheetLock_Id: string
    c_Course_Id: string
    courseName: string
    description: string
    dow: string

    startTee: number
    flightSeq: string

    startTime: string
    endTime: string
    startTimeValue: Date
    endTimeValue: Date

    isDeleted: boolean
}

export class BookingSession {
    id?: string
    c_Org_Id?: string
    isActive?: string
    bookingCode?: string
    device_Id?: string
    userId?: string
    dateId?: Date
    start_Time?: Date
    end_Time?: Date
    c_Course_Id?: string
    courseName?: string
    tee_Time?: Date
    status?: string
    bookingType?: string
    m_Promotion_Id?: string
}
export class Booking {
    id?: string;
    c_Org_Id?: string
    orgName?: string
    orgCode?: string
    gF_Booking_Session_Id?: string
    isActive?: boolean
    bookingCode?: string
    device_Id?: string
    userId?: string
    customerCode?: string
    cardFullName?: string
    dateId: Date
    start_Time?: Date
    end_Time?: Date
    c_Course_Id?: string
    courseName?: string
    tee_Time?: Date
    timeDisplay?: string
    status?: string
    statusName?: string
    bookingType?: string
    m_Promotion_Id?: string // Cho Promotion
    confirm_User?: string
    confirm_Date: Date
    confirm_Description: string
    confirm_Type: string
    payment_Time: string
    cancel_Time: string
    cancel_User?: string
    cancel_UserName?: string
    cancel_Reason_Id: string
    cancelReasonName?: string
    cancel_Description?: string
    tien_hoan?: number
    bookingTeetime?: BookingLine[]
    bookingSpecialRequests?: BookingSpecialRequest[]
    specialNameServices?:string
    numberPlayers?: number    
    totalTeetimeDisplay?: string
    countPlayers?: number
    showUpCount?: number
    cardEmail?: string
    cardMobilePhone?: string
    golfCardNo?: string
    cardCourseName?: string
    specialServices?: string
    totalEstimateFee?: number
    nonRefundedFee?: number
    createdDate: Date
    isCancel?: boolean
    transid?: string
    orgId?: string
    golfers?: number  
}

export class PaymentCompare {
    inTransactionDetails?: InTransactionDetails
    bookingDTO?: Booking
    rc_code?: string
    userRc_code?: string
    old_userRc_code?: string; // check ui changed
    isSummary:boolean;
}

export class InTransactionDetails {
    phone_Number?: string
    orgCode?: string
    orgName?: string
    sourceType?: string
    pan_number?: string
    customerName?: string
    debitAcc?: string
    creditAcc?: string
    trans_Date?: string
    trans_Time?: string
    trans_Amt?: number
    traceId?: string
    payment_Detail?: string
    co_Code?: string
    fT_Id?: string
    return_Acc?: string
    trans_Id?: string
    tien_hoan?: number
    trans_type?: string
    refund_Trans_Date?:Date
}

export class BookingLine {
    id?: string;
    c_Org_Id?: string
    gF_Booking_Id?: string
    isActive?: boolean
    cardNo?: string
    mB_CustomerGroup_Id?: string
    customerGroupName?: string
    dateId?: Date
    startTee?: number
    tee_Time?: Date
    teeTimeEnd?:Date
    turnLength?: number
    turn?: Date
    hole?: number
    flightSeq?: number
    caddie_No?: string
    customerFullName?: string
    telephone?: string
    email?: string
    description?: string
    bookingStatus?: string
    public_Price?: number
    promotion_Price?: number
    net_Ammount?: number
    discount_Value?: number
    discount_Amount?: number
    total_Amount?: number
    deposit_Amount?: number
    estimatedPrice?:number
    golf_Price_Description?: string
    golf_Promotion_Id?: string
    golf_Promotion_Name?: string
    is_NoShow?: boolean
    updateNoShow_Time: Date
    updateNoShow_UserName?: string
    part?: string
    isUpdateNoShow?: boolean
    notes: string
    nonRefundedFee?: number
    cancelDate?: Date
    cancelTime?: string
    cancelType?: string
    durationPlay?: string
    diffHour?: number
    totalplay?: number
    booking?: Booking
}


export class BookingSpecialRequest {
    id?: string
    gF_Booking_Id?: string;
    isActive?: boolean
    c_Booking_OtherType_Id?: string
    bookingOtherTypeName?: string
    description?: string
}

export class BookingLineStatistic {
    dateId: Date
    c_Org_Id: string
    organizationName: string
    c_Course_Id: string
    courseName: string
    part: string
    noBooking: number
    noReality: number
    numRows: number
    bookStatByParts: BookStatByPart[]
}
export class BookStatByOrg {
    dateId: Date
    c_Org_Id: string
    organizationName: string
    numRows: number
    totalBooking: number
    totalNoReality: number
    bookByCourse: BookingLineStatistic[]
}
export class BookStatByPart {
    partName: string
    noBooking: number
    noReality: number
}
export class BookingStatisticModel {
    noBooking?: number;
    noReality?: number;
    numRows?: number;
}

export class BookingStatisticOrgModel extends BookingStatisticModel {
    orgId?: string;
    organizationName?: string;
    listCourse?: BookingStatisticCourseModel[]
}
export class BookingStatisticCourseModel extends BookingStatisticModel {
    courseId?: string;
    courseName?: string;
    listPart?: BookingStatisticPartModel[]
}

export class BookingStatisticPartModel extends BookingStatisticModel {
    partName?: string;
}

export class TransactionInFilter extends GridFilterModel {
    FindDate: Date;
    FromDate: Date;
    ToDate: Date;
    headerId: string;
    status: string;
    outTransHeaderId?: string
    EmailFilterDates: Date[];
}

export class InTransactionHeader {
    id: string;
    fileName: string;
    filePath: string;
    dateTrans: string;
    createdUser: string;
    createdDate: string;
    updatedUser: string;
    updatedDate: string;
    status: string;
    isActive:boolean;
    inTransactionDetails: InTransactionDetails[];
    createdUserName: string;
}

export class OutTransactionHeader {
    id: string;
    fileName: string;
    filePath: string;
    dateTrans: string;
    createdUser: string;
    createdDate: string;
    updatedUser: string;
    updatedDate: string;
    status: string;
    approverUserName: string;
    approveDate:Date;
    outTransactionDetails: OutTransactionDetails[];
}

export class OutTransactionDetails {
    phone_Number?: string
    orgCode?: string
    orgName?: string
    sourceType?: string
    pan_number?: string
    customerId?: string
    customerName?: string
    debitAcc?: string
    creditAcc?: string
    trans_Date?: string
    trans_Time?: string
    trans_Amt?: number
    tien_hoan?:number
    totalMoney?: number
    traceId?: string
    payment_Detail?: string
    co_Code?: string
    fT_Id?: string
    ftid?:string
    return_Acc?: string
    rc_code?: string
    userRc_code?: string
    trans_Id?: string
    statusLine?: number
    moneyAtAcc?: Date
    totalMoneyAcc?: number
    inMoneyUser?: string
    isSelected?: boolean
    booking?: Booking
    bookingCode?:string
    datePayment?:Date
    totalTeetimeDisplay?:string
    playerCount?:number
    cardFullName?:string
    trans_type?:string
    cancelStatus?:string
    cancelTime?:Date
    sdkRefundMoney?:number
}

export class SaveCompareModel {
    orgId: string;
    userId: string;
    findDate?: Date;
    rows: PaymentCompare[];
}

export enum SeaBankCheckCodeEnum {
    SbYesPartNo = "111",
    SbNoPartYes = "117",
    IncorrectInfo = "119",
    ReturnCasd = "115",
    Compensation = "900"
}

export class SeaBankCheckCode {
    static SbYesPartNo = "SB có, đối tác không có";
    static SbNoPartYes = "Đối tác có, SB không có";
    static IncorrectInfo = "Giao dịch sai lệch thông tin";
    static ReturnCasd = "Giao dịch thành công nhưng đối tác hoàn trả tiền";
    static Compensation = "Thu bù";
}

export class TransactionNotCompareLine {
    uiIndex: number;
    uiSelected: boolean;
    transDate: Date;
    noOfRecord: number;
    paymentsCompare: PaymentCompare[];
}

export class NotConfirmMoneyList {
    id?: string;
    dateId?: Date;
    numberNotConfirm?: number;
    totalMoney?: number;
}

export class TransactionNotCompareFilter extends GridFilterModel {
}

export class TransactionNotApproveLine {
    uiIndex: number;
    uiSelected: boolean;
    transDate: Date;
    noOfRecord: number;
    outHeader: OutTransactionHeader;
}

export class TransactionNotApproveFilter extends GridFilterModel {

}

export class BookingOutEmailSetting {
    transaction_out_email_to: string;
    transaction_out_email_cc: string;
    transaction_out_email_bcc: string;
    transaction_out_email_hour: string;
    transaction_out_email_dow: string;
    hourGetFileIn:string;
}

export class TransactionDetailReportResult {
    pagingData: any;
    transAmt: number;
    cusCancelReturnBf24HAmt: number;
    notReceivedAmt: number;
    totalReceived: number;
}

export class TransactionDetailReportDTO {
    Id: string;
    CreatedUser: string;
    CreatedDate: string;
    UpdatedUser: string;
    UpdatedDate: string;

    // Mã KH SB
    seaBankCustomerCode: string;

    // Tên KH SB
    seaBankCustomerName: string;

    // Mã đặt chỗ
    bookingCode: string;

    // Sân Golf
    golfCourseCode: string;

    // Ngày đặt
    bookingDate: string;

    // TK ghi co
    creditAccount: string;

    // CARD.type
    card_type: string;

    // số card masking
    cardMaskingNo: string;

    // Số TK debit
    debitAccountNo: string;

    // Nội dung giao dịch
    transDesciption: string;

    // Ngày giao dịch
    transDate: string;

    // Số tiền phát sinh
    transAmt: number;

    // Hoàn tiền do khách hủy GD trước 24h
    cusCancelReturnBf24HAmt: number;

    // Số tiền chưa về
    notReceivedAmt: number;

    // Tổng số tiền đã về
    totalReceived: number;
}

export class FileDownload {
    static arrayBufferProcess(buffer, fileName, isError = false) {
        const blob = new Blob([buffer], { type: 'text/plain' });
        const reader = new FileReader();
        const t = this;
        reader.onload = function (evt) {
            const target: any = evt.target;
            const tmp = target.result as string;
            if (isError) {
                console.log(tmp);
            }
            else {
                const contentType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
                const blob = new Blob([buffer], { type: contentType });
                // Doing it this way allows you to name the file
                const link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = fileName;
                link.click();
            }
        };
        reader.readAsText(blob, 'UTF-8');
    }
}

export class TransactionMonthlyReportResult {
    pagingData: any;
    notReceivedAmtAfterPrevMonth: number;
    monthBookedAmt: number;
    monthTransAmtForPrevMonths: number;
    monthTransAmt: number;
    totalMonthAmt: number;
    remainNotReceivedAmtAfterPrevMonth: number;
    monthNotReceivedAmt: number;
    totalNotReceivedAmt: number;
}

export class TransactionMonthlyReportDTO {
    id: string;
    CreatedUser: string;
    createdDate: Date;
    updatedUser: string;
    updatedDate: Date;

    // Mã KH SB
    seaBankCustomerCode: string;

    // Họ tên khách
    customerName: string;

    // Sân Golf
    golfOrgCode: string;

    // Số tiền chưa về kỳ trước chuyển sang - 9
    notReceivedAmtAfterPrevMonth: number;

    // Số tiền khách book trong tháng - 10
    monthBookedAmt: number;

    // Số tiền hoàn trả của giao dịch kỳ trước - 11
    monthTransAmtForPrevMonths: number;

    // Số tiền về của giao dịch trong tháng - 12
    monthTransAmt: number;

    // Tổng số tiền về trong kỳ - 13 = (11 + 12)
    totalMonthAmt: number;

    // Số dư tiền chưa về của kỳ trước (lũy kế) - 14 = 9 - 11
    remainNotReceivedAmtAfterPrevMonth: number;

    // Số tiền chưa về p/s trong tháng - 15 = 10 - 12
    monthNotReceivedAmt: number;

    // Tổng số tiền chưa về đến kỳ này 16 = 14 + 15
    totalNotReceivedAmt: number;
}

export class TransactionSummaryReportResult {
    pagingData: any;
    /** Tháng */
    // Số tiền giao dịch thành công
    monthReceivedAmt: number;

    // Số tiền giao dịch chưa về TK
    monthNotReceivedAmt: number;

    // Tổng số tiền giao dịch
    monthTotalAmt: number;

    /** Lũy kế */
    // Số tiền giao dịch thành công
    receivedAmt: number;

    // Số tiền giao dịch chưa về TK
    notReceivedAmt: number;

    // Tổng số tiền giao dịch
    totalAmt: number;
}

export class TransactionSummaryReportDTO {
    id: string;
    CreatedUser: string;
    createdDate: Date;
    updatedUser: string;
    updatedDate: Date;

    // Mã KH SB
    seaBankCustomerCode: string;

    // Họ tên khách
    customerName: string;

    /** Tháng */
    // Số tiền giao dịch thành công
    monthReceivedAmt: number;

    // Số tiền giao dịch chưa về TK
    monthNotReceivedAmt: number;

    // Tổng số tiền giao dịch
    monthTotalAmt: number;

    /** Lũy kế */
    // Số tiền giao dịch thành công
    receivedAmt: number;

    // Số tiền giao dịch chưa về TK
    notReceivedAmt: number;

    // Tổng số tiền giao dịch
    totalAmt: number;
}

export class FtpTransJobHistory {
    id: string;
    dateRun: Date;
    dateId: Date;
    fileName: string;
    filePath: string;
    seabankFilePath: string;
    isFtpFile: boolean;
    isReadFile: boolean;
    isInsertData: boolean;
    status: string;    
}