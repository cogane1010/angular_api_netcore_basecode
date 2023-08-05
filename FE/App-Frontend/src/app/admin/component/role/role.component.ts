import { Component, OnInit, ViewChild } from '@angular/core'
import { MatPaginator, PageEvent } from '@angular/material/paginator'
import { MatDialog } from '@angular/material/dialog'
import { RoleFilterModel, Role } from '../../model'
import { RoleService } from '../../service'
import { BaseComponent } from '../../../common/base-component/base-component.component'
import { DialogRoleDetailComponent } from './dialog-role-detail/dialog-role-detail.component'

@Component({
    selector: 'app-role',
    templateUrl: './role.component.html',
    styleUrls: ['./role.component.scss']
})
export class RoleComponent extends BaseComponent implements OnInit {

    displayedColumns: string[] = ['STT', 'name', 'description', 'status', 'action']
    listRole: Role[]
    filter: RoleFilterModel = new RoleFilterModel()

    @ViewChild(MatPaginator) paginator: MatPaginator

    constructor(private roleService: RoleService, public dialog: MatDialog) {
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
        this.roleService.getPaging(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res?.data)
                    return
                }
                this.listRole = res.data.data
                this.paginator.length = res.data.count
            }
        )
    }

    detailRole(role: Role) {
        const dialogRef = this.dialog.open(DialogRoleDetailComponent, {
            width: '60vw',
            data: role
        })

        dialogRef.afterClosed().subscribe(res => {
            if (res) this.getData()
        })
    }

    async deleteRole(item: Role) {
        if (!(await this.confirm('Xóa role?'))) return

        this.roleService.detele(item.id).then(
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

    pageIndexChange(event: PageEvent) {
        this.filter.pageIndex = event.pageIndex
        this.filter.pageSize = event.pageSize
        this.getData()
    }

}
