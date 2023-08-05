import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CustomerGroup, Organization } from 'src/app/booking_online/model/model';
import { CustomerGroupService } from 'src/app/booking_online/service/customer-group.service';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';


@Component({
  selector: 'app-dialog-customer-group-detail',
  templateUrl: './dialog-customer-group-detail.component.html',
  styleUrls: ['./dialog-customer-group-detail.component.scss']
})
export class DialogCustomerGroupDetailComponent extends BaseComponent implements OnInit {

  customerGroup: CustomerGroup
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions:  Observable<Organization[]>

  constructor(public dialogRef: MatDialogRef<DialogCustomerGroupDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    private customerGroupService: CustomerGroupService, 
    private organizationSv: OrganizationService,
    public dialog: MatDialog) 
  {
    super(dialog);
    this.dialogRef.disableClose = true
    this.customerGroup = data || new CustomerGroup()
    
  }
  async ngOnInit() {
    this.listOrganization = (await this.organizationSv.getAll()).data || []
    this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.customerGroup.c_Org_Id));
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
    this.customerGroup.c_Org_Id = this.organizationSelectControl.value?.id || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }

  addOrEdit(customerGroup: CustomerGroup) {
    this.customerGroupService.addOrEdit(customerGroup).then(
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
