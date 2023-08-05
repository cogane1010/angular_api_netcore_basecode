import { HttpClient } from '@angular/common/http'
import { RespondData, GridFilterModel } from '../model'
import { BaseDataService, IBaseDataService } from '.'
import { TokenStorage } from 'src/app/admin/auth/token.storage';

export interface IBaseGridService extends IBaseDataService {
    getPaging(filterModel: GridFilterModel): Promise<RespondData>
}

export abstract class BaseGridService extends BaseDataService implements IBaseGridService {

    abstract _getPagingUrl: string

    constructor(protected httpClient: HttpClient) {
        super(httpClient);
    }

    getPaging(filterModel: GridFilterModel): Promise<RespondData> {
        return this.postData(this._getPagingUrl, filterModel)
    }
}
