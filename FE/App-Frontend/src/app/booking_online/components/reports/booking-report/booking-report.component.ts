import { Component, OnInit, ViewChild } from '@angular/core';
import { SmsFilterModel } from '../../../model/paging';
import { Booking, BookingLine, Customer, Organization } from '../../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { ReportsService } from '../../../service/reports.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { asEnumerable } from 'linq-es2015';
import { BookingLineService } from 'src/app/booking_online/service/booking.service';

@Component({
   selector: 'app-booking-report',
   templateUrl: './booking-report.component.html',
   styleUrls: ['./booking-report.component.scss']
})
export class BookingReportComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'customerCode', 'fullName', 'cardNo', 'createDate', 'bookingCode', 'org', 'course', 'timeplay', 'teetime', 'players','showup','status'];
  statusList = asEnumerable([{"code": "booked","title": "Đã đặt"},{"code": "cancel","title": "Đã hủy"}]).ToArray();
  filter: SmsFilterModel = new SmsFilterModel()
  bookings : Booking[] = []
  disabledOrgFilter = true;
  curOrg: Organization;
  sum: number = 0;
  teeCount: number = 0;
  showUpCount : number = 0;
  listOrganization : Organization[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;
  constructor( private reportsService: ReportsService
    , private bookingLineService: BookingLineService
    , public dialog: MatDialog) { 
     super(dialog)
   }

   async ngOnInit() {
    this.filter.timeTo =  new Date();
    var today = new Date();
    today.setMonth(today.getMonth() -1);
    this.filter.timeFrom =  today;
    this.getData();
    this.bookingLineService.getOrgByUserId().then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }        
        this.listOrganization = res.data  
        this.curOrg = this.listOrganization[0] || null
        this.filter.c_Org_Id = this.curOrg.id   
        if (this.listOrganization && this.listOrganization.length > 1) {
          this.disabledOrgFilter = false   
          this.search();   
        }
        else {
          this.filter.c_Org_Id = this.curOrg.id
          this.disabledOrgFilter = true
          this.search();   
        }    
      }
    ) 
  }

  search() {
    this.filter.pageIndex = 0;
    this.paginator.length = 0;    
    this.getData()
  }

  getData() {
    this.reportsService.getBookingPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.message)
          this.alert(res?.message)
          return
        }
        this.bookings = res.data.data;
        this.paginator.length = res.data.count;  
        this.calculation();      
      }
    )
    
  }

  calculation() {
    this.sum = 0;
    this.teeCount = 0;
    this.showUpCount = 0;
    if (this.bookings)
      for (let row of this.bookings) {
        this.sum += 1;
        this.teeCount += row.golfers;
        this.showUpCount += row.showUpCount
      }   
  }

  excelExport(){
    this.reportsService.exportExcelbooking(this.filter).then(
        (res: any) => {
            this.downloadFile(res.data, res.filename);
        }
        , error => {
            console.log(error);
        }
    );
}

downloadFile(data: any, fileName: string) {
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(data);
    link.download = fileName;
    link.click();
}

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }

  
}

