import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { LoginComponent } from './common/login/login.component'
import { AuthGuard } from './admin/auth/auth.guard'
import { LayoutComponent } from './core/layout/layout.component'

import { AuthCallbackComponent } from './common/auth-callback/auth-callback.component'

const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'callback', component: AuthCallbackComponent },
    {
        path: '', component: LayoutComponent, canActivate: [AuthGuard],
        children: [
            //{path: '', component: MainDashboardComponent},
            { path: 'admin', loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule) },
            { path: 'booking', loadChildren: () => import('./booking_online/booking_online.module').then(m => m.BookingOnlineModule) },
        ]
    },
    { path: '**', redirectTo: '' }
]

@NgModule({
    imports: [RouterModule.forRoot(routes, { useHash: true })],
    exports: [RouterModule]
})
export class AppRoutingModule { }
