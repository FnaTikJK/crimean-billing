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
  },

  {
    path: 'used-services',
    title: 'Подключённые услуги',
    loadComponent: () => import('./submodules/service/components/used-services/used-services.component'),
    canActivate: [userWithSubscriptionCanActivateFn]
  },

  {
    path: 'add-services',
    title: 'Подключить услугу',
    loadComponent: () => import('./submodules/service/components/add-services/add-services.component'),
    canActivate: [userWithSubscriptionCanActivateFn]
  }
] satisfies Route[];

