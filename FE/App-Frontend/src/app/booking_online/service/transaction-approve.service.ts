import { Injectable } from '@angular/core'
import { HttpClient, HttpParams } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { GridFilterModel, RespondData } from 'src/app/common/model'
import { BookingOutEmailSetting, OutTransactionDetails, OutTransactionHeader, SaveCompareModel, TransactionInFilter, TransactionNotApproveFilter, TransactionNotCompareFilter } from '../model/model'
import { BookingLineFilterModel } from '../model/paging'

@Injectable({
    providedIn: 'root'
})
export class TransactionInService extends BaseGridService {   

    _baseUrl = AppConfig.apiEndpoint;
    _getUrl: string = this._baseUrl + AppConfig.urls.Transaction.get;
    _getAllUrl: string = this._baseUrl + AppConfig.urls.Transaction.getAll;
    _getPagingUrl: string = this._baseUrl + AppConfig.urls.Transaction.getPaging;
    _addOrEditUrl: string = this._baseUrl + AppConfig.urls.Transaction.addOrEdit;
    _deleteUrl: string = this._baseUrl + AppConfig.urls.Transaction.delete;
    _uploadFileUrl: string = this._baseUrl + AppConfig.urls.Transaction.uploadFile;
    _getseabankFileUrl: string = this._baseUrl + AppConfig.urls.Transaction.seabankFileUrl;
    _getDetailsUrl: string = this._baseUrl + AppConfig.urls.Transaction.getDetails;
    _exportExcelFileOutUrl: string = this._baseUrl + AppConfig.urls.Transaction.exportExcelFileOut;
    _exportExcelFileOutAllUrl: string = this._baseUrl + AppConfig.urls.Transaction.exportExcelFileOutAll;
    _exportExcelConfirmMoneyUrl: string = this._baseUrl + AppConfig.urls.Transaction.exportExcelConfirmMoney;
    _getPagingOutFileUrl: string = this._baseUrl + AppConfig.urls.Transaction.getPagingOutFile;
    _getPagingOutFileAllUrl: string = this._baseUrl + AppConfig.urls.Transaction.getPagingOutFileAll;
    _getDetailOutFileUrl: string = this._baseUrl + AppConfig.urls.Transaction.getDetailOutFile;
    _approveInUrl: string = this._baseUrl + AppConfig.urls.Transaction.approveIn;
    _checkTransSaveUrl: string = this._baseUrl + AppConfig.urls.Transaction.checkTransSaveUrl;
    _approveOutUrl: string = this._baseUrl + AppConfig.urls.Transaction.approveOut;
    _unApproveOutUrl: string = this._baseUrl + AppConfig.urls.Transaction.unApproveOut;
    _getTransactionCompareUrl: string = this._baseUrl + AppConfig.urls.Transaction.getTransactionCompare;
    _saveCompareUrl: string = this._baseUrl + AppConfig.urls.Transaction.saveCompare;
    _sendEmailFileOutUrl: string = this._baseUrl + AppConfig.urls.Transaction.sendEmailFileOut;
    _getNumOfDayNotCompareUrl: string = this._baseUrl + AppConfig.urls.Transaction.getNumOfDayNotCompare;
    _getStatusCompareTransUrl: string = this._baseUrl + AppConfig.urls.Transaction.getStatusCompareTrans;
    _getNotCompareDateUrl: string = this._baseUrl + AppConfig.urls.Transaction.getNotCompareDate;
    _getNumOfDayNotApproveUrl: string = this._baseUrl + AppConfig.urls.Transaction.getNumOfDayNotApprove;
    _getNotApproveDateUrl: string = this._baseUrl + AppConfig.urls.Transaction.getNotApproveDate;
    _getDetailOutMoneyUrl: string = this._baseUrl + AppConfig.urls.Transaction.getDetailOutMoneyUrl;
    _getOutEmailSettingUrl: string = this._baseUrl + AppConfig.urls.Transaction.getOutEmailSetting;
    _saveOutEmailSettingUrl: string = this._baseUrl + AppConfig.urls.Transaction.saveOutEmailSetting;
    _testSendMailUrl: string = this._baseUrl + AppConfig.urls.Transaction.testSendMail;
    _cancelOutDataUrl: string = this._baseUrl + AppConfig.urls.Transaction.cancelOutDataUrl;
    _confirmMoneyUrl: string = this._baseUrl + AppConfig.urls.Transaction.confirmMoneyUrl;
    _cancelConfirmMoneyUrl: string = this._baseUrl + AppConfig.urls.Transaction.cancelConfirmMoneyUrl;
    _manualConfirmUrl: string = this._baseUrl + AppConfig.urls.Transaction.manualConfirmUrl;
    _getNumOfDayNotConfirmMoneyUrl: string = this._baseUrl + AppConfig.urls.Transaction.getNumOfDayNotConfirmMoneyUrl;
    _getNotConfirmMoneyDateUrl: string = this._baseUrl + AppConfig.urls.Transaction.getNotConfirmMoneyDate;
    _getNumOfDayNotConfirmMoneyCountUrl: string = this._baseUrl + AppConfig.urls.Transaction.getNotConfirmMoneyDateCountUrl;
    _getFtpTransPagingUrl: string = this._baseUrl + AppConfig.urls.Transaction.getFtpTransPagingUrl;
    _uploadFtpExcelUrl: string = this._baseUrl + AppConfig.urls.Transaction.uploadFtpExcelUrl;
    constructor(protected httpClient: HttpClient) {
        super(httpClient)
    }

