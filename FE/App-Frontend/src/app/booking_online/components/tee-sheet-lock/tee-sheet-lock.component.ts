import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { LockReason, Organization, TeeSheetLock } from '../../model/model';
import { TeeSheetLockFilterModel } from '../../model/paging';
import { BookingLineService } from '../../service/booking.service';
import { LockReasonService } from '../../service/lock-reason.service';
import { OrganizationService } from '../../service/organization.service';
import { TeeSheetLockService } from '../../service/tee-sheet-lock.service';


@Component({
  selector: 'app-tee-sheet-lock',
  templateUrl: './tee-sheet-lock.component.html',
  styleUrls: ['./tee-sheet-lock.component.scss']
})
export class TeeSheetLockComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'lockReasonName', 'description', 'organizationName', 'startDate', 'endDate', 'isActive', 'actions'];
  filter: TeeSheetLockFilterModel = new TeeSheetLockFilterModel()
  listTeeSheetLock : TeeSheetLock[] = []
  isReadonly:boolean= false;
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  listLockReason : LockReason[] = []
  lockReasonSelectControl = new FormControl()
  lockReasonFilteredOptions: Observable<LockReason[]>

  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( 
    private teeSheetLockService: TeeSheetLockService, 
    private reasonService: LockReasonService,
    private orgService: OrganizationService,
    private lineSv: BookingLineService,
    public dialog: MatDialog,
     ) { 
     super(dialog)
   }

  ngOnInit() {
    this.initial();    
  }

  async initial() {
    this.listOrganization = (await this.lineSv.getOrgByUserId())?.data || [];
    if(this.listOrganization.length == 1){
      this.filter.c_Org_Id = this.listOrganization[0].id;      
      this.isReadonly = true;      
    }else{
      this.isReadonly = false;
    }
    
    this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterOrganization(value))
    )

    this.listLockReason = (await this.reasonService.getAll())?.data || [];
    this.lockReasonFilteredOptions = this.lockReasonSelectControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterlockReason(value))
    )
    this.setValue() ;
    this.getData();
  }
  
  setValue() {
    this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id === this.filter.c_Org_Id));
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
  
  private _filterlockReason(value: string): LockReason[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listLockReason.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  onSelectedlockReason(event) {
  }
  changelockReasonText() {
    this.filter.c_LockReason_Id = this.lockReasonSelectControl.value?.id || null
  }
  
  displaylockReason(reason?: LockReason): string {
    return reason?.name || ''
  }
  search() {
    this.filter.pageIndex = 0
    this.paginator.length = 0
    this.getData()
  }

  getData() {
    this.teeSheetLockService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listTeeSheetLock = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }




  async delete(item: TeeSheetLock) {
    if (!(await this.confirm('Xóa?'))) return;

    this.teeSheetLockService.detele(item.id).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res)
          alert('error')
        }
        this.getData()
      },
      err => {
        console.error(err)
      }
    )
  }

}
