import { computed, inject, Injectable, signal } from '@angular/core';
import { EntityState } from '../../shared/help-entities';
import { HttpService } from '@angular-monorepo/infrastructure';
import { ISubscriptionsResponseDTO } from '../DTO/response/ISubscriptionsResponseDTO';
import { catchError, filter, of, switchMap, tap } from 'rxjs';
import { AppSettingsService } from '../../shared/services/app-settings.service';
import { ISubscription } from '../models/ISubscription';
import { ISubscribeRequestDTO } from '../DTO/request/ISubscribeRequestDTO';
import { toObservable } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService {

  #httpS = inject(HttpService);
  #appSettingsS = inject(AppSettingsService);

  #subscriptionState = signal<EntityState<ISubscription>>({ entity: null, loaded: false });
  subscriptionState = this.#subscriptionState.asReadonly();

  constructor() {
    this.listenForSelectedAccountChange();
  }

  changeSubscription$(subscribeRequest: ISubscribeRequestDTO) {
    return this.#httpS.post<ISubscriptionsResponseDTO>('Subscriptions/Subscribe', subscribeRequest)
      .pipe(
        tap(res => {
          this.#subscriptionState.update(state => (
            {...state, entity: {...state.entity as ISubscription, tariff: res.tariff, preferredTariff: res.preferredTariff}}))
        })
      );
  }
  private getAccountSubscription$(accountID: string) {
    return this.#httpS.get<ISubscriptionsResponseDTO | null>(`Subscriptions?AccountId=${accountID}`)
      .pipe(
        catchError(() => of(null))
      );
  }

  private listenForSelectedAccountChange() {
    const accountChangeSignal = computed(() => this.#appSettingsS.appSettingsState().entity?.accountSelected);

    toObservable(accountChangeSignal)
      .pipe(
        filter(accountID => !!accountID),
        tap(() => this.#subscriptionState.update(state => ({...state, loaded: false}))),
        switchMap((accountID) => this.getAccountSubscription$(accountID as string)),
        tap((subscription) => this.#subscriptionState.set({ entity: subscription, loaded: true })),
      ).subscribe();
  }
}
