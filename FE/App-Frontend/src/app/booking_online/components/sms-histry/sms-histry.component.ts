import { Component, OnInit, ViewChild } from '@angular/core';
import { SmsFilterModel } from '../../model/paging';
import { SmsHistory, Organization } from '../../model/model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { SmsHistoryService } from '../../service/smsHistory.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { asEnumerable } from 'linq-es2015';

@Component({
  selector: 'app-sms-histry',
  templateUrl: './sms-histry.component.html',
  styleUrls: ['./sms-histry.component.scss']
})
export class SmsHistryComponent extends BaseComponent implements OnInit {

  displayedColumns = ['STT', 'time', 'mobiphone', 'action'];
  filter: SmsFilterModel = new SmsFilterModel()
  listSms : SmsHistory[] = []
  @ViewChild(MatPaginator) paginator: MatPaginator;
  c_Org_Id: string;
  constructor( private smsHistoryService: SmsHistoryService, public dialog: MatDialog) { 
     super(dialog)
   }

   async ngOnInit() {
    this.getData();
  }

  search() {
    this.filter.pageIndex = 0;
    this.paginator.length = 0;
    this.getData()
  }

  getData() {
    this.smsHistoryService.getPaging(this.filter).then(
      res => {
        if (!res?.isSuccess) {
          console.error(res?.data)
          return
        }
        this.listSms = res.data.data;
        this.paginator.length = res.data.count;
      }
    )
  }

  pageIndexChange(event: PageEvent) {
    this.filter.pageIndex = event.pageIndex
    this.filter.pageSize = event.pageSize
    this.getData()
  }

  
}
