import { NgModule } from '@angular/core'
import { CommonModule, DatePipe } from '@angular/common'
import { RouterModule, Routes } from '@angular/router'

import { ShareModule } from '../share/share.module'
import { MenuComponent } from './component/menu/menu.component'
import { DialogMenuDetailComponent } from './component/menu/dialog-menu-detail/dialog-menu-detail.component'
import { RoleComponent } from './component/role/role.component'
import { DialogRoleDetailComponent } from './component/role/dialog-role-detail/dialog-role-detail.component'
import { UserComponent } from './component/user/user.component'
import { DialogUserDetailComponent } from './component/user/dialog-user-detail/dialog-user-detail.component'
import { SettingsComponent } from './component/settings/settings.component'
import { DialogSettingDetailComponent } from './component/settings/dialog-setting-detail/dialog-setting-detail.component'
import { TestAppComponent } from './component/test-app/test-app.component'

export const routes: Routes = [
    { path: 'menu', component: MenuComponent },
    { path: 'role', component: RoleComponent },
    { path: 'user', component: UserComponent },
    { path: 'setting', component: SettingsComponent },
    { path: 'test-app', component: TestAppComponent },
]

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        ShareModule,
    ],
    declarations: [
        MenuComponent,
        DialogMenuDetailComponent,
        RoleComponent,
        DialogRoleDetailComponent,
        UserComponent,
        DialogUserDetailComponent,
        SettingsComponent,
        DialogSettingDetailComponent,
        TestAppComponent
    ],
    providers:[DatePipe]
})
export class AdminModule { }
