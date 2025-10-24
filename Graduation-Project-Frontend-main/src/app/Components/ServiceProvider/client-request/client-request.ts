import { IProviderRequest, RequestStatus } from './../../../Models/ServiceProviderModels/iprovider-request';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ServiceProviderService } from '../../../Services/service-provider-service';
import { TokenHandlerService } from '../../../Services/token-handler-service';
import { IdisplaySessionWithStatus } from '../../../Models/ServiceProviderModels/idisplay-session-with-status';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { take } from 'rxjs/operators';
import { SideBarServiceProvider } from "../../../Layouts/side-bar-service-provider/side-bar-service-provider";


@Component({
  selector: 'app-client-request',
  standalone: true,
  imports: [FormsModule, CommonModule, SideBarServiceProvider],
  templateUrl: './client-request.html',
  styleUrl: './client-request.css'
})
export class ClientRequest implements OnInit {
  providerId: number | null = null;
  sessionRequest: IdisplaySessionWithStatus[] = [];
  selectedSession!: IdisplaySessionWithStatus;
  cancle: RequestStatus = RequestStatus.Canceled;
  Posponed: RequestStatus = RequestStatus.Posponed;
  reason: string = '';
  newdate: string = '';
  notes: string = '';
  rejectReason: string = '';
  loading: any;

  constructor(
    private service: ServiceProviderService,
    private cdr: ChangeDetectorRef,
    private tokenHelper: TokenHandlerService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.providerId = this.tokenHelper.UserId;
    if (!this.providerId) return;
    console.log(this.providerId);
    this.loadSessions();
    this.cdr.detectChanges()
  }

  loadSessions() {
    if (!this.providerId) return;

    this.service.getSessionForClientRequest(this.providerId)
      .pipe(take(1))
      .subscribe({
        next: (data) => {
          this.sessionRequest = [...data]; 
          this.cdr.detectChanges();
        },
        error: (err) => console.log(err)
      });
  }

  isCanceled(status: string | RequestStatus): boolean {
    if (typeof status === 'string') {
      return status === RequestStatus[RequestStatus.Canceled];
    }
    return status === RequestStatus.Canceled;
  }

  isPostponed(status: string | RequestStatus): boolean {
    if (typeof status === 'string') {
      return status === RequestStatus[RequestStatus.Posponed];
    }
    return status === RequestStatus.Posponed;
  }

  getStatusString(status: RequestStatus): string {
    return RequestStatus[status];
  }

  openAcceptModal(session: IdisplaySessionWithStatus) {
    this.selectedSession = session;
    this.reason = '';
  }

  openPostponeModal(session: IdisplaySessionWithStatus) {
    this.selectedSession = session;
    this.newdate = '';
    this.notes = '';
  }

  openRejectModal(session: IdisplaySessionWithStatus) {
    this.selectedSession = session;
    this.rejectReason = '';
  }
//Accept cancel
  sendAcceptForClientReq() {

    const request: IProviderRequest = {
      sessionId: this.selectedSession.id,
      isApproved: true,
      clientRequetStatus: 0,
      newStartDateTime: null,
      notes: this.reason
    };

    this.service.handleDecideOnSession(request).subscribe({
      next: () => {
        this.loadSessions();
      },
      error: (err) => console.log(err)
    });
  }
//accept Posponed
  sendReqForPosponed() {
    const request: IProviderRequest = {
      sessionId: this.selectedSession.id,
      isApproved: true,
      clientRequetStatus: 1,
      newStartDateTime: this.newdate,
      notes: this.notes
    };
    console.log(request)

    this.service.handleDecideOnSession(request).subscribe({
      next: () => {
        this.loadSessions();
      },
      error: (err) => console.log(err)
    });
  }

  rejectSession() {
    if (!this.rejectReason) {
      alert('Please provide a reason for rejection');
      return;
    }

    const request: IProviderRequest = {
      sessionId: this.selectedSession.id,
      isApproved: false,
      clientRequetStatus: RequestStatus[this.selectedSession.status as any],
      newStartDateTime: null,
      notes: this.rejectReason
    };

    this.service.handleDecideOnSession(request).subscribe({
      next: () => {
        this.loadSessions();
      },
      error: (err) => console.log(err)
    });
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
    });
  }
}
