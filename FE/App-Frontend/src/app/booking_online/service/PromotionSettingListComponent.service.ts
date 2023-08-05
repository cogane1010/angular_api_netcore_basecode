import { Injectable } from '@angular/core'
import { HttpClient,HttpHeaders, HttpParams } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData, GridFilterModel } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class PromotionSettingComponentService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.delete
  _getCustomersUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.getCustomers
  _getAllCourseUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.getAllCourse

  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }

  getCustomers(): Promise<RespondData> {
    return this.post(this._getCustomersUrl)
  }

  addOrEditWithImage(formData: FormData): Promise<RespondData> {
    return this.httpClient.post<RespondData>(this._addOrEditUrl, formData, {
      headers: new HttpHeaders({
          'Content-Type': 'application/json; charset=utf-8'
        })
    }).toPromise();
    //return this.postData(this._addOrEditUrl, model)
  }

}