import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Customer, MemberCard, Organization } from 'src/app/booking_online/model/model';
import { MemberCardFilterModel } from 'src/app/booking_online/model/paging';
import { MemberCardService } from 'src/app/booking_online/service/member-card.service';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';

@Component({
  selector: 'app-dialog-member-card-list',
  templateUrl: './dialog-member-card-list.component.html',
  styleUrls: ['./dialog-member-card-list.component.scss']
})
export class DialogMemberCardListComponent extends  BaseComponent implements OnInit {

  displayedColumns = ['STT', 'golf_CardNo', 'golf_FullName','organizationName','golf_IDNo', 'golf_Mobilephone','golf_Email',
                      'golf_Effective_Date','golf_Expire_Date', 'golf_IsActive', 'golf_IsLock'];
  filter: MemberCardFilterModel = new MemberCardFilterModel()
  listMemberCard: MemberCard[]
  
  listOrganization : Organization[] = []
  organizationSelectControl = new FormControl()
  organizationFilteredOptions: Observable<Organization[]>
  memberCard : MemberCard;
  customer: Customer;
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor(
    public dialogRef: MatDialogRef<DialogMemberCardListComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    private organizationSv: OrganizationService,
    private memberCardSv: MemberCardService,
    public dialog: MatDialog) 
  {
    super(dialog);
    this.customer = data;
    this.filter.golf_FullName = this.customer?.fullName;
  }

  async ngOnInit() {
    this.search();
    this.listOrganization = (await this.organizationSv.getAll()).data || []    
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
    this.filter.orgCode = this.organizationSelectControl.value?.code || null
  }
  
  displayOrganization(org?: Organization): string {
    return org?.name || ''
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.search()
  }

  addMemberCard() {
    this.memberCard.mB_Customer_Id = this.customer.id;
    
    this.memberCardSv.assign(this.memberCard).then(
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

  async search() {
    if(this.filter.golf_CardNo){
      this.memberCardSv.search(this.filter).then(
        res => {
          if (!res?.isSuccess) {
            this.alert(res.message)
            return;
          }
          this.listMemberCard = res?.data || []
          this.paginator.length = res?.data.count
        },
        err => {
          console.log(err)
        })
    }
    
      //var res = (await this.memberCardSv.search(this.filter))?.data;      
  }

  chooseMemberCard (row) {
     this.memberCard = row;
  }

}
