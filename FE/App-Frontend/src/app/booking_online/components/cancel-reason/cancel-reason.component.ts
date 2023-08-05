import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { CancelReason } from '../../model/model';
import { CancelReasonFilterModel } from '../../model/paging';
import { CancelReasonService } from '../../service/cancel-reason.service';
import { DialogCancelReasonDetailComponent } from './dialog-cancel-reason-detail/dialog-cancel-reason-detail.component';

@Component({
  selector: 'app-Cancel-reason',
  templateUrl: './Cancel-reason.component.html',
  styleUrls: ['./Cancel-reason.component.scss']
})
export class CancelReasonComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'organizationName', 'actions'];
  filter: CancelReasonFilterModel = new CancelReasonFilterModel()
  listCancelReason : CancelReason[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private CancelReasonService: CancelReasonService, public dialog: MatDialog) { 
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
    this.CancelReasonService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listCancelReason = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(cancelReason: CancelReason) {
    if(!cancelReason) {
      cancelReason = new CancelReason();
      cancelReason.isActive = true;
    }
    const dialogRef = this.dialog.open(DialogCancelReasonDetailComponent, {
      width: '60vw',
      data: Object.assign({},cancelReason)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }

  async delete(item: CancelReason) {
    if (!(await this.confirm('Xóa Cancel reason?'))) return;

    this.CancelReasonService.detele(item.id).then(
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
