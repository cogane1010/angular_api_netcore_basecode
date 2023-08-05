import { Component, OnInit } from '@angular/core';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { BookingOutEmailSetting } from 'src/app/booking_online/model/model';
import { TransactionInService } from 'src/app/booking_online/service/transaction-approve.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { COMMA, ENTER } from '@angular/cdk/keycodes';

@Component({
    selector: 'app-booking-out-email-setting',
    templateUrl: './booking-out-email-setting.component.html',
    styleUrls: ['./booking-out-email-setting.component.scss']
})
export class BookingOutEmailSettingComponent extends BaseComponent implements OnInit {

    item = new BookingOutEmailSetting();
    dows = [{ code: '2', name: 'Thứ 2', check: false }, { code: '3', name: 'Thứ 3', check: false },
    { code: '4', name: 'Thứ 4', check: false }, { code: '5', name: 'Thứ 5', check: false }, { code: '6', name: 'Thứ 6', check: false },
    { code: '7', name: 'Thứ 7', check: false },{ code: '1', name: 'Chủ nhật', check: false }, { code: '0', name: 'Ngày lễ', check: false }];

    addOnBlur = true;
    readonly separatorKeysCodes = [ENTER, COMMA] as const;
    mailTos = [];
    mailCcs = [];
    mailBccs = [];

    constructor(private _service: TransactionInService, public dialog: MatDialog) {
        super(dialog)
    }

    ngOnInit() {
        this.getData();
    }

    getData() {
        this._service.getOutEmailSetting().then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res);
                } else {
                    this.item = res.data;

                    // dows
                    if (this.item && this.item.transaction_out_email_dow && this.item.transaction_out_email_dow.trim() !== '') {
                        for (let i = 0; i < this.item.transaction_out_email_dow.length; i++) {
                            const element = this.item.transaction_out_email_dow[i];
                            var dow = this.dows.find(x => x.code === element);
                            if (dow) {
                                dow.check = true;
                            }
                        }
                    }

                    // mail addresses
                    if (this.item && this.item.transaction_out_email_to && this.item.transaction_out_email_to.trim() !== '') {
                        const addresses = this.item.transaction_out_email_to.split(';');
                        this.mailTos = addresses && Array.isArray(addresses) && addresses.length > 0 ? addresses : [];
                    }
                    if (this.item && this.item.transaction_out_email_cc && this.item.transaction_out_email_cc.trim() !== '') {
                        const addresses = this.item.transaction_out_email_cc.split(';');
                        this.mailCcs = addresses && Array.isArray(addresses) && addresses.length > 0 ? addresses : [];
                    }
                    if (this.item && this.item.transaction_out_email_bcc && this.item.transaction_out_email_bcc.trim() !== '') {
                        const addresses = this.item.transaction_out_email_bcc.split(';');
                        this.mailBccs = addresses && Array.isArray(addresses) && addresses.length > 0 ? addresses : [];
                    }
                }
            }
        )
    }

    getFormDate() {
        // dow
        let dow = '';
        for (let i = 0; i < this.dows.length; i++) {
            const element = this.dows[i];
            if (element.check) {
                dow += element.code + ',';
            }
        }
        // xóa dấu ',' cuối
        if (dow && dow.length > 0) {
            dow = dow.substring(0, dow.length - 1);
        }
        this.item.transaction_out_email_dow = dow;

        // mail addresses
        this.item.transaction_out_email_to = this.mailTos && Array.isArray(this.mailTos) && this.mailTos.length > 0 ? this.mailTos.join(';') : '';
        this.item.transaction_out_email_cc = this.mailCcs && Array.isArray(this.mailCcs) && this.mailCcs.length > 0 ? this.mailCcs.join(';') : '';
        this.item.transaction_out_email_bcc = this.mailBccs && Array.isArray(this.mailBccs) && this.mailBccs.length > 0 ? this.mailBccs.join(';') : '';
    }

    save() {
        this.getFormDate();
        this._service.saveOutEmailSetting(this.item).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res);
                    this.error(res.message);
                } else {
                    this.alert("Lưu thông tin thành công")
                }
            }
        )
    }

    testSendMail() {
        this.getFormDate();
        this._service.testSendMail(this.item).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res);
                    this.error(res.message)
                } else {
                    this.alert("Gửi email thành công")
                }
            }
        )
    }

    addMailTo(event: MatChipInputEvent): void {
        const value = (event.value || '').trim();

        // Add our address
        if (value) {
            this.mailTos.push(value);
        }

        // Clear the input value
        event.chipInput!.clear();
    }

    addMailCc(event: MatChipInputEvent): void {
        const value = (event.value || '').trim();

        // Add our fruit
        if (value) {
            this.mailCcs.push(value);
        }

        // Clear the input value
        event.chipInput!.clear();
    }

    addMailBcc(event: MatChipInputEvent): void {
        const value = (event.value || '').trim();

        // Add our fruit
        if (value) {
            this.mailBccs.push(value);
        }

        // Clear the input value
        event.chipInput!.clear();
    }

    removeMailTo(address: string): void {
        const index = this.mailTos.indexOf(address);

        if (index >= 0) {
            this.mailTos.splice(index, 1);
        }
    }

    removeMailCc(fruit: string): void {
        const index = this.mailCcs.indexOf(fruit);

        if (index >= 0) {
            this.mailCcs.splice(index, 1);
        }
    }

    removeMailBcc(fruit: string): void {
        const index = this.mailBccs.indexOf(fruit);

        if (index >= 0) {
            this.mailBccs.splice(index, 1);
        }
    }
}

