import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { UserNameGuard } from './change-password-guard';

describe('changePasswordGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => UserNameGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
