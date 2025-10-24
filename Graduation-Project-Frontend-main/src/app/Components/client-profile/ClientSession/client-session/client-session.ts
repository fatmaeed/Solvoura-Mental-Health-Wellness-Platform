import { CommonModule } from '@angular/common';
import { IClientRequest, SessionActionType } from './../../../../Models/Client/client-request';
import { ChangeDetectorRef, Component } from '@angular/core';
import { ClientService } from '../../../../Services/client-service';
import { TokenHandlerService } from '../../../../Services/token-handler-service';
import { IClientSession } from '../../../../Models/Client/iclient-session';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-client-session',
  imports: [FormsModule,CommonModule ,RouterLink],
  templateUrl: './client-session.html',
  styleUrl: './client-session.css'
})
export class ClientSession {
isSessionStartingSoon(_t9: IClientSession) {
throw new Error('Method not implemented.');
}
clientSessions: IClientSession[] = [];
  Client:any;
  apiUrl: string = `${environment.apiBaseUrl}`;
clientid:number|null =null;
  constructor(private service:ClientService ,private cdr :ChangeDetectorRef, private tokenHandler:TokenHandlerService, private router: Router) { }

  ngOnInit(): void {
   this.clientid = this.tokenHandler.UserId;
   if(!this.clientid)return;
    this.loadSessions();
    this.loadClient();
  }

 loadSessions() {
     if (!this.clientid) return
     this.service.getClientProfile(this.clientid).subscribe({
        next: (data) => {
          this.clientSessions = data;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Error fetching client profile:', err);
        }
      });
  }

  loadClient(){
      if (!this.clientid) return
      this.service.getClientById(this.clientid).subscribe({
      next: (data) => {
        this.Client = data;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching client profile:', err);
      }
    });
  }

  selectedsession!:IClientSession |null;
  openCancelModal(session :IClientSession){
     this.selectedsession = session;
     console.log(this.selectedsession)
  }

  reason!:string;
  SendCancelRequest(){
    if (!this.selectedsession) return;
    if(!this.clientid)return;
     const request: IClientRequest = {
      clientId:this.clientid,
    sessionId: this.selectedsession?.id,
    reason: this.reason  ,
    actionType :  Number(SessionActionType.Cancel)
  };
  console.log(request)
  this.service.handleSessionForClient(request).subscribe({
    next:()=>{
           this.loadSessions();
    },
    error:(err)=>{
          console.log(err);
    }
  })}

  SendPostponementRequest(){
if (!this.selectedsession) return;
   if(!this.clientid)return;
     const request: IClientRequest = {
      clientId:this.clientid,
    sessionId: this.selectedsession?.id,
    reason: this.reason  ,
    actionType :  Number(SessionActionType.Postpone)
  };
  this.service.handleSessionForClient(request).subscribe({
    next:()=>{
          // this.router.navigate(['/client-profile/client-session']);
          this.loadSessions();
    },
    error:(err)=>{
          console.log(err);
    }
  })
  }


formatDate(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleString('en-US', {
    weekday: 'short',
    month: 'short',
    day: 'numeric',
    hour: 'numeric',
    minute: '2-digit',
    hour12: true
  });
}


formatTime(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleTimeString('en-US', {
    hour: '2-digit',
    minute: '2-digit',
    hour12: true
  });}
  formatDuration(duration: string): string {
  const [hours, minutes, seconds] = duration.split(':').map(Number);
  const hourStr = hours > 0 ? `${hours} hour${hours > 1 ? 's' : ''}` : '';
  const minStr = minutes > 0 ? `${minutes} minute${minutes > 1 ? 's' : ''}` : '';
  return [hourStr, minStr].filter(Boolean).join(' ');
}

isBefore10Minutes(session: IClientSession): boolean {
  const now = new Date();
  const sessionTime = new Date(session.startDateTime);
  const diff = (sessionTime.getTime() - now.getTime()) / (1000 * 60); 
  return diff <= 15;
}

isSessionNotPast(session: IClientSession): boolean {
  const nowPlus15Min = new Date(Date.now() + 15 * 60 * 1000); 
  return new Date(session.endDateTime) < nowPlus15Min;
}
state:string="Finished"
Posponed:string="Posponed"

shouldShowButtons(status :any): boolean {
  return status !== 'Canceled' && status !== 'AcceptCancelation' && status !== 'Posponed';
}
}
