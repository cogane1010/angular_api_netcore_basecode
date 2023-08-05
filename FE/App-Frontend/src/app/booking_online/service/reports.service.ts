import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData, GridFilterModel } from '../../../app/common/model'
import { SmsFilterModel } from '../model/paging'

@Injectable({
  providedIn: 'root'
})
export class ReportsService extends BaseGridService {
  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.delete
  _getCancelPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.reports.getCancelPaging
  _getRegistrationPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.reports.getRegistrationPaging
  _getRBookingPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.reports.getBookingPaging
  _getExportRBookingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.reports.exportRBookingUrl
  _getExportRegistrationUrl: string = AppConfig.apiEndpoint + AppConfig.urls.reports.exportRegistrationUrl

  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }

  getCancelPaging(filterModel: GridFilterModel): Promise<RespondData> {
    return this.postData(this._getCancelPagingUrl, filterModel)
  }

  getRegistraionPaging(filterModel: GridFilterModel): Promise<RespondData> {
    return this.postData(this._getRegistrationPagingUrl, filterModel)
  }

  getBookingPaging(filterModel: GridFilterModel): Promise<RespondData> {
    return this.postData(this._getRBookingPagingUrl, filterModel)
  }

  exportExcelbooking(filter: SmsFilterModel) {
    return new Promise((resolve, reject) => {
      this.httpClient.post(this._getExportRBookingUrl, filter
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
  exportExcelRegistration(filter: SmsFilterModel) {
    return new Promise((resolve, reject) => {
      this.httpClient.post(this._getExportRegistrationUrl, filter
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

}