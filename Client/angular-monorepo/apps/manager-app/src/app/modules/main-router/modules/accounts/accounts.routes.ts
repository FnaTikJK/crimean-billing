import { Route } from '@angular/router';

export default [
  { path: ':id', loadComponent: () => import('./modules/page/account-page.component') }
] as Route[]
