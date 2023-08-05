import { Component, OnInit, ViewChild } from '@angular/core';
import { PromotionSettingFilterModel } from '../../../model/paging';
import { M_Promotion, Organization,Course } from '../../../model/model';
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

@Component({
  selector: 'app-promotion-setting-list',
  templateUrl: './promotion-setting-list.component.html',
  styleUrls: ['./promotion-setting-list.component.scss']
})
export class PromotionSettingListComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'time','course','appliedObj','isHot','isActive', 'actions'];
  filter: PromotionSettingFilterModel = new PromotionSettingFilterModel()
  listPromotion : M_Promotion[] = []
  listOrganization : Organization[] = []
  listCourse : Course[] = [];
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;
  statusList = asEnumerable([{"code": true,"title": "Hiệu lực"},{"code": false,"title": "Hết hiệu lực"}]).ToArray();
  constructor( private promotionService: PromotionSettingComponentService, public dialog: MatDialog
    ,private organizationSv: OrganizationService,private courseSv: CourseService) { 
     super(dialog)
   }

   async ngOnInit() {
    this.filter.isActive = true;
    this.filter.promotionType = 'normal';
    this.listCourse = (await this.courseSv.getCourseByUser()).data || [];   
    this.getData();
    
  }
 

  search() {
    this.filter.pageIndex = 0
    this.paginator.length = 0
    this.getData()
  }

  getData() {
    this.promotionService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listPromotion = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }
  
  async delete(item: M_Promotion) {
    if (!(await this.confirm('Xóa bản ghi này không?'))) return;

    this.promotionService.detele(item.id).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res)
          alert(res.message)
        }
        this.getData()
      },
      err => {
        console.error(err)
      }
    )
  }
}
