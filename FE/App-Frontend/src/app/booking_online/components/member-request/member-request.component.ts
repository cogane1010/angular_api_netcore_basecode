import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { MemberRequest, Organization } from '../../model/model';
import { MemberRequestFilterModel } from '../../model/paging';
import { BookingLineService } from '../../service/booking.service';
import { MemberRequestService } from '../../service/member-request.service';
import { OrganizationService } from '../../service/organization.service';


@Component({
  selector: 'app-member-request',
  templateUrl: './member-request.component.html',
  styleUrls: ['./member-request.component.scss']
})
export class MemberRequestComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'fullName', 'mobilePhone', 'email', 'organizationName', 'request_Date', 'request_user','description'];
  filter: MemberRequestFilterModel = new MemberRequestFilterModel()
  listMemberRequest : MemberRequest[] = []
  orgDisable:boolean = false;
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( 
     private memberRequestService: MemberRequestService, 
     private orgService: OrganizationService,
     private bookingLineService: BookingLineService,
     public dialog: MatDialog) { 
     super(dialog)
   }

  async ngOnInit() {
    await this.initial()
    this.getData()
  }

  async initial () {
    this.listOrganization = (await this.bookingLineService.getOrgByUserId())?.data || [];
    if(this.listOrganization.length == 1){
      this.filter.c_Org_Id = this.listOrganization[0]?.id;
      this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id === this.filter.c_Org_Id));
      this.orgDisable = true;
    }else{
      this.orgDisable = false;
    }
    this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterOrganization(value))
    )
  }


  private _filterOrganization(value: string): Organization[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listOrganization.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  onSelectedOrganization(event) {
  }
  changeOrganizationText() {
    this.filter.c_Org_Id = this.organizationSelectControl.value?.id || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }
  

  search() {
    this.filter.pageIndex = 0
    this.paginator.length = 0
    this.getData()
    
  }

  getData() {
    this.memberRequestService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listMemberRequest = res.data.data
        this.paginator.length = res.data.count
      }
    )
   
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  saveNote(request: MemberRequest) {
    this.memberRequestService.saveNote(request).then(
      res => {
         if (!res?.isSuccess) {
          this.alert(res.message)
          return
         }
          this.alert('Cập nhật thành công');
      },
      error => {
          console.log(error);
          this.error(error);
      })
  }
}
