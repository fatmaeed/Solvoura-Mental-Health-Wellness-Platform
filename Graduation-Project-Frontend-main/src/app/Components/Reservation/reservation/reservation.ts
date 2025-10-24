
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IResrvationRequest } from '../../../Models/Reservation/iresrvation-request';
import { ReservationService } from '../../../Services/reservation-service';
import { ServiceProviderService } from '../../../Services/service-provider-service';
import { IDisplayServiceProvider } from '../../../Models/ServiceProviderModels/idisplay-service-provider';
import { ISessionForReserv } from '../../../Models/Session/isession-for-reserv';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TokenHandlerService } from '../../../Services/token-handler-service';
import { CommonModule } from '@angular/common';
import { PaypalButton } from "../../paypal-button/paypal-button";
import { NavBar } from "../../../Layouts/nav-bar/nav-bar";
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-reservation',
  imports: [FormsModule, CommonModule, PaypalButton, NavBar],
  templateUrl: './reservation.html',
  styleUrl: './reservation.css'
})
export class Reservation implements OnInit {


  imgError = false;

  freesessions!: ISessionForReserv[];
  provider!: IDisplayServiceProvider;
  providerId!: number;
  selectedSessionCount!: number ;
  selectedSessionType: string = 'Offline';
  showSessions : boolean = false;
  selectedSessionIds: number[] = [];
  apiurl:string = environment.apiBaseUrl;


   constructor(private service: ReservationService,private providerService:ServiceProviderService,
    private crd: ChangeDetectorRef,private route :ActivatedRoute , private tokenHandler:TokenHandlerService,private router :Router) {

    }

  ngOnInit(): void {
     this.providerId = Number(this.route.snapshot.paramMap.get('id'));
    this.providerService.getServiceProviderById(this.providerId).subscribe({
      next: (data) => {
      this.provider = data;
        this.crd.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching service provider:', err);
      }})

  }
  backToHome() {
    this.router.navigate(['/home']);
  }
  isSessionSelected(sessionId: number): boolean {
    return this.selectedSessionIds.includes(sessionId);
  }

  onSessionTypeChange(event:Event): void {
    this.selectedSessionType = (event.target as HTMLSelectElement).value;
  }
noshowsession: boolean =false ;
startDate?: string;
endDate?: string;
duration?: string ;
durationInMinutes: number = 0;
FindSessions():void{
  if (this.selectedSessionType) {
      if (this.durationInMinutes <= 0) {
      this.duration = undefined;
    } else {
      const hours = Math.floor(this.durationInMinutes / 60);
      const minutes = this.durationInMinutes % 60;
      this.duration = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:00`;
    }
      this.service.getFreeSessions(this.providerId, this.selectedSessionType,this.startDate,this.endDate,this.duration).subscribe({
      next: (data) => {
        this.freesessions = data;
        this.crd.detectChanges();
        console.log(this.freesessions);
        this.selectedSessionIds = [];
        if(this.freesessions.length === 0) {
          this.showSessions = false;
          this.noshowsession = true;
        this.crd.detectChanges();

        } else {
          this.showSessions = true;
          this.noshowsession = false;
        this.crd.detectChanges();

        }
      },
      error: (err) => {
        console.error('Error fetching sessions:', err);
      }
    });
  }
}
toggleSessionSelection(sessionId: number): void {
    const index = this.selectedSessionIds.indexOf(sessionId);
  if (index === -1) {
      this.selectedSessionIds.push(sessionId);
      this.calcTotalSesseionsPrice()
  } else {
    this.selectedSessionIds.splice(index, 1);
    this.calcTotalSesseionsPrice();
  }
}



formatDate(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-US', {
    weekday: 'short',
    month: 'short',
    day: 'numeric'
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


totalSessionsPrice:number = 0 ;
isReadyToPay:boolean = false ;
calcTotalSesseionsPrice():void {
 this.totalSessionsPrice = 0 ;
  let reserveedSessions = this.freesessions.filter(session => this.selectedSessionIds.includes(session.id));
  for (let session of reserveedSessions) {
    this.totalSessionsPrice += session.sessionPrice;

  }
  this.isReadyToPay = false
  console.log(this.totalSessionsPrice)

}

readyToPay() {
  if(this.selectedSessionIds.length > 0) {
    this.isReadyToPay = true;
  }else {
    this.isReadyToPay = false;

  }
}
isPayed:boolean = false


recievePaid(data :any) {
  console.log(data)
  this.isPayed = data.isPaid ;
  this.isReadyToPay = false;


  this.confirmReservation(data.paymentId)
}
confirmReservation(paymentId:any): void {
  const reservationRequest: IResrvationRequest = {
    serviceProviderId: this.providerId,
    clientId:this.tokenHandler.UserId!,

    paymentId: paymentId,
    status: this.selectedSessionType,
    sessionsNumber: this.selectedSessionIds.length,
    sessionIds: this.selectedSessionIds
  };
  console.log(reservationRequest.sessionsNumber)

  this.service.createReservation(reservationRequest).subscribe({
    next: (res) => {
     this.router.navigate(['/client-profile/client-session']);
    },
    error: (err) => {
      console.error('Error creating reservation:', err);
      alert('Failed to create reservation. Please try again.');
    }
  });
}

}
