import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class TeeSheetLockService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLock.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLock.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLock.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLock.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLock.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}


@Injectable({
  providedIn: 'root'
})
export class TeeSheetLockLineService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLockLine.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLockLine.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLockLine.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLockLine.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.teeSheetLockLine.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}