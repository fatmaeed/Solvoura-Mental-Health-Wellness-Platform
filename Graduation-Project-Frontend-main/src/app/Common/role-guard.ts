import { TokenHandlerService } from './../Services/token-handler-service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AccountService } from '../Services/Auth/account-service'; // adjust this path

@Injectable({
  providedIn: 'root'
})
export class RoleRedirectGuard implements CanActivate{
  constructor(private TokenHandlerService: TokenHandlerService, private router: Router) {}

  canActivate(): boolean {

    const userRole = this.TokenHandlerService.Role;

    if (userRole == 'SERVICEPROVIDER') {
      this.router.navigate(['/service-provider-dashboard']);
      return false;
    }

    if (userRole == 'ADMIN') {
      this.router.navigate(['/admin-dashboard']);
      return false;
    }

    return true;
  }
}
