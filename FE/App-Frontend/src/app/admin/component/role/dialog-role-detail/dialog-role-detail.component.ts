import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog'
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree'
import { Role, Menu } from '../../../model'
import { MenuService, RoleService } from '../../../service'

@Component({
    selector: 'app-dialog-role-detail',
    templateUrl: './dialog-role-detail.component.html',
    styleUrls: ['./dialog-role-detail.component.scss']
})
export class DialogRoleDetailComponent implements OnInit {

    role: Role = new Role()

    treeControl = new NestedTreeControl<Menu>(menu => menu.sub)
    dataSource = new MatTreeNestedDataSource<Menu>()
    hasChild = (_: number, node: Menu) => !!node.sub && node.sub.length > 0

    constructor(
        public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogRoleDetailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: Role,
        private roleService: RoleService
    ) {
        this.dialogRef.disableClose = true
        this.dialogRef.keydownEvents().subscribe(event => {
            if (event.key === "Escape") 
                this.dialogRef.close()
        })
        
        if (data) {
            this.roleService.get(data.id).then(
                res => {
                    this.role = res.data
                    this.dataSource.data = this.role.treeMenuPermission
                    this.updatePermissionTree()
                }
            )
        }
        else {
            this.role = new Role()
            this.role.isActive = true
            this.role.protected= false;
            this.roleService.getTreeMenuPermission().then(
                res => {
                    this.role.treeMenuPermission = res.data
                    this.dataSource.data = this.role.treeMenuPermission
                }
            )
        }
    }

    ngOnInit() {

    }

    addOrEdit(item: Role) {
        this.role.normalizedName = this.role.name.toUpperCase()
        this.roleService.addOrEdit(item).then(
            res => {
                if (!res?.isSuccess) {
                    alert('error')
                    console.log(res)
                    return
                }
                this.dialogRef.close(true)
            },
            err => {
                console.log(err)
            }
        )
    }

    changeNode(checked: boolean, node: Menu) {
        node.hasMenu = checked
        if (node.sub?.length) {
            node.sub.forEach(x => {
                x.hasMenu = checked

                if (x.sub?.length) {
                    x.sub.forEach(y => {
                        y.hasMenu = checked

                        if (y.sub?.length) {
                            y.sub.forEach(z => {
                                z.hasMenu = checked

                                if (z.sub?.length) {
                                    z.sub.forEach(a => a.hasMenu = true)
                                }
                            })
                        }
                    })
                }
            })
        }
        this.updatePermissionTree()
    }

    updatePermissionTree() {
        this.role.treeMenuPermission.forEach(rootMenu => {
            if (rootMenu.sub?.length) {
                rootMenu.allSubHasMenu = false
                rootMenu.someSubHasMenu = false

                if (rootMenu.sub.every(sub => sub.hasMenu)) {
                    rootMenu.hasMenu = true
                    rootMenu.allSubHasMenu = true
                } else if (rootMenu.sub.some(sub => sub.hasMenu)) {
                    rootMenu.hasMenu = true
                    rootMenu.someSubHasMenu = true
                }

                rootMenu.sub.forEach(menuLevel1 => {
                    menuLevel1.allSubHasMenu = false
                    menuLevel1.someSubHasMenu = false

                    if (menuLevel1.sub?.length) {
                        if (menuLevel1.sub.every(sub => sub.hasMenu)) {
                            menuLevel1.hasMenu = true
                            menuLevel1.allSubHasMenu = true

                            rootMenu.hasMenu = true
                            if (!rootMenu.allSubHasMenu) 
                                rootMenu.someSubHasMenu = true
                        } else if (menuLevel1.sub.some(sub => sub.hasMenu)) {
                            menuLevel1.hasMenu = true
                            menuLevel1.someSubHasMenu = true

                            rootMenu.hasMenu = true
                            rootMenu.allSubHasMenu = false
                            rootMenu.someSubHasMenu = true
                        }

                        menuLevel1.sub.forEach(menuLevel2 => {
                            menuLevel2.allSubHasMenu = false
                            menuLevel2.someSubHasMenu = false

                            if (menuLevel2.sub?.length) {
                                if (menuLevel2.sub.every(sub => sub.hasMenu)) {
                                    menuLevel1.hasMenu = true
                                    menuLevel2.hasMenu = true
                                    menuLevel2.allSubHasMenu = true

                                    rootMenu.hasMenu = true
                                    if (!menuLevel1.allSubHasMenu) 
                                        menuLevel1.someSubHasMenu = true
                                    if (!rootMenu.allSubHasMenu) 
                                        rootMenu.someSubHasMenu = true
                                } else if (menuLevel2.sub.some(sub => sub.hasMenu)) {
                                    menuLevel2.hasMenu = true
                                    menuLevel2.someSubHasMenu = true

                                    menuLevel1.hasMenu = true
                                    menuLevel1.allSubHasMenu = false
                                    menuLevel1.someSubHasMenu = true

                                    rootMenu.hasMenu = true
                                    rootMenu.allSubHasMenu = false
                                    rootMenu.someSubHasMenu = true
                                }
                            }
                        })
                    }
                })
            }
        })
    }

}
