import { effect, inject, Injectable, signal } from '@angular/core';
import { EntityState } from '../help-entities';
import { ProfileService } from '../../profile/services/profile.service';

@Injectable({
  providedIn: 'root'
})
export class AppSettingsService {
  #profileS = inject(ProfileService);

  #appSettingsState = signal<EntityState<IAppSettings, true>>({ entity: null, loaded: false, checked: false });
  appSettingsState = this.#appSettingsState.asReadonly();

  constructor() {
    this.listenForAccountsInit();
  }

  selectAccount(accountID: string) {
    this.#appSettingsState.update(appSettings => ({...appSettings, entity: { accountSelected: accountID } }));
  }
  private listenForAccountsInit() {
    const profileInitEffect = effect(() => {
      const profileState = this.#profileS.profileState();
      if (profileState.checked) {
        const firstAccountID = profileState.entity?.accounts[0].id;
        if (firstAccountID) {
          this.#appSettingsState.set({ entity: { accountSelected: firstAccountID }, loaded: true, checked: true });
        } else {
          this.#appSettingsState.update(appSettings => ({...appSettings, checked: true}));
        }
        profileInitEffect.destroy();
      }
    }, { allowSignalWrites: true });
  }
}

export interface IAppSettings {
  accountSelected?: string;
}
