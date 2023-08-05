import { HttpEventType } from '@angular/common/http';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { FtpTransJobHistory, InTransactionHeader, TransactionInFilter } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { AppGlobals } from 'src/app/common/app-global';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { UploadSizeMax } from 'src/app/common/enum';


@Component({
  selector: 'app-ftp-view-list',
  templateUrl: './ftp-view-list.component.html',
  styleUrls: ['./ftp-view-list.component.scss']
})
export class FtpViewListComponent extends BaseComponent implements OnInit, OnDestroy {

    displayedColumns: string[] = ['STT', 'filename', 'datetrans', 'isFtp', 'isInsert','status', 'CreatedDate']
    filter: TransactionInFilter = new TransactionInFilter()
    dataSource: FtpTransJobHistory[] = []

    /** upload */
    isUploaded = true;
    extensionFileAccept = ["xlsx"];
    uploadFiles: File;
    fileProgress: string;

    @ViewChild(MatPaginator) paginator: MatPaginator

    @ViewChild('upFile') upFile: ElementRef;

    constructor(private _service: TransactionInService, public dialog: MatDialog) {
        super(dialog)
    }

    ngOnInit() {
        this.filter = AppGlobals.getFilter(TransactionInFilter) || new TransactionInFilter()       
        this.getData();
    }

    ngOnDestroy(): void {
        AppGlobals.setFilter(TransactionInFilter, this.filter)
    }

    search() {
        this.filter.pageIndex = 0
        this.paginator.length = 0
        this.getData()
    }

    getData() {
        this._service.getFtpTransPaging(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res);
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

   
   
}
