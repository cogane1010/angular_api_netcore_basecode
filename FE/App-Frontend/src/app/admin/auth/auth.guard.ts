import { Injectable } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, NavigationEnd, CanDeactivate } from '@angular/router'
import { BaseComponent } from 'src/app/common/base-component/base-component.component'
import { AuthService } from './auth.service'
import { TokenStorage } from './token.storage'
import { AppConfig } from '../../common/app-config'
import { Observable } from 'rxjs'

@Injectable({
    providedIn: 'root'
})
export class AuthGuard extends BaseComponent implements CanActivate {

    constructor(
        private router: Router, 
        private authService: AuthService,
        public dialog: MatDialog
    ) { 
        super(dialog)
        this.router.events.subscribe((val) => {
            if (val instanceof NavigationEnd) {
                if (!TokenStorage.checkUserMenu(val.url) && !val.url.startsWith('/login')) {
                    this.error(`Không có quyền truy cập: ${val.url}`)
                    this.router.navigate(['/'])
                }
            }
        })
    }

    async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (await this.authService.pingAuth()) return true
        
        //window.location.href = AppConfig.authority +  "/account/login"
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } })
        return false
    }
}


export interface ComponentCanDeactivate {
  canDeactivate: () => boolean | Observable<boolean>;
}

@Injectable()
export class PendingChangesGuard implements CanDeactivate<ComponentCanDeactivate> {
    canDeactivate(component: ComponentCanDeactivate): boolean | Observable<boolean> {
        // if there are no pending changes, just allow deactivation; else confirm first
        return component.canDeactivate() ?
        true :
        confirm('WARNING: You have unsaved changes. Press Cancel to go back and save these changes, or OK to lose these changes.');
    }
}