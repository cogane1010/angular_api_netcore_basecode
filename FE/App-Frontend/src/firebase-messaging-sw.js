importScripts('https://www.gstatic.com/firebasejs/9.0.1/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/9.0.1/firebase-messaging-compat.js');

firebase.initializeApp({
  apiKey: "AIzaSyCIHW64TV8XRGm7hF2FLwMx7QDly_vM_J8",
  authDomain: "bookingonline-a3bdf.firebaseapp.com",
  projectId: "bookingonline-a3bdf",
  storageBucket: "bookingonline-a3bdf.appspot.com",
  messagingSenderId: "607146859734",
  appId: "1:607146859734:web:e99b168750ed8e53f93d77",
  measurementId: "G-P463G1HEFG"
});

//firebase.initializeApp(firebaseConfig);

// Retrieve firebase messaging
const messaging = firebase.messaging();

messaging.onBackgroundMessage(function(payload) {
  console.log('Received background message ', payload);

  const notificationTitle = payload.notification.title;
  const notificationOptions = {
    body: payload.notification.body,
  };

  self.registration.showNotification(notificationTitle,
    notificationOptions);
});

// const isSupported = firebase.messaging.isSupported();
// if (isSupported) {
//     const messaging = firebase.messaging();
//     messaging.onBackgroundMessage(({ notification: { title, body, image } }) => {
//         self.registration.showNotification(title, { body, icon: image || '/assets/icons/icon-72x72.png' });
//     });
// }


// const messaging = getMessaging(initializeApp({
//   apiKey: "AIzaSyCIHW64TV8XRGm7hF2FLwMx7QDly_vM_J8",
//   authDomain: "bookingonline-a3bdf.firebaseapp.com",
//   projectId: "bookingonline-a3bdf",
//   storageBucket: "bookingonline-a3bdf.appspot.com",
//   messagingSenderId: "607146859734",
//   appId: "1:607146859734:web:e99b168750ed8e53f93d77",
//   measurementId: "G-P463G1HEFG"
// }));

// const broadcast = new BroadcastChannel('myAppChannel');

// onBackgroundMessage(messaging, (payload) => {
//     broadcast.postMessage(payload);
//   })

//   this.broadcast.onmessage = (event) => {
//     // event.data - do whatever you want
//   }