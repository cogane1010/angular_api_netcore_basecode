import { HttpEventType } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { OutTransactionHeader, TransactionInFilter } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { AppGlobals } from 'src/app/common/app-global';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { BookingOutDetailComponent } from '../booking-out-appr-detail/booking-out-detail/booking-out-detail.component';

@Component({
    selector: 'app-booking-out-all',
    templateUrl: './booking-out-all.component.html',
    styleUrls: ['./booking-out-all.component.scss']
})
export class BookingOutAllComponent extends BaseComponent implements OnInit, OnDestroy {

    displayedColumns: string[] = ['STT', 'FileName', 'CreatedUser', 'CreatedDate', 'Status', 'updatedUser', 'updatedDate'
        , 'approverUserName', 'approveDate', 'action']
    filter: TransactionInFilter = new TransactionInFilter()
    dataSource: OutTransactionHeader[] = []

    /** upload */
    isUploaded = true;
    extensionFileAccept = ["xlsx"];
    uploadFiles: File;
    fileProgress: string;

    @ViewChild(MatPaginator) paginator: MatPaginator

    constructor(private _service: TransactionInService, public dialog: MatDialog) {
        super(dialog)
    }

    ngOnInit() {
        this.filter = AppGlobals.getFilter(TransactionInFilter) || new TransactionInFilter()
        this.filter.FindDate = new Date();
        this.filter.FindDate.setDate(new Date().getDate()-1)
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
        this._service.getOutTransPagingAll(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res)
                    this.dataSource = [];
                    this.paginator.length = 0;
                } else {
                    this.dataSource = res.data.data
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

    exportExcel() {
        this._service.exportExcelAll(this.filter).then(
            (res: any) => {
                this.downloadFile(res.data, res.filename);
            }
            , error => {
                console.log(error);
            }
        );
    }
    uploadFtpExcel() {
        this._service.uploadFtpExcel().then(
            res => {
                if (!res?.isSuccess) {
                    this.alert(res?.message);
                }
            }
        )
        // this._service.uploadFtpExcel().then(
        //     (res: any) => {
        //         //this.downloadFile(res.data, res.filename);
        //     }
        //     , error => {
        //         console.log(error);
        //     }
        // );
    }
    downloadFile(data: any, fileName: string) {
        const link = document.createElement('a');
        link.href = window.URL.createObjectURL(data);
        link.download = fileName;
        link.click();
    }

    /* debug */
    // sendEmail() {
    //     //this.filter.EmailFilterDates = [this.filter.FindDate];
    //     this.filter.EmailFilterDates = [new Date('2022-07-29T00:00:00.000Z'), new Date('2022-07-30T00:00:00.000Z')
    //         , new Date('2022-08-08T00:00:00.000Z'), new Date('2022-08-09T00:00:00.000Z'), new Date('2022-08-13T00:00:00.000Z')];
    //     this._service.sendEmailFileOut(this.filter).then(
    //         (res: any) => {
    //             if (res) {
    //                 if (!res.isSuccess) {
    //                     this.error(res.message);
    //                     console.log(res);
    //                 } else {
    //                     this.alert('Gửi email thành công');
    //                 }
    //             }
    //             else {
    //                 this.error('Có lỗi xảy ra');
    //                 console.log(res);
    //             }
    //         }
    //         , error => {
    //             console.log(error);
    //         }
    //     );
    // }
}

