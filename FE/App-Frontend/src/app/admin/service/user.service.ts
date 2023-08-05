import { Injectable } from '@angular/core'
import { HttpClient, HttpParams } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
    providedIn: 'root'
})
export class UserService extends BaseGridService {

    _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.get
    _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.getAll
    _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.getPaging
    _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.addOrEdit
    _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.delete
    _getUserMenuUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.getUserMenu
    _addOrEditUserUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.addOrEditUser

    _changePasswordUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.changePassword
    _getOrgUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.getOrg

    constructor(protected httpClient: HttpClient) {
        super(httpClient)
    }

    changePassword(data: any): Promise<RespondData> {
        return this.postData(this._changePasswordUrl, data)
    }

    getUserMenu(): Promise<RespondData> {
        return this.post(this._getUserMenuUrl)
    }

    addOrEditUser(data: any): Promise<RespondData> {
        return this.postData(this._addOrEditUserUrl, data)
    }

    getOrg(): Promise<RespondData> {
        return this.post(this._getOrgUrl)
    }
}
