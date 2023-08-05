import { Component, OnInit } from '@angular/core'
import { MatDrawerMode } from '@angular/material/sidenav'
import { MediaObserver } from '@angular/flex-layout'

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

    //over: thò ra thụt vào
    //push: thò ra thụt vào
    //side: ở nguyên bên trái
    sideNavMode: MatDrawerMode = 'side'

    constructor(private media: MediaObserver) { }

    ngOnInit() {
        this.media.asObservable().subscribe(() => {
            this.toggleView();
        });
    }

    toggleView() {
        if (this.media.isActive('gt-md')) {
            this.sideNavMode = 'side'
            return
        }

        if (this.media.isActive('gt-xs')) {
            this.sideNavMode = 'over'
            return
        }

        if (this.media.isActive('lt-sm')) {
            this.sideNavMode = 'over'
            return
        }
    }

}
