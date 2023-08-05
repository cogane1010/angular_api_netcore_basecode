import { HttpEventType } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Setting } from 'src/app/admin/model';
import { OutTransactionHeader, TransactionInFilter } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { AppGlobals } from 'src/app/common/app-global';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { BookingOutDetailComponent } from '../booking-out-appr-detail/booking-out-detail/booking-out-detail.component';
import { BookingOutApproveSelectDateComponent } from './booking-out-appr-select-date/booking-out-appr-select-date.component';

@Component({
    selector: 'app-booking-out-appr',
    templateUrl: './booking-out-appr.component.html',
    styleUrls: ['./booking-out-appr.component.scss']
})
export class BookingOutApprComponent extends BaseComponent implements OnInit, OnDestroy {

    displayedColumns: string[] = ['STT', 'FileName', 'CreatedUser', 'CreatedDate', 'Status', 'approverUserName', 'approveDate', 'action']
    filter: TransactionInFilter = new TransactionInFilter()
    dataSource: OutTransactionHeader[] = []

    /** upload */
    isUploaded = true;
    extensionFileAccept = ["xlsx"];
    uploadFiles: File;
    fileProgress: string;

    @ViewChild(MatPaginator) paginator: MatPaginator

    numOfDayNotApprove: number;

    constructor(private _service: TransactionInService, public dialog: MatDialog) {
        super(dialog)

    }

    ngOnInit() {
        this.getNumOfDayNotApprove();
        this.filter = AppGlobals.getFilter(TransactionInFilter) || new TransactionInFilter()
        this.filter.FromDate = new Date();
        this.filter.FromDate.setDate(this.filter.FromDate.getDate() - 1);
        this.filter.ToDate = new Date();
        this.filter.ToDate.setDate(this.filter.ToDate.getDate() - 1);
        this.getData()
    }

    ngOnDestroy(): void {
        AppGlobals.setFilter(TransactionInFilter, this.filter)
    }

    search() {
        this.filter.pageIndex = 0
        this.paginator.length = 10
        this.getData()
    }

    getData() {
        this._service.getOutTransPaging(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res)
                    this.dataSource = [];
                    this.paginator.length = 0;
                } else {
                    this.dataSource = res.data.data
                    console.log(res.data.data)
                    this.paginator.length = res.data.count
                }
            }
        )
    }

    pageIndexChange(event: PageEvent) {
        this.filter.pageIndex = event.pageIndex
        this.filter.pageSize = event.pageSize
        this.getData()
    }

    showDetail(item: OutTransactionHeader) {
        const dialogRef = this.dialog.open(BookingOutDetailComponent, {
            width: '60vw',
            data: item
        })

        dialogRef.afterClosed().subscribe(res => {
            if (res) this.getData()
        })
    }

    exportExcel(item: OutTransactionHeader) {
        this.filter.outTransHeaderId = item.id;
        this._service.exportExcel(this.filter).then(
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

    approve(item: OutTransactionHeader) {
        if (!confirm('Bạn có chắc muốn duyệt dữ liệu đối soát?')) {
            return;
        }

        this._service.approveOut(item).then(
            res => {
                if (!res?.isSuccess) {
                    this.alert(`Có lỗi xảy ra ${res?.message}`);
                    console.error(res);
                    return;
                } else {
                    this.getData();
                }
            },
            error => {
                console.log(error);
            }
        )
    }

    unApprove(item: OutTransactionHeader) {
        if (!confirm('Bạn có chắc muốn hủy duyệt dữ liệu đối soát?')) {
            return;
        }

        this._service.unApproveOut(item).then(
            res => {
                if (!res?.isSuccess) {
                    this.alert(`Có lỗi xảy ra ${res?.message}`);
                    console.error(res);
                    return;
                } else {
                    this.getData();
                }
            },
            error => {
                console.log(error);
            }
        )
    }

    selectDate() {
        const dialogRef = this.dialog.open(BookingOutApproveSelectDateComponent, {
            width: '60vw',
            maxWidth: '60vw'
        });
    }

    getNumOfDayNotApprove() {
        this._service.getNumOfDayNotApprove().then(
            res => {
                const cfg = res.data as Setting;
                this.numOfDayNotApprove = cfg && !isNaN(+cfg.value) && +cfg.value > 0 ? + cfg.value : 0;
            },
            err => {
                console.log(err);
            }
        );
    }
}

