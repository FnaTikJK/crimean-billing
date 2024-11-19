import { Route } from '@angular/router';
import serviecsRoutes from './modules/services/services.routes';
serviecsRoutes.forEach(route => route.path = `services/${route.path}`)
import accountsRoutes from './modules/accounts/accounts.routes';
accountsRoutes.forEach(route => route.path = `accounts/${route.path}`)
import abonentsRoutes from './modules/abonents/abonents.routes';
abonentsRoutes.forEach(route => route.path = `abonents/${route.path}`)
import managersRoutes from './modules/managers/managers.routes';
managersRoutes.forEach(route => route.path = `managers/${route.path}`)
import paymentsRoutes from './modules/payments/payments.routes';
paymentsRoutes.forEach(route => route.path = `payments/${route.path}`)
import invoicesRoutes from './modules/invoices/invoices.routes';
invoicesRoutes.forEach(route => route.path = `services/${route.path}`)



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
  ...accountsRoutes,
  {
    path: 'abonents',
    loadComponent: () => import('./modules/abonents/abonents.component'),
    loadChildren: () => import('./modules/abonents/abonents.routes'),
  },
  ...abonentsRoutes,
  {
    path: 'services',
    loadComponent: () => import('./modules/services/services.component'),
    loadChildren: () => import('./modules/services/services.routes'),
  },
  ...serviecsRoutes,
  {
    path: 'managers',
    loadComponent: () => import('./modules/managers/managers.component'),
    loadChildren: () => import('./modules/managers/managers.routes'),
  },
  ...managersRoutes,
  {
    path: 'payments',
    loadComponent: () => import('./modules/payments/payments.component'),
    loadChildren: () => import('./modules/payments/payments.routes'),
  },
  ...paymentsRoutes,
  {
    path: 'invoices',
    loadComponent: () => import('./modules/invoices/invoices.component'),
    loadChildren: () => import('./modules/invoices/invoices.routes'),
  },
  ...invoicesRoutes,
] satisfies Route[]
