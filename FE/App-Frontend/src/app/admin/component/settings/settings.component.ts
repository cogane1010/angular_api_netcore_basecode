import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { Setting, SettingFilterModel } from '../../model';
import { SettingService } from '../../service/setting.service';
import { DialogSettingDetailComponent } from './dialog-setting-detail/dialog-setting-detail.component';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.scss']
})
export class SettingsComponent extends BaseComponent implements OnInit {

    displayedColumns: string[] = ['STT', 'code', 'value', 'description', 'action']
    filter: SettingFilterModel = new SettingFilterModel()
    listMenu: Setting[] = []

    @ViewChild(MatPaginator) paginator: MatPaginator

    constructor(private settingService: SettingService, public dialog: MatDialog) {
        super(dialog);
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
        this.settingService.getPaging(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res?.data)
                    return
                }
                this.listMenu = res.data.data
                this.paginator.length = res.data.count
            }
        )
    }

    detailSetting(setting: Setting) {
        const dialogRef = this.dialog.open(DialogSettingDetailComponent, {
            width: '35vw',
            data: setting
        })

        dialogRef.afterClosed().subscribe(res => {
            if (res) this.getData()
        })
    }

    async deleteSetting(item: Setting) {
        if (!(await this.confirm('Xóa setting?'))) return

        this.settingService.detele(item.id).then(
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

    pageIndexChange(event: PageEvent) {
        this.filter.pageIndex = event.pageIndex
        this.filter.pageSize = event.pageSize
        this.getData()
    }

}
