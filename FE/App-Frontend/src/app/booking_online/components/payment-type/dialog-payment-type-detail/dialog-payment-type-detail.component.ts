import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Organization, PaymentType } from 'src/app/booking_online/model/model';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { PaymentTypeService } from 'src/app/booking_online/service/payment-type.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
  selector: 'app-dialog-payment-type-detail',
  templateUrl: './dialog-payment-type-detail.component.html',
  styleUrls: ['./dialog-payment-type-detail.component.scss']
})
export class DialogPaymentTypeDetailComponent extends BaseComponent implements OnInit {

  paymentType: PaymentType
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>

  constructor(public dialogRef: MatDialogRef<DialogPaymentTypeDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    private paymentTypeService: PaymentTypeService, 
    private organizationSv: OrganizationService,
    public dialog: MatDialog) 
  {
    super(dialog);
    this.dialogRef.disableClose = true
    this.paymentType = data || new PaymentType()
    
  }
  async ngOnInit() {
    this.listOrganization = (await this.organizationSv.getAll()).data || []
    this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.paymentType.c_Org_Id));
    this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterOrganization(value))
    )
  }

  private _filterOrganization(value: string): Organization[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listOrganization.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  onSelectedOrganization(event) {
  }
  changeOrganizationText() {
    this.paymentType.c_Org_Id = this.organizationSelectControl.value?.id || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }

  addOrEdit(paymentType: PaymentType) {
    this.paymentTypeService.addOrEdit(paymentType).then(
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
