import { Component, HostListener, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { LockReason, Organization, TeeSheetLock, TeeSheetLockLine } from 'src/app/booking_online/model/model';
import { LockReasonService } from 'src/app/booking_online/service/lock-reason.service';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { TeeSheetLockService } from 'src/app/booking_online/service/tee-sheet-lock.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { DialogTeeSheetLockLineComponent } from '../dialog-tee-sheet-lock-line/dialog-tee-sheet-lock-line.component';
import * as _ from 'lodash';
import { BookingLineService } from 'src/app/booking_online/service/booking.service';

@Component({
  selector: 'app-tee-sheet-lock-detail',
  templateUrl: './tee-sheet-lock-detail.component.html',
  styleUrls: ['./tee-sheet-lock-detail.component.scss']
})
export class TeeSheetLockDetailComponent extends BaseComponent implements OnInit  {

  isEdit = true;
  id : string;
  teeSheetLock: TeeSheetLock;
  oldTeeSheetLock: TeeSheetLock;

  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  listLockReason : LockReason[] = []
  lockReasonSelectControl = new FormControl()
  lockReasonFilteredOptions: Observable<LockReason[]>
  isReadonly = false;

  oldLines: TeeSheetLockLine[] = []

  range = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  constructor(
    public dialog: MatDialog, 
    protected _route: ActivatedRoute,
    protected _router: Router,
    private teeSheetLockService: TeeSheetLockService,
    private orgService: OrganizationService,
    private reasonService: LockReasonService,
    private lineSv: BookingLineService,
  ) {
    super(dialog);
  }


  async ngOnInit() {
    this.teeSheetLock = new TeeSheetLock();
    
    await this.initial();
    this._route.params.subscribe(params => {
      this.id = params['id'] 
    });

    if(this.id === "add") {
       this.isEdit = false;
       this.teeSheetLock.isActive = true
       this.teeSheetLock.lockStatus = ""
       this.setValue();
       
    }
    else {
       this.loadData();      
    }    
  }

  async initial(): Promise<void> {
     this.listOrganization = (await this.lineSv.getOrgByUserId())?.data || [];
     this.listLockReason = (await this.reasonService.getAll())?.data || [];
  }

  setValue() {
     this.teeSheetLock.teeSheetLockLines = this.teeSheetLock.teeSheetLockLines || []
     this.oldLines = Object.assign([], this.teeSheetLock.teeSheetLockLines);
    //  this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.teeSheetLock.c_Org_Id));
    //  this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
    //    startWith(''),
    //    map(value => this._filterOrganization(value))
    //  )
    //  this.lockReasonSelectControl.setValue(this.listLockReason.find(x => x.id == this.teeSheetLock.c_LockReason_Id));
    //  this.lockReasonFilteredOptions = this.lockReasonSelectControl.valueChanges.pipe(
    //    startWith(''),
    //    map(value => this._filterlockReason(value))
    //  )

     if (this.teeSheetLock && this.teeSheetLock.startDate && this.teeSheetLock.endDate) {
      this.range.setValue({start: this.teeSheetLock.startDate, end: this.teeSheetLock.endDate})
    }

    this.copyToSavedObject();
	
  }

  copyToSavedObject() {
    this.oldTeeSheetLock = Object.assign({}, this.teeSheetLock);
    this.oldTeeSheetLock.teeSheetLockLines =  Object.assign([], this.teeSheetLock.teeSheetLockLines);
  }

  private _filterOrganization(value: string): Organization[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listOrganization.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  async onSelectedOrganization(event: MatAutocompleteSelectedEvent) {
    if (this.teeSheetLock.teeSheetLockLines.length > 0) {
      let oldId = this.teeSheetLock.teeSheetLockLines[0].c_Org_Id;
      if (event.option.value?.id != oldId) {
        if ((await this.confirm('Bạn có muốn thay đổi đơn vị?. Nếu thay đổi sẽ xóa toàn bộ chi tiết!'))) {
          this.teeSheetLock.teeSheetLockLines = []
        }
        else {
          this.teeSheetLock.c_Org_Id = oldId;
          this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == oldId));
          
        }
      }
    }
  }

  changeOrganizationText() {
    this.teeSheetLock.c_Org_Id = this.organizationSelectControl.value?.id || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }
  
  private _filterlockReason(value: string): LockReason[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listLockReason.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  onSelectedlockReason(event: MatAutocompleteSelectedEvent) {
  }
  changelockReasonText() {
    this.teeSheetLock.c_LockReason_Id = this.lockReasonSelectControl.value?.id || null
  }
  
  displaylockReason(reason?: LockReason): string {
    return reason?.name || ''
  }

  loadData() {
    this.teeSheetLockService.get(this.id).then(
      res => {
        if (!res?.isSuccess) {
          this.alert(res.message)
          return
        }
        else {
          this.teeSheetLock = res.data;
          this.setValue();
        }
      },
      err => {
          this.alert('error');
          console.log(err);
      }
    );
  }
  
  dateChange() {
    this.teeSheetLock.startDate = this.range.value['start']
    this.teeSheetLock.endDate = this.range.value['end']

  }

  update() { 
    
    let lineIds = this.teeSheetLock.teeSheetLockLines.map(x => x.id);
    this.oldLines.forEach(x => {
      if (!lineIds.includes(x.id)) {
         x.isDeleted = true;
         this.teeSheetLock.teeSheetLockLines.push(x);
      }
    })

    this.teeSheetLock.startDate = this.range.value['start']
    this.teeSheetLock.endDate = this.range.value['end']

    if (this.teeSheetLock.startDate > this.teeSheetLock.endDate) {
      this.alert('Thời gian áp dụng không đúng');
    }
 
    
    this.teeSheetLockService.addOrEdit(this.teeSheetLock).then(
      res => {
         if (!res?.isSuccess) {
          this.alert(res.message)
          return;
         }
          this.alert('Cập nhật thành công');
          // tao moi
          if (res.data) {
              this.id = res.data;
          }
          this.loadData();
          this._router.navigate(['/booking/teesheet-lock', this.id]);
      },
      error => {
          console.log(error);
          this.error(error);
      })
  }
  async back() {
    if(_.isEqual(this.oldTeeSheetLock, this.teeSheetLock)) {
      this._router.navigateByUrl('/booking/teesheet-lock');
    }
    else {
      if(!await this.confirm('Chưa lưu dữ liệu! Bạn vẫn muốn thoát khỏi màn hình này?')) {
          return;
      }
      else {
        this._router.navigateByUrl('/booking/teesheet-lock');
      }
    }
  }

  addOrEditLine(line: TeeSheetLockLine) {
    if(!line) {
      line = new TeeSheetLockLine();
      line.isActive = true;
      line.gF_TeeSheetLock_Id = this.isEdit? this.id : null;
      line.c_Org_Id = this.teeSheetLock.c_Org_Id;
    }

    if (!line.c_Org_Id) {
       this.alert("Chưa chọn đơn vị");
       return;
    }
    const dialogRef = this.dialog.open(DialogTeeSheetLockLineComponent, {
      width: '50vw',
      data: Object.assign({},line)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.teeSheetLock.teeSheetLockLines.push(res);
      //this.loadData();
    })
  }

  async deleteLine(line: TeeSheetLockLine, index : number) {
    if(!await this.confirm('Bạn có muốn xóa dữ liệu?')) return;
    this.teeSheetLock.teeSheetLockLines.splice(index, 1);
  }

}
