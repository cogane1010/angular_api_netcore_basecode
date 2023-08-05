import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core'
import { MatPaginator, PageEvent } from '@angular/material/paginator'
import { MatDialog } from '@angular/material/dialog'
import { MenuService } from '../../service/menu.service'
import { Menu } from '../../model'
import { MenuFilterModel } from '../../model'
import { DialogMenuDetailComponent } from './dialog-menu-detail/dialog-menu-detail.component'
import { BaseComponent } from '../../../common/base-component/base-component.component'
import { AppGlobals } from 'src/app/common/app-global'

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss']
})
export class MenuComponent extends BaseComponent implements OnInit, OnDestroy {

    displayedColumns: string[] = ['STT', 'name', 'url', 'icon', 'translate-key', 'displayOrder', 'parent', 'status', 'action']
    filter: MenuFilterModel = new MenuFilterModel()
    listMenu: Menu[] = []

    @ViewChild(MatPaginator) paginator: MatPaginator

    constructor(private menuService: MenuService, public dialog: MatDialog) {
        super(dialog)
    }

    ngOnInit() {
        this.filter = AppGlobals.getFilter(MenuFilterModel) || new MenuFilterModel()
        this.getData()
    }

    ngOnDestroy(): void {
        AppGlobals.setFilter(MenuFilterModel, this.filter)
    }

    search() {
        this.filter.pageIndex = 0
        this.paginator.length = 0
        this.getData()
    }

    getData() {
        this.menuService.getPaging(this.filter).then(
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

    pageIndexChange(event: PageEvent) {
        this.filter.pageIndex = event.pageIndex
        this.filter.pageSize = event.pageSize
        this.getData()
    }

    detailMenu(menu: Menu) {
        const dialogRef = this.dialog.open(DialogMenuDetailComponent, {
            width: '60vw',
            data: menu
        })

        dialogRef.afterClosed().subscribe(res => {
            if (res) this.getData()
        })
    }

    async deleteMenu(item: Menu) {
        if (!(await this.confirm('Xóa menu?'))) return

        this.menuService.detele(item.id).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res)
                    this.error(res?.message)
                }
                this.getData()
            },
            err => {
                console.error(err)
            }
        )
    }
}
