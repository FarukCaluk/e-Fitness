import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserRole } from '../models/user-role.enum';

export const roleGuard: CanActivateFn = (route) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const allowedRoles = route.data['roles'] as UserRole[] | undefined;
  const currentUser = authService.getCurrentUser();

  if (!currentUser) {
    return router.createUrlTree(['/auth/login']);
  }

  if (allowedRoles && !allowedRoles.includes(currentUser.role)) {
    return router.createUrlTree(['/auth/login']);
  }

  return true;
};
