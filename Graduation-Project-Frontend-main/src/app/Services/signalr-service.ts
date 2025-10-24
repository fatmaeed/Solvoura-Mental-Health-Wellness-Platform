import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';



@Injectable({
  providedIn: 'root'
})
export class SignalRService {
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


  stop(): void {
    console.log('SignalR connection stopped');
    this.connection?.stop();
    this.connection = null;
  }

}
