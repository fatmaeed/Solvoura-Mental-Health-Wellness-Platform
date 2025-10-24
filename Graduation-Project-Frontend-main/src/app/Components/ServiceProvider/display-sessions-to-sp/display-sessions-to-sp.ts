import { IEditSessions, SessionType } from './../../../Models/ServiceProviderModels/icreate-sessions';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IDisplaySessionsToSP } from '../../../Models/ServiceProviderModels/idisplay-sessions-to-sp';
import { ServiceProviderService } from '../../../Services/service-provider-service';
import { CommonModule } from '@angular/common';
import { TokenHandlerService } from '../../../Services/token-handler-service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-display-sessions-to-sp',
  imports: [CommonModule,FormsModule],
  templateUrl: './display-sessions-to-sp.html',
  styleUrl: './display-sessions-to-sp.css'
})
export class DisplaySessionsToSp implements OnInit {
  constructor(private serviceProviderService:ServiceProviderService , private cdr:ChangeDetectorRef , private tokenHelper:TokenHandlerService) { }
   sessions: IDisplaySessionsToSP[] = [];
  //  selectedsession!:any ;
  selectedsession: any = {
  id: 0,
  startDateTime: new Date().toISOString(),
  duration: '00:00:00',
  type: 0,
  status: 0
};
  daysInView: any[] = [];
  currentWeekStart!: Date;
  currentWeekEnd!: Date;
  currentWeekDays: { date: Date }[] = [];
  timeSlots = Array.from({ length: 16 }, (_, i) => 8 + i); 
 serviceProviderId:number|null = null

ngOnInit() {
  this.serviceProviderId = this.tokenHelper.UserId;
    if (!this.serviceProviderId) return 
    // this.serviceProviderService.getSessionsToSP(this.serviceProviderId).subscribe({
    //   next: (data) => {
    //     this.sessions = data;
    //     this.cdr.detectChanges();
    //     console.log(this.sessions);
    //   },
    //   error: (error) => {
    //     console.log(this.sessions);
    //     console.error('Error fetching sessions:', error);
    //   }
    // });
    this.loadsessions(this.serviceProviderId) ;
    this.setCurrentWeek(new Date());
    this.cdr.detectChanges()

}
loadsessions(id:number){
   this.serviceProviderService.getSessionsToSP(id).subscribe({
      next: (data) => {
        this.sessions = data;
        this.cdr.detectChanges();
        console.log(this.sessions);
      },
      error: (error) => {
        console.log(this.sessions);
        console.error('Error fetching sessions:', error);
      }
    });
}

setCurrentWeek(date: Date) {
    const dayOfWeek = date.getDay();
    this.currentWeekStart = new Date(date);
    this.currentWeekStart.setDate(date.getDate() - dayOfWeek);
    
    this.currentWeekEnd = new Date(this.currentWeekStart);
    this.currentWeekEnd.setDate(this.currentWeekStart.getDate() + 6);
    
    this.generateWeekDays();
  }

  generateWeekDays() {
    this.currentWeekDays = Array.from({ length: 7 }, (_, i) => {
      const date = new Date(this.currentWeekStart);
      date.setDate(this.currentWeekStart.getDate() + i);
      return { date };
    });
  }

  previousWeek() {
    const prevWeek = new Date(this.currentWeekStart);
    prevWeek.setDate(prevWeek.getDate() - 7);
    this.setCurrentWeek(prevWeek);
  }

  nextWeek() {
    const nextWeek = new Date(this.currentWeekStart);
    nextWeek.setDate(nextWeek.getDate() + 7);
    this.setCurrentWeek(nextWeek);
  }

  isToday(date: Date): boolean {
    return date.toDateString() === new Date().toDateString();
  }

  getSessionsForDay(dayDate: Date) {
    return this.sessions.filter(session => {
      const sessionDate = new Date(session.startDateTime);
      return sessionDate.toDateString() === dayDate.toDateString();
    });
  }

calculateSessionTop(session: any): string {
  const sessionDate = new Date(session.startDateTime);
  const hours = sessionDate.getHours();
  const minutes = sessionDate.getMinutes();
  return `${(hours * 60) + minutes}px`;
}

calculateSessionHeight(session: any): string {
  const start = new Date(session.startDateTime);
  const end = new Date(session.endDateTime);
  const durationMinutes = (end.getTime() - start.getTime()) / (1000 * 60);
  return `${durationMinutes}px`;
}
  hasTimeConflict(session: any, dayDate: Date): boolean {
    const currentSessions = this.getSessionsForDay(dayDate);
    const sessionStart = new Date(session.startDateTime).getTime();
    const sessionEnd = new Date(session.endDateTime).getTime();
    
    return currentSessions.some(s => {
      if (s.id === session.id) return false;
      const sStart = new Date(s.startDateTime).getTime();
      const sEnd = new Date(s.endDateTime).getTime();
      return (sessionStart < sEnd) && (sessionEnd > sStart);
    });
  }

  parseDuration(duration: string): number {
     if (!duration) return 1; 
 
  const parts = duration.split(':');
  
  if (parts.length === 3) {

    const hours = parseInt(parts[0], 10);
    const minutes = parseInt(parts[1], 10);
    const seconds = parseInt(parts[2], 10);
    
    return hours + (minutes / 60) + (seconds / 3600);
  }
  
  return 1;
  }
loadSession(id:number){
  console.log(id)
  this.serviceProviderService.getSessionById(id).subscribe({
    next:(data)=>{
      console.log(data)
        this.selectedsession = data || {
        id: 0,
        startDateTime: new Date().toISOString(),
        duration: '00:00:00',
        type: 0,
        status: 0
      };
      this.cdr.detectChanges();
      console.log(data)
    },
    error:(err)=>{
      console.log(err);
    }
  })
}
sessionid!:number;
getSession(id:number){
  this.sessionid = id;
  console.log(this.sessionid)
}
 DeleteSession(){  
  console.log(this.sessionid)
this.serviceProviderService.deleteSession(this.sessionid).subscribe({
  next:()=>{
    if(!this.serviceProviderId)return
    this.loadsessions(this.serviceProviderId);
   
  },
  error:(err)=>{
    console.log(err)
  }
})}
 
 updateSession(){
 const request:IEditSessions ={
  id : this.selectedsession.id, 
  serverProvider:this.serviceProviderId,
  startDateTime:this.selectedsession.startDateTime,
  duration:this.selectedsession.duration,
  status:0,
  type:this.selectedsession.type
  }
  this.serviceProviderService.editSession(request).subscribe({
    next:()=>{
      if(!this.serviceProviderId)return
      this.loadsessions(this.serviceProviderId);
    },
    error:(err)=>{
      console.log(err)
    }
  })
 }
}
