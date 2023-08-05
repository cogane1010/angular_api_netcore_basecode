import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class HolidayService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.holiday.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.holiday.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.holiday.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.holiday.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.holiday.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}