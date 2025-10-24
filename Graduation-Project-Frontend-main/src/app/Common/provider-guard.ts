import { CanActivateFn } from '@angular/router';
import { JwtHelperService } from '../Services/jwt-helper-service';
import { inject } from '@angular/core';

export const providerGuard: CanActivateFn = (route, state) => {
  const jwtHelper = inject(JwtHelperService);
  const decodedToken = jwtHelper.getDecodedToken();
  if (!decodedToken) return false;
  if (decodedToken.role !== 'SERVICEPROVIDER') return false;
  return true;
};
