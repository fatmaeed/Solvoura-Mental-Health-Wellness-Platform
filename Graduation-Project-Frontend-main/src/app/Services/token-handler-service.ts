import { JwtHelperService } from './jwt-helper-service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenHandlerService {
  constructor(private jwtHelperService: JwtHelperService) { }

  get Role(): string | null {
    return this.jwtHelperService.getClaim('Role');
  }

  get UserName (): string | null {
    return this.jwtHelperService.getClaim('UserName');
    
  }
  get UserId (): number | null {
    return this.jwtHelperService.getClaim('UserId');
  }

  get Email() : string | null {
    return this.jwtHelperService.getClaim('Email');
  }
  removeToken(): void {
    localStorage.removeItem('token');
  }
}
