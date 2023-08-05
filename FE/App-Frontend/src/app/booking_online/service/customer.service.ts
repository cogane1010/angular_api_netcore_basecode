import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { RespondData } from '../../common/model'

@Injectable({
  providedIn: 'root'
})
export class CustomerService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customer.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customer.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customer.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customer.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customer.delete
  _resetPasswordUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customer.resetPassword

  _uploadFileUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customer.uploadFile
  _downloadFileUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customer.downloadFile

  _changeStatusUrl: string = AppConfig.apiEndpoint + AppConfig.urls.customer.changeStatus


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }

  resetPassword(model: any): Promise<RespondData> {
    return this.postData(this._resetPasswordUrl, model)
  }

  uploadFile(formData: FormData) {
    return this.httpClient.post(this._uploadFileUrl, formData, {
        reportProgress: true,
        observe: 'events'
    });
  }

  downloadFile(fileId: any) {
    return new Promise((resolve, reject) => {
        this.httpClient.post(this._downloadFileUrl, { id: fileId }
            , { responseType: 'arraybuffer' }).subscribe((response: any) => {
                resolve(response);
            }, reject);
    });
  }

  changeStatus(model: any) {
    return this.postData(this._changeStatusUrl, model)
  }


}