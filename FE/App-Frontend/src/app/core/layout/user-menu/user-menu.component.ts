import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { TranslateService } from 'src/app/common/service/translate.service'
import { TokenStorage } from 'src/app/admin/auth/token.storage'
import { AuthService } from 'src/app/admin/auth/auth.service'
import { MatDialog } from '@angular/material/dialog'
import { DialogChangePasswordComponent } from './dialog-change-password/dialog-change-password.component'

@Component({
    selector: 'app-user-menu',
    templateUrl: './user-menu.component.html',
    styleUrls: ['./user-menu.component.scss']
})
export class UserMenuComponent implements OnInit {

    username: string = ''
    isOpen: boolean = false

    constructor(private router: Router,
        private trlService: TranslateService,
        private authService: AuthService,
        public dialog: MatDialog) { }

    ngOnInit() {
        this.username = TokenStorage.getUsername()
    }

    changelang(lang: string): void {
        this.isOpen = false
        this.trlService.use(lang)
    }

    logout() {
        this.authService.signOut()
    }

    onCloseMenu() {
        this.isOpen = false
    }
    
    changePassword() {
        const dialogRef = this.dialog.open(DialogChangePasswordComponent, {
            width: '35vw'
        })
    }
}
