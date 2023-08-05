import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { TokenStorage } from 'src/app/admin/auth/token.storage';
import { UserService } from 'src/app/admin/service';
import { Booking, BookingLine, Course, Organization } from 'src/app/booking_online/model/model';
import { BookingFilterModel, BookingLineFilterModel } from 'src/app/booking_online/model/paging';
import { BookingLineService,BookingService } from 'src/app/booking_online/service/booking.service';
import { CourseService } from 'src/app/booking_online/service/course.service';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { Constant, DefaultCourseTemplate } from 'src/app/common/enum';
import { BookingDetailComponent } from '../booking-detail/booking-detail.component';
import { DialogBookingSearchComponent } from '../dialog-booking-search/dialog-booking-search.component';
import { DialogBookingStatisticComponent } from '../dialog-booking-statistic/dialog-booking-statistic.component';


@Component({
  selector: 'app-booking-list',
  templateUrl: './booking-list.component.html',
  styleUrls: ['./booking-list.component.scss']
})

export class BookingListComponent extends BaseComponent implements OnInit {
  curOrg: Organization;
  filter : BookingLineFilterModel = new BookingLineFilterModel();
  listBooking: BookingLine[] = []
  
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  listCourse : Course[] = []
  courseSelectControl = new FormControl()
  courseFilteredOptions: Observable<Course[]>
  
  disabledOrgFilter = false;
  disabledCourseFilter = false;

  listPart = Constant.Part;

  enableSearch = true;
  //@ViewChild('print') print;
  constructor(
    private userSv: UserService,
    private orgService: OrganizationService,
    private bookingLineService: BookingLineService,
    private bookingService: BookingService,
    public dialog: MatDialog
  ) {
    super(dialog);
  }


  ngOnInit() {
    this.filter.pageSize = -1   
    this.filter.dateId = new Date()    
    this.bookingLineService.getOrgByUserId().then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        
        this.listOrganization = res.data
        this.curOrg = this.listOrganization[0] || null
        this.filter.c_Org_Id = this.curOrg.id
        this.setValue()
        if (this.curOrg) {
          if(!this.curOrg.isSummary) {
            this.filter.c_Org_Id = this.curOrg.id
            this.disabledOrgFilter = true
            //this.search();
          }     
        }
        else {
          this.disabledOrgFilter = true
          this.disabledCourseFilter = true
          this.enableSearch = false;
        }    
      }
    )
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
    if(!this.filter.c_Org_Id)
        this.enableSearch = false;
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
      if(!this.filter.c_Course_Id ){
        this.enableSearch = false;
      }else{
        this.enableSearch = true;
      }       
      
      this.filter.c_Course_Id = this.courseSelectControl.value?.id || null
  }
  
  displayCourse(org?: Course): string {
    return org?.name || ''
  }

  async search() {
    if (this.filter.dateId == null) {
      this.filter.dateId = new Date();
    }

    this.listBooking = (await this.bookingLineService.getPaging(this.filter))?.data?.data || [];

    //compute turn 
    // this.listBooking.forEach(x => {
    //   x.turnLength = x.turnLength || DefaultCourseTemplate.turnLength
    //   x.turn = new Date(x.tee_Time)
    //   x.turn.setMinutes(x.turn.getMinutes() + x.turnLength)
    // })
   
  }

  searchPrevDay() {
    if(this.filter.dateId && this.filter.c_Org_Id && this.filter.c_Course_Id && this.filter.part){
      this.filter.dateId = this.filter.dateId? new Date(this.filter.dateId) : new Date();
      this.filter.dateId.setDate(this.filter.dateId.getDate() -1);
      this.search();
    }else{
      this.alert("Chọn hết các điều kiện");
    }
    
    
  }
  searchNextDay(){
    if(this.filter.dateId && this.filter.c_Org_Id && this.filter.c_Course_Id && this.filter.part){
      this.filter.dateId = this.filter.dateId? new Date(this.filter.dateId) : new Date();
    this.filter.dateId.setDate(this.filter.dateId.getDate() + 1);
    this.search();
    }else{
      this.alert("Chọn hết các điều kiện");
    }    
  }
  
  goToSearch() {

    const dialogRef = this.dialog.open(DialogBookingSearchComponent, {
      width: '70vw',
      data: {
        listOrg: this.listOrganization,
        curOrg: this.curOrg,
        filter: {
          dateId: this.filter.dateId,
          c_Org_Id : this.filter.c_Org_Id,
          c_Course_Id: this.filter.c_Course_Id
        }
      }
    })
    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
    })
  }
  goToStat() {
    const dialogRef = this.dialog.open(DialogBookingStatisticComponent, {
      width: '70vw',
      data: {
        listOrg: this.listOrganization,
        curOrg: this.curOrg,
        filter: {
          dateId: this.filter.dateId,
          c_Org_Id : this.filter.c_Org_Id,
        }
      }
    })
    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
    })
  }
  report() {

  }

  saveNotes(item: BookingLine, isUpdateNoShow = false) {
    item.isUpdateNoShow = isUpdateNoShow;
    this.bookingLineService.updateNoShow(item).then(
      res => {
        if (!res?.isSuccess) {
          this.alert('Cập nhật thất bại')
          return
        }
        //this.alert('Cập nhật thành công');
      },
      err => {
        console.log(err)
      });
  }

  detailBooking(item: BookingLine) {   
    const dialogRef = this.dialog.open(BookingDetailComponent, {
      width: '85vw',
      height:'60vw',
      maxWidth: '85vw',
      data: item.gF_Booking_Id
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      //this.getData();
    })
  }
}
