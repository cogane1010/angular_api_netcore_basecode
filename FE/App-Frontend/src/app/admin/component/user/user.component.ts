import { Component, OnInit, ViewChild } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { MatPaginator, PageEvent } from '@angular/material/paginator'
import { BookingLineService } from 'src/app/booking_online/service/booking.service'
import { BaseComponent } from '../../../common/base-component/base-component.component'
import { Organization, RoleFilterModel, User } from '../../model'
import { UserService } from '../../service'
import { DialogUserDetailComponent } from './dialog-user-detail/dialog-user-detail.component'

@Component({
    selector: 'app-user',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.scss']
})
export class UserComponent extends BaseComponent implements OnInit {

    displayedColumns: string[] = ['STT', 'userName', 'fullName', 'email', 'organizationName','roleName', 'status', 'action']
    listUser: User[]
    filter: RoleFilterModel = new RoleFilterModel()
    listOrganization : Organization[] = [];
    @ViewChild(MatPaginator) paginator: MatPaginator

    constructor(public dialog: MatDialog, private userService: UserService,private lineSv: BookingLineService) {
        super(dialog)
    }

    ngOnInit() {
        this.getData()
        this.getOrg()
    }

    search() {
        this.filter.pageIndex = 0
        this.paginator.length = 0
        this.getData()
    }

    getData() {
        this.userService.getPaging(this.filter).then(
            res => {
                if (!res?.isSuccess) {
                    console.error(res?.data)
                    return
                }
                this.listUser = res.data.data
                this.paginator.length = res.data.count
            }
        )
    }

    detailUser(user: User) {
        const dialogRef = this.dialog.open(DialogUserDetailComponent, {
            width: '60vw',
            minHeight: '50vh',
            data: user
        })

        dialogRef.afterClosed().subscribe(res => {
            if (res) this.getData()
        })
    }

    async deleteUser(item: User) {
        if (!(await this.confirm('Xóa user?'))) return

        this.userService.detele(item.id).then(
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

    getOrg() {
        this.lineSv.getOrgByUserId().then(
          res => {
            if (!res?.isSuccess) {
              console.error(res?.data)
              return
            }
            this.listOrganization = res.data;            
            this.getData();
          }
        )
      }

    pageIndexChange(event: PageEvent) {
        this.filter.pageIndex = event.pageIndex
        this.filter.pageSize = event.pageSize
        this.getData()
    }

}
