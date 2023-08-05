import { Injectable } from '@angular/core'
import { HttpClient, HttpParams } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from 'src/app/common/model'

@Injectable({
  providedIn: 'root'
})
export class CourseService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.course.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.course.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.course.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.course.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.course.delete
  _getCourseByUserUrl: string = AppConfig.apiEndpoint + AppConfig.urls.course.getAllByUser
  _getByOrgUrl: string = AppConfig.apiEndpoint + AppConfig.urls.course.getByOrg


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }

  getByOrg(c_Org_Id: string): Promise<RespondData> {
    var params = (new HttpParams()).append('orgId', c_Org_Id)
    return this.postParams(this._getByOrgUrl, params)
  }

  getCourseByUser(): Promise<RespondData> {
    return this.post(this._getCourseByUserUrl)
  }

}