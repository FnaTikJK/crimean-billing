import { Route } from '@angular/router';
import { userWithoutSubscriptionCanActivateFn, userWithSubscriptionCanActivateFn } from './guards/subscription-guards';

export default [
  { path: '', pathMatch: 'full', title: 'Услуги', loadComponent: () => import('./subscription.component') },

  {
    path: 'change-tariff',
    title: 'Смена тарифа',
    loadComponent: () => import('./submodules/tariff/components/change-tariff/change-tariff.component'),
    canActivate: [userWithSubscriptionCanActivateFn]
  },

  {
    path: 'add-tariff',
    title: 'Выбор тарифа',
    loadComponent: () => import('./submodules/tariff/components/add-tariff/add-tariff.component'),
    canActivate: [userWithoutSubscriptionCanActivateFn]
  },

  {
    path: 'pick-tariff/:id',
    title: 'Подключение тарифа',
    loadComponent: () => import('./submodules/tariff/components/pick-tariff/pick-tariff.component'),
    canActivate: [userWithoutSubscriptionCanActivateFn]
  },

  {
    path: 'compare-tariff/:id',
    title: 'Сравнение тарифов',
    loadComponent: () => import('./submodules/tariff/components/compare-tariff/compare-tariff.component'),
    canActivate: [userWithSubscriptionCanActivateFn]
  }
] satisfies Route[];

