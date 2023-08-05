import { Component } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { DialogAlertComponent } from '../dialog-alert/dialog-alert.component'
import { DialogConfirmComponent } from '../dialog-confirm/dialog-confirm.component'
import { DialogErrorComponent } from '../dialog-error/dialog-error.component'

@Component({
    selector: 'app-base-component',
    templateUrl: './base-component.component.html',
    styleUrls: ['./base-component.component.scss']
})
export class BaseComponent {

    constructor(public dialog: MatDialog) { }

    async alert(msg: any) {
        let dialogRef = this.dialog.open(DialogAlertComponent, {
            width: '30vw',
            data: msg
        })
        return await dialogRef.afterClosed().toPromise();
    }

    async confirm(msg: string) {
        let dialogRef = this.dialog.open(DialogConfirmComponent, {
            width: '30vw',
            data: msg,
            disableClose: true
        })
        return await dialogRef.afterClosed().toPromise()
    }

    async error(msg: string) {
        let dialogRef =  this.dialog.open(DialogErrorComponent, {
            width: '30vw',
            data: msg
        })
        return await dialogRef.afterClosed().toPromise();
    }

}
