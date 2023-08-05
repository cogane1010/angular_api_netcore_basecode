import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConfig } from '../app-config';
import { RespondData } from '../model';
import { BaseService } from './base.service';

@Injectable({
    providedIn: 'root'
})
export class CommonService extends BaseService {

    constructor(protected httpClient: HttpClient) {
        super(httpClient);
    }

}
