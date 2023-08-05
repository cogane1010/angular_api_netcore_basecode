import { Injectable } from "@angular/core"
import { HttpClient, HttpBackend } from "@angular/common/http"
import { environment } from 'src/environments/environment'
import { AppGlobals } from './app-global'
import { TranslateService } from "./service/translate.service"
import { ApiEndpoint, Urls } from './model'

@Injectable()
export class AppConfig {

    static apiEndpoint: string
    static authority: string
    static client: string
    static notifyServer: string

    static urls = Urls
    
    private httpClient: HttpClient

    // https://stackoverflow.com/questions/46469349/how-to-make-an-angular-module-to-ignore-http-interceptor-added-in-a-core-module/49013534#49013534
    // ko inject httpClient vì sẽ trigger AuthInterceptor => ko load đc resource
    constructor(handler: HttpBackend, private trlService: TranslateService) {
        this.httpClient = new HttpClient(handler)
    }

    async load() {
        console.log('*** Setting app ***')

        this.trlService.use(AppGlobals.getLang())

        const endpointFile = `assets/config/endpoint.${environment.name}.json?v=${Math.random()}`

        try {
            let res = await this.httpClient.get(endpointFile).toPromise()
            AppConfig.apiEndpoint = (<ApiEndpoint>res).apiEndpoint
            AppConfig.authority = (<ApiEndpoint>res).authority
            AppConfig.client = (<ApiEndpoint>res).client
            AppConfig.notifyServer = (<ApiEndpoint>res).notifyServer
    
            
            //
        } catch (error) {
            console.error(error)
        }

        console.log('*** end Setting app ***')
    }
}

export function InitApp(appConfig: AppConfig) {
    return () => appConfig.load();
}

// https://stackoverflow.com/questions/54057030/how-to-change-itemsperpagelabel-in-mat-paginator-in-angular-6/54058741
import { MatPaginatorIntl, MatPaginator } from '@angular/material/paginator'

export function CustomPaginator() {
    const customPaginatorIntl = new MatPaginatorIntl()
    customPaginatorIntl.itemsPerPageLabel = ''
    customPaginatorIntl.firstPageLabel = ''
    customPaginatorIntl.lastPageLabel = ''
    customPaginatorIntl.nextPageLabel = ''
    customPaginatorIntl.previousPageLabel = ''
    return customPaginatorIntl
}
