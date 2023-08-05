import { Component } from '@angular/core'
import { FcmService } from './admin/service/messaging.service'
import { AngularFireMessaging } from '@angular/fire/compat/messaging';
import { mergeMapTo } from 'rxjs/operators';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    title = 'App-Frontend'
    message
    constructor(private afMessaging: AngularFireMessaging) { }

    ngOnInit() {
        const userId = 'user001';
        // this.messagingService.requestToken()
        // this.messagingService.receiveMessage()
        // this.message = this.messagingService.currentMessage
        this.requestPermission()
      }

      requestPermission() {
        this.afMessaging.requestToken.subscribe({
            next: (token) => {
              // Upload the user FCM token to your server.
              console.log("11111111111111");
              console.log(token);
            },
            error: err => {
              console.error("Fetching FCM token failed: " +err.message )
            }
        })
      }
}
