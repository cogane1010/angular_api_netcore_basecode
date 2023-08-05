import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class CustomerGroupService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customerGroup.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customerGroup.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customerGroup.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customerGroup.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customerGroup.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}