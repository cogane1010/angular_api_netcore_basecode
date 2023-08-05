import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData, GridFilterModel } from '../../../app/common/model'

@Injectable({
  providedIn: 'root'
})
export class TransactionDetailReportService extends BaseGridService {

  _getUrl: string;
  _getAllUrl: string;
  _getPagingUrl: string;
  _addOrEditUrl: string;
  _deleteUrl: string;
  _getReportDataUrl: string = AppConfig.apiEndpoint + AppConfig.urls.transactionDetailReport.getReportData
  _exportExcelUrl: string = AppConfig.apiEndpoint + AppConfig.urls.transactionDetailReport.exportExcel

  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }

  getReportData(filterModel: GridFilterModel): Promise<RespondData> {
    return this.postData(this._getReportDataUrl, filterModel)
  }
    
  exportExcel(filter: GridFilterModel) {
      return new Promise((resolve, reject) => {
          this.httpClient.post(this._exportExcelUrl, filter
              , { responseType: 'arraybuffer' })
              .subscribe((response: any) => {
                  resolve(response);
              }, reject);
      });
  }
}