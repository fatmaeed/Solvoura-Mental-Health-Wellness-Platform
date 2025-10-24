import { CanActivateFn } from '@angular/router';
import { JwtHelperService } from '../Services/jwt-helper-service';
import { inject } from '@angular/core';

export const UserNameGuard: CanActivateFn = (route, state) => {
 let username = route. params['username'];
  const jwtHelper = inject(JwtHelperService);
  const decodedToken = jwtHelper.getDecodedToken();
  if (!decodedToken) return false;
  if(username !== decodedToken.UserName) return false;
  
  return true;
};
