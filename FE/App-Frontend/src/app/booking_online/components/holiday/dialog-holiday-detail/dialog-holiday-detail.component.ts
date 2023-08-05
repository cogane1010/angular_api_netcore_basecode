import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Holiday, Organization } from 'src/app/booking_online/model/model';
import { BookingLineService } from 'src/app/booking_online/service/booking.service';
import { HolidayService } from 'src/app/booking_online/service/holiday.service';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
  selector: 'app-dialog-holiday-detail',
  templateUrl: './dialog-holiday-detail.component.html',
  styleUrls: ['./dialog-holiday-detail.component.scss']
})
export class DialogHolidayDetailComponent extends BaseComponent implements OnInit {

  holiday: Holiday
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  constructor(public dialogRef: MatDialogRef<DialogHolidayDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    private holidayService: HolidayService, 
    private organizationSv: OrganizationService,
    private bookingLineService: BookingLineService,    
    public dialog: MatDialog) 
  {
    super(dialog);
    this.dialogRef.disableClose = true
    this.holiday = data || new Holiday()
    
  }
  async ngOnInit() {
    this.listOrganization = (await this.bookingLineService.getOrgByUserId()).data || []
    if(this.listOrganization.length == 1){
      this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.listOrganization[0].id));
      this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
        startWith(''),
        map(value => this._filterOrganization(value))
      )
    }
    if(this.holiday){
      this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.holiday.c_Org_Id));
      this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
        startWith(''),
        map(value => this._filterOrganization(value))
      )
    }    
  }

  private _filterOrganization(value: string): Organization[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listOrganization.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  onSelectedOrganization(event) {
  }
  changeOrganizationText() {
    this.holiday.c_Org_Id = this.organizationSelectControl.value?.id || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }

  addOrEdit(holiday: Holiday) {
    this.holidayService.addOrEdit(holiday).then(
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
