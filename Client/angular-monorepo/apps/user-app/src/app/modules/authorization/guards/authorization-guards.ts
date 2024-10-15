import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const userAuthorizedCanActivateFn: CanActivateFn = () => {
  const router = inject(Router);
  router.navigate(['authorization'], { relativeTo: null });
  return false;
};
