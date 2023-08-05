import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { AuthService } from '../../admin/auth/auth.service'

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

    form: FormGroup
    returnUrl = ''

    constructor(private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private authService: AuthService) {
        this.form = this.formBuilder.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        })
    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => this.returnUrl = params['returnUrl'] || '/');
        this.loginIdS();
    }

    login() {
        this.doLogin()
    }

    doLogin() {
        const val = this.form.value
        if (val.username && val.password) {
            this.authService.signIn(val.username, val.password, this.returnUrl)
        }
    }

    loginIdS(){
        this.authService.loginIdS(this.returnUrl);
    }

}
