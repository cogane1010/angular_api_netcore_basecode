import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class MemberRequestService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberRequest.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberRequest.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberRequest.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberRequest.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberRequest.delete
  _saveNoteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberRequest.saveNote


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }

  saveNote(model: any): Promise<RespondData> {
    return this.postData(this._saveNoteUrl, model)
  }
}