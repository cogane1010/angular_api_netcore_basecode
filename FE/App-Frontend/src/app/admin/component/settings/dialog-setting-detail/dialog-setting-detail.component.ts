import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Setting } from 'src/app/admin/model';
import { SettingService } from 'src/app/admin/service/setting.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
    selector: 'app-dialog-setting-detail',
    templateUrl: './dialog-setting-detail.component.html',
    styleUrls: ['./dialog-setting-detail.component.scss']
})
export class DialogSettingDetailComponent extends BaseComponent implements OnInit {

    setting: Setting = new Setting()

    constructor(
        private settingService: SettingService, 
        public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogSettingDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any) 
    {
        super(dialog);
        this.dialogRef.keydownEvents().subscribe(event => {
            if (event.key === "Escape") 
                this.dialogRef.close()
        })
        
        if (data)
            this.setting = Object.assign({}, data)
        else {
            this.setting = new Setting()
        }
    }

    ngOnInit() {
    }

    addOrEdit(setting: Setting) {

        this.settingService.addOrEdit(setting).then(
            res => {
                if (!res?.isSuccess) {
                    console.log(res)
                    this.error(res?.message)
                    return
                }
                this.dialogRef.close(true)
            },
            err => {
                console.log(err)
            }
        )
    }

}
