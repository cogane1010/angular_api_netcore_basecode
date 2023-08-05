import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Booking, OutTransactionDetails, SaveCompareModel, TransactionNotCompareFilter, TransactionNotCompareLine } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
   selector: 'app-confirm-money-dialog',
   templateUrl: './confirm-money-dialog.component.html',
   styleUrls: ['./confirm-money-dialog.component.scss']
})
export class ConfirmMoneyDialogComponent extends BaseComponent implements OnInit {
    
    dataSource: OutTransactionDetails;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    isReadonly:boolean= true;
    bookingCode:string
    //uiAllSelected = false;

    constructor(public dialogRef: MatDialogRef<ConfirmMoneyDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private _service: TransactionInService,
        public dialog: MatDialog) {
        super(dialog);
        this.dataSource = data;
        if(!this.dataSource.booking){
          this.dataSource.booking = new Booking();
        }
    }

    ngOnInit(): void {
       
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

   

    save() {
      this._service.manualConfirmMoney(this.dataSource).then(
        res => {
            if (!res?.isSuccess) {
                this.alert(`Có lỗi xảy ra ${res?.message}`);
                console.error(res);
                return;
            }else{
                this.dialogRef.close(true); 
            }           
        }
    )
    }

    

    close() {
        this.dialogRef.close(false);
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
