import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { BookingLineStatistic, BookingStatisticCourseModel, BookingStatisticOrgModel, BookStatByOrg, Organization } from 'src/app/booking_online/model/model';
import { BookingStatisticFilterModel } from 'src/app/booking_online/model/paging';
import { BookingLineService } from 'src/app/booking_online/service/booking.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import * as _ from 'lodash';
import { Constant } from 'src/app/common/enum';

@Component({
  selector: 'app-dialog-booking-statistic',
  templateUrl: './dialog-booking-statistic.component.html',
  styleUrls: ['./dialog-booking-statistic.component.scss']
})
export class DialogBookingStatisticComponent extends BaseComponent implements OnInit {
  filter: BookingStatisticFilterModel = new BookingStatisticFilterModel();
  curOrg: Organization
  disabledOrgFilter = false;
  enableSearch = true;
  listBookingLineStats: BookStatByOrg[] = [];
  listOrgStatistic: BookingStatisticOrgModel[] = []
  totalBooking: number = 0;
  totalReality: number = 0;

  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  constructor(
    public dialogRef: MatDialogRef<DialogBookingStatisticComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    public bookingLineSv: BookingLineService,
    public dialog: MatDialog
  ) {
    super(dialog);
    this.filter = data.filter || new BookingStatisticFilterModel()
    this.listOrganization = data.listOrg || []
    this.curOrg = data.curOrg || null;
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
      this.enableSearch = false;
    }
    
  }

  ngOnInit() {
    this.initial();
  }

  sum(listNo : number[]): number {
    return listNo.reduce((a, b) => a+b, 0);
  }

  processStats() {
    // this.listOrgStatistic = []
    // this.totalBooking = this.sum(this.listBookingLineStats.map(x => x.noBooking));
    // this.totalReality = this.sum(this.listBookingLineStats.map(x => x.noReality));
    // let lstOrgId = _.uniq(this.listBookingLineStats.map(x => x.c_Org_Id));
    // lstOrgId.forEach(orgId => {
    //    let listBookingLineStatsOrg =  this.listBookingLineStats.filter(x => x.c_Org_Id == orgId);
    //    let organizationName = listBookingLineStatsOrg.find(x => x.c_Org_Id == orgId)?.organizationName
    //    let orgStat: BookingStatisticOrgModel = {
    //      orgId: orgId,
    //      organizationName: organizationName,
    //      numRows: 0,
    //      noBooking: this.sum(listBookingLineStatsOrg.map(x => x.noBooking)),
    //      noReality: this.sum(listBookingLineStatsOrg.map(x => x.noReality)),
    //      listCourse: []
    //    };

    //    let lstCourseId = _.uniq(listBookingLineStatsOrg.map(x => x.c_Course_Id))
    //    orgStat.numRows = lstCourseId.length;
       
    //    lstCourseId.forEach(courseId => {
    //       let listBookingLineStatsCourse =  listBookingLineStatsOrg.filter(x => x.c_Course_Id == courseId);
    //       let courseName = listBookingLineStatsCourse.find(x => x.c_Course_Id == courseId)?.courseName

    //       let courseStat: BookingStatisticCourseModel = {
    //         courseId: courseId,
    //         courseName: courseName,
    //         numRows: 0,
    //         noBooking: this.sum(listBookingLineStatsCourse.map(x => x.noBooking)),
    //         noReality: this.sum(listBookingLineStatsCourse.map(x => x.noReality)),
    //         listPart: []
    //       };
    //       orgStat.numRows = listBookingLineStatsCourse.length; 
          
    //       listBookingLineStatsCourse.forEach(part => {
    //           courseStat.listPart.push({
    //              partName: Constant.Part.find(x => x.value == part.part)?.translateKey || "",
    //              numRows : 1,
    //              noBooking: part.noBooking,
    //              noReality: part.noReality
    //           })
    //       })

    //       orgStat.listCourse.push(courseStat);
    //    })
       
    //    this.listOrgStatistic.push(orgStat);
    // })
  }


  async initial() {
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
    this.enableSearch = true;
  }
  changeOrganizationText() {
     if(!this.filter.c_Org_Id) {
        this.enableSearch = false;
     }
     this.filter.c_Org_Id = this.organizationSelectControl.value?.id || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }


  async search() {
    if (this.filter.dateId == null) {
      this.filter.dateId = new Date();
    }

    if (this.enableSearch){
      this.listBookingLineStats = (await this.bookingLineSv.getStats(this.filter))?.data || [];
      this.totalBooking = this.sum(this.listBookingLineStats.map(x => x.totalBooking));
      this.totalReality = this.sum(this.listBookingLineStats.map(x => x.totalNoReality));
      //this.processStats();
    }
  }


}
