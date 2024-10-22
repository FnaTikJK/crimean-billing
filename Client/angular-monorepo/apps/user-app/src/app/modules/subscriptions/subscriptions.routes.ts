import { Route } from '@angular/router';

export default [
  { path: '', pathMatch: 'full', title: 'Услуги', loadComponent: () => import('./subscription.component') },
  { path: 'change-tariff', title: 'Смена тарифа', loadComponent: () => import('./submodules/tariff/components/change-tariff/change-tariff.component') },
  { path: 'add-tariff', title: 'Выбор тарифа', loadComponent: () => import('./submodules/tariff/components/add-tariff/add-tariff.component') },
  { path: 'tariff/:id', title: 'Тариф', loadComponent: () => import('./submodules/tariff/components/tariff/tariff.component') }
] satisfies Route[];
