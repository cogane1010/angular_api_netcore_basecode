import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Customer } from 'src/app/booking_online/model/model';
import { CustomerFilterModel } from 'src/app/booking_online/model/paging';
import { CustomerService } from 'src/app/booking_online/service/customer.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';


@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss']
})
export class CustomerComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'customerCode', 'fullName', 'dob', 'mobilePhone','email', 'golf_CardNo', 'status', 'actions'];
  filter: CustomerFilterModel = new CustomerFilterModel()
  listCustomer : Customer[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private customerService: CustomerService, public dialog: MatDialog) { 
     super(dialog)
   }

   ngOnInit() {
    this.search();
  }

  search() {
    this.filter.pageIndex = 0;
    this.filter.pageSize = 20;
    //this.paginator.length = 0;    
    this.getData();
  }

  getData() {
    this.customerService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listCustomer = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  async changeStatus(customer: Customer) {
    let message= "";
    if (customer.isActive) {
      message= "Bạn có muốn khóa tài khoản " + customer.customerCode +" - " +customer.fullName + "?";     
    }
    else {
      message= "Bạn có muốn mở khóa tài khoản " + customer.customerCode +" - " + customer.fullName + "?";
    }

    if (!(await this.confirm(message))) return;
    this.customerService.changeStatus(customer).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          this.alert(res?.message);
          return
        }
        customer.isActive = !customer.isActive
      }
    )   
  }
}
