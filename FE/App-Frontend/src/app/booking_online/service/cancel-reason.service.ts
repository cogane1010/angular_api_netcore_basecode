import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { GridFilterModel, RespondData } from '../../common/model'
import { Booking } from 'src/app/booking_online/model/model';

@Injectable({
  providedIn: 'root'
})
export class CancelReasonService extends BaseGridService {
   

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.cancelReason.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.cancelReason.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.cancelReason.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.cancelReason.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.cancelReason.delete
  _getAlCancelReasonlUrl: string = AppConfig.apiEndpoint + AppConfig.urls.cancelReason.getAllCancelReason
  _cancelBookingsUrl: string = AppConfig.apiEndpoint + AppConfig.urls.cancelReason.cancelBookingsFromWeb

  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }

  getAllCancelReason(): Promise<RespondData> {
    return this.post(this._getAlCancelReasonlUrl)
  }

  cancelBookings(listBooking: Booking[]) : Promise<RespondData>{
    return this.postData(this._cancelBookingsUrl,listBooking)
  }

}