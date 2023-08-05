import { Component, OnInit, ViewChild } from '@angular/core';
import { TransactionMonthlyReportFilterModel } from '../../../model/paging';
import { FileDownload, TransactionMonthlyReportDTO, TransactionMonthlyReportResult } from '../../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { TransactionMonthlyReportService } from 'src/app/booking_online/service/trans-monthly-report.service';

import * as _moment from 'moment';
// tslint:disable-next-line:no-duplicate-imports
import { default as _rollupMoment, Moment } from 'moment';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { FormControl } from '@angular/forms';
import { MatDatepicker } from '@angular/material/datepicker';

const moment = _rollupMoment || _moment;

// See the Moment.js docs for the meaning of these formats:
// https://momentjs.com/docs/#/displaying/format/
export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-trans-monthly-report',
  templateUrl: './trans-monthly-report.component.html',
  styleUrls: ['./trans-monthly-report.component.scss'],
  providers: [
    // `MomentDateAdapter` can be automatically provided by importing `MomentDateModule` in your
    // application's root module. We provide it at the component level here, due to limitations of
    // our example generation script.
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },

    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})
export class TransactionMonthlyReportComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'seaBankCustomerCode', 'customerName', 'notReceivedAmtAfterPrevMonth', 'monthBookedAmt', 'monthTransAmtForPrevMonths'
    , 'monthTransAmt', 'totalMonthAmt', 'remainNotReceivedAmtAfterPrevMonth'];
  displayedFooterColumns = ['STT', 'notReceivedAmtAfterPrevMonth', 'monthBookedAmt', 'monthTransAmtForPrevMonths', 'monthTransAmt', 'totalMonthAmt'
    , 'remainNotReceivedAmtAfterPrevMonth'];
  filter = new TransactionMonthlyReportFilterModel()
  reportResult = new TransactionMonthlyReportResult();
  dataSource: TransactionMonthlyReportDTO[] = [];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;

  date = new FormControl(moment());

  constructor(private reportsService: TransactionMonthlyReportService, public dialog: MatDialog) {
    super(dialog)
  }

  async ngOnInit() {
    this.filter.filterDate = new Date();
    //this.date.setValue(this.filter.filterDate);
    this.getData();
  }

  setMonthAndYear(normalizedMonthAndYear: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date.value!;
    ctrlValue.month(normalizedMonthAndYear.month());
    ctrlValue.year(normalizedMonthAndYear.year());
    this.date.setValue(ctrlValue);
    this.filter.filterDate = this.date.value;
    datepicker.close();
  }

  search() {
    this.filter.pageIndex = 0;
    this.paginator.length = 0;

    this.getData()
  }

  getData() {
    this.reportsService.getReportData(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.message)
          return
        }

        this.reportResult = res.data;
        if (this.reportResult && this.reportResult.pagingData && this.reportResult.pagingData.data) {
          this.dataSource = this.reportResult.pagingData.data;
          this.paginator.length = this.reportResult.pagingData.count;
        }
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }

  exportExcel() {
    this.reportsService.exportExcel(this.filter).then(
      res => {
        // this.downloadFile(res);
        FileDownload.arrayBufferProcess(res, 'bao_cao_giao_dich_thang.xlsx');
      },
      err => {
        console.log(err);
        if (err.error) {
          FileDownload.arrayBufferProcess(err.error, 'bao_cao_giao_dich_thang.xlsx', true);
        }
      }
    );
  }
}

