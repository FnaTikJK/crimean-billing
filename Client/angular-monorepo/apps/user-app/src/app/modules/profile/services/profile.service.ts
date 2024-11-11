import { effect, EffectRef, inject, Injectable, signal } from '@angular/core';
import { catchError, map, of, tap } from 'rxjs';
import { HttpService } from '@angular-monorepo/infrastructure';
import { EntityState } from '../../shared/help-entities';
import { AccountService } from '../submodules/account/services/account.service';
import { IAccount } from '../submodules/account/models/IAccount';
import { IProfile } from '../models/IProfile';
import { IUserDTO } from '../DTO/IUserDTO';
import { IPatchUserRequestDTO } from '../DTO/request/IPatchUserRequestDTO';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  #httpS = inject(HttpService);
  #accountS = inject(AccountService);

  constructor() {
    this.setupProfile$().subscribe();
    this.listenForAccountsUpdate();
  }

  #profileState = signal<EntityState<IProfile, true>>({ entity: null, loaded: false, checked: false });
  profileState = this.#profileState.asReadonly();

  #accountUpdateEffect?: EffectRef;

  setProfile(profile: IProfile) {
    this.#profileState.set({ entity: profile, loaded: true, checked: true });
    this.#accountS.setAccounts(profile.accounts);
  }

  getProfile$() {
    return this.#httpS.get<IUserDTO>('Users/My')
      .pipe(
        map((userDTO) => userDTO as IProfile)
      );
  }

  patchProfile$(pathUserRequest: IPatchUserRequestDTO) {
    return this.#httpS.patch<IUserDTO>('Users', pathUserRequest)
      .pipe(
        map((userDTO) => userDTO as IProfile),
        tap((updatedProfile) => this.setProfile(updatedProfile))
      );
  }

  setupProfile$() {
    return this.getProfile$()
      .pipe(
        tap({
          next: (profile) => this.setProfile(profile)
        }),
        catchError(() => {
          this.#profileState.update(state => ({...state, checked: true}));
          return of(null);
        })
      );
  }

  private listenForAccountsUpdate() {
    this.#accountUpdateEffect = effect(() => {
      const accountsState = this.#accountS.accountsState();
      if (accountsState.loaded) {
        this.#profileState.update(state => ({
          ...state, entity: {...state.entity as IProfile, accounts: accountsState.entities as IAccount[] }
        }))
      }
    }, { allowSignalWrites: true })
  }
}



