import { Injectable } from '@angular/core'
import { HttpClient, HttpParams } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
    providedIn: 'root'
})
export class SettingService extends BaseGridService {

    _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.setting.get
    _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.setting.getAll
    _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.setting.getPaging
    _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.setting.addOrEdit
    _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.setting.delete
    _getByCodeUrl: string = AppConfig.apiEndpoint + AppConfig.urls.setting.getByCode

    constructor(protected httpClient: HttpClient) {
        super(httpClient)
    }

    getByCode(code): Promise<RespondData> {
        var params = (new HttpParams()).append('code', code);
        return this.postParams(this._getByCodeUrl, params);
    }
}
