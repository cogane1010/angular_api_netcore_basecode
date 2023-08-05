import { Component, OnInit } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { BaseComponent } from 'src/app/common/base-component/base-component.component'
import { TestappService } from '../../service'

@Component({
    selector: 'app-test-app',
    templateUrl: './test-app.component.html',
    styleUrls: ['./test-app.component.scss']
})
export class TestAppComponent extends BaseComponent implements OnInit {

    mailMessage: any = {}
    
    frontNow: Date
    backNow: Date

    frontNow1: Date
    frontNowReturn: Date

    datePicker: Date
    datePicker1: Date
    
    constructor(
        private testappService: TestappService,
        public dialog: MatDialog
    ) {
        super(dialog)
    }

    ngOnInit() {
        this.mailMessage.isDefault = true;
    }

    sendmail() {
        this.testappService.sendMail(this.mailMessage).then(
            res => {
                if (!res?.isSuccess) {
                    this.error(res.message)
                } else {
                    this.alert("Mail sent!")
                }
            }
        )
    }

    getServerTime() {
        this.frontNow = new Date()
        this.testappService.getServerTime().then(res=> {
            this.backNow = res.data
        })
    }

    getTimeReturn() {
        this.frontNow1 = new Date()
        this.testappService.getTimeReturn(this.frontNow1).then(res=> {
            this.frontNowReturn = res.data
        })
    }

    conSoleDate() {
        this.datePicker = new Date()
        console.log(this.datePicker)
        console.log(this.datePicker1)
    }
}
