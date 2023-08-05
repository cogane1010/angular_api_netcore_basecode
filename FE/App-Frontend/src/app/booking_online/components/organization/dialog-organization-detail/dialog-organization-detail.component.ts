import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Organization, OrganizationInfo, OrganizationType } from 'src/app/booking_online/model/model';
import { OrganizationService, OrganizationTypeService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
  selector: 'app-dialog-organization-detail',
  templateUrl: './dialog-organization-detail.component.html',
  styleUrls: ['./dialog-organization-detail.component.scss']
})
export class DialogOrganizationDetailComponent extends BaseComponent implements OnInit {

  organization: Organization

  listOrganizationType : OrganizationType[] = []
  organizationTypeSelectControl = new FormControl()
  organizationTypeFilteredOptions: Observable<OrganizationType[]>

  constructor(public dialogRef: MatDialogRef<DialogOrganizationDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
      private organizationService: OrganizationService, 
      private organizationTypeSv: OrganizationTypeService,
      public dialog: MatDialog) 
  {
    super(dialog);
    this.dialogRef.disableClose = true;
    this.organization = data || new Organization();
    this.organization.organizationInfo = this.organization.organizationInfo || new OrganizationInfo();
    this.organization.organizationInfo.c_Org_Id = this.organization.id;
    
  }
  async ngOnInit() {
    this.listOrganizationType = (await this.organizationTypeSv.getAll()).data || []
    this.organizationTypeSelectControl.setValue(this.listOrganizationType.find(x => x.id == this.organization.c_OrgType_Id));
    this.organizationTypeFilteredOptions = this.organizationTypeSelectControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterOrganizationType(value))
    )
  }

  private _filterOrganizationType(value: string): OrganizationType[] {
    const filterValue = value?.toString().toLowerCase();
    return this.listOrganizationType.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
  }
  
  onSelectedOrganizationType(event) {
  }
  changeOrganizationTypeText() {
    this.organization.c_OrgType_Id = this.organizationTypeSelectControl.value?.id || null
  }
  
  displayOrganizationType(orgType?: OrganizationType): string {
    return orgType?.name || ''
  }
  addOrEdit(Organization: Organization) {
    this.organizationService.addOrEdit(Organization).then(
      res => {
        if (!res?.isSuccess) {
          this.alert(res.message)
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
