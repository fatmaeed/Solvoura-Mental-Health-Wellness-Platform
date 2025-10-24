import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Notification } from "./Components/notification/notification";
import { environment } from '../environments/environment';
import { TokenHandlerService } from './Services/token-handler-service';
import { NotificationSignalR } from './Services/notification-signal-r';
import { INotificationModel } from './Models/Notification/INotificationModel';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Notification],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit{
    private audio: HTMLAudioElement;

  constructor(private notificationSignalR:NotificationSignalR
    ,
     private tokenHandlerService:TokenHandlerService,
    private changeDetectorRef: ChangeDetectorRef
  ) {
     this.audio = new Audio();
    this.audio.src = 'assets/sounds/notification3.mp3'; // You need to add this file
    this.audio.load();  
  }
  ngOnInit(): void {
    if(this.tokenHandlerService.UserId ){
   this.notificationSignalR.initConnection(`${environment.apiBaseUrl}hubs/system` , () => {
    this.notificationSignalR.sendMessage('RegisterUser', this.tokenHandlerService.UserId);
    });}
    this.notificationSignalR.message$.subscribe(msg => {
      this.notification = msg;
      this.playNotificationSound();
      this.show();
    })

  }
  notification!:INotificationModel;
  isVisible = false;
  protected title = 'SOLVOURA';


    show() {
    this.isVisible = true;
    setTimeout(() => {
      this.isVisible = false;
       this.changeDetectorRef.detectChanges();
    }, 4000);
    
    this.changeDetectorRef.detectChanges();

  }
  playNotificationSound() {
    // Only play sound for unread notifications
    if (!this.notification?.read) {
      this.audio.play().catch(e => {
        console.warn('Could not play notification sound:', e);
        // Fallback to browser's built-in sound
        const ctx = new AudioContext();
        const oscillator = ctx.createOscillator();
        const gainNode = ctx.createGain();
        
        oscillator.connect(gainNode);
        gainNode.connect(ctx.destination);
        
        oscillator.type = 'sine';
        oscillator.frequency.value = 880;
        gainNode.gain.value = 0.1;
        
        oscillator.start();
        setTimeout(() => {
          oscillator.stop();
        }, 200);
      });
    }
  }
}
