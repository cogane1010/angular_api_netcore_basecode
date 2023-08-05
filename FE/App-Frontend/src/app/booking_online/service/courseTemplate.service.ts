import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'

@Injectable({
  providedIn: 'root'
})
export class CourseTemplateService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.courseTemplate.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.courseTemplate.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.courseTemplate.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.courseTemplate.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.courseTemplate.delete


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
}