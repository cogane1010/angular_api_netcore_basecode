import { NgModule } from '@angular/core'
import { MatSidenavModule } from '@angular/material/sidenav'
import { MatToolbarModule } from '@angular/material/toolbar'
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar'
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar'
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar'
import { MatListModule } from '@angular/material/list'
import { MatChipsModule } from '@angular/material/chips'
import { MatMenuModule } from '@angular/material/menu'

import { ShareModule } from '../share/share.module'
import { LayoutComponent } from './layout/layout.component'
import { SidemenuComponent } from './layout/sidemenu/sidemenu.component'
import { SidemenuItemComponent } from './layout/sidemenu-item/sidemenu-item.component'
import { ToolbarComponent } from './layout/toolbar/toolbar.component'
import { UserMenuComponent } from './layout/user-menu/user-menu.component'
import { DialogChangePasswordComponent } from './layout/user-menu/dialog-change-password/dialog-change-password.component'


const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
    suppressScrollX: true,
    wheelSpeed: 0.2
}

@NgModule({
    imports: [
        ShareModule,
        MatSidenavModule,
        MatToolbarModule,
        PerfectScrollbarModule,
        MatListModule,
        MatChipsModule,
        MatMenuModule,
    ],
    declarations: [
        LayoutComponent,
        SidemenuComponent,
        SidemenuItemComponent,
        ToolbarComponent,
        UserMenuComponent,
        DialogChangePasswordComponent
    ],
    exports: [
        LayoutComponent,
    ],
    providers: [
        {
            provide: PERFECT_SCROLLBAR_CONFIG,
            useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
        }
    ]
})
export class CoreModule { }
