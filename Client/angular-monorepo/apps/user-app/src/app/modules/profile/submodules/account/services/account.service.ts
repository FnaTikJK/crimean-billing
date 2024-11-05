import { Injectable, signal } from '@angular/core';
import { EntitiesState } from '../../../../shared/help-entities';
import { IAccount } from '../models/IAccount';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  #accountsState = signal<EntitiesState<IAccount>>({ entities: null, loaded: false });
  accountsState = this.#accountsState.asReadonly();

  setMoney(accountID: string, newAmount: number) {
    const currentAccounts: IAccount[] = JSON.parse(JSON.stringify(this.#accountsState().entities));
    const updatedAccountIndex = currentAccounts.find(account => account.id === accountID);
    updatedAccountIndex!.money = newAmount;
    this.#accountsState.update(state => ({...state, entities: currentAccounts}));
  }

  spendMoney(accountID: string, amount: number) {
    const currentAccounts: IAccount[] = JSON.parse(JSON.stringify(this.#accountsState().entities));
    const updatedAccountIndex = currentAccounts.find(account => account.id === accountID);
    updatedAccountIndex!.money -= amount;
    this.#accountsState.update(state => ({...state, entities: currentAccounts}));
  }

  setAccounts(accounts: IAccount[]) {
    this.#accountsState.set({ entities: accounts, loaded: true });
  }

  getID(accountID: string) {
    return this.accountsState().entities?.find(account => account.id === accountID);
  }
}

