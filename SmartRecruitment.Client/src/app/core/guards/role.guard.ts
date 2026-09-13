import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const allowedRoles = route.data?.['roles'] as string[] | undefined;
  const userRole = authService.getUserRole();

  if (!userRole) {
    router.navigate(['/login']);
    return false;
  }

  if (!allowedRoles || allowedRoles.length === 0) {
    return true;
  }

  const roleMatches = allowedRoles.some(
    r => r.toLowerCase() === userRole.toLowerCase() ||
         (r.toLowerCase() === 'admin' && userRole.toLowerCase() === 'administrator') ||
         (r.toLowerCase() === 'administrator' && userRole.toLowerCase() === 'admin')
  );

  if (roleMatches) {
    return true;
  }

  // Redirect to correct dashboard based on role
  if (userRole.toLowerCase() === 'admin' || userRole.toLowerCase() === 'administrator') {
    router.navigate(['/admin']);
  } else if (userRole.toLowerCase() === 'employer') {
    router.navigate(['/employer']);
  } else {
    router.navigate(['/seeker']);
  }

  return false;
};
