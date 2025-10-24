import { ChangeDetectorRef, Component, Input, OnInit   } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterModule } from '@angular/router';
import { ServiceProviderService } from '../../Services/service-provider-service';
import { AccountService } from '../../Services/Auth/account-service';
import { TokenHandlerService } from '../../Services/token-handler-service';
import { IDisplayServiceProvider } from '../../Models/ServiceProviderModels/idisplay-service-provider';
import { environment } from '../../../environments/environment';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-side-bar-service-provider',
  imports: [RouterLink, RouterModule , CommonModule],
  templateUrl: './side-bar-service-provider.html',
  styleUrl: './side-bar-service-provider.css'
})
export class SideBarServiceProvider implements OnInit {
 apiurl:string = environment.apiBaseUrl;
  @Input() doctorName: string = 'John Smith';
  @Input() specialization: string = 'Cardiologist';
 upcomingSessionsCount!: number ;
  @Input() unreadMessagesCount: number = 2;
  @Input() activeTab: string = 'dashboard';
  serviceProvider!:IDisplayServiceProvider;
  constructor(private tokenHandlerService: TokenHandlerService, private serviceProviderService: ServiceProviderService, private cdr: ChangeDetectorRef, private accountService: AccountService, private router: Router) { }
  ngOnInit(): void {
    this.doctorName = this.tokenHandlerService.UserName || '';
    this.serviceProviderService.getSessionsToSP(this.tokenHandlerService.UserId!).subscribe({
      next: (data) => {

        this.upcomingSessionsCount = data.length;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error fetching sessions:', error);
      }
    });
    this.serviceProviderService.getServiceProviderById(this.tokenHandlerService.UserId!).subscribe({
      next: (data) => {

        this.serviceProvider =data;
        this.cdr.detectChanges();
        console.log(this.serviceProvider)
      },
      error: (error) => {
        console.error('Error fetching provider:', error);
      }
    });
  }


  logout() {
    this.accountService.logout();
    this.router.navigate(['/login']);
  }


}
