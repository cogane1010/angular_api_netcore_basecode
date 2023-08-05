import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConfig } from 'src/app/common/app-config';
import { RespondData } from 'src/app/common/model';
import { BaseService } from 'src/app/common/service/base.service';

@Injectable({
    providedIn: 'root'
})
export class TestappService extends BaseService {

    private _sendMailUrl: string = AppConfig.apiEndpoint + AppConfig.urls.testApp.sendMail
    private _getServerTimeUrl: string = AppConfig.apiEndpoint + AppConfig.urls.testApp.getServerTime
    private _getTimeReturnUrl: string = AppConfig.apiEndpoint + AppConfig.urls.testApp.getTimeReturn

    constructor(protected httpClient: HttpClient) {
        super(httpClient);
    }

    sendMail(data: any): Promise<RespondData> {
        return this.postData(this._sendMailUrl, data)
    }

    getServerTime(): Promise<RespondData> {
        return this.post(this._getServerTimeUrl)
    }

    getTimeReturn(frontDate: Date): Promise<RespondData> {
        return this.postData(this._getTimeReturnUrl, {FrontDate: frontDate})
    }

}
