import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

@Component({
    selector: 'app-dialog-error',
    templateUrl: './dialog-error.component.html',
    styleUrls: ['./dialog-error.component.scss']
})
export class DialogErrorComponent {

    msg: string

    constructor(public dialogRef: MatDialogRef<DialogErrorComponent>,
        @Inject(MAT_DIALOG_DATA) public data: string) {
        this.dialogRef.disableClose = true
        this.msg = data
    }

}
