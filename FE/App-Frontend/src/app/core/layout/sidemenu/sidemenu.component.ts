import { Component, OnInit } from '@angular/core'
import { menus } from './menu-element';
import { UserService } from 'src/app/admin/service'
import { Menu } from 'src/app/admin/model'
import { TokenStorage } from 'src/app/admin/auth/token.storage'

@Component({
    selector: 'app-sidemenu',
    templateUrl: './sidemenu.component.html',
    styleUrls: ['./sidemenu.component.scss']
})
export class SidemenuComponent implements OnInit {

    // menus = menus;
    menus: Menu[]

    constructor(private userService: UserService) { }

    ngOnInit() {
        this.userService.getUserMenu().then(
            res => {
                console.log('user menu', res)
                this.menus = res?.data || []
                TokenStorage.setUserMenu(this.menus)
            }
        )
        TokenStorage.setUserMenu(this.menus)
    }

}
