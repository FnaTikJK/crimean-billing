import { Route } from '@angular/router';

export default [
  { path: 'create', loadComponent: () => import('./modules/create/abonent-create.component') },
  { path: ':id', loadComponent: () => import('./modules/page/abonent-page.component') }
] as Route[]
