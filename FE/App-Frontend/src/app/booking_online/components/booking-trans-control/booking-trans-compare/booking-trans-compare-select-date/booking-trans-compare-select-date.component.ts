import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { SaveCompareModel, TransactionNotCompareFilter, TransactionNotCompareLine } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
    selector: 'app-booking-trans-compare-select-date',
    templateUrl: './booking-trans-compare-select-date.component.html',
    styleUrls: ['./booking-trans-compare-select-date.component.scss']
})
export class BookingTransCompareSelectDateComponent extends BaseComponent implements OnInit {

    displayedColumns: string[] = ['uiSelected', 'STT', 'transDate', 'noOfRecord'];
    dataSource: TransactionNotCompareLine[] = [];
    @ViewChild(MatPaginator) paginator: MatPaginator;

    filter = new TransactionNotCompareFilter();

    //uiAllSelected = false;

    constructor(public dialogRef: MatDialogRef<BookingTransCompareSelectDateComponent>,
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
        this._service.getNotCompareDate(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res?.data)
                }

                console.error(res?.data)
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

    rowClicked(row: TransactionNotCompareLine) {
        row.uiSelected = !row.uiSelected;
    }

    select() {
        const row = this.dataSource.find(x => x.uiSelected);
        if (row) {
            this.dialogRef.close(row);
        }
    }

    save() {
        if (this.dataSource && Array.isArray(this.dataSource) && this.dataSource.length > 0) {
            const row = this.dataSource.find(x => x.uiSelected);
            if (row) {
                var saveModel = new SaveCompareModel();
                saveModel.rows = row.paymentsCompare;
                this._service.saveCompare(saveModel).then(
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
