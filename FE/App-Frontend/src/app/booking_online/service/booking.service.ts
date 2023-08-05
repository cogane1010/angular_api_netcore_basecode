import { Injectable } from '@angular/core'
import { HttpClient, HttpParams } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { BookingStatisticFilterModel } from '../model/paging'
import { GridFilterModel, RespondData } from 'src/app/common/model'

@Injectable({
  providedIn: 'root'
})
export class BookingService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.booking.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.booking.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.booking.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.booking.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.booking.delete
  _getPagingCancelUrl: string = AppConfig.apiEndpoint + AppConfig.urls.booking.getPagingCancel
  _getPagingHistoryUrl: string = AppConfig.apiEndpoint + AppConfig.urls.booking.getPagingHistory
  _getCheckPaymentUrl: string = AppConfig.apiEndpoint + AppConfig.urls.booking.getCheckPayment

  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
  
  getPagingCancel(filterModel: GridFilterModel): Promise<RespondData> {
    return this.postData(this._getPagingCancelUrl, filterModel)
  }

  getPagingHistory(filterModel: GridFilterModel): Promise<RespondData> {
    return this.postData(this._getPagingHistoryUrl, filterModel)
  }
  getCheckPayment(filterModel: GridFilterModel): Promise<RespondData> {
    return this.postData(this._getCheckPaymentUrl, filterModel)
  }
}


@Injectable({
  providedIn: 'root'
})

export class BookingLineService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingLine.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingLine.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingLine.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingLine.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingLine.delete
  _updateNoShowUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingLine.updateNoShow
  _getStatsUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingLine.getStats
  _getAllByUserIdUrl: string = AppConfig.apiEndpoint + AppConfig.urls.bookingLine.getOrgByUserId

  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }

  getStats(filter: BookingStatisticFilterModel): Promise<RespondData> {
    return this.postData(this._getStatsUrl, filter)
  }

  getOrgByUserId(): Promise<RespondData> {
    return this.post(this._getAllByUserIdUrl)
  }

  updateNoShow(model: any): Promise<RespondData> {
    return this.postData(this._updateNoShowUrl, model)
  }

}