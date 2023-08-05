import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class OrganizationTypeService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organizationType.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organizationType.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organizationType.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organizationType.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organizationType.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}



@Injectable({
  providedIn: 'root'
})
export class OrganizationService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organization.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organization.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organization.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organization.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.organization.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}
