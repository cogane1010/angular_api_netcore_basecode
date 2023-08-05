import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { OutTransactionHeader, SaveCompareModel, TransactionNotApproveFilter, TransactionNotApproveLine, TransactionNotCompareFilter, TransactionNotCompareLine } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { TransactionInDetailsDialogComponent } from '../../booking-trans-in-appr/trans-details-dialog/trans-details-dialog.component';

@Component({
    selector: 'app-booking-out-appr-select-date',
    templateUrl: './booking-out-appr-select-date.component.html',
    styleUrls: ['./booking-out-appr-select-date.component.scss']
})
export class BookingOutApproveSelectDateComponent extends BaseComponent implements OnInit {

    displayedColumns: string[] = ['uiSelected', 'STT', 'transDate', 'noOfRecord'];
    dataSource: TransactionNotApproveLine[] = [];
    @ViewChild(MatPaginator) paginator: MatPaginator;

    filter = new TransactionNotApproveFilter();

    //uiAllSelected = false;

    constructor(public dialogRef: MatDialogRef<BookingOutApproveSelectDateComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private _service: TransactionInService,
        public dialog: MatDialog) {
        super(dialog);
    }

    ngOnInit(): void {
        this.search();
    }

    search() {
        this.filter.pageIndex = 0
        this.getData()
    }

    getData() {
        this._service.getNotApproveDate(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res?.data)
                }

                this.dataSource = res.data ? res.data : [];
                this.paginator.length = res.data ? res.data.length : 0;

                // if (this.dataSource && Array.isArray(this.dataSource) && this.dataSource.length > 0) {
                //     for (let i = 0; i < this.dataSource.length; i++) {
                //         const element = this.dataSource[i];
                //         element.uiIndex = i;
                //     }
                // }
            }
        )
    }

    pageIndexChange(event: PageEvent) {
        this.filter.pageIndex = event.pageIndex
        this.filter.pageSize = event.pageSize
        this.getData()
    }

    checkChanged(row: TransactionNotCompareLine) {
        // uncheck row khác
        if (this.dataSource && Array.isArray(this.dataSource) && this.dataSource.length > 0 && row.uiSelected) {
            const check = row.uiSelected;
            for (let i = 0; i < this.dataSource.length; i++) {
                const element = this.dataSource[i];
                element.uiSelected = false;
            }
            row.uiSelected = check;
        }
    }

    save() {
        if (this.dataSource && Array.isArray(this.dataSource) && this.dataSource.length > 0) {
            const row = this.dataSource.find(x => x.uiSelected);
            if (row) {
                const saveModel = row.outHeader;
                console.log(saveModel);
                
                this._service.approveOut(saveModel).then(
                    res => {
                        if (!res?.isSuccess) {
                            this.alert(`Có lỗi xảy ra ${res?.message}`);
                            console.error(res);
                            return;
                        }
                        this.search();
                    }
                )
            }
        }
    }

    get enableSave() {
        return this.dataSource && Array.isArray(this.dataSource) && this.dataSource.length > 0 && this.dataSource.find(x => x.uiSelected);
    }

    close() {
        this.dialogRef.close();
    }

    // checkAllChanged() {
    //     if (this.dataSource && Array.isArray(this.dataSource) && this.dataSource.length > 0) {
    //         for (let i = 0; i < this.dataSource.length; i++) {
    //             const element = this.dataSource[i];
    //             element.uiSelected = this.uiAllSelected;
    //         }
    //     }
    // }
}
