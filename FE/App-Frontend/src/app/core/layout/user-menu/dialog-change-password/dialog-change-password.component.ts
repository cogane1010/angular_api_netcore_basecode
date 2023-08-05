import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthService } from 'src/app/admin/auth/auth.service';
import { UserService } from 'src/app/admin/service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
    selector: 'app-dialog-change-password',
    templateUrl: './dialog-change-password.component.html',
    styleUrls: ['./dialog-change-password.component.scss']
})
export class DialogChangePasswordComponent extends BaseComponent implements OnInit {

    changePassword: any = {}

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogChangePasswordComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private userService: UserService,
        private authService: AuthService
    ) {
        super(dialog);
    }

    ngOnInit() {
    }

    save() {
        this.userService.changePassword(this.changePassword).then(
            res => {
                if (res?.isSuccess) {
                    this.alert("Change password success")
                    this.dialogRef.close()
                    this.authService.signOut()
                } else {
                    console.log(res)
                    this.error(res.message)
                }
            }
        )
    }
}
