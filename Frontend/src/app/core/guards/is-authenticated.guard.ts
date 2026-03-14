import { CanMatchFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AuthStatusType } from '../../auth/interfaces';


export const isAuthenticatedGuard: CanMatchFn = (route, state) => {

  const authService = inject(AuthService);
  const router = inject(Router);

  if(authService.authStatus() === AuthStatusType.authenticated){
    return true;
  }

  const lastRequestedUrl = '/' + state.map(s => s.path).join('/');
  return router.createUrlTree(['/auth/login'],
    {
      queryParams: {
        returnUrl: lastRequestedUrl
      }
    }
  );
};
