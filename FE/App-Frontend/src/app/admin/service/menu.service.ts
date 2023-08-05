import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
    providedIn: 'root'
})
export class MenuService extends BaseGridService {

    _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.menu.get
    _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.menu.getAll
    _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.menu.getPaging
    _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.menu.addOrEdit
    _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.menu.delete

    constructor(protected httpClient: HttpClient) {
        super(httpClient)
    }

}
