import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CancelReason, Organization } from 'src/app/booking_online/model/model';
import { CancelReasonService } from 'src/app/booking_online/service/cancel-reason.service';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
  selector: 'app-dialog-cancel-reason-detail',
  templateUrl: './dialog-cancel-reason-detail.component.html',
  styleUrls: ['./dialog-cancel-reason-detail.component.scss']
})
export class DialogCancelReasonDetailComponent extends BaseComponent implements OnInit {
 
  cancelReason: CancelReason
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  constructor(public dialogRef: MatDialogRef<DialogCancelReasonDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    private cancelReasonService: CancelReasonService, 
    private organizationSv: OrganizationService,
    public dialog: MatDialog) 
  {
    super(dialog);
    this.dialogRef.disableClose = true
    this.cancelReason = data || new CancelReason()
    
  }
  async ngOnInit() {
    this.listOrganization = (await this.organizationSv.getAll()).data || []
    this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.cancelReason.c_Org_Id));
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
    this.cancelReason.c_Org_Id = this.organizationSelectControl.value?.id || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }

  addOrEdit(cancelReason: CancelReason) {
    this.cancelReasonService.addOrEdit(cancelReason).then(
      res => {
        if (!res?.isSuccess) {
          this.alert('Cập nhật thất bại')
          return
        }
        this.dialogRef.close(true);
        this.alert('Cập nhật thành công');
      },
      err => {
        console.log(err)
      }
    )
  }

}
