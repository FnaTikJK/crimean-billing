import { Route } from '@angular/router';

export default [
  {
    path: 'create',
    loadComponent: () => import('./modules/create/managers-create.component'),
  },
] satisfies Route[]
