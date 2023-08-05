import { Pipe, PipeTransform } from '@angular/core';
import { AppGlobals } from '../app-global';
import { TranslateService } from '../service/translate.service'

@Pipe({
    name: 'translate',
    pure: false
})
export class TranslatePipe implements PipeTransform {

    constructor(private translate: TranslateService) { }

    transform(key: string, obj: any = null): string {
        var transTemplate = this.translate.get(key) || key || "";
        if (obj) {
            return AppGlobals.generateTemplateString(transTemplate)(obj)
        }
        else {
            return transTemplate
        }
       
    }
}
