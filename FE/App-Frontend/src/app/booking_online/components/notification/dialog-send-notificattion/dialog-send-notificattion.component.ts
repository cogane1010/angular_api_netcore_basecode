import { Component, OnInit, Inject } from '@angular/core'
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog'
import { FormControl } from '@angular/forms'
import { Observable } from 'rxjs'
import { map, startWith } from 'rxjs/operators'
import { FirebaseBody,BodyData,GF_Notification } from '../../../model/model'
import { NotificationService } from '../../../service/notification.service'
import { BaseComponent } from 'src/app/common/base-component/base-component.component'
import { AppConfig } from '../../../../common/app-config'

@Component({
   selector: 'app-dialog-send-notificattion',
   templateUrl: './dialog-send-notificattion.component.html',
   styleUrls: ['./dialog-send-notificattion.component.scss']
})
export class DialogSendNotificattionComponent extends BaseComponent implements OnInit {

    model: GF_Notification
    serverKey:string
    parentSelectControl = new FormControl()

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogSendNotificattionComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any, private notificationService: NotificationService) 
    {
        super(dialog)
        this.dialogRef.disableClose = true
        this.dialogRef.keydownEvents().subscribe(event => {
            if (event.key === "Escape") 
                this.dialogRef.close()
        })

        if (data)
            this.model = Object.assign({}, data)
        else {
            this.model = new GF_Notification()            
        }
        console.log(this.model )
    }

    ngOnInit() {
        
    }  
    
    addOrEdit(serverKey: string) {        
        //var data:FirebaseBody = {to: serverKey, data:{title: this.model.title, body: this.model.body, imgUrl: this.model.imgUrl}}
        this.notificationService.pushNotifi(this.model).then(
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

