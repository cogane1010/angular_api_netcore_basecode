import { Component, Inject } from '@angular/core'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

@Component({
    selector: 'app-dialog-alert',
    templateUrl: './dialog-alert.component.html',
    styleUrls: ['./dialog-alert.component.scss']
})
export class DialogAlertComponent {

    msg: any
    get isArrayMsg(): boolean {
        return Array.isArray(this.msg)
    }

    constructor(public dialogRef: MatDialogRef<DialogAlertComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any) 
    {
        this.msg = data
    }

}
