
export class AppGlobals {

    private static filterSet = {}

    public static getFilter(className: any) {
        return this.filterSet[className]
    }

    public static setFilter(className: any, filter: any) {
        this.filterSet[className] = filter
    }
    
    public static setLang(lang: string) {
        localStorage.setItem('LANGUAGE', lang);
    }

    public static getLang(): string {
        if (!localStorage.getItem('LANGUAGE')) {
            localStorage.setItem('LANGUAGE', 'vn');
        }
        return localStorage.getItem('LANGUAGE') || '';
    }
    
    public static arrayBufferToString(buffer, encoding, callback) {
        const blob = new Blob([buffer], { type: 'text/plain' });
        const reader = new FileReader();
        reader.onload = function (evt) {
            const target: any = evt.target;
            callback(target.result);
        };
        reader.readAsText(blob, encoding);
    }

    public static generateTemplateString = (function(){
        var cache = {};
    
        function generateTemplate(template){
            var fn = cache[template];
    
            if (!fn){    
                var sanitized = template
                    .replace(/\$\{([\s]*[^;\s\{]+[\s]*)\}/g, function(_, match){
                        return `\$\{map.${match.trim().replaceAll('.', '?.')}\}`;
                        })
                    .replace(/(\$\{(?!map\.)[^}]+\})/g, '');
                fn = Function('map', `return \`${sanitized}\`.replaceAll(/null/g, '').replaceAll(/undefined/g, '')`);
            }
    
            return fn;
        }
    
        return generateTemplate;
    })();
}