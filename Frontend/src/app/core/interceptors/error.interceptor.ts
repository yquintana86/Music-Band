import { HttpErrorResponse, HttpInterceptorFn, HttpResponse } from "@angular/common/http";

import { catchError, map, throwError } from 'rxjs';

import { APIOperationResultBase } from "../../shared/interfaces";

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    map((event) => {
      if(!(event instanceof HttpResponse)) {
        return event;
      }

      const body = event.body as APIOperationResultBase;

      if(!body) return event;

      if(!body.isSuccess) {
        const errors = body.errors ?? [];
        const message = errors.map(e => e.message).join(', ');
        throw new Error(message);
      }

      return event;
    }),
    catchError((error: HttpErrorResponse | Error) => {
      if(error instanceof HttpErrorResponse) {
        switch(error.status) {
          case 400:
            console.log('Bad Request');
            break;
          case 404:
             console.log('Resource not Found');
             break;
          case 500:
            console.log('Internal Server Error');
            break;
          default:
            const errorMessage = error.message ?? 'Something went wrong';
            console.log(errorMessage);
        }
      }
      return throwError(() => error);
    })
  )
}
