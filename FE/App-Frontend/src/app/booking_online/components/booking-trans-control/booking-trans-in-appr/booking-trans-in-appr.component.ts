import { HttpEventType } from '@angular/common/http';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { InTransactionHeader, TransactionInFilter } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { AppGlobals } from 'src/app/common/app-global';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { UploadSizeMax } from 'src/app/common/enum';
import { TransactionInDetailsDialogComponent } from './trans-details-dialog/trans-details-dialog.component';

@Component({
    selector: 'app-booking-trans-in-appr',
    templateUrl: './booking-trans-in-appr.component.html',
    styleUrls: ['./booking-trans-in-appr.component.scss']
})
export class BookingTransactionInApproveComponent extends BaseComponent implements OnInit, OnDestroy {

    displayedColumns: string[] = ['STT', 'FileName', 'CreatedUser', 'CreatedDate', 'Status', 'isActive', 'action']
    filter: TransactionInFilter = new TransactionInFilter()
    dataSource: InTransactionHeader[] = []

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
        this.filter.FindDate = new Date();
        this.filter.FindDate.setDate(new Date().getDate()-1)
        this.getData();
    }

    ngOnDestroy(): void {
        AppGlobals.setFilter(TransactionInFilter, this.filter)
    }

    search() {
        this.filter.pageIndex = 0
        this.paginator.length = 0
        this.getData();
    }

    getData() {
        this._service.getPaging(this.filter).then(
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

    fileChanged(fileInput) {
        var extension = fileInput.target.files[0].name.split('.').pop().toLowerCase();
        console.log(extension);
        if (!this.extensionFileAccept.includes(extension)) {
            this.error("Chỉ được upload file .xlsx");
            return
        }

        this.isUploaded = false;
        this.uploadFiles = fileInput.target.files[0];
        this.fileProgress = null;
    }

    fileInputClick(elementId: any) {
        var file_input = document.getElementById(elementId);
        file_input.click();
    }

    getseabankfile(){
        if (!confirm('Lấy file đối xoát sẽ phải đuyệt đối xoát lại ở các sân?')) {
            return;
        }

        if (!confirm('Bạn có chắc muốn lấy file đối xoát ngày ' + new Date(this.filter.FindDate).toLocaleDateString('en-GB') + ' từ seabank?')) {
            return;
        }

        this._service.getseabankfile(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res);
                    this.alert(res?.message);        
                } else {
                    this.search();
                }
            }
        );


        // this._service.getseabankfile(this.filter).subscribe(events => {
        //     this.search();
        // },
        //     error => {
        //         this.alert(`Có lỗi xảy ra ${error?.data}`);
        //         console.log(error);
        //     }
        // );

    }

    uploadFile() {
        if (!confirm('Bạn có chắc muốn nhập dữ liệu?')) {
            return;
        }
        if(!this.uploadFiles.name.includes('INC_SB_BRG')){
            this.alert('File upload sai định dạng tên.');
            return;
        }
        this._service.checkTransSave(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    if (!confirm(`${res?.message}`)) {
                        return;
                    }else{
                        const formData = new FormData();
                        formData.append(new Date(this.filter.FindDate).toLocaleString("en-US"), this.uploadFiles, this.uploadFiles.name);
                        if (this.uploadFiles)
                            this._service.uploadFile(formData).subscribe(events => {
                                if (events.type === HttpEventType.UploadProgress) {
                                    this.fileProgress = Math.round(events.loaded / events.total * 100) + '%';
                                } else if (events.type === HttpEventType.Response) {                    
                                    if(!(events.body as any).isSuccess){
                                        this.alert(`Có lỗi xảy ra ${(events.body as any).message}`);
                                    }else{                       
                                        this.clearFile();
                                        this.alert('Upload thành công');
                                        this.search();
                                    }                    
                                }
                            },
                                error => {
                                    this.alert(`Có lỗi xảy ra ${error?.data}`);
                                    console.log(error);
                                }
                            );
                        }      
                    }else{
                        const formData = new FormData();
                        formData.append(new Date(this.filter.FindDate).toLocaleString("en-US"), this.uploadFiles, this.uploadFiles.name);
                        if (this.uploadFiles)
                            this._service.uploadFile(formData).subscribe(events => {
                                if (events.type === HttpEventType.UploadProgress) {
                                    this.fileProgress = Math.round(events.loaded / events.total * 100) + '%';
                                } else if (events.type === HttpEventType.Response) {                    
                                    if(!(events.body as any).isSuccess){
                                        this.alert(`Có lỗi xảy ra ${(events.body as any).message}`);
                                    }else{                       
                                        this.clearFile();
                                        this.alert('Upload thành công');
                                        this.search();
                                    }                    
                                }
                            },
                                error => {
                                    this.alert(`Có lỗi xảy ra ${error?.data}`);
                                    console.log(error);
                                }
                            );
                    }                   
                }
            )
        }

    clearFile() {
        this.fileProgress = null;
        this.isUploaded = true;
        this.uploadFiles = null;
        this.upFile.nativeElement.value = null;
    }

    viewDetails(item: InTransactionHeader) {
        const dialogRef = this.dialog.open(TransactionInDetailsDialogComponent, {
            width: '98vw',
            maxWidth: '100vw',
            data: item
        })
    }

    approve() {
        if (!confirm('Bạn có chắc muốn duyệt dữ liệu giao dịch?')) {
            return;
        }

        this._service.approveIn(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    this.alert(`Có lỗi xảy ra ${res?.message}`);
                    console.error(res);
                    return;
                }
                this.getData();
            }
        )
    }
}
