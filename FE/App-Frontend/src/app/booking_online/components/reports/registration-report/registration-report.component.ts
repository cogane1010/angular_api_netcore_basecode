import { Component, OnInit, ViewChild } from '@angular/core';
import { SmsFilterModel } from '../../../model/paging';
import { BookingLine, Customer, Organization } from '../../../model/model';
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
   selector: 'app-registration-report',
   templateUrl: './registration-report.component.html',
   styleUrls: ['./registration-report.component.scss']
})
export class RegistrationReportComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'customerCode', 'fullName', 'createDate', 'org','course', 'cardNo', 'phone', 'email'];
  filter: SmsFilterModel = new SmsFilterModel()
  listOrganization : Organization[] = []
  customer : Customer[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;
  constructor( private reportsService: ReportsService,
    private bookingLineService: BookingLineService,
    public dialog: MatDialog) { 
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
      }
    ) 
  }

  search() {
    this.filter.pageIndex = 0;
    this.paginator.length = 0;
    
    this.getData()
  }

  getData() {
    this.reportsService.getRegistraionPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.message)
          this.alert(res?.message)
          return
        }
        this.customer = res.data.data;
        this.paginator.length = res.data.count;
        console.log(this.customer);
      }
    )
  }
  excelExport(){
    this.reportsService.exportExcelRegistration(this.filter).then(
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

