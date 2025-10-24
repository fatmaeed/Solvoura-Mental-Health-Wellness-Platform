import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TokenHandlerService } from '../../../Services/token-handler-service';
import { ServiceProviderService } from '../../../Services/ServiceProviderService/serviceProviderService';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-incoming-session',
  imports: [RouterLink ],
  templateUrl: './incoming-session.html',
  styleUrl: './incoming-session.css'
})
export class IncomingSession implements OnInit {
 providerId!:number|null;
 incomingSessions!:IMeetingSession[];

constructor(private token:TokenHandlerService,private service:ServiceProviderService,private cdr:ChangeDetectorRef ) {}
  ngOnInit(): void {
   this.providerId = this.token.UserId;
   if(!this.providerId) return
   console.log(this.providerId)
   this.loadIncomingSessions(this.providerId);

  }

  loadIncomingSessions(id:number){
this.service.getIncomingSession(id).subscribe({
  next:(data)=>{
    this.incomingSessions = data.sort((a, b) =>new Date(a.startDateTime).getTime() - new Date(b.startDateTime).getTime() );
    this.cdr.detectChanges();
  },
  error:(err)=>{
    console.log("Error In Fetching sessions",err);
  }
})}
formatDateOnly(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-US', {
    weekday: 'long',
    month: 'long',
    day: 'numeric',
    year: 'numeric'
  });
}

formatTimeOnly(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleTimeString('en-US', {
    hour: '2-digit',
    minute: '2-digit',
    hour12: true
  });
}


isSessionJoinable(session:IMeetingSession): boolean {
const now = new Date();
  const sessionTime = new Date(session.startDateTime);
  const diff = (sessionTime.getTime() - now.getTime()) / (1000 * 60);
  const nowPlus15Min = new Date(Date.now() + 15 * 60 * 1000);
  return diff <= 15 && !(new Date(session.endDateTime) < nowPlus15Min);
  //return true
}
isSessionMissed(session: IMeetingSession): boolean {
  const now = new Date();
  const sessionEnd = new Date(session.endDateTime);
  return now > sessionEnd;
}



 formatDuration(duration: string): string {
  const [hours, minutes, seconds] = duration.split(':').map(Number);
  const hourStr = hours > 0 ? `${hours} hour${hours > 1 ? 's' : ''}` : '';
  const minStr = minutes > 0 ? `${minutes} minute${minutes > 1 ? 's' : ''}` : '';
  return [hourStr, minStr].filter(Boolean).join(' ');
}

}
