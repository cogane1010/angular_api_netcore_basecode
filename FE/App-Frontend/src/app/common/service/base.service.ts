import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http'
import { RespondData } from '../model'

export interface IBaseService {
    post(url: string): Promise<any>
    postData(url: string, data: any): Promise<any>
    postParams(url: string, params: HttpParams): Promise<any>
}

export class BaseService implements IBaseService {

    constructor(protected httpClient: HttpClient) { }

    post(url: string): Promise<RespondData> {
        return this.httpClient.post<RespondData>(url, null).toPromise()
    }

    postData(url: string, data: any): Promise<RespondData> {
        return this.httpClient.post<RespondData>(url, data, {
            headers: new HttpHeaders({
                'Content-Type': 'application/json; charset=utf-8'
            })
        }).toPromise();
    }


    getData(url: string) {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json; charset=utf-8'
            })
          };
      
          return this.httpClient.get<any>(url, httpOptions).toPromise();
    }
    

    postParams(url: string, params: HttpParams): Promise<RespondData> {
        return this.httpClient.post<RespondData>(url, null, { params: params }).toPromise();
    }



}
