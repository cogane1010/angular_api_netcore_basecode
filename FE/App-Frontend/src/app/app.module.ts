import { NgModule, APP_INITIALIZER } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MatPaginatorIntl, MAT_PAGINATOR_DEFAULT_OPTIONS } from '@angular/material/paginator'
import { HTTP_INTERCEPTORS } from '@angular/common/http'

import { AppComponent } from './app.component'
import { AppRoutingModule } from './app-routing.module'
import { CoreModule } from './core/core.module'
import { TranslateService } from './common/service/translate.service'
import { AppConfig, InitApp, CustomPaginator } from './common/app-config'
import { LoginComponent } from './common/login/login.component'
import { ShareModule } from './share/share.module'
import { AuthInterceptor } from './admin/auth/auth.interceptor'
import { AngularFireMessagingModule } from '@angular/fire/compat/messaging'
import { environment } from 'src/environments/environment'
import { AngularFireModule } from '@angular/fire/compat'
import { FcmService } from './admin/service/messaging.service'
import { AngularFireMessaging } from '@angular/fire/compat/messaging';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        CoreModule,
        ShareModule,
        AngularFireModule.initializeApp(environment.firebase),
        AngularFireMessagingModule
    ],
    providers: [
        AppConfig,
        TranslateService,
        {
            provide: APP_INITIALIZER,
            useFactory: InitApp,
            deps: [AppConfig],
            multi: true
        },
        {
            provide: MAT_PAGINATOR_DEFAULT_OPTIONS,
            useValue: {
                pageSizeOptions: [10, 20, 50, 100, 500, 10000],
                showFirstLastButtons: true,
            }
        },
        { provide: MatPaginatorIntl, useValue: CustomPaginator() },
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        FcmService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
