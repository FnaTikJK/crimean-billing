import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { map, take } from 'rxjs';
import { AuthorizationService } from '../services/authorization.service';

export const userAuthorizedCanActivateFn: CanActivateFn = () => {
  const authorizationS = inject(AuthorizationService);
  const router = inject(Router);
  return authorizationS.authorizationChecked$
    .pipe(
      map(() => {
        if (authorizationS.isAuthorized()) { return true; }
        router.navigate(['authorization'], { relativeTo: null });
        return false;
      }),
      take(1)
    );
};

export const userUnauthorizedCanActivateFn: CanActivateFn = () => {
  const authorizationS = inject(AuthorizationService);
  const router = inject(Router);
  return authorizationS.authorizationChecked$
    .pipe(
      map(() => {
        if (!authorizationS.isAuthorized()) { return true; }
        router.navigate(['main'], { relativeTo: null });
        return false;
      }),
      take(1)
    );
}
