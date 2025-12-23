import { HttpInterceptorFn, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, map, throwError } from 'rxjs';

/**
 * HTTP Interceptor for:
 * 1. Unwrapping API response format { success, data } to direct data
 * 2. Adding Authorization header with JWT token
 * 3. Handling errors consistently
 */
export const apiResponseInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  // Add Authorization header if token exists
  const token = authService.getToken();
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req).pipe(
    map((event) => {
      // Unwrap API response format: { success: true, data: {...} } -> {...}
      if (event instanceof HttpResponse) {
        const body = event.body as any;
        
        // Check if response follows API standard format
        if (body && typeof body === 'object' && 'success' in body) {
          // If success is true, return the data directly
          if (body.success === true && 'data' in body) {
            return event.clone({ body: body.data });
          }
          // If success is false, it's an error response - let error handler deal with it
          if (body.success === false && 'error' in body && body.error) {
            throw new HttpErrorResponse({
              error: body.error,
              status: event.status,
              statusText: event.statusText,
              url: event.url || undefined
            });
          }
        }
      }
      return event;
    }),
    catchError((error: HttpErrorResponse) => {
      // Handle API error format: { success: false, error: {...} }
      if (error.error && typeof error.error === 'object') {
        const errorBody = error.error as any;
        if ('success' in errorBody && errorBody.success === false && 'error' in errorBody && errorBody.error) {
          const apiError = errorBody.error;
          return throwError(() => new HttpErrorResponse({
            error: {
              code: apiError.code || 'UNKNOWN_ERROR',
              message: apiError.message || 'An error occurred',
              details: apiError.details || {}
            },
            status: error.status,
            statusText: error.statusText,
            url: error.url || undefined
          }));
        }
      }
      
      // Handle standard HTTP errors
      return throwError(() => error);
    })
  );
};

