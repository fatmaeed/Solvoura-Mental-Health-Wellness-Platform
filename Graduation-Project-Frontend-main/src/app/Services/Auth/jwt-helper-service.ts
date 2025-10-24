import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class JwtHelperService {
  private get token(): string | null {
    return localStorage.getItem('token');
  }

  getToken(): string | null {
    return this.token;
  }

  getDecodedToken(): any | null {
    if (!this.token) return null;

    try {
      return jwtDecode(this.token);
    } catch (error) {
      console.error('Invalid token:', error);
      return null;
    }
  }

  getClaim<T = any>(claim: string): T | null {
    const decoded = this.getDecodedToken();
    return decoded ? decoded[claim] as T : null;
  }
}
