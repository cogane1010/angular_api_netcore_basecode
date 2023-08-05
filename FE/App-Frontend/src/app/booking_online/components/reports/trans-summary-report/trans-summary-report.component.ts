import { Component, OnInit, ViewChild } from '@angular/core';
import { TransactionSummaryReportFilterModel } from '../../../model/paging';
import { FileDownload, TransactionSummaryReportDTO, TransactionSummaryReportResult } from '../../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { TransactionSummaryReportService } from 'src/app/booking_online/service/trans-summary-report.service';


@Component({
  selector: 'app-trans-summary-report',
  templateUrl: './trans-summary-report.component.html',
  styleUrls: ['./trans-summary-report.component.scss']
})
export class TransactionSummaryReportComponent extends BaseComponent implements OnInit {
  displayedColumns1 = ['STT', 'seaBankCustomerCode', 'customerName', 'monthAmount', 'totalAmount'];
  displayedColumns2 = ['STT1', 'seaBankCustomerCode1', 'customerName1', 'monthReceivedAmt', 'monthNotReceivedAmt', 'monthTotalAmt', 'receivedAmt', 'notReceivedAmt'
    , 'totalAmt'];
  displayedFooterColumns = ['STT1', 'monthReceivedAmt', 'monthNotReceivedAmt', 'monthTotalAmt', 'receivedAmt', 'notReceivedAmt', 'totalAmt'];
  filter = new TransactionSummaryReportFilterModel()
  reportResult = new TransactionSummaryReportResult();
  dataSource: TransactionSummaryReportDTO[] = [];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;

  constructor(private reportsService: TransactionSummaryReportService, public dialog: MatDialog) {
    super(dialog)
  }

  async ngOnInit() {
    this.filter.toDate = new Date();
    this.filter.fromDate = new Date();
    // var today = new Date();
    // today.setMonth(today.getMonth() - 1);
    // this.filter.fromDate = today;
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
        FileDownload.arrayBufferProcess(res, 'bao_cao_tong_hop_giao_dich.xlsx');
      },
      err => {
        console.log(err);
        if (err.error) {
          FileDownload.arrayBufferProcess(err.error, 'bao_cao_tong_hop_giao_dich.xlsx', true);
        }
      }
    );
  }
}

