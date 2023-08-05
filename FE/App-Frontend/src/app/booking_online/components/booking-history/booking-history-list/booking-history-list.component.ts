import { Component, OnInit, ViewChild } from '@angular/core';
import { CancelBookingFilterModel } from '../../../model/paging';
import { M_Promotion, Organization,Course, Booking } from '../../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { PromotionSettingComponentService } from '../../../service/PromotionSettingListComponent.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { asEnumerable } from 'linq-es2015';
import { CourseService } from '../../../service/course.service';
import { UserService } from 'src/app/admin/service';
import { BookingLineService, BookingService } from '../../../service/booking.service';
import { BookingDetailComponent } from '../../booking/booking-detail/booking-detail.component';

@Component({
   selector: 'app-booking-history-list',
   templateUrl: './booking-history-list.component.html',
   styleUrls: ['./booking-history-list.component.scss']
})
export class BookingHistoryListComponent extends BaseComponent implements OnInit {

  displayedColumns = ['stt','code', 'clientInfo', 'infoBook', 'orgName','specService','price','status', 'createdDate','action'];
  filter: CancelBookingFilterModel = new CancelBookingFilterModel()
  listBooking : Booking[] = []
 
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;
  statusList = asEnumerable([{"code": "booked","title": "Đã đặt"},{"code": "cancel","title": "Đã hủy"}]).ToArray();

  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  listCourse : Course[] = []
  courseSelectControl = new FormControl()
  courseFilteredOptions: Observable<Course[]>
  disabledOrgFilter = false;
  disabledCourseFilter = false;
  enableSearch = true;
  curOrg: Organization;

  constructor(
    private userSv: UserService,
    private orgService: OrganizationService,
    private bookingLineService: BookingLineService,
    private bookingService: BookingService,
    private courseSv: CourseService,
    public dialog: MatDialog
  ) {
    super(dialog);
  }

   async ngOnInit() {
    this.filter.isActive = true;
    
    this.listOrganization = (await this.bookingLineService.getOrgByUserId())?.data || [];
    this.curOrg = this.listOrganization[0] || null

    if (this.curOrg) {
      if(!this.curOrg.isSummary) {
        this.filter.c_Org_Id = this.curOrg.id;
        this.disabledOrgFilter = true
      } 
    }
    else {
      this.disabledOrgFilter = true
      this.disabledCourseFilter = true
      this.enableSearch = false;
    }

    this.setValue();
    this.search()
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
   this.enableSearch = true;
   this.setValueCourse()
 }
 changeOrganizationText() {   
   this.filter.c_Org_Id = this.organizationSelectControl.value?.id || null
   if(!this.filter.c_Org_Id){
    this.enableSearch = false;
   }else{
    this.enableSearch = true;
   }
       
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
     if(!this.filter.c_Course_Id && this.listCourse.length > 0)
       this.enableSearch = false;
     
     this.filter.c_Course_Id = this.courseSelectControl.value?.id || null
 }
 
 displayCourse(org?: Course): string {
   return org?.name || ''
 }


  search() {
    this.filter.pageIndex = 0
    this.paginator.length = 10
    this.getData()
  }

  getData() {
    this.bookingService.getPagingHistory(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listBooking = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }
    
  detailBooking(item: Booking) {   
    const dialogRef = this.dialog.open(BookingDetailComponent, {
      width: '85vw',
      height:'60vw',
      maxWidth: '85vw',
      data: item.id
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }
}
