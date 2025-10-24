import { TokenHandlerService } from './../../Services/token-handler-service';
import { ActivatedRoute, Router } from '@angular/router';
import { SessionService } from './../../Services/session-service';
import { environment } from '../../../environments/environment';
import { SignalRService } from '../../Services/signalr-service';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingComp } from "../loading/loading";

@Component({
  selector: 'app-meeting-session',
  standalone: true,
  imports: [CommonModule, LoadingComp],
  templateUrl: './meeting-session.html',
  styleUrls: ['./meeting-session.css']
})
export class MeetingSession implements OnInit, OnDestroy {
  @ViewChild('localVideo') localVideo!: ElementRef<HTMLVideoElement>;
  @ViewChild('remoteVideo') remoteVideo!: ElementRef<HTMLVideoElement>;

  sessionModel!: IMeetingSession;
  private peerConnection!: RTCPeerConnection;
  private localStream!: MediaStream;
  private roomId: string = '';
  private iceCandidateBuffer: RTCIceCandidateInit[] = [];
  private isRemoteDescriptionSet = false;

  isRemoteVideoActive = true;
  callStartTime: Date | null = null;
  elapsedTime: string = '00:00:00';
  private timerInterval: any;

  // UI State
  isCallStarted = false;
  isMuted = false;
  isVideoOff = false;
  isScreenSharing = false;
  isConnecting = false;
  connectionError: string | null = null;
  participants: string[] = [];
  role!: string | null;
  isSessionStarted: boolean = false;

  constructor(
    private signalRService: SignalRService,
    private sessionService: SessionService,
    private changeDetectorRef: ChangeDetectorRef,
    private tokenHandlerService: TokenHandlerService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    this.role = this.tokenHandlerService.Role;
  }

  async ngOnInit() {
    const sessionId = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    this.sessionService.GetMeetingSession(sessionId).subscribe({
      next: (data: IMeetingSession) => {
        this.sessionModel = data;
        this.roomId = data.sessionId.toString();
        this.participants = [data.doctorName, data.patientName].filter(name => name);
        this.changeDetectorRef.detectChanges();
      },
      error: (err: any) => {
        console.error('Error loading session:', err);
        this.connectionError = 'Failed to load session details';
      }
    });

    await this.initWebRTC();
    this.signalRService.initConnection(`${environment.apiBaseUrl}hubs/video`,
      () => {
        console.log(this.tokenHandlerService.UserId, this.roomId);
        this.signalRService.sendMessage('ConnectSession', this.tokenHandlerService.UserId, this.roomId);
      }
    );
    this.registerSignalREvents();
  }

  ngOnDestroy() {
    this.cleanUp();
  }

  onRemoteVideoEnded() {
    this.isRemoteVideoActive = false;
    this.changeDetectorRef.detectChanges();
  }

  private registerSignalREvents() {
    this.signalRService.onMessage('IsStarted', async (IsStarted: boolean) => {
      console.log(IsStarted);
      this.isSessionStarted = IsStarted;
      this.changeDetectorRef.detectChanges();
    });
    this.signalRService.onMessage('UserLeft', async (IsStarted: boolean) => {
      this.connectionError = 'User left. Redirecting...';
      setTimeout(() => window.location.reload(), 2000);

    });

    this.signalRService.onMessage('ReceiveOffer', async (offerStr: string) => {
      try {
        const offer = JSON.parse(offerStr);
        console.log("offer",offer);
        await this.peerConnection.setRemoteDescription(new RTCSessionDescription(offer));
        this.isRemoteDescriptionSet = true;
        await this.processBufferedCandidates();

        const answer = await this.peerConnection.createAnswer();
        await this.peerConnection.setLocalDescription(answer);
        console.log("room for answer : ",this.roomId);
        await this.signalRService.sendMessage('SendAnswer', JSON.stringify(answer), this.roomId);
      } catch (err) {
        console.error('Error handling offer:', err);
      }
    });

    this.signalRService.onMessage('ReceiveAnswer', async (answerStr: string) => {
      try {
        const answer = JSON.parse(answerStr);
        console.log("answer",answer);
        await this.peerConnection.setRemoteDescription(new RTCSessionDescription(answer));
        this.isRemoteDescriptionSet = true;
        await this.processBufferedCandidates();
      } catch (err) {
        console.error('Error handling answer:', err);
      }
    });

    this.signalRService.onMessage('ReceiveIceCandidate', async (candidateStr: string) => {
      try {
        const candidate = JSON.parse(candidateStr);
        console.log("candidate",candidate);
        if (!this.isRemoteDescriptionSet) {
          this.iceCandidateBuffer.push(candidate);
          return;
        }
        await this.peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
      } catch (err) {
        console.error('Error adding ICE candidate:', err);
      }
    });

    this.signalRService.onMessage('MeetingEnded', () => {
      this.cleanUp();
      this.connectionError = 'The meeting has ended';
            this.changeDetectorRef.detectChanges();

      setTimeout(() => {
        this.router.navigate([`/create-feedback/${this.sessionModel.sessionId}`]);
      }, 2000);
    });

    this.signalRService.onMessage('Failure', (failure: string) => {
      this.cleanUp();
      this.connectionError = failure;
      this.changeDetectorRef.detectChanges();
    });
  }

