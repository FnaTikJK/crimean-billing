import { Route } from '@angular/router';

export default [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'services'
  },
  {
    path: ':id',
    loadComponent: () => import('./services.component')
  }
] satisfies Route[]
