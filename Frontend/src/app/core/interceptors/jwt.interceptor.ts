import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { Router } from '@angular/router';
import { inject } from '@angular/core';

import { catchError, Observable, switchMap, throwError } from 'rxjs';

import { AuthService } from '../services/auth.service';


export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);

  if (req.url.includes('login') ||
    req.url.includes('register') ||
    req.url.includes('refresh') )
    {
      return next(req);
    }
  const accessToken = authService.accessToken();
  const authReq = accessToken ? req.clone({
    setHeaders: {
      Authorization: `Bearer ${accessToken}`
    }
  }) : req;

  return next(authReq)
    .pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status !== 401) {
          return throwError(() => error);
        }
        const userId = authService.currentUser()?.sub;

        if (!authService.refreshToken() || !userId) {
          return errorHandler(authService, router, error);
        }

        return authService.renewCredentials(userId)
          .pipe(
            switchMap(() => {
              const retryReq = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${authService.accessToken()}`
                }
              });
              return next(retryReq);
            }),
            catchError(refreshError => {
              return errorHandler(authService, router, refreshError);
            })
          )
      })
    )
};

const errorHandler = (authService: AuthService, router: Router, error: any): Observable<never> => {
  authService.logout();
  router.navigateByUrl('auth/login');
  return throwError(() => error);
}


