import { Component, OnInit, ViewChild } from '@angular/core';
import { TransactionDetailReportFilterModel } from '../../../model/paging';
import { FileDownload, TransactionDetailReportDTO, TransactionDetailReportResult } from '../../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { ReportsService } from '../../../service/reports.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { TransactionDetailReportService } from 'src/app/booking_online/service/trans-detail-report.service';


@Component({
  selector: 'app-trans-detail-report',
  templateUrl: './trans-detail-report.component.html',
  styleUrls: ['./trans-detail-report.component.scss']
})
export class TransactionDetailReportComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'seaBankCustomerCode', 'seaBankCustomerName', 'bookingCode', 'golfOrgCode', 'bookingDate', 'creditAccount', 'card_type', 'CardMaskingNo', 'debitAccountNo', 'transDesciption', 'transDate', 'transAmt'
    , 'cusCancelReturnBf24HAmt', 'notReceivedAmt', 'totalReceived'];
  displayedFooterColumns = ['STT', 'transDesciption', 'transDate', 'transAmt'
    , 'cusCancelReturnBf24HAmt', 'notReceivedAmt', 'totalReceived'];
  filter = new TransactionDetailReportFilterModel()
  reportResult = new TransactionDetailReportResult();
  dataSource: TransactionDetailReportDTO[] = [];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;

  constructor(private reportsService: TransactionDetailReportService, public dialog: MatDialog) {
    super(dialog)
  }

  async ngOnInit() {
    this.filter.toDate = new Date();
    var today = new Date();
    today.setMonth(today.getMonth() - 1);
    this.filter.fromDate = today;
    this.getData();
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
        FileDownload.arrayBufferProcess(res, 'bao_cao_chi_tiet_giao_dich.xlsx');
      },
      err => {
        console.log(err);
        if (err.error) {
          FileDownload.arrayBufferProcess(err.error, 'bao_cao_chi_tiet_giao_dich.xlsx', true);
        }
      }
    );
  }
}

