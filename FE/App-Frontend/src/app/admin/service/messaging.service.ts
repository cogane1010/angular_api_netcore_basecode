import { Injectable } from '@angular/core';
import { AngularFireMessaging } from '@angular/fire/compat/messaging';
import { BehaviorSubject } from 'rxjs'

@Injectable()
export class FcmService  {
    currentMessage = new BehaviorSubject(null);
    constructor(private fireMessaging: AngularFireMessaging) {
        this.fireMessaging.messages.subscribe((message: any) => {
            console.log('Foreground message: ' + message)
          })
    }

    requestToken(): void {
        this.fireMessaging.requestToken.subscribe({
            next: (token) => {
              // Upload the user FCM token to your server.
              console.log("11111111111111");
              console.log(token);
            },
            error: err => {
              console.error('Fetching FCM token failed: ', +err)
            }
        })
        this.fireMessaging.getToken.subscribe({
          next: (token) => {
            // Upload the user FCM token to your server.
            console.log("2222222");
            console.log(token);
          },
          error: err => {
            console.error('Fetching FCM token failed: ', +err)
          }
      })
    }

    receiveMessage() {
        this.fireMessaging.messages.subscribe(
          (payload) => {
            console.log("new message received. ", payload);
            this.currentMessage.next(payload);
          })
      }
}