    uploadFile(formData: FormData) {
        return this.httpClient.post(this._uploadFileUrl, formData, {
            reportProgress: true,
            observe: 'events'
        });
    }

    getseabankfile(filter: TransactionInFilter) {
        return this.postData(this._getseabankFileUrl, filter);       
    }

    getDetails(filterModel: GridFilterModel): Promise<RespondData> {
        return this.postData(this._getDetailsUrl, filterModel)
    }

    exportExcel(filter: GridFilterModel) {
        return new Promise((resolve, reject) => {
            this.httpClient.post(this._exportExcelFileOutUrl, filter
                , { observe: 'response', responseType: 'blob' })
                .subscribe((res: any) => {
                    // file name
                    const contentDisp = res.headers.get('content-disposition');
                    const fileName = this.getDownloadFileName(contentDisp);

                    const data = {
                        data: new Blob([res.body], { type: res.headers.get('Content-Type') }),
                        filename: fileName
                    }                    
                    resolve(data);
                }, reject);
        });
    }

    getOutTransPaging(filterModel: GridFilterModel): Promise<RespondData> {
        return this.postData(this._getPagingOutFileUrl, filterModel)
    }

    getOutTransPagingAll(filterModel: GridFilterModel): Promise<RespondData> {
        return this.postData(this._getPagingOutFileAllUrl, filterModel)
    }

    getOutTransDetail(filterModel: GridFilterModel): Promise<RespondData> {
        return this.postData(this._getDetailOutFileUrl, filterModel)
    }

    approveIn(item: any) {
        return this.postData(this._approveInUrl, item)
    }

    checkTransSave(item: any) {
        return this.postData(this._checkTransSaveUrl, item)
    }

    approveOut(item: OutTransactionHeader) {
        return this.postData(this._approveOutUrl, item)
    }

    unApproveOut(item: OutTransactionHeader) {
        return this.postData(this._unApproveOutUrl, item)
    }

    getTransactionCompare(filter: TransactionInFilter) {
        return this.postData(this._getTransactionCompareUrl, filter)
    }

    getDetailOutMoneyData(filter: TransactionInFilter) : Promise<RespondData>{
        return this.postData(this._getDetailOutMoneyUrl, filter)
    }

    saveCompare(filter: SaveCompareModel) {
        return this.postData(this._saveCompareUrl, filter)
    }

    cancelOutDataTrans(data: OutTransactionDetails[]) {
        return this.postData(this._cancelOutDataUrl, data)
    }

