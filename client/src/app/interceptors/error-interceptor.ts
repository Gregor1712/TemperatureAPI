import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from '../shared/components/error-dialog/error-dialog.component';
import { getErrorMessage } from '../shared/utils/error-message.util';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const dialog = inject(MatDialog);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 400) {
        if (err.error.errors) {
          const modalStateErrors = [];
          for (const key in err.error.errors) {
            if (err.error.errors[key]) {
              modalStateErrors.push(err.error.errors[key])
            }
          }
          const message = modalStateErrors.flat().join('\n');
          dialog.open(ErrorDialogComponent, {
            width: '420px',
            data: { title: 'Validation Error', message }
          });
        } else {
          dialog.open(ErrorDialogComponent, {
            width: '420px',
            data: { title: 'Bad Request', message: getErrorMessage(err) }
          });
        }
      }
      if (err.status === 401) {
        dialog.open(ErrorDialogComponent, {
          width: '420px',
          data: { title: 'Unauthorized', message: 'You are not authorized. Please log in.' }
        });
      }
      if (err.status === 403) {
        dialog.open(ErrorDialogComponent, {
          width: '420px',
          data: { title: 'Forbidden', message: 'You do not have permission to access this resource.' }
        });
      }
      if (err.status === 404) {
        router.navigateByUrl('/not-found');
      }
      if (err.status === 500) {
        const navigationExtras: NavigationExtras = {state: {error: err.error}};
        router.navigateByUrl('/server-error', navigationExtras);
      }
      return throwError(() => err)
    })
  )
};
