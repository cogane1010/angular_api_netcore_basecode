import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'

@Injectable({
  providedIn: 'root'
})
export class SmsHistoryService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.smsHistory.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}