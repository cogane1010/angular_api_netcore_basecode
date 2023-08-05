import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { LockReason } from '../../model/model';
import { LockReasonFilterModel } from '../../model/paging';
import { LockReasonService } from '../../service/lock-reason.service';
import { DialogLockReasonDetailComponent } from './dialog-lock-reason-detail/dialog-lock-reason-detail.component';

@Component({
  selector: 'app-lock-reason',
  templateUrl: './lock-reason.component.html',
  styleUrls: ['./lock-reason.component.scss']
})
export class LockReasonComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'code', 'name', 'organizationName', 'actions'];
  filter: LockReasonFilterModel = new LockReasonFilterModel()
  listLockReason : LockReason[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator
  constructor( private lockReasonService: LockReasonService, public dialog: MatDialog) { 
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
    this.lockReasonService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listLockReason = res.data.data
        this.paginator.length = res.data.count
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }


  openDetail(lockReason: LockReason) {
    if(!lockReason) {
      lockReason = new LockReason();
      lockReason.isActive = true;
    }
    const dialogRef = this.dialog.open(DialogLockReasonDetailComponent, {
      width: '60vw',
      data: Object.assign({},lockReason)
    })

    dialogRef.afterClosed().subscribe(res => {
      if (!res) return;
      this.getData();
    })
  }

  async delete(item: LockReason) {
    if (!(await this.confirm('Xóa lock reason?'))) return;

    this.lockReasonService.detele(item.id).then(
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