  private async processBufferedCandidates() {
    while (this.iceCandidateBuffer.length > 0) {
      const candidate = this.iceCandidateBuffer.shift();
      if (candidate) {
        try {
          await this.peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
        } catch (err) {
          console.error('Error adding buffered ICE candidate:', err);
        }
      }
    }
  }

  private async initWebRTC() {
    try {
      await this.getMedia();
      this.setupPeerConnection();
    } catch (err) {
      console.error('Error initializing WebRTC:', err);
      this.connectionError = 'Could not access camera/microphone. Please check permissions.';
      this.changeDetectorRef.detectChanges();
    }
  }

  private async getMedia() {
    this.localStream = await navigator.mediaDevices.getUserMedia({
      video: true,
      audio: true
    });
    this.localVideo.nativeElement.srcObject = this.localStream;
    this.localVideo.nativeElement.volume = 0;
  }

  private setupPeerConnection() {
    const iceServers = {
      iceServers: [
        { urls: 'stun:stun.l.google.com:19302' },
        // Add TURN servers if needed
      ]
    };

    this.peerConnection = new RTCPeerConnection(iceServers);

    // Add local stream
    this.localStream.getTracks().forEach(track => {
      this.peerConnection.addTrack(track, this.localStream);
    });

    // ICE Candidate handling
    this.peerConnection.onicecandidate = (event) => {
      if(this.connectionError) return;
      if (event.candidate) {
        const candidate = {
          candidate: event.candidate.candidate,
          sdpMid: event.candidate.sdpMid,
          sdpMLineIndex: event.candidate.sdpMLineIndex,
          usernameFragment: event.candidate.usernameFragment
        };
        this.signalRService.sendMessage('SendIceCandidate', JSON.stringify(candidate), this.roomId);
      }
    };

    // Track handling
    this.peerConnection.ontrack = (event) => {

      if (event.streams && event.streams[0]) {
        const remoteStream = event.streams[0];
        this.remoteVideo.nativeElement.srcObject = remoteStream;
        this.isRemoteVideoActive = true;

        // Add event listeners to detect when tracks are ended
        remoteStream.getVideoTracks().forEach(track => {
          track.onmute = () => {
            this.isRemoteVideoActive = false;
            this.changeDetectorRef.detectChanges();
          };
          track.onunmute = () => {
            this.isRemoteVideoActive = true;
            this.changeDetectorRef.detectChanges();
          };
        });


        this.changeDetectorRef.detectChanges();
      }
    };

    this.peerConnection.onconnectionstatechange = () => {
      console.log('Connection state:', this.peerConnection.connectionState);

      switch (this.peerConnection.connectionState) {
        case 'connected':
          this.isCallStarted = true;
          this.isConnecting = false;
          this.callStartTime = new Date();
          this.startTimer();
          break;
        case 'disconnected':
        case 'failed':
          this.isRemoteVideoActive = false;
          this.connectionError = 'Connection lost';
          break;
      }
      this.changeDetectorRef.detectChanges();
    };
  }

  // Timer methods
  private startTimer() {
    this.timerInterval = setInterval(() => {
      if (this.callStartTime) {
        const now = new Date();
        const diff = now.getTime() - this.callStartTime.getTime();
        this.elapsedTime = this.formatTime(diff);
        this.changeDetectorRef.detectChanges();
      }
    }, 1000);
  }

