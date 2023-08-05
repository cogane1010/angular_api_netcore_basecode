import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { LockReason, Organization } from 'src/app/booking_online/model/model';
import { LockReasonService } from 'src/app/booking_online/service/lock-reason.service';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
  selector: 'app-dialog-lock-reason-detail',
  templateUrl: './dialog-lock-reason-detail.component.html',
  styleUrls: ['./dialog-lock-reason-detail.component.scss']
})
export class DialogLockReasonDetailComponent extends BaseComponent implements OnInit {

  lockReason: LockReason
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  constructor(public dialogRef: MatDialogRef<DialogLockReasonDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    private lockReasonService: LockReasonService, 
    private organizationSv: OrganizationService,
    public dialog: MatDialog) 
  {
    super(dialog);
    this.dialogRef.disableClose = true
    this.lockReason = data || new LockReason()
    
  }
  async ngOnInit() {
    this.listOrganization = (await this.organizationSv.getAll()).data || []
    this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.lockReason.c_Org_Id));
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
    this.lockReason.c_Org_Id = this.organizationSelectControl.value?.id || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }

  addOrEdit(LockReason: LockReason) {
    this.lockReasonService.addOrEdit(LockReason).then(
      res => {
        if (!res?.isSuccess) {
          this.alert('Cập nhật thất bại')
          return
        }
        this.dialogRef.close(true)
        this.alert('Cập nhật thành công')
      },
      err => {
        console.log(err)
      }
    )
  }

}
