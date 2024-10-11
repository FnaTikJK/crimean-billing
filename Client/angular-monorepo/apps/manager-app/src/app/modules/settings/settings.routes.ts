import { Route } from '@angular/router';

export default [
  { path: '', pathMatch: 'full', redirectTo: 'change-password' },
  { path: 'change-password', loadComponent: () => import('./components/change-password.component') }
] satisfies Route[]
