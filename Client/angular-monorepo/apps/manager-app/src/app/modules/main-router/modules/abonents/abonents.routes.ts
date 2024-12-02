import { Route } from '@angular/router';

export default [
  { path: ':id', loadComponent: () => import('./modules/page/abonent-page.component') }
] as Route[]
