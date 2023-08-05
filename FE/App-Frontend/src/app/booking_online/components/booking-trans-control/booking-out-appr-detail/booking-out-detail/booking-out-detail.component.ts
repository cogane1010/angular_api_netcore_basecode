import { HttpEventType } from '@angular/common/http';
import { Component, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { OutTransactionDetails, OutTransactionHeader, TransactionInFilter } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { AppGlobals } from 'src/app/common/app-global';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
   selector: 'app-booking-out-detail',
   templateUrl: './booking-out-detail.component.html',
   styleUrls: ['./booking-out-detail.component.scss']
})
export class BookingOutDetailComponent extends BaseComponent implements OnInit, OnDestroy {

    displayedColumns: string[] = ['STT', 'FileName', 'FilePath', 'CreatedUser', 'CreatedDate', 'Status','ApproveUser','ApproveDate']
    filter: TransactionInFilter = new TransactionInFilter()
    dataSource: OutTransactionDetails[] = []

    /** upload */
    isUploaded = true;
    extensionFileAccept = ["xlsx"];
    uploadFiles: File;
    fileProgress: string;
   
    @ViewChild(MatPaginator) paginator: MatPaginator

    constructor(public dialogRef: MatDialogRef<BookingOutDetailComponent>
      ,@Inject(MAT_DIALOG_DATA) public data: OutTransactionHeader
      ,private _service: TransactionInService
      , public dialog: MatDialog) 
    {
        super(dialog);
        this.filter.outTransHeaderId = data.id
        if(this.filter.outTransHeaderId){
          this.getData()
        }
        this.dialogRef.disableClose = true
    }

    ngOnInit() {
        this.filter = AppGlobals.getFilter(TransactionInFilter) || new TransactionInFilter()
        //this.getData()
    }

    ngOnDestroy(): void {
        AppGlobals.setFilter(TransactionInFilter, this.filter)
    }

    search() {
        this.filter.pageIndex = 0
        this.paginator.length = 100
        this.getData()
    }

    getData() {
      if(this.filter.outTransHeaderId){
        this._service.getOutTransDetail(this.filter).then(
          res => {
              console.log(res);
              if (!res?.isSuccess) {
                  console.error(res?.data)
              }
              this.dataSource = res.data
              console.log(res?.data)
              this.paginator.length = res.data.count
          })
      }
    }

    pageIndexChange(event: PageEvent) {
        this.filter.pageIndex = event.pageIndex
        this.filter.pageSize = event.pageSize
        this.getData()
    }
}

