import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { BookingOtherType } from '../../model/model';
import { BookingOtherTypeFilterModel } from '../../model/paging';
import { BookingOtherTypeService } from '../../service/booking-other-type.service';
import { DialogBookingOtherTypeDetailComponent } from './dialog-booking-other-type-detail/dialog-booking-other-type-detail.component';

@Component({
  selector: 'app-booking-other-type',
  templateUrl: './booking-other-type.component.html',
  styleUrls: ['./booking-other-type.component.scss']
})
export class BookingOtherTypeComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'organizationName', 'actions'];
  filter: BookingOtherTypeFilterModel = new BookingOtherTypeFilterModel()
  listBookingOtherType : BookingOtherType[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private BookingOtherTypeService: BookingOtherTypeService, public dialog: MatDialog) { 
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
    this.BookingOtherTypeService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listBookingOtherType = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(bookingOtherType: BookingOtherType) {
    if(!bookingOtherType) {
      bookingOtherType = new BookingOtherType();
      bookingOtherType.isActive = true;
    }
    const dialogRef = this.dialog.open(DialogBookingOtherTypeDetailComponent, {
      width: '60vw',
      data: Object.assign({},bookingOtherType)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }

  async delete(item: BookingOtherType) {
    if (!(await this.confirm('Xóa booking other type?'))) return;

    this.BookingOtherTypeService.detele(item.id).then(
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
