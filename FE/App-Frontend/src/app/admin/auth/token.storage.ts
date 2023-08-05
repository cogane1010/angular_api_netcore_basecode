import jwt_decode from 'jwt-decode'
import { AppGlobals } from '../../common/app-global'
import { Menu } from 'src/app/admin/model'

export class TokenStorage {

    public static readonly JWT_TOKEN = 'JWT_TOKEN'
    public static readonly USER_NAME = 'username'
    public static readonly FULL_NAME = 'fullname'
    public static readonly USER_MENU = 'usermenu'
    public static readonly ORG_ID = 'orgid'

    public static saveToken(token: any, type = 1) {
        if (type == 1) {
            localStorage.setItem(this.JWT_TOKEN, token.access_token)

            let userInfo: any = jwt_decode(token.access_token)
    
            //console.log('userInfo', userInfo)
    
            localStorage.setItem(this.USER_NAME, userInfo[this.USER_NAME])
            localStorage.setItem(this.FULL_NAME, userInfo[this.FULL_NAME])
            localStorage.setItem(this.ORG_ID, userInfo[this.ORG_ID])
        }
        else if (type == 2) {
            localStorage.setItem(this.JWT_TOKEN, token.access_token)

            let userInfo: any = jwt_decode(token.access_token)
    
            //console.log('userInfo', userInfo, token)
    
            localStorage.setItem(this.USER_NAME, userInfo['given_name'])
            localStorage.setItem(this.FULL_NAME, token.profile.name)
            localStorage.setItem(this.ORG_ID, token.profile.org)
        }
       
    }

    public static getUsername() {
        return localStorage.getItem(this.USER_NAME)
    }

    public static getUserFullname() {
        return localStorage.getItem(this.FULL_NAME)
    }

    public static getOrgId() {
        return localStorage.getItem(this.ORG_ID)
    }

    public static getToken() {
        return localStorage.getItem(this.JWT_TOKEN)
    }

    public static isLoggedIn() {
        return localStorage.getItem(this.JWT_TOKEN) != null
    }

    public static setUserMenu(data: Menu[]) {
        if(!data) {
           data = []; 
        }  
        localStorage.setItem(this.USER_MENU, JSON.stringify(data))
        let menus: Menu[] = JSON.parse(localStorage.getItem(this.USER_MENU))
         
        
    }

    public static checkUserMenu(menu: string): boolean {
        if (!menu || menu === '/') return true

        let menus: Menu[] = JSON.parse(localStorage.getItem(this.USER_MENU))
        if (!menus || !menus.length) return true

        for (let i = 0; i < menus.length; i++) {
            let menuLevel1 = menus[i]
            if (menuLevel1.url === menu
                || (menuLevel1.url && menu.indexOf(menuLevel1.url) >= 0)) return true

            for (let j = 0; j < menuLevel1.sub?.length; j++) {
                let menuLevel2 = menuLevel1.sub[j]
                if (menuLevel2.url === menu
                    || (menuLevel2.url && menu.indexOf(menuLevel2.url) >= 0)) return true
    
                for (let k = 0; k < menuLevel2.sub?.length; k++) {
                    let menuLevel3 = menuLevel2.sub[k]
                    if (menuLevel3.url === menu
                        || (menuLevel3.url && menu.indexOf(menuLevel3.url) >= 0)) return true
                }
            }
        }
        return false
    }

    public static clearToken() {
        const lang = AppGlobals.getLang()
        localStorage.clear()
        AppGlobals.setLang(lang)
    }
}

