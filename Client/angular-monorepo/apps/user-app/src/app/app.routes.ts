import { Route } from '@angular/router';
import { userAuthorizedCanActivateFn } from './modules/authorization/guards/authorization-guards';

export const appRoutes: Route[] = [
  { path: '', redirectTo: 'main', pathMatch: 'full' },
  {
    path: 'main',
    loadComponent: () => import('./modules/main/main.component'),
    canActivate: [ userAuthorizedCanActivateFn ]
  },
  {
    path: 'authorization',
    loadComponent: () => import('./modules/authorization/components/login/login.component'),
  }
];
