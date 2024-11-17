import { Route } from '@angular/router';

export default [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'accounts'
  },
  {
    path: 'accounts',
    loadComponent: () => import('./modules/accounts/accounts.component'),
    loadChildren: () => import('./modules/accounts/accounts.routes'),
  },
  {
    path: 'abonents',
    loadComponent: () => import('./modules/abonents/abonents.component'),
    loadChildren: () => import('./modules/abonents/abonents.routes'),
  },
  {
    path: 'services',
    loadComponent: () => import('./modules/services/services.component'),
    loadChildren: () => import('./modules/services/services.routes'),
  },
  {
    path: 'managers',
    loadComponent: () => import('./modules/managers/managers.component'),
    loadChildren: () => import('./modules/managers/managers.routes'),
  },
  {
    path: 'payments',
    loadComponent: () => import('./modules/payments/payments.component'),
    loadChildren: () => import('./modules/payments/payments.routes'),
  },
  {
    path: 'invoices',
    loadComponent: () => import('./modules/invoices/invoices.component'),
    loadChildren: () => import('./modules/invoices/invoices.routes'),
  }
] satisfies Route[]
