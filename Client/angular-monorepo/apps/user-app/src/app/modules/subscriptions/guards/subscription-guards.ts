import { CanActivateFn, Router } from '@angular/router';
import { inject, Injector } from '@angular/core';
import { filter, map, switchMap, take, timer } from 'rxjs';
import { SubscriptionService } from '../services/subscription.service';
import { toObservable } from '@angular/core/rxjs-interop';

export const userWithSubscriptionCanActivateFn: CanActivateFn = () => {
  const subscriptionS = inject(SubscriptionService);
  const router = inject(Router);
  const injector = inject(Injector);

  return timer(0)
    .pipe(
      switchMap(() => toObservable(subscriptionS.subscriptionState, {injector: injector}) ),
      filter(state => state.loaded),
      map(state => {
        if (state.entity) { return true; }
        router.navigate(['subscriptions', 'add-tariff'], { relativeTo: null });
        return false;
      }),
      take(1)
    )
};

export const userWithoutSubscriptionCanActivateFn: CanActivateFn = () => {
  const subscriptionS = inject(SubscriptionService);
  const router = inject(Router);
  const injector = inject(Injector);

  return timer(0)
    .pipe(
      switchMap(() => toObservable(subscriptionS.subscriptionState, {injector: injector})),
      filter(state => state.loaded),
      map(state => {
        if (!state.entity) { return true; }
        router.navigate(['subscriptions', 'change-tariff'], { relativeTo: null });
        return false;
      }),
      take(1)
    );
};
