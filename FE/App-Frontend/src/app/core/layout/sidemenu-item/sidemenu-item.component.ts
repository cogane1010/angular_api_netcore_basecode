import { Component, OnInit, Input } from '@angular/core'
import { Menu } from 'src/app/admin/model'

@Component({
    selector: 'app-sidemenu-item',
    templateUrl: './sidemenu-item.component.html',
    styleUrls: ['./sidemenu-item.component.scss']
})
export class SidemenuItemComponent implements OnInit {

    @Input() menu: Menu

    constructor() { }

    ngOnInit() {
    }

}
