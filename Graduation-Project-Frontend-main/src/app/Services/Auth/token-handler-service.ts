import { Injectable } from '@angular/core';
import { JwtHelperService } from './jwt-helper-service';

@Injectable({
  providedIn: 'root'
})
export class TokenHandlerService {
  constructor(private jwtHelperService: JwtHelperService) {}

  get Role(): string | null {
    return this.jwtHelperService.getClaim<string>('Role');
  }

  get UserName(): string | null {
    return this.jwtHelperService.getClaim<string>('UserName');
  }

  get UserId(): number | null {
    return this.jwtHelperService.getClaim<number>('UserId');
  }

  get Email(): string | null {
    return this.jwtHelperService.getClaim<string>('Email');
  }

  get Token(): string | null {
    return this.jwtHelperService.getToken();
  }

  isLoggedIn(): boolean {
    return !!this.Token;
  }

  removeToken(): void {
    localStorage.removeItem('token');
  }
}
