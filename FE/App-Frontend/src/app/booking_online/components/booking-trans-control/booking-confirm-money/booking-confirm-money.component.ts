import { Component, OnInit, ViewChild } from '@angular/core';
import { MatOption } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSelect } from '@angular/material/select';
import { asEnumerable } from 'linq-es2015';
import { Setting } from 'src/app/admin/model';
import { SettingService } from 'src/app/admin/service/setting.service';
import {
    NotConfirmMoneyList,
    Organization, OutTransactionDetails, PaymentCompare, SaveCompareModel, SeaBankCheckCode, SeaBankCheckCodeEnum, TransactionInFilter
    , TransactionNotCompareLine
} from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { BookingTransCompareSelectDateComponent } from '../booking-trans-compare/booking-trans-compare-select-date/booking-trans-compare-select-date.component';
import { ConfirmMoneyDialogComponent } from './confirm-money-dialog/confirm-money-dialog.component';
import { TransDateConfirmComponent } from './trans-date-confirm/trans-date-confirm.component';



@Component({
   selector: 'app-booking-confirm-money',
   templateUrl: './booking-confirm-money.component.html',
   styleUrls: ['./booking-confirm-money.component.scss']
})

export class BookingConfirmMoneyComponent extends BaseComponent implements OnInit {
    curOrg: Organization;
    filter: TransactionInFilter = new TransactionInFilter();
    paymentCompare: OutTransactionDetails[] = []
    item = new SaveCompareModel();
    allSelected=false;
    rcCodes = [{ value: null, viewValue: '----' },
    { value: SeaBankCheckCodeEnum.SbYesPartNo, viewValue: SeaBankCheckCode.SbYesPartNo },
    { value: SeaBankCheckCodeEnum.SbNoPartYes, viewValue: SeaBankCheckCode.SbNoPartYes },
    { value: SeaBankCheckCodeEnum.IncorrectInfo, viewValue: SeaBankCheckCode.IncorrectInfo },
    { value: SeaBankCheckCodeEnum.ReturnCasd, viewValue: SeaBankCheckCode.ReturnCasd },
    { value: SeaBankCheckCodeEnum.Compensation, viewValue: SeaBankCheckCode.Compensation }]
    @ViewChild('select') select: MatSelect;
    numOfDayNotCompare: number;
    totalMoney: number;
    totalCancelMoney: number;
    moneyGoAcc: number;
    statusList = asEnumerable([{"code": "Map","title": "Khớp dữ liệu"},{"code": "NotMap","title": "Không khớp dữ liệu"},{"code": "MoneyGoAcc","title": "Tiền đã về"},{"code": "CancelLine","title": "Hủy giao dịch"}]).ToArray();


    constructor(private _service: TransactionInService,
        public dialog: MatDialog,
        private _settingService: SettingService) {
        super(dialog);
    }

    ngOnInit() {
        this.getNumOfDayNotCompare();
        this.filter.pageSize = -1;
        this.filter.FindDate = new Date();
        this.filter.FindDate.setDate(this.filter.FindDate.getDate() - 1);
        this.search();
    }

    getNumOfDayNotCompare() {
        this._service.getNumOfDayNotConfirmMoneyCount().then(
            res => {
                const cfg = res.data as Setting;
                this.numOfDayNotCompare = cfg && !isNaN(+cfg) && + cfg > 0 ? +cfg : 0;
            },
            err => {
                console.log(err);
            }
        );
    }

    search() {
        this._service.getDetailOutMoneyData(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res);
                    this.paymentCompare = [];
                    return
                }
                this.paymentCompare = res.data;
                this.calculation();
                console.log(this.paymentCompare)              
            },
            err => {
                console.log(err);
            }
        )
    }
    calculation() {
        this.totalMoney = 0;
        this.totalCancelMoney = 0;
        this.moneyGoAcc = 0;
        if (this.paymentCompare)
          for (let row of this.paymentCompare) {
            if(row.trans_type == '1'){
                this.totalMoney += row.totalMoney;      
            }
            if(row.trans_type == '0'){
                this.totalCancelMoney += row.sdkRefundMoney;  
            }
                           
            this.moneyGoAcc += row.totalMoneyAcc;
          }   
      }
    manualConfirm(row: OutTransactionDetails){
        const dialogRef = this.dialog.open(ConfirmMoneyDialogComponent, {
            width: '60vw',
            maxWidth: '60vw',
            height: '30vw',            
            data: row
        });

        dialogRef.afterClosed().subscribe(
            (res: boolean) => {
                if (res) {                    
                    this.search();
                }
            },
            err => {
                console.log(err);
            }
        );
    }
    confirmMoney() {
        var data = this.paymentCompare.filter(x => x.isSelected);
        if(data.length == 0){
            this.alert('Chọn ít nhất 1 giao dịch');
            return;
        }
        data.forEach(x => {
            if(x.statusLine == 2){
                this.alert('Có 1 dòng chưa khớp với dữ liệu');
                return;
            }
        });
        var outData = data.filter(x => x.statusLine == 1);
        if(outData.length > 0){
            this._service.confirmMoney(outData).then(
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

    cancelConfirmMoney() {
        var data = this.paymentCompare.filter(x => x.isSelected);
        if(data.length == 0){
            this.alert('Chọn ít nhất 1 giao dịch');
            return;
        }
        data.forEach(x => {
            if(!x.inMoneyUser){
                this.alert('Bạn chỉ hủy xác nhận với những giao dịch được bạn xác nhận');
                return;
            }
        });
        var outData = data.filter(x => x.inMoneyUser );
        if(outData.length > 0){
            this._service.cancelconfirmMoney(outData).then(
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

    async cancelTrans() {
        const isConfirm = await this.confirm('Bạn chắc chắn hủy giao dịch này không?');
        if (!isConfirm) {
            return;
        }
       
        var cancelData = this.paymentCompare.filter(x => x.isSelected);        
        console.log(cancelData);
        if(cancelData.length == 0){
            this.alert('Chọn ít nhất 1 giao dịch');
            return;
        }else{
            this._service.cancelOutDataTrans(cancelData).then(
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

    get enableSave() {
        return this.paymentCompare && Array.isArray(this.paymentCompare) && this.paymentCompare.length > 0 && this.paymentCompare.filter(x => x.isSelected).length > 0
    }
    get enableCancelTrans() {
        return this.paymentCompare && Array.isArray(this.paymentCompare) && this.paymentCompare.length > 0
    }

    selectDate() {
        const dialogRef = this.dialog.open(TransDateConfirmComponent, {
            width: '60vw',
            maxWidth: '60vw'
        });

        dialogRef.afterClosed().subscribe(
            (res: NotConfirmMoneyList) => {
                if (res) { 
                    this.filter.FindDate = res.dateId;                    
                    this.search();
                }
            },
            err => {
                console.log(err);
            }
        );
    }

    // get changedRows() {
    //     return this.paymentCompare.filter(x => x.userRc_code !== x.old_userRc_code);
    // }

    toggleAllSelection(completed: boolean) {       
        if (this.paymentCompare == null) {
        return;
        }
        this.paymentCompare.forEach(t => (t.isSelected = completed));
    }
   
    export(){
        this._service.exportExcelConfirmMoney(this.filter).then(
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
    
}

