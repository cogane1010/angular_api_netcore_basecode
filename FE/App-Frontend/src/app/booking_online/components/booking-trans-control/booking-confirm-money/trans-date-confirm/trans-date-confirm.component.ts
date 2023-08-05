import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { NotConfirmMoneyList, SaveCompareModel, TransactionNotCompareFilter, TransactionNotCompareLine } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
   selector: 'app-trans-date-confirm',
   templateUrl: './trans-date-confirm.component.html',
   styleUrls: ['./trans-date-confirm.component.scss']
})
export class TransDateConfirmComponent extends BaseComponent implements OnInit {

    displayedColumns: string[] = ['STT', 'transDate', 'noOfRecord', 'totalMoney', 'action'];
    dataSource: NotConfirmMoneyList[] = [];
    @ViewChild(MatPaginator) paginator: MatPaginator;

    filter = new TransactionNotCompareFilter();

    //uiAllSelected = false;

    constructor(public dialogRef: MatDialogRef<TransDateConfirmComponent>,
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
        this._service.getNotConfirmMoneyDate(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res?.data)
                }

                console.error(res?.data)
                this.dataSource = res.data ? res.data : [];   
            }
        )
        this.getNumOfDayNotCompare();
    }

    getNumOfDayNotCompare() {
        this._service.getNumOfDayNotConfirmMoneyCount().then(
            res => {
                const cfg = res.data;
                this.paginator.length = cfg && !isNaN(+cfg) && + cfg > 0 ? +cfg : 0;
            },
            err => {
                console.log(err);
            }
        );
    }

    pageIndexChange(event: PageEvent) {
        this.filter.pageIndex = event.pageIndex
        this.filter.pageSize = event.pageSize
        this.getData()
    }

    rowClicked(row: TransactionNotCompareLine) {
        row.uiSelected = !row.uiSelected;
    }

    select(row:NotConfirmMoneyList) {      
      if (row) {
          this.dialogRef.close(row);
      }
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
