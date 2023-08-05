import { Injectable } from '@angular/core'
import { HttpClient,HttpHeaders, HttpParams } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData, GridFilterModel } from '../../common/model'
import { GF_Notification,FirebaseBody } from '../model/model'

@Injectable({
  providedIn: 'root'
})
export class NotificationService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.notificationSetting.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.notificationSetting.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.notificationSetting.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.notificationSetting.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.notificationSetting.delete
  _getCustomersUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.getCustomers
  _getAllCourseUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.getAllCourse
  _pushAllNotifiUrl: string = AppConfig.apiEndpoint + AppConfig.urls.promotionSetting.pushAllNotifi
  _notifyServerUrl: string = AppConfig.notifyServer

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

  sendNotification(model: FirebaseBody): Promise<RespondData> {
    return this.httpClient.post<RespondData>(this._notifyServerUrl, model, {
      headers: new HttpHeaders({
          'Content-Type': 'application/json; charset=utf-8'
        })
    }).toPromise();
  }

  pushNotifi(model: any): Promise<RespondData> {
    return this.postData(this._pushAllNotifiUrl, model)
  }
}