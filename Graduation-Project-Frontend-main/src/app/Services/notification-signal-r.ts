import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationSignalR {
public message$ = new BehaviorSubject<any>(null);

    public connection: signalR.HubConnection | null = null;
  
    initConnection(url: string , AfterConnected?: () => void) : void {
        if (this.connection) return; // Prevent duplicate connections
  
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(url)
        .build();
  
      this.connection
        .start()
        .then(() => {
          console.log('Connected to', url);
          AfterConnected?.();
          this.registerDefaultEvents();
        })
        .catch(err => console.error('Connection error:', err));
    }
  
    sendMessage(eventName: string, ...args : any[]): void {
      if (this.connection) {
        this.connection.invoke(eventName, ...args);
      }
    }
  
    onMessage(eventName: string, callback:  any): void {
      this.connection?.on(eventName, callback);
    }
  private registerDefaultEvents() {
  this.connection?.on('ReceiveNotification', (msg) => {
    this.message$.next(msg);
    console.log(msg , "msg");
  });
}
  
    stop(): void {
      console.log('SignalR connection stopped');
      this.connection?.stop();
      this.connection = null;
    }
  
  
  
}
