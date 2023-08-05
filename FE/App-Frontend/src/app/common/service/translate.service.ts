import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { map, first } from 'rxjs/operators'
import { Observable } from 'rxjs'

import { LANGUAGUE_CONFIG } from '../model'
import { AppGlobals } from 'src/app/common/app-global'

@Injectable()
export class TranslateService {

    data: Map<string, string> = new Map<string, string>()

    constructor(private http: HttpClient) { }

    use(lang: string): Promise<{}> {
        console.log(`lang: ${lang}`)
        const ret = new Promise<{}>(
            (resolve, reject) => {
                AppGlobals.setLang(lang || 'vn')
                const that = this
                LANGUAGUE_CONFIG.forEach(function (item) {
                    const langPath = `assets/i18n/${item}.${lang}.json`
                    console.log(`lang file: ${langPath}`)
                    that.http.get(langPath).subscribe(
                        translation => {
                            const tmp: any = Object.assign({}, translation || {})
                            Object.keys(tmp).forEach(key => {
                                that.data.set(key, tmp[key])
                            })
                        },
                        error => {
                            console.error(error)
                        }
                    )
                })
                resolve(that.data)
            })
        return ret
    }

    get(key: string): string {
        return this.data.get(key) || ''
    }
}
