import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { CustomerGroup } from 'src/app/booking_online/model/model';
import { CustomerGroupFilterModel } from 'src/app/booking_online/model/paging';
import { CustomerGroupService } from 'src/app/booking_online/service/customer-group.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { DialogCustomerGroupDetailComponent } from '../dialog-customer-group-detail/dialog-customer-group-detail.component';

@Component({
  selector: 'app-customer-group-list',
  templateUrl: './customer-group-list.component.html',
  styleUrls: ['./customer-group-list.component.scss']
})
export class CustomerGroupListComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'organizationName', 'isActive', 'actions'];
  filter: CustomerGroupFilterModel = new CustomerGroupFilterModel()
  listCustomerGroup : CustomerGroup[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private customerGroupService: CustomerGroupService, public dialog: MatDialog) { 
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
    this.customerGroupService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listCustomerGroup = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(customerGroup: CustomerGroup) {
    if(!customerGroup) {
      customerGroup = new CustomerGroup();
      customerGroup.isActive = true;
    }
    const dialogRef = this.dialog.open(DialogCustomerGroupDetailComponent, {
      width: '60vw',
      data: Object.assign({}, customerGroup)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }

  async delete(item: CustomerGroup) {
    if (!(await this.confirm('Xóa nhóm khách hàng?'))) return;

    this.customerGroupService.detele(item.id).then(
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
