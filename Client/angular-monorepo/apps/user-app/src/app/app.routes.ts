import { Route } from '@angular/router';
import {
  userAuthorizedCanActivateFn,
  userUnauthorizedCanActivateFn
} from './modules/authorization/guards/authorization-guards';

export const appRoutes: Route[] = [
  { path: '', redirectTo: 'main', pathMatch: 'full' },

  {
    path: 'main',
    loadComponent: () => import('./modules/main/main.component'),
    title: 'Главная',
    canActivate: [ userAuthorizedCanActivateFn ]
  },

  {
    path: 'profile',
    loadComponent: () => import('./modules/profile/profile.component'),
    title: 'Профиль',
    canActivate: [ userAuthorizedCanActivateFn ]
  },

  {
    path: 'expenses',
    loadComponent: () => import('./modules/expenses/expenses.component'),
    title: 'Контроль расходов',
    canActivate: [ userAuthorizedCanActivateFn ]
  },

  {
    path: 'subscriptions',
    loadChildren: () => import('./modules/subscriptions/subscriptions.routes'),
    canActivate: [ userAuthorizedCanActivateFn ]
  },

  {
    path: 'authorization',
    loadComponent: () => import('./modules/authorization/components/login/login.component'),
    canActivate: [ userUnauthorizedCanActivateFn ],
    title: 'Авторизация',
  }
];
