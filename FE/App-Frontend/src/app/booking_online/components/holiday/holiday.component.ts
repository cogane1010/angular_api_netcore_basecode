import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { Holiday } from '../../model/model';
import { HolidayFilterModel } from '../../model/paging';
import { BookingLineService } from '../../service/booking.service';
import { HolidayService } from '../../service/holiday.service';
import { DialogHolidayDetailComponent } from './dialog-holiday-detail/dialog-holiday-detail.component';

@Component({
  selector: 'app-holiday',
  templateUrl: './holiday.component.html',
  styleUrls: ['./holiday.component.scss']
})
export class HolidayComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'date', 'organizationName', 'actions'];
  filter: HolidayFilterModel = new HolidayFilterModel()
  listholiday : Holiday[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private holidayService: HolidayService, 
    private bookingLineService: BookingLineService,    
    public dialog: MatDialog) { 
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
    this.holidayService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listholiday = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(holiday: Holiday) {
    if(!holiday) {
      holiday = new Holiday();
      holiday.isActive = true;
    }
    const dialogRef = this.dialog.open(DialogHolidayDetailComponent, {
      width: '60vw',
      data: Object.assign({},holiday)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }

  async delete(item: Holiday) {
    if (!(await this.confirm('Xóa holiday?'))) return;

    this.holidayService.detele(item.id).then(
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
