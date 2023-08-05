import { Component, OnInit } from '@angular/core'
import { Subject, BehaviorSubject, Subscription } from 'rxjs'
import { LoadingService } from './loading.service'

@Component({
    selector: 'app-loading-mask',
    templateUrl: './loading-mask.component.html',
    styleUrls: ['./loading-mask.component.scss']
})
export class LoadingMaskComponent implements OnInit {

    isLoading: boolean = false
    loadingSubscription: Subscription

    constructor(private loadingService: LoadingService) { }

    ngOnInit() {
        this.loadingSubscription = this.loadingService.getLoading().subscribe(newLoading => {
            setTimeout(() => {
                this.isLoading = newLoading
            })
        })
    }

    ngOnDestroy() {
        this.loadingSubscription.unsubscribe()
    }

}
