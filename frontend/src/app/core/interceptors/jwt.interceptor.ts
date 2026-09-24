import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TokenStorageService } from '../services/token-storage.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenStorage = inject(TokenStorageService);
  const accessToken = tokenStorage.getAccessToken();

  if (!accessToken || req.url.includes('/auth/login') || req.url.includes('/auth/refresh')) {
    return next(req);
  }

  const authorizedRequest = req.clone({
    setHeaders: { Authorization: `Bearer ${accessToken}` }
  });

  return next(authorizedRequest);
};
