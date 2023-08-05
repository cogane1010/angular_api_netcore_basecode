import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { PaymentType } from '../../model/model';
import { PaymentTypeFilterModel } from '../../model/paging';
import { PaymentTypeService } from '../../service/payment-type.service';
import { DialogPaymentTypeDetailComponent } from './dialog-payment-type-detail/dialog-payment-type-detail.component';

@Component({
  selector: 'app-payment-type',
  templateUrl: './payment-type.component.html',
  styleUrls: ['./payment-type.component.scss']
})
export class PaymentTypeComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'organizationName', 'actions'];
  filter: PaymentTypeFilterModel = new PaymentTypeFilterModel()
  listPaymentType : PaymentType[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private paymentTypeService: PaymentTypeService, public dialog: MatDialog) { 
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
    this.paymentTypeService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listPaymentType = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(paymentType: PaymentType) {
    if(!paymentType) {
      paymentType = new PaymentType();
      paymentType.isActive = true;
    }
    const dialogRef = this.dialog.open(DialogPaymentTypeDetailComponent, {
      width: '60vw',
      data: Object.assign({},paymentType)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }

  async delete(item: PaymentType) {
    if (!(await this.confirm('Xóa Payment type?'))) return;

    this.paymentTypeService.detele(item.id).then(
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
