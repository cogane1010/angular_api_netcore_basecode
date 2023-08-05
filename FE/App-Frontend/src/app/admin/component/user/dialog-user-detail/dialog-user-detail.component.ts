import { DatePipe } from '@angular/common';
import { Component, OnInit, Inject } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog'
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Organization } from 'src/app/booking_online/model/model';
import { OrganizationService } from 'src/app/booking_online/service/organization.service';
import { BaseComponent } from 'src/app/common/base-component/base-component.component';
import { Role, User } from '../../../model'
import { RoleService, UserService } from '../../../service'


@Component({
    selector: 'app-dialog-user-detail',
    templateUrl: './dialog-user-detail.component.html',
    styleUrls: ['./dialog-user-detail.component.scss']
})
export class DialogUserDetailComponent extends BaseComponent implements OnInit {

    user: User = new User();
    listOrganization : Organization[] = []
    organizationSelectControl = new FormControl()
    organizationFilteredOptions: Observable<Organization[]>
    
    constructor(
        public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogUserDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private userService: UserService,
        private roleService: RoleService,
        private organizationSv: OrganizationService,
        private datePipe: DatePipe
    ) {
        super(dialog);
        this.dialogRef.disableClose = true
        this.dialogRef.keydownEvents().subscribe(event => {
            if (event.key === "Escape")
                this.dialogRef.close()
        })
    }

    async ngOnInit() {
        this.listOrganization = (await this.organizationSv.getAll()).data || []
      
        if (this.data) {
            const res = await this.userService.get(this.data.id);
            if (res) {
                this.user = res.data;
                this.setValue();
                this.roleService.getAll().then(
                    res => {
                        this.user.roles = res.data
                        this.setValue();
                        this.roleService.getRoleUser(this.user.userId).then(
                            res => {
                                if(res.data && this.user.roles){
                                    this.user.roles.forEach(x => {
                                        res.data.forEach(y=> {
                                            if(x.id == y){
                                                x.hasRole = true
                                            }
                                        })    
                                      })                  
                                }  
                        } )
                    }            
                )                
            }
        }
        else {
            this.user = new User()
            this.user.isActive = true
            this.user.password = 'Brg@123456'
            this.user.confirmPassword = 'Brg@123456'    
            this.roleService.getAll().then(
                res => {
                    this.user.roles = res.data
                    this.setValue();                    
                }            
            )             
        }        
    }

    setValue (){
        this.organizationSelectControl.setValue(this.listOrganization.find(x => x.id == this.user.c_Org_Id));
        this.organizationFilteredOptions = this.organizationSelectControl.valueChanges.pipe(
          startWith(''),
          map(value => this._filterOrganization(value))
        )        
    }

    private _filterOrganization(value: string): Organization[] {
        const filterValue = value?.toString().toLowerCase();
        return this.listOrganization.filter(x => x.name?.toLowerCase().indexOf(filterValue) >= 0);
      }
      
      onSelectedOrganization(event) {
      }
      changeOrganizationText() {
        this.user.c_Org_Id = this.organizationSelectControl.value?.id || null
      }
      
      displayOrganization(org?: Organization): string {
        return org?.name || ''
      }

    addOrEdit(item: User) {
        item.confirmPassword = item.password;
        this.userService.addOrEditUser(item).then(
            res => {
                if (!res?.isSuccess) {
                    console.log(res)
                    this.error(res?.message)
                    return
                }
                this.dialogRef.close(true)
            },
            err => {
                console.log(err)
            }
        )
    }
}
