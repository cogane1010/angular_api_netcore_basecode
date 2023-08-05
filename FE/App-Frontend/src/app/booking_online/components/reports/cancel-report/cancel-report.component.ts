import { Component, OnInit, ViewChild } from '@angular/core';
import { SmsFilterModel } from '../../../model/paging';
import { BookingLine, Organization } from '../../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { ReportsService } from '../../../service/reports.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { asEnumerable } from 'linq-es2015';

@Component({
   selector: 'app-cancel-report',
   templateUrl: './cancel-report.component.html',
   styleUrls: ['./cancel-report.component.scss']
})
export class CancelReportComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'bookingCode', 'date', 'Teetime', 'course', 'numberPlayer', 'nonFund', 'fullName', 'cancelDate', 'cancelTime', 'groupcode', 'note', 'durationPlay'];
  filter: SmsFilterModel = new SmsFilterModel()
  listBookingLine : BookingLine[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;
  constructor( private reportsService: ReportsService, public dialog: MatDialog) { 
     super(dialog)
   }

   async ngOnInit() {
    this.filter.timeTo =  new Date();
    var today = new Date();
    today.setMonth(today.getMonth() -1);
    this.filter.timeFrom =  today;
    this.getData();
  }

  search() {
    this.filter.pageIndex = 0;
    this.paginator.length = 0;
    
    this.getData()
  }

  getData() {
    this.reportsService.getCancelPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.message)
          return
        }
        this.listBookingLine = res.data.data;
        this.paginator.length = res.data.count;
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }

  
}

