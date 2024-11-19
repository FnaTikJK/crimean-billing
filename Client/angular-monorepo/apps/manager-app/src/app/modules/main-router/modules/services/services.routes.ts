import { Route } from '@angular/router';

export default [
  {
    path: 'create',
    loadComponent: () => import('./modules/create/service-create.component')
  },
  {
    path: ':id',
    loadComponent: () => import('./services.component')
  },
] satisfies Route[]
