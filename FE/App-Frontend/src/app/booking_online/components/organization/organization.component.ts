import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { Organization } from '../../model/model';
import { OrganizationFilterModel } from '../../model/paging';
import { OrganizationService } from '../../service/organization.service';
import { DialogOrganizationDetailComponent } from './dialog-organization-detail/dialog-organization-detail.component';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name','organizationTypeName','phoneSupport','emailSupport','noteSupport','isActive', 'isSummary','actions'];
  filter: OrganizationFilterModel = new OrganizationFilterModel()
  listOrganization : Organization[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private organizationService: OrganizationService, public dialog: MatDialog) { 
     super(dialog)
   }

   ngOnInit() {
    this.getData()
  }



  search() {
    this.filter.pageIndex = 0
    this.paginator.length = 0
    this.getData()
  }

  getData() {
    this.organizationService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listOrganization = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(organization: Organization) {
    if(!organization){
      organization = new Organization();
      organization.isActive = true;
    }
    const dialogRef = this.dialog.open(DialogOrganizationDetailComponent, {
      width: '60vw',
      data: Object.assign({},organization)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }

  async delete(item: Organization) {
    if (!(await this.confirm('Xóa organization?'))) return;

    this.organizationService.detele(item.id).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res)
          alert('error')
        }
        this.getData()
      },
      err => {
        console.error(err)
      }
    )
  }

}
