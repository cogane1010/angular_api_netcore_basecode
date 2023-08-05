import { Injectable } from '@angular/core'
import { HttpClient } from "@angular/common/http"
import { AppConfig } from '../../common/app-config'
import { BaseGridService } from '../../common/service'
import { MemberCardFilterModel } from '../model/paging'
import { RespondData } from 'src/app/common/model'
import { MemberCard } from '../model/model'


@Injectable({
  providedIn: 'root'
})
export class MemberCardService extends BaseGridService {

  _getUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberCardService.get
  _getAllUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberCardService.getAll
  _getPagingUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberCardService.getPaging
  _addOrEditUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberCardService.addOrEdit
  _deleteUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberCardService.delete


  _searchUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberCardService.search
  _assignUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberCardService.assign
  _reassignUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberCardService.reassign
  _refreshUrl: string = AppConfig.apiEndpoint + AppConfig.urls.memberCardService.refresh


  constructor(protected httpClient: HttpClient) {
    super(httpClient)
  }
  
  search(filter: MemberCardFilterModel): Promise<RespondData> {
    return this.postData(this._searchUrl, filter)
  }

  assign(memberCard: MemberCard): Promise<RespondData>  {
    return this.postData(this._assignUrl, memberCard)
  }
  reassign(memberCard: MemberCard): Promise<RespondData>  {
    return this.postData(this._reassignUrl, memberCard)
  }
  refresh(memberCard: MemberCard): Promise<RespondData>  {
    return this.postData(this._refreshUrl, memberCard)
  }
 
 
}


