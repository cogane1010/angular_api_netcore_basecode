import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class BookingOtherTypeService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingOtherType.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingOtherType.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingOtherType.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingOtherType.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingOtherType.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}