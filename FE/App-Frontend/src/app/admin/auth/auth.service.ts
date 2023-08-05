import { Injectable } from '@angular/core'
import { Router } from '@angular/router'
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http'
import { AppConfig } from '../../common/app-config'
import { RespondData } from '../../common/model'
import { BaseService } from '../../common/service/base.service'
import { TokenStorage } from './token.storage'
import { MatDialog } from '@angular/material/dialog'
import { DialogErrorComponent } from 'src/app/common/dialog-error/dialog-error.component'
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { BehaviorSubject } from 'rxjs'


@Injectable({
    providedIn: 'root'
})
export class AuthService extends BaseService {

    private apiUrl = AppConfig.apiEndpoint + AppConfig.urls.auth.token
    private pingAuthUrl = AppConfig.apiEndpoint + AppConfig.urls.auth.pingAuth

    //identity server
    private manager = new UserManager(getClientSettings());
    private _authNavStatusSource = new BehaviorSubject<boolean>(false);
    authNavStatus$ = this._authNavStatusSource.asObservable();
    private user: User | null;


    constructor(
        public dialog: MatDialog,
        protected httpClient: HttpClient,
        private router: Router) {
        super(httpClient)
        this.manager.getUser().then(user => { 
            this.user = user;      
            this._authNavStatusSource.next(this.isAuthenticated());
          });
    }

    async signIn(username: string, password: string, returnUrl: string = '', noRedirect= false) {
        try {
            const body = new HttpParams()
                .set('username', username)
                .set('password', encodeURIComponent(password))
                .set('grant_type', 'password')

            const response = await this.sendSignIn(body)

            TokenStorage.saveToken(response)
            if(!noRedirect)
                 this.router.navigateByUrl(returnUrl)
        } catch (error) {
            console.log(error);
            this.error(error.error.error);
        }
    }
    async sendSignIn(body: HttpParams) {
        return this.httpClient.post(this.apiUrl, body, {
            headers: new HttpHeaders()
                .set('Content-Type', 'application/x-www-form-urlencoded')
        }).toPromise()
    }

    async signOut() {
        TokenStorage.clearToken()
        await this.manager.signoutRedirect();
        //this.router.navigate(['/login'])
    }

    async pingAuth() {
        await this.manager.getUser().then(user => { 
            this.user = user;   
        
          });
        //let res = await this.post(this.pingAuthUrl)
        //return res?.isSuccess
        return this.isAuthenticated();
    }

    error(msg: string) {
        this.dialog.open(DialogErrorComponent, {
            width: '30vw',
            data: msg
        })
    }

    // Identity server login
    loginIdS(returnUrl: string = '') { 
        return this.manager.signinRedirect({state: returnUrl});   
    }

    async completeAuthentication() {        
        this.user = await this.manager.signinRedirectCallback();
        TokenStorage.saveToken(this.user, 2)
        this._authNavStatusSource.next(this.isAuthenticated());    
        return this.user.state
    } 
    

    isAuthenticated(): boolean {
        return this.user != null && !this.user.expired;
    }

    async signoutIds() {
        await this.manager.signoutRedirectCallback();
    }

    get authorizationHeaderValue(): string {
        return `${this.user.token_type} ${this.user.access_token}`;
    }


}

export class ResetPasswordModel {
    password?: string
    retypePassword?: string
    newPassword?: string
}


export function getClientSettings(): UserManagerSettings {
    return {
        authority: AppConfig.authority,
        client_id: 'spaCodeClient',
        redirect_uri: AppConfig.client + '/#/callback',
        post_logout_redirect_uri: AppConfig.client,
        response_type:"code",
        scope:"openid profile api1",
        filterProtocolClaims: true,
        loadUserInfo: true,
        automaticSilentRenew: true,
        silent_redirect_uri: AppConfig.client+ '/silent-refresh.html'
    };
}
