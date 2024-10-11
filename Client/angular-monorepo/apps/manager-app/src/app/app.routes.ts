import { Route } from '@angular/router';
import {
  managerAuthorizedCanActivateFn,
  managerUnauthorizedCanActivateFn
} from './modules/authorization/guards/authorization-guards';

export const appRoutes: Route[] = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'main'
  },
  {
    path: 'main',
    loadComponent: () => import('./modules/main/main.component'),
    loadChildren: () => import('./modules/main/main.routes'),
    canActivate: [ managerAuthorizedCanActivateFn ]
  },
  {
    path: 'authorization',
    loadComponent: () => import('./modules/authorization/components/login.component'),
    canActivate: [ managerUnauthorizedCanActivateFn ]
  },
  {
    path: 'settings',
    loadComponent: () => import('./modules/settings/settings.component'),
    loadChildren: () => import('./modules/settings/settings.routes'),
    canActivate: [ managerAuthorizedCanActivateFn ]
  }
];