    confirmMoney(data: OutTransactionDetails[]) {
        return this.postData(this._confirmMoneyUrl, data)
    }

    cancelconfirmMoney(data: OutTransactionDetails[]) {
        return this.postData(this._cancelConfirmMoneyUrl, data)
    }

    manualConfirmMoney(data: OutTransactionDetails) {
        return this.postData(this._manualConfirmUrl, data)
    }

    exportExcelAll(filter: GridFilterModel) {
        return new Promise((resolve, reject) => {
            this.httpClient.post(this._exportExcelFileOutAllUrl, filter
                , { observe: 'response', responseType: 'blob' })
                .subscribe((res: any) => {
                    // file name
                    const contentDisp = res.headers.get('content-disposition');
                    const fileName = this.getDownloadFileName(contentDisp);

                    const data = {
                        data: new Blob([res.body], { type: res.headers.get('Content-Type') }),
                        filename: fileName
                    }
                    console.log(data);
                    resolve(data);
                }, reject);
        });
    }

    uploadFtpExcel() {
        return this.post(this._uploadFtpExcelUrl);
    }

    getDownloadFileName(contentDisp: any) {
        let fileName = "result_out.xlsx";
        if (contentDisp && typeof contentDisp === 'string' && contentDisp.trim() !== '') {
            var r = new RegExp("(?:filename=)(.*?)(?:;)");
            var x = r.exec(contentDisp);
            if (x && x.length > 0) {
                fileName = x[x.length - 1];
            }
        }
        return fileName;
    }

    exportExcelConfirmMoney(filter: GridFilterModel) {
        return new Promise((resolve, reject) => {
            this.httpClient.post(this._exportExcelConfirmMoneyUrl, filter
                , { observe: 'response', responseType: 'blob' })
                .subscribe((res: any) => {
                    // file name
                    const contentDisp = res.headers.get('content-disposition');
                    const fileName = this.getDownloadFileName(contentDisp);

                    const data = {
                        data: new Blob([res.body], { type: res.headers.get('Content-Type') }),
                        filename: fileName
                    }
                    console.log(data);
                    resolve(data);
                }, reject);
        });
    }


    sendEmailFileOut(filter) {
        return this.postData(this._sendEmailFileOutUrl, filter)
    }

    getNumOfDayNotCompare(): Promise<RespondData> {
        return this.postData(this._getNumOfDayNotCompareUrl, null);
    }

    getStatusCompareTrans(filter): Promise<RespondData> {
        return this.postData(this._getStatusCompareTransUrl, filter);
    }

    getNumOfDayNotConfirmMoney(): Promise<RespondData> {
        return this.postData(this._getNumOfDayNotConfirmMoneyUrl, null);
    }

    getNumOfDayNotConfirmMoneyCount(): Promise<RespondData> {
        return this.postData(this._getNumOfDayNotConfirmMoneyUrl, null);
    }

    getNotCompareDate(filter: TransactionNotCompareFilter) {
        return this.postData(this._getNotCompareDateUrl, filter);
    }

    getNotConfirmMoneyDate(filter: TransactionNotCompareFilter) {
        return this.postData(this._getNotConfirmMoneyDateUrl, filter);
    }

    getNumOfDayNotApprove(): Promise<RespondData> {
        return this.postData(this._getNumOfDayNotApproveUrl, null);
    }

    getNotApproveDate(filter: TransactionNotApproveFilter) {
        return this.postData(this._getNotApproveDateUrl, filter);
    }
    
    getOutEmailSetting() {
        return this.getData(this._getOutEmailSettingUrl);
    }

    saveOutEmailSetting(item: BookingOutEmailSetting) {
        return this.postData(this._saveOutEmailSettingUrl, item);
    }

    testSendMail(item: BookingOutEmailSetting) {
        return this.postData(this._testSendMailUrl, item);
    }

    getFtpTransPaging(filterModel: GridFilterModel): Promise<RespondData> {
        return this.postData(this._getFtpTransPagingUrl, filterModel)
    }
}