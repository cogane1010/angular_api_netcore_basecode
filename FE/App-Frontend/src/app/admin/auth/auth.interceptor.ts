import { Injectable } from '@angular/core'
import { Router, ActivatedRoute, Params } from '@angular/router'
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http/http'
import { Observable, of, throwError } from 'rxjs'
import { catchError, tap } from 'rxjs/operators'
import { LoadingService } from '../../core/layout/loading-mask/loading.service'
import { TokenStorage } from './token.storage'
import { BaseComponent } from 'src/app/common/base-component/base-component.component'
import { MatDialog } from '@angular/material/dialog'

// https://blog.angular-university.io/angular-jwt-authentication/
@Injectable()
export class AuthInterceptor extends BaseComponent implements HttpInterceptor {

    // for loading service
    private totalRequests = 0
    private returnUrl: string

    constructor(
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private loadingService: LoadingService,
        public dialog: MatDialog) {
        super(dialog)
        this.activatedRoute.queryParams.subscribe((params: Params) => {
            this.returnUrl = params['returnUrl']
        });
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        const token = TokenStorage.getToken()
        if (token) {
            const cloned = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${token}`)
            })
            
            const result = next.handle(cloned).pipe(
                tap(
                    event => {
                        // If request was sent
                        if ((event.type == 0)) {
                            this.totalRequests++
                            this.loadingService.setLoading(true)
                        }
                        else {
                            this.decreaseRequests()
                        }
                    },
                    (err: any) => {
                        this.decreaseRequests()
                        this.handleAuthError(err)
                    })
            )
            return result.pipe(catchError(err => this.handleAuthError(err)))
        }

        return next.handle(req).pipe(
            tap(
                event => {
                    // If request was sent
                    if ((event.type == 0)) {
                        this.totalRequests++
                        this.loadingService.setLoading(true)
                    }
                    else {
                        this.decreaseRequests()
                    }
                },
                (err: any) => {
                    this.decreaseRequests()
                    this.handleAuthError(err)
                }
            )
        )
    }

    private handleAuthError(err: HttpErrorResponse): Observable<any> {
        this.decreaseRequests()
        // handle your auth error or rethrow
        if (err.status === 401) {
            if (TokenStorage.isLoggedIn())
                this.alert('Hết phiên đăng nhập. Xin mời đăng nhập lại!')
            TokenStorage.clearToken()
            this.router.navigate(['/login'], { queryParams: { returnUrl: this.returnUrl || this.router.routerState.snapshot.url } })
            return of(err)
        }
        if (err.status === 403) {
            this.error('Tài khoản không có quyền truy cập!')
            this.router.navigateByUrl('')
            return of(err)
        }
        return throwError(err)
    }

    private decreaseRequests() {
        if (this.totalRequests > 0) {
            this.totalRequests--
        }
        if (this.totalRequests === 0) {
            this.loadingService.setLoading(false)
        }
    }
}
