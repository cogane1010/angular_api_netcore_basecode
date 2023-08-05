import { Component, OnInit, Inject } from '@angular/core'
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog'
import { FormControl } from '@angular/forms'
import { Observable } from 'rxjs'
import { map, startWith } from 'rxjs/operators'
import { Menu } from '../../../model'
import { MenuService } from '../../../service'
import { BaseComponent } from 'src/app/common/base-component/base-component.component'

@Component({
    selector: 'app-dialog-menu-detail',
    templateUrl: './dialog-menu-detail.component.html',
    styleUrls: ['./dialog-menu-detail.component.scss']
})
export class DialogMenuDetailComponent extends BaseComponent implements OnInit {

    menu: Menu

    allMenu: Menu[]

    parentSelectControl = new FormControl()
    filteredOptions: Observable<Menu[]>

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogMenuDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any, private menuService: MenuService) 
    {
        super(dialog)
        this.dialogRef.disableClose = true
        this.dialogRef.keydownEvents().subscribe(event => {
            if (event.key === "Escape") 
                this.dialogRef.close()
        })

        if (data)
            this.menu = Object.assign({}, data)
        else {
            this.menu = new Menu()
            this.menu.isActive = true
        }
    }

    ngOnInit() {
        this.menuService.getAll().then(
            res => {
                this.allMenu = res.data
                let parentMenu = this.allMenu.filter(x => x.id == this.menu.parentId)[0]
                if (parentMenu) this.parentSelectControl.setValue(parentMenu)

                this.filteredOptions = this.parentSelectControl.valueChanges.pipe(
                    startWith(''),
                    map(value => this._filter(value))
                )
            },
            err => {
                this.error(err)
            }
        )
    }

    private _filter(value: string): Menu[] {
        const filterValue = value.toString().toLowerCase();
        return this.allMenu?.filter(menu => menu.name.toLowerCase().indexOf(filterValue) >= 0);
    }

    display(menu?: Menu): string {
        return menu?.name || ''
    }

    onSelectedParent(event) {

    }

    addOrEdit(menu: Menu) {
        if (this.parentSelectControl.value) {
            menu.parentId = this.parentSelectControl.value.id
        }

        this.menuService.addOrEdit(menu).then(
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
