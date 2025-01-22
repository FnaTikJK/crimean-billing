import { Route } from '@angular/router';

export default [
  { path: 'create', loadComponent: () => import('./modules/create/account-create.component') },
  { path: ':id', loadComponent: () => import('./modules/page/account-page.component') }
] satisfies Route[]
