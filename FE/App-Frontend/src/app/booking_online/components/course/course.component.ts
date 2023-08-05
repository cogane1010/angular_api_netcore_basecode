import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { Course } from '../../model/model';
import { CourseFilterModel } from '../../model/paging';
import { CourseService } from '../../service/course.service';
import { DialogCourseDetailComponent } from './dialog-course-detail/dialog-course-detail.component';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.scss']
})
export class CourseComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'organizationName', 'actions'];
  filter: CourseFilterModel = new CourseFilterModel()
  listCourse : Course[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private courseService: CourseService, public dialog: MatDialog) { 
     super(dialog)
   }

   ngOnInit() {
    this.getData()
  }



  search() {
    this.filter.pageIndex = 0
    this.paginator.length = 0
    this.getData()
  }

  getData() {
    this.courseService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listCourse = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(course: Course) {
    if(!course) {
      course = new Course();
      course.isActive = true;
    }
    const dialogRef = this.dialog.open(DialogCourseDetailComponent, {
      width: '60vw',
      data: Object.assign({},course)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }

  async delete(item: Course) {
    if (!(await this.confirm('Xóa Course?'))) return;

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
