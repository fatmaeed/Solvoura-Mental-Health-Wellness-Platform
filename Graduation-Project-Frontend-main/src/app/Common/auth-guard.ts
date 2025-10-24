import { CanActivateFn } from '@angular/router';
import { JwtHelperService } from '../Services/jwt-helper-service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const jwtHelper = inject(JwtHelperService);
  const decodedToken = jwtHelper.getDecodedToken();
  if (!decodedToken) return false; 
  if(!decodedToken.UserId || !decodedToken.Role || !decodedToken.Email || !decodedToken.UserName) return false;
  return true;
};
