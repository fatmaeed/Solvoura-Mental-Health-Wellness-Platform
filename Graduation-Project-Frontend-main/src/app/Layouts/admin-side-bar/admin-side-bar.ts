import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { TokenHandlerService } from '../../Services/Auth/token-handler-service';
import { ServiceProviderService } from '../../Services/service-provider-service';
import { AccountService } from '../../Services/Auth/account-service';
import { Router, RouterLink, RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-side-bar',
  imports: [RouterLink, RouterModule],
  templateUrl: './admin-side-bar.html',
  styleUrl: './admin-side-bar.css'
})
export class AdminSideBar implements AfterViewInit {
  adminName: string | null = null;
  activeTab: string = 'dashboard';
  adminImage: string | null = null;

  constructor(
    private tokenHandlerService: TokenHandlerService,
    private serviceProviderService: ServiceProviderService,
    private cdr: ChangeDetectorRef,
    private accountService: AccountService,
    private router: Router
  ) {}

  ngOnInit() {

      this.adminName = this.tokenHandlerService.UserName;

  }

  logout() {
    this.accountService.logout();
    this.router.navigate(['/login']);
  }

  getInitials(name: string): string {
    if (!name) return 'AD';
    const names = name.split(' ');
    let initials = names[0].substring(0, 1).toUpperCase();
    if (names.length > 1) {
      initials += names[names.length - 1].substring(0, 1).toUpperCase();
    }
    return initials;
  }

  ngAfterViewInit() {
    this.cdr.detectChanges();
  }
}
