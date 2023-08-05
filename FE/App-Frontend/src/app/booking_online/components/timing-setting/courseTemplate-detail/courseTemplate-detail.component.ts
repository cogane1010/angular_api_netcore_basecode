import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CourseTemplate, Organization,Course, CourseTemplateLine } from 'src/app/booking_online/model/model';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { CourseTemplateService } from '../../../service/courseTemplate.service';
import { CourseService } from '../../../service/course.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { asEnumerable } from 'linq-es2015';
import { CourseTemplateFilterModel } from '../../../model/paging';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTable } from '@angular/material/table';
import { BookingLineService } from 'src/app/booking_online/service/booking.service';

@Component({
  selector: 'app-courseTemplate-detail',
  templateUrl: './courseTemplate-detail.component.html',
  styleUrls: ['./courseTemplate-detail.component.scss']
})

export class CourseTemplateDetailComponent extends BaseComponent implements OnInit {

  id : string;
  course: CourseTemplate = {code:'',name:'',courseTemplateLine:[{part:'1'}]};
  listOrganization : Organization[] = [];
  listCourse : Course[] = [];
  origCourse : Course[] = [];
  courseDisable:boolean = true;
  organizationSelectControl = new FormControl();
  courseSelectControl = new FormControl();
  organizationFilteredOptions: Observable<Organization[]>;
  courseFilteredOptions: Observable<Course[]>;
  isReadonly = false;
  displayedColumns = ['STT', 'course','part', 'startTime','endTime','interval','turnLength','maxHole', 'actions'];
  partList = asEnumerable([{"code": "1","title": "ca sáng"},{"code": "2","title": "ca chiều"},{"code": "3","title": "ca tối"}]).ToArray();
  filter: CourseTemplateFilterModel = new CourseTemplateFilterModel()
  @ViewChild('relTbl') relTbl: MatTable<any>;
  
  constructor(
    public dialog: MatDialog, 
    protected _route: ActivatedRoute,
    protected _router: Router,
    private courseService: CourseTemplateService,
    private organizationSv: OrganizationService,
    private lineSv: BookingLineService,
    private courseSv: CourseService,
    private _domSanitizer: DomSanitizer

  ) {
    super(dialog);
    this._route.params.subscribe(params => {
      this.id = params['id'] 
    });
    if(this.id === "add") {
      this.course.isActive = true      
    }
    else {
        this.loadData();
    }   

  }

  async ngOnInit() {
    this.listOrganization = (await this.lineSv.getOrgByUserId()).data || [];
    this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.course.c_Org_Id));
    this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterOrganization(value))
    );

    this.listCourse = (await this.courseSv.getAll()).data || [];
    this.origCourse = this.listCourse;
    if(this.course.c_Org_Id){
      this.courseDisable = false;
    }
    // this.courseSelectControl.setValue(this.listCourse.find(x => x.id == this.course.C_Course_Id));
    // this.courseFilteredOptions = this.courseSelectControl.valueChanges.pipe(
    //   startWith(''),
    //   map(value => this._filterCourse(value))
    // )
  }

  loadData() {
    this.courseService.get(this.id).then(
      res => {
        if (!res?.isSuccess) {
          this.alert(res.message)
          return
        }
        else {
          this.course = res.data;
          if(!this.course.courseTemplateLine || this.course.courseTemplateLine.length == 0){
            this.course.courseTemplateLine = [{}];
          }
          console.log(this.course);
        }
      },
      err => {
          this.alert('error');
          console.log(err);
      }
    );
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    //this.getData()
  }

  private _filterOrganization(value: string): Organization[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listOrganization.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  private _filterCourse(value: string): Course[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listCourse.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  onSelectedOrganization(event) {
  }
  onSelectedCourse(event) {
    this.course.C_Course_Id = this.courseSelectControl.value?.id || null;
  }
  changeOrganizationText() {
    //this.course.c_Org_Id = this.organizationSelectControl.value?.id || null;
    this.courseDisable = false;
    this.listCourse= this.origCourse;
    if(this.course.c_Org_Id){
      this.listCourse=this.listCourse.filter(x => x.c_Org_Id?.toLowerCase().indexOf(this.course.c_Org_Id) >= 0);
    }    
  }
  
  changeCourseText(event) {
    this.course.C_Course_Id = this.courseSelectControl.value?.id || null;
  }

  displayOrganization(org?: Organization): string {
    return org?.name || '';
  }

  displayCourse(course?: Course): string {
    return course?.name || '';
  }

  back() {
    this._router.navigateByUrl('/booking/timing-setting');
  }

  addOrEdit(Course: CourseTemplate) {
    var keepGoing = true;
    this.course.courseTemplateLine.forEach(obj => {
      if(keepGoing) {
        if(!obj.c_Course_Id || !obj.startTime || !obj.interval || !obj.endTime || !obj.hole)  {
          keepGoing = false;
        }
      }      
    });
    if(!keepGoing){
      this.alert('Nhập đủ thông tin khung giờ chi tiết.');
      return;
    }

    this.courseService.addOrEdit(Course).then(
      res => {
        if (!res?.isSuccess) {
          this.alert(res?.message);
          return;
        }
        this.course = res.data;
        this.alert('Cập nhật thành công');
      },
      err => {
        console.log(err);
      }
    )
  }

  isNotBufferTaskItem(index: number, data: any[]) {
    if(data != null){
      return index < data.length - 1;
    }
    return false;
  }

  async deleteTask(i: number, data: any[], tb: MatTable<any>) {
    const isConfirm = await this.confirm('Bạn chắc chắn xóa bản ghi này?');
    if (!isConfirm) {
      return;
    }
    data.splice(i, 1);
    if(data.length == 0){
      data.push({});
    }
    tb.renderRows();    
  }

  addTask(i: number, data: any[], tb: MatTable<any>) {
    const currentTaskBuffer = data[data.length - 1];
    if (currentTaskBuffer == null) {
        return;
    }
    this.addTaskBuffer(data,tb);
  }
 
  protected addTaskBuffer(data: any[], tb: MatTable<any>) {
    if (this.isReadonly) {
      return;
    }
    data.push({});
    tb.renderRows();
  }

  onChangeToDate(rd:CourseTemplateLine, isFrom: boolean){
    console.log(rd);
    this.calTime(rd,isFrom);
  }

  calTime(rd:CourseTemplateLine, isFrom: boolean){
    var now = new Date();
    var fToday: Date;
    var tToday: Date;
    if(rd.startTime){
      var fdate = rd.startTime.split(':');
      fToday = new Date(now.getFullYear(), now.getMonth(), now.getDate(), Number(fdate[0]), Number(fdate[1]), 0);
    }
        
    if(rd.endTime){
      var tdate = rd.endTime.split(':');
      tToday = new Date(now.getFullYear(), now.getMonth(), now.getDate(), Number(tdate[0]), Number(tdate[1]), 0);
    }
    if(fToday && tToday && fToday.getTime() >= tToday.getTime()){
      if(isFrom){
        rd.startTime = '';
      }else{
        rd.endTime = '';
      }
      alert('StartTime phải sớm hơn EndTime');
    }
  }
}
