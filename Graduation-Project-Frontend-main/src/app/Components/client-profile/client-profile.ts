import { ChangeDetectorRef, Component, OnInit, Pipe } from '@angular/core';
import { ClientService } from '../../Services/client-service';
import { IClientSession } from '../../Models/Client/iclient-session';
import { TokenHandlerService } from '../../Services/token-handler-service';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive, RouterModule } from '@angular/router';
import { AccountService } from '../../Services/Auth/account-service';
import { NavBar } from "../../Layouts/nav-bar/nav-bar";
import { environment } from '../../../environments/environment';
import { ClientInfo } from "./client-info/client-info";



@Component({
  selector: 'app-client-profile',
  imports: [CommonModule, RouterModule, NavBar],
  templateUrl: './client-profile.html',
  styleUrl: './client-profile.css'
})
export class ClientProfile implements OnInit {
  clientSessions: IClientSession[] = [];
  Client:any;
  apiUrl: string =environment.apiBaseUrl;
 isBefore10Minutes: boolean = false;
  constructor(private service:ClientService ,private cdr :ChangeDetectorRef, private tokenHandler:TokenHandlerService,private router:Router,public accountService: AccountService) { }

  ngOnInit(): void {
    const clientId:any = this.tokenHandler.UserId;
    if (clientId) {
      this.service.getClientProfile(clientId).subscribe({
        next: (data) => {
          this.clientSessions = data;
          this.cdr.detectChanges();
          console.log('Client sessions fetched successfully:', this.clientSessions);
        },
        error: (err) => {
          console.error('Error fetching client profile:', err);
        }
      });
    } else {
      console.error('Client ID not found in token');
    }


    this.service.getClientById(clientId).subscribe({
      next: (data) => {
        this.Client = data;
        this.cdr.detectChanges();
        let  datenow = new Date();
        let   sessionStart = new Date(this.Client.startDateTime);
        let tenMinutesBeforeStart = new Date(sessionStart.getTime() - 10 * 60 * 1000);
        this.isBefore10Minutes = datenow < tenMinutesBeforeStart;
        console.log('Client profile fetched successfully:', this.Client);
      },
      error: (err) => {
        console.error('Error fetching client profile:', err);
      }
    });

  }
    logout(){
    this.accountService.logout();
    this.router.navigate(['/login']);
   }

}
