import { computed, inject, Injectable, signal } from '@angular/core';
import { EntityState } from '../../shared/help-entities';
import { HttpService } from '@angular-monorepo/infrastructure';
import { ISubscriptionsResponseDTO } from '../DTO/response/ISubscriptionsResponseDTO';
import { catchError, filter, forkJoin, map, of, switchMap, tap } from 'rxjs';
import { AppSettingsService } from '../../shared/services/app-settings.service';
import { ISubscription } from '../models/ISubscription';
import { ISubscribeRequestDTO } from '../DTO/request/ISubscribeRequestDTO';
import { toObservable } from '@angular/core/rxjs-interop';
import { SubscriptionMapper } from '../mappers/subscription.mapper';
import { IServiceUsageDTO } from '../submodules/service/DTO/IServiceUsageDTO';

@Injectable({
  providedIn: 'root'
})
export class SubscriptionService {

  #httpS = inject(HttpService);
  #appSettingsS = inject(AppSettingsService);
  #subscriptionMapper = inject(SubscriptionMapper);

  #subscriptionState = signal<EntityState<ISubscription>>({ entity: null, loaded: false });
  subscriptionState = this.#subscriptionState.asReadonly();

  constructor() {
    this.listenForSelectedAccountChange();
  }

  changeSubscription$(subscribeRequest: ISubscribeRequestDTO) {
    return this.#httpS.post<ISubscriptionsResponseDTO>('Subscriptions/Subscribe', subscribeRequest)
      .pipe(
        map((res) => this.#subscriptionMapper.fromDTO(res)),
        tap(res => {
          this.#subscriptionState.update(state => (
            {...state, entity: {...state.entity as ISubscription, ...res}}))
        })
      );
  }

  private getAccountSubscription$(accountID: string) {
    return this.#httpS.get<ISubscriptionsResponseDTO | null>(`Subscriptions?AccountId=${accountID}`)
      .pipe(
        map((res) => res ? this.#subscriptionMapper.fromDTO(res) : res),
        catchError(() => of(null))
      );
  }

  public addServicesToSubscription$(servicesIDs: string[]) {
    const currentAccount = this.#appSettingsS.appSettingsState().entity!.accountSelected;
    const addReqs = servicesIDs
      .map(serviceID => this.#httpS.post<IServiceUsageDTO>('Subscriptions/Services', {
        accountId: currentAccount,
        serviceId: serviceID
      }));
    return forkJoin(addReqs)
      .pipe(
        tap(newServices => {
          const currentEntityState = this.subscriptionState().entity as ISubscription;
          const updatedSubscriptionEntity: ISubscription = {...currentEntityState, serviceUsages: [...currentEntityState.serviceUsages, ...newServices]}
          this.#subscriptionState.update(subscription => ({...subscription, entity: updatedSubscriptionEntity }))
        })
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