  private formatTime(ms: number): string {
    const totalSeconds = Math.floor(ms / 1000);
    const hours = Math.floor(totalSeconds / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;

    return [
      hours.toString().padStart(2, '0'),
      minutes.toString().padStart(2, '0'),
      seconds.toString().padStart(2, '0')
    ].join(':');
  }

  private stopTimer() {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
      this.timerInterval = null;
    }
  }

  joinRoomFunction() {
    try {
      this.signalRService.sendMessage('JoinRoom', this.tokenHandlerService.UserId, this.roomId);
      this.OnConnectionStatus();
    } catch (err) {
      this.HandleError(err);
    }
  }

  async startCall() {
    this.isConnecting = true;
    try {
      await this.signalRService.sendMessage('StartSession', this.tokenHandlerService.UserId, this.roomId);

      if (this.tokenHandlerService.Role === 'SERVICEPROVIDER') {
              if(this.connectionError) {
                this.isConnecting = false;
                this.changeDetectorRef.detectChanges();
                return;
              };

        await this.createAndSendOffer();
      }
    } catch (err) {
      this.HandleError(err);
    }
  }

  private HandleError(err: unknown) {
    console.error('Error starting call:', err);
    this.connectionError = 'Failed to start call';
    this.isConnecting = false;
    this.changeDetectorRef.detectChanges();
  }

  private OnConnectionStatus() {
    this.peerConnection.onconnectionstatechange = () => {
      if (this.peerConnection.connectionState === 'connected') {
        this.callStartTime = new Date();
        this.startTimer();
        this.isCallStarted = true;
        this.isConnecting = false;
        this.changeDetectorRef.detectChanges();
      }
    };
  }

  private async createAndSendOffer() {
    try {
      const offer = await this.peerConnection.createOffer();
      await this.peerConnection.setLocalDescription(offer);
      await this.signalRService.sendMessage('SendOffer', JSON.stringify(offer), this.roomId);
    } catch (err) {
      console.error('Error creating/sending offer:', err);
      throw err;
    }
  }

  async toggleMute() {
    this.isMuted = !this.isMuted;
    this.localStream.getAudioTracks().forEach(track => {
      track.enabled = !this.isMuted;
    });
    this.changeDetectorRef.detectChanges();
  }

  async toggleVideo() {
  this.isVideoOff = !this.isVideoOff;

  if (this.isVideoOff) {
    const blankStream = new MediaStream();
    this.replaceStream(blankStream);
    this.localVideo.nativeElement.srcObject = null; 
  } else {
    try {
      const newStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
      this.replaceStream(newStream);
      this.localVideo.nativeElement.srcObject = newStream;
    } catch (err) {
      console.error('Error re-enabling video:', err);
    }
  }

  this.changeDetectorRef.detectChanges();
}

  async toggleScreenShare() {
    if (this.isScreenSharing) {
      // Stop screen sharing
      this.localStream.getVideoTracks().forEach(track => track.stop());
      const newStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
      this.replaceStream(newStream);
      this.isScreenSharing = false;
    } else {
      try {
        const screenStream = await navigator.mediaDevices.getDisplayMedia({ video: true });
        this.replaceStream(screenStream);
        this.isScreenSharing = true;
      } catch (err) {
        console.error('Error sharing screen:', err);
      }
    }
    this.changeDetectorRef.detectChanges();
  }

  private replaceStream(newStream: MediaStream) {
    const senders = this.peerConnection.getSenders();
    this.localStream.getTracks().forEach(track => track.stop());
    this.localStream = newStream;
    this.localVideo.nativeElement.srcObject = this.localStream;

    newStream.getTracks().forEach(track => {
      const existingSender = senders.find(s => s.track?.kind === track.kind);
      if (existingSender) {
        existingSender.replaceTrack(track);
      } else {
        this.peerConnection.addTrack(track, newStream);
      }
    });
  }

  endCall() {
    this.cleanUp();
    this.signalRService.sendMessage('EndMeeting',this.tokenHandlerService.UserId, this.roomId);
  }

  private cleanUp() {
    this.stopTimer();
    this.callStartTime = null;
    this.elapsedTime = '00:00:00';
    this.isRemoteVideoActive = false;

    if (this.peerConnection) {
      this.peerConnection.close();
    }
    if (this.localStream) {
      this.localStream.getTracks().forEach(track => track.stop());
    }
    this.isCallStarted = false;
    this.changeDetectorRef.detectChanges();
  }
}

