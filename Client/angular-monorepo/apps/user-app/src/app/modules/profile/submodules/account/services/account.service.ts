import { inject, Injectable, signal } from '@angular/core';
import { EntitiesState } from '../../../../shared/help-entities';
import { HttpService } from '@angular-monorepo/infrastructure';
import { IAddMoneyRequestDTO } from '../../../DTO/request/IAddMoneyRequestDTO';
import { IAddMoneyResponseDTO } from '../../../DTO/response/IAddMoneyResponseDTO';
import { tap } from 'rxjs';
import { IAccount } from '../models/IAccount';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  #accountsState = signal<EntitiesState<IAccount>>({ entities: null, loaded: false });
  accountsState = this.#accountsState.asReadonly();
  #httpS = inject(HttpService);


  addMoney$(addMoneyData: IAddMoneyRequestDTO) {
    return this.#httpS.post<IAddMoneyResponseDTO>('Payments/Add', addMoneyData)
      .pipe(
        tap(() => this.addMoney(addMoneyData.accountId, addMoneyData.toAdd))
      );
  }

  addMoney(accountID: string, amount: number) {
    const currentAccounts: IAccount[] = JSON.parse(JSON.stringify(this.#accountsState().entities));
    const updatedAccountIndex = currentAccounts.find(account => account.id === accountID);
    updatedAccountIndex!.money += amount;
    this.#accountsState.update(state => ({...state, entities: currentAccounts}));
  }

  spendMoney(accountID: string, amount: number) {
    const currentAccounts: IAccount[] = JSON.parse(JSON.stringify(this.#accountsState().entities));
    const updatedAccountIndex = currentAccounts.find(account => account.id === accountID);
    updatedAccountIndex!.money += amount;
    this.#accountsState.update(state => ({...state, entities: currentAccounts}));
  }

  setAccounts(accounts: IAccount[]) {
    this.#accountsState.set({ entities: accounts, loaded: true });
  }

  getID(accountID: string) {
    return this.accountsState().entities?.find(account => account.id === accountID);
  }
}
