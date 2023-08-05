import { Component, OnInit, ViewChild } from '@angular/core';
import { NotificationFilterModel } from '../../../model/paging';
import { GF_Notification } from '../../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { NotificationService } from '../../../service/notification.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { asEnumerable } from 'linq-es2015';
import { CourseService } from '../../../service/course.service';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.scss']
})
export class NotificationListComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'content','status','time','sendUser', 'actions'];
  filter: NotificationFilterModel = new NotificationFilterModel()
  listNotification : GF_Notification[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  statusList = asEnumerable([{"id": 0,"name": "Chưa gửi"},{"id": 1,"name": "Đã gửi"}]).ToArray()

  constructor( private notificationService: NotificationService, public dialog: MatDialog) { 
     super(dialog)
   }

   async ngOnInit() {  
    this.getData()
  }
 

  search() {
    this.filter.pageIndex = 0
    this.paginator.length = 0
    this.getData()
  }

  getData() {
    this.notificationService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listNotification = res.data.data       
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }
  
  async delete(item: GF_Notification) {
    if (!(await this.confirm('Xóa bản ghi này không?'))) return;

    this.notificationService.detele(item.id).then(
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
