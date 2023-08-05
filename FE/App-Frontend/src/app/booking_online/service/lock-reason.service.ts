import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class LockReasonService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.lockReason.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.lockReason.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.lockReason.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.lockReason.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.lockReason.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}