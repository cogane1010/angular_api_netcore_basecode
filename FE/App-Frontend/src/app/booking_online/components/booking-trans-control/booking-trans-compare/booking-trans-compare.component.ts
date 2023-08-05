import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Setting } from 'src/app/admin/model';
import { SettingService } from 'src/app/admin/service/setting.service';
import {
    Organization, PaymentCompare, SaveCompareModel, SeaBankCheckCode, SeaBankCheckCodeEnum, TransactionInFilter
    , TransactionNotCompareLine
} from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { BookingTransCompareSelectDateComponent } from './booking-trans-compare-select-date/booking-trans-compare-select-date.component';

@Component({
    selector: 'app-booking-trans-compare',
    templateUrl: './booking-trans-compare.component.html',
    styleUrls: ['./booking-trans-compare.component.scss']
})

export class BookingTransCompareComponent extends BaseComponent implements OnInit {
    curOrg: Organization;
    filter: TransactionInFilter = new TransactionInFilter();
    paymentCompare: PaymentCompare[] = []
    item = new SaveCompareModel();
    total:number = 0;
    totalTransAmtSB: number = 0;
    totalTienHoanSB: number=0;
    totalTienDatCocBRG:number=0;
    totalTienHoanBrg:number=0;
    statusComparetrans:string;
    statusCompareInt:string;

    rcCodes = [{ value: null, viewValue: '----' },
    { value: SeaBankCheckCodeEnum.SbYesPartNo, viewValue: SeaBankCheckCode.SbYesPartNo },
    { value: SeaBankCheckCodeEnum.SbNoPartYes, viewValue: SeaBankCheckCode.SbNoPartYes },
    { value: SeaBankCheckCodeEnum.IncorrectInfo, viewValue: SeaBankCheckCode.IncorrectInfo },
    { value: SeaBankCheckCodeEnum.ReturnCasd, viewValue: SeaBankCheckCode.ReturnCasd },
    { value: SeaBankCheckCodeEnum.Compensation, viewValue: SeaBankCheckCode.Compensation }]

    numOfDayNotCompare: number;

    constructor(private _service: TransactionInService,
        public dialog: MatDialog,
        private _settingService: SettingService) {
        super(dialog);
    }

    ngOnInit() {
        this.getNumOfDayNotCompare();
        this.getStatusCompareTrans();
        this.filter.pageSize = -1;
        this.filter.FindDate = new Date();
        this.filter.FindDate.setDate(this.filter.FindDate.getDate() - 1);
        this.search();
    }

    getNumOfDayNotCompare() {
        this._service.getNumOfDayNotCompare().then(
            res => {
                const cfg = res.data as Setting;
                this.numOfDayNotCompare = cfg && !isNaN(+cfg.value) && +cfg.value > 0 ? +cfg.value : 0;
            },
            err => {
                console.log(err);
            }
        );
    }

    getStatusCompareTrans() {
        this._service.getStatusCompareTrans(this.filter).then(
            res => {                
                this.statusComparetrans = 'Chưa xác nhận';
                this.statusCompareInt = res.data
                if(res.data == 'created'){
                    this.statusComparetrans = 'Đã xác nhận';                    
                }
                if(res.data == 'outApprove'){
                    this.statusComparetrans = 'Đã duyệt';
                }
            },
            err => {
                console.log(err);
            }
        );
    }

    search() {
        this._service.getTransactionCompare(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res);
                    this.paymentCompare = [];
                    return
                }
                this.paymentCompare = res.data;
                console.log(res.data);                
                console.log(this.paymentCompare);
                this.total = 0;
                if (this.paymentCompare && this.paymentCompare.length > 0) {
                    for (let i = 0; i < this.paymentCompare.length; i++) {
                       this.total += 1;
                    }
                    this.total += -1;
                }
            },
            err => {
                console.log(err);
            }
        )
        this.getStatusCompareTrans();
    }

    save() {
        this.item.rows = JSON.parse(JSON.stringify(this.paymentCompare));
        this.item.findDate = this.filter.FindDate;
        console.log(this.item.rows);
        this._service.saveCompare(this.item).then(
            res => {
                if (!res?.isSuccess) {
                    this.alert(`Có lỗi xảy ra ${res?.message}`);
                    console.error(res);
                    return;
                }
                this.search();
                this.alert('Xác nhận thành công.');
            }
        )
    }

    get enableSave() {
        return this.paymentCompare && Array.isArray(this.paymentCompare) && this.paymentCompare.length > 0
    }

    selectDate() {
        const dialogRef = this.dialog.open(BookingTransCompareSelectDateComponent, {
            width: '60vw',
            maxWidth: '60vw'
        });

        dialogRef.afterClosed().subscribe(
            (res: TransactionNotCompareLine) => {
                if (res) {
                    this.filter.FindDate = res.transDate;                    
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
}
