import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { BookingLine, Course, Organization } from 'src/app/booking_online/model/model';
import { BookingLineFilterModel } from 'src/app/booking_online/model/paging';
import { BookingLineService } from 'src/app/booking_online/service/booking.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { BookingDetailComponent } from '../booking-detail/booking-detail.component';

@Component({
  selector: 'app-dialog-booking-search',
  templateUrl: './dialog-booking-search.component.html',
  styleUrls: ['./dialog-booking-search.component.scss']
})
export class DialogBookingSearchComponent extends BaseComponent implements OnInit {
  listBooking: BookingLine[] = []

  filter: BookingLineFilterModel;

  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  curOrg: Organization
  disabledOrgFilter = false;
  disabledCourseFilter = false;
  enableSearch = true;

  listCourse : Course[] = []
  courseSelectControl = new FormControl()
  courseFilteredOptions: Observable<Course[]>

  constructor(
    public dialogRef: MatDialogRef<DialogBookingSearchComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    public bookingLineService: BookingLineService, 
    protected _router: Router,
    public dialog: MatDialog
  ) {
    super(dialog);
    this.filter = data.filter || new BookingLineFilterModel()
    this.filter.pageSize = -1;
    this.listOrganization = data.listOrg || []
    this.curOrg = data.curOrg || null;

  }

  ngOnInit() {
    this.initial()
  }

  async initial() {    
    if (this.curOrg) {
      if(!this.curOrg.isSummary) {
        this.filter.c_Org_Id = this.curOrg.id;
        this.disabledOrgFilter = true
      } 
      else {
         
      }
    }
    else {
      this.disabledOrgFilter = true
      this.disabledCourseFilter = true
      this.enableSearch = false;
    }
    this.setValue();
  }

  setValue() {
    this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id === this.filter.c_Org_Id));
    this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
     startWith(''),
     map(value => this._filterOrganization(value))
   )
   this.setValueCourse();
  
 }
 setValueCourse() {
   if (this.curOrg) {
     this.listCourse = this.listOrganization.find(x => x.id === this.filter.c_Org_Id)?.courses || [] 
     if (this.listCourse.length > 1) {
       if (!this.filter.c_Course_Id) 
          this.enableSearch = false;
       this.disabledCourseFilter = false;
     }
     else {
       if(this.listCourse.length == 1) {
         this.filter.c_Course_Id = this.listCourse[0].id;
         this.disabledCourseFilter = true;
       }
       else {
         this.disabledCourseFilter = true;
       }
     }

   }
   else {
     this.listCourse = []
   }
   
   this.courseSelectControl.setValue(this.listCourse.find(x => x.id === this.filter.c_Course_Id));
   this.courseFilteredOptions = this.courseSelectControl.valueChanges.pipe(
     startWith(''),
     map(value => this._filterCourse(value))
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

  private _filterCourse(value: string): Course[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listCourse.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  onSelectedCourse(event) {
    this.enableSearch = true;
  }
  changeCourseText() {
     this.enableSearch = false;
     this.filter.c_Course_Id = this.courseSelectControl.value?.id || null
  }
  
  displayCourse(org?: Course): string {
    return org?.name || ''
  }
   
  async search() {
    if (this.filter.dateId == null) {
      this.filter.dateId = new Date();
    }

    if (this.enableSearch){
      this.filter.isSearch = true;
      this.listBooking = (await this.bookingLineService.getPaging(this.filter))?.data?.data || [];
      
      let turnLength= 100; // fake
      this.listBooking.forEach(x => {
        x.turnLength = turnLength
        x.turn = new Date(x.tee_Time)
        x.turn.setMinutes(x.turn.getMinutes() + x.turnLength)
      })
    }
  }

  goToDetail(row: BookingLine) {
    // this.dialogRef.close(true);
    // this._router.navigate(['/booking/booking-detail', row.gF_Booking_Id])

    const dialogRef = this.dialog.open(BookingDetailComponent, {
      width: '85vw',
      height:'60vw',
      maxWidth: '85vw',
      data: row.gF_Booking_Id
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      //this.getData();
    })
  }


}
