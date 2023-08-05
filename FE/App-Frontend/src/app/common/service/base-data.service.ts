import { HttpClient, HttpParams } from '@angular/common/http'
import { BaseService } from './base.service'
import { RespondData } from '../model'
import { AppConfig } from '../../common/app-config'

export interface IBaseDataService {
    get(id: string): Promise<RespondData>
    getAll(): Promise<RespondData>
    addOrEdit(model: any): Promise<RespondData>
    detele(model: any): Promise<RespondData>
}

export abstract class BaseDataService extends BaseService implements IBaseDataService {

    abstract _getUrl: string
    abstract _getAllUrl: string
    abstract _addOrEditUrl: string
    abstract _deleteUrl: string
    _roleUserUrl: string = AppConfig.apiEndpoint + AppConfig.urls.user.roleUser

    constructor(protected httpClient: HttpClient) {
        super(httpClient)
    }

    get(id: string): Promise<RespondData> {
        var params = (new HttpParams()).append('id', id)
        return this.postParams(this._getUrl, params)
    }

    getAll(): Promise<RespondData> {
        return this.post(this._getAllUrl)
    }
    getRoleUser(id: string): Promise<RespondData> {
        var params = (new HttpParams()).append('userId', id)
        return this.postParams(this._roleUserUrl, params)
       
    }

    addOrEdit(model: any): Promise<RespondData> {
        return this.postData(this._addOrEditUrl, model)
    }

    
    detele(id: string): Promise<RespondData> {
        var params = (new HttpParams()).append('id', id)
        return this.postParams(this._deleteUrl, params)
    }


}
