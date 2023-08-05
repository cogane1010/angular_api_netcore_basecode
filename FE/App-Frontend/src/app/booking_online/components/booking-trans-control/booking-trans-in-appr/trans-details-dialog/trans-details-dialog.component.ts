import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { InTransactionDetails, InTransactionHeader, TransactionInFilter } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { AppGlobals } from 'src/app/common/app-global';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
    selector: 'app-trans-details-dialog',
    templateUrl: './trans-details-dialog.component.html',
    styleUrls: ['./trans-details-dialog.component.scss']
})
export class TransactionInDetailsDialogComponent extends BaseComponent implements OnInit {

    displayedColumns: string[] = ['STT','trans_type', 'trans_Id', 'orgCode', 'sourceType', 'pAN_NUMBER', 'customerName', 'debitAcc', 'creditAcc', 'trans_Date'
        , 'trans_Time', 'trans_Amt', 'payment_Detail', 'co_Code', 'fT_Id', 'return_Acc', 'hoan_tien'];
    dataSource: InTransactionDetails[] = [];
    @ViewChild(MatPaginator) paginator: MatPaginator;

    header: InTransactionHeader;
    filter: TransactionInFilter = new TransactionInFilter()

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<TransactionInDetailsDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any, private _service: TransactionInService) {
        super(dialog)
        this.dialogRef.disableClose = true
        this.dialogRef.keydownEvents().subscribe(event => {
            if (event.key === "Escape")
                this.dialogRef.close()
        })

        this.header = data;
    }

    ngOnInit() {
        this.filter = AppGlobals.getFilter(TransactionInFilter) || new TransactionInFilter()
        this.filter.headerId = this.header ? this.header.id : null;
        this.getData()
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
        if (!this.filter.headerId) {
            return null;
        }

        this._service.getDetails(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res?.data)
                }

                this.dataSource = res.data ? res.data.data : [];
                console.log(res.data.data);
                this.paginator.length = res.data ? res.data.count : 0;
            }
        )
    }

    pageIndexChange(event: PageEvent) {
        this.filter.pageIndex = event.pageIndex
        this.filter.pageSize = event.pageSize
        this.getData()
    }
}
