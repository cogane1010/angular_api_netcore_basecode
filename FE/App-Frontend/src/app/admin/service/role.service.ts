import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
    providedIn: 'root'
})
export class RoleService extends BaseGridService {

    _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.role.get
    _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.role.getAll
    _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.role.getPaging
    _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.role.addOrEdit
    _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.role.delete
    _getTreeMenuPermissionUrl: string = AppConfig.apiEndpoint + AppConfig.urls.role.getTreeMenuPermission

    constructor(protected httpClient: HttpClient) {
        super(httpClient)
    }

    getTreeMenuPermission(): Promise<RespondData> {
        return this.post(this._getTreeMenuPermissionUrl)
    }

}
