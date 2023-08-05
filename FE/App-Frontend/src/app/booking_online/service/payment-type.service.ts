import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class PaymentTypeService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.paymentType.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.paymentType.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.paymentType.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.paymentType.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.paymentType.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}