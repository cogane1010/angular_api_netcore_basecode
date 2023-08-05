import { Component, OnInit, ViewChild } from '@angular/core';
import { CourseTemplateFilterModel } from '../../model/paging';
import { CourseTemplate, Organization } from '../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { CourseTemplateService } from '../../service/courseTemplate.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { asEnumerable } from 'linq-es2015';
import { BookingLineService } from '../../service/booking.service';

@Component({
  selector: 'app-timing-setting',
  templateUrl: './timing-setting.component.html',
  styleUrls: ['./timing-setting.component.scss']
})
export class TimingSettingComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'organizationName','appliedDate','startDate','endDate','isActive', 'actions'];
  filter: CourseTemplateFilterModel = new CourseTemplateFilterModel()
  listCourse : CourseTemplate[] = [];
  isReadonly:boolean = false;
  listOrganization : Organization[] = [];  
  organizationSelectControl = new FormControl();
  organizationFilteredOptions: Observable<Organization[]>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;
  statusList = asEnumerable([{"code": true,"title": "Hiệu lực"},{"code": false,"title": "Hết hiệu lực"}]).ToArray();
  constructor( private courseService: CourseTemplateService, public dialog: MatDialog
    ,private organizationSv: OrganizationService,
    private lineSv: BookingLineService) { 
     super(dialog)
   }

   async ngOnInit() {
    this.filter.status = true;     
    this.getOrg();   
    
  }

  private _filterOrganization(value: string): Organization[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listOrganization.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }

  search() {  
    this.paginator.length = 0;
    this.filter.pageIndex = 0;
    this.getData()
  }

  getOrg() {
    this.lineSv.getOrgByUserId().then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listOrganization = res.data;
        if(this.listOrganization.length == 1){
          this.filter.orgId = this.listOrganization[0].id;
          this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.listOrganization[0].id));
          this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
            startWith(''),
            map(value => this._filterOrganization(value))
          );
          this.isReadonly = true;
        }else{
          this.isReadonly = false;
        }
        this.getData();
      }
    )
  }

  getData() {
    this.courseService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listCourse = res.data.data;        
        this.paginator.length = res.data.count;
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(course: CourseTemplate) {
    if(!course) {
      course = new CourseTemplate();
      course.isActive = true;
    }    
  }

  async delete(item: CourseTemplate) {
    if (!(await this.confirm('Xóa bản ghi này không?'))) return;

    this.courseService.detele(item.id).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res)
          alert('error')
        }
        this.getData()
      },
      err => {
        console.error(err)
      }
    )
  }
}
