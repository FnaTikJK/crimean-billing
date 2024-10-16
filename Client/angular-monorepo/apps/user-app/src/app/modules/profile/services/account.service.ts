import { inject, Injectable, signal } from '@angular/core';
import { EntitiesState } from '../../shared/help-entities';
import { HttpService } from '@angular-monorepo/infrastructure';
import { IAddMoneyRequest } from '../DTO/requests/IAddMoneyRequest';
import { IAddMoneyResponse } from '../DTO/responses/IAddMoneyResponse';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  #accountsState = signal<EntitiesState<IAccount>>({ entities: null, loaded: false });
  accountsState = this.#accountsState.asReadonly();
  #httpS = inject(HttpService);


  addMoney$(addMoneyData: IAddMoneyRequest) {
    return this.#httpS.post<IAddMoneyResponse>('Payments/Add', addMoneyData)
      .pipe(
        tap(() => {
          const currentAccounts: IAccount[] = JSON.parse(JSON.stringify(this.#accountsState().entities));
          const updatedAccountIndex = currentAccounts.find(account => account.id === addMoneyData.accountId);
          updatedAccountIndex!.money += addMoneyData.toAdd;
          this.#accountsState.update(state => ({...state, entities: currentAccounts}));
        })
      )
  }
  setAccounts(accounts: IAccount[]) {
    this.#accountsState.set({ entities: accounts, loaded: true });
  }

  getID(accountID: string) {
    return this.accountsState().entities?.find(account => account.id === accountID);
  }
}

export interface IAccount {
  id: string;
  phoneNumber?: string;
  number: number;
  money: number;
  accountType: AccountType;
}

export enum AccountType {
  SIM,
  TV,
  Internet
}
