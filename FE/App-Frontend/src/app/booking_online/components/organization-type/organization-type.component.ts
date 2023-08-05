import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { OrganizationType } from '../../model/model';
import { OrganizationTypeFilterModel } from '../../model/paging';
import { OrganizationTypeService } from '../../service/organization.service';
import { DialogOrganizationTypeDetailComponent } from './dialog-organization-type-detail/dialog-organization-type-detail.component';

@Component({
  selector: 'app-organization-type',
  templateUrl: './organization-type.component.html',
  styleUrls: ['./organization-type.component.scss']
})
export class OrganizationTypeComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'actions'];
  filter: OrganizationTypeFilterModel = new OrganizationTypeFilterModel()
  listOrganizationType : OrganizationType[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private organizationTypeService: OrganizationTypeService, public dialog: MatDialog) { 
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
    this.organizationTypeService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listOrganizationType = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(OrganizationType: OrganizationType) {
    const dialogRef = this.dialog.open(DialogOrganizationTypeDetailComponent, {
      width: '60vw',
      data: Object.assign({},OrganizationType)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }

  async delete(item: OrganizationType) {
    if (!(await this.confirm('Xóa organization type?'))) return;

    this.organizationTypeService.detele(item.id).then(
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
