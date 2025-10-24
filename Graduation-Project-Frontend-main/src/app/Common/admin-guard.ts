import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { JwtHelperService } from '../Services/jwt-helper-service';

export const adminGuard: CanActivateFn = (route, state) => {
  const jwtHelper = inject(JwtHelperService);
  const decodedToken = jwtHelper.getDecodedToken();
  if (!decodedToken) return false;
  if (decodedToken.role !== 'ADMIN') return false;
  return true;
};
