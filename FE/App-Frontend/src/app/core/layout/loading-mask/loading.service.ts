import { Injectable } from '@angular/core'
import { BehaviorSubject, Observable } from 'rxjs'

@Injectable({
    providedIn: 'root'
})
export class LoadingService {

    private loading = new BehaviorSubject<boolean>(false)

    getLoading(): Observable<boolean> {
        return this.loading.asObservable()
    }

    setLoading(isShow: boolean) {
        this.loading.next(isShow)
    }
}
