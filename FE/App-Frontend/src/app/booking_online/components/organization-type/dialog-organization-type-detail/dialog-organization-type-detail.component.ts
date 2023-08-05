import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { OrganizationType } from 'src/app/booking_online/model/model';
import { OrganizationTypeService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
  selector: 'app-dialog-organization-type-detail',
  templateUrl: './dialog-organization-type-detail.component.html',
  styleUrls: ['./dialog-organization-type-detail.component.scss']
})
export class DialogOrganizationTypeDetailComponent extends BaseComponent implements OnInit {

  organizationType: OrganizationType

  constructor(public dialogRef: MatDialogRef<DialogOrganizationTypeDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private organizationTypeService: OrganizationTypeService, public dialog: MatDialog) 
  {
    super(dialog);
    this.dialogRef.disableClose = true
    this.organizationType = data || new OrganizationType()
    
  }
  ngOnInit() {
  }

  addOrEdit(OrganizationType: OrganizationType) {
    this.organizationTypeService.addOrEdit(OrganizationType).then(
      res => {
        if (!res?.isSuccess) {
          this.alert('Cập nhật thất bại')
          return
        }
        this.dialogRef.close(true);
        this.alert('Cập nhật thành công');
      },
      err => {
        console.log(err)
      }
    )
  }

}
