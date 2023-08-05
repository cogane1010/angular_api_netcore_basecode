import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CancelReason, Organization,Booking } from 'src/app/booking_online/model/model';
import { CancelReasonService } from 'src/app/booking_online/service/cancel-reason.service';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
   selector: 'app-cancel-booking-admin-dialog',
   templateUrl: './cancel-booking-admin-dialog.component.html',
   styleUrls: ['./cancel-booking-admin-dialog.component.scss']
})
export class CancelBookingAdminDialogComponent extends BaseComponent implements OnInit {
  
  cancelReason: CancelReason
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<CancelReason[]>
  listBooking : Booking[] = []
  listCancelReason: CancelReason[] = []
  cancelId: string
  description: string

  constructor(public dialogRef: MatDialogRef<CancelBookingAdminDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Booking[], 
    private cancelReasonService: CancelReasonService, 
    private organizationSv: OrganizationService,
    public dialog: MatDialog) 
  {
    super(dialog);
    this.dialogRef.disableClose = true
    this.listBooking = data || new Booking[1]
    
  }
  async ngOnInit() {
    this.listCancelReason = (await this.cancelReasonService.getAllCancelReason()).data || []
    console.log(this.listCancelReason)
    this.organizationSelectControl.setValue(this.listCancelReason);
    this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterOrganization(value))
    )
    console.log(this.organizationSelectControl)
  }

  private _filterOrganization(value: string): CancelReason[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listCancelReason.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  onSelectedOrganization(event) {
  }

  changeOrganizationText() {
    this.cancelId = this.organizationSelectControl.value?.id || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }

  cancelBookings() {
    this.listBooking.forEach(x => {
      x.cancel_Reason_Id = this.cancelId
      x.cancel_Description = this.description
    })
    
    this.cancelReasonService.cancelBookings(this.listBooking).then(
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

