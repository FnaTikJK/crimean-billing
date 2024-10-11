import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { map, take } from 'rxjs';

export const managerAuthorizedCanActivateFn: CanActivateFn = () => {
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
}

export const managerUnauthorizedCanActivateFn: CanActivateFn = () => {
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
