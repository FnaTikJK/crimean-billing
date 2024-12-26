import { inject, Injectable } from '@angular/core';
import { IAddMoneyRequestDTO } from '../DTO/request/IAddMoneyRequestDTO';
import { IPaymentResponseDTO } from '../DTO/response/IPaymentResponseDTO';
import { tap } from 'rxjs';
import { HttpService } from '@angular-monorepo/infrastructure';
import { AccountService } from '../../../../profile/submodules/account/services/account.service';
import { ISpendMoneyRequest } from '../DTO/request/ISpendMoneyRequest';
import { ISearchPaymentsRequestDTO } from '../DTO/request/ISearchPaymentsRequestDTO';
import { ISearchPaymentsResponseDTO } from '../DTO/response/ISearchPaymentsResponseDTO';

@Injectable({
  providedIn: 'root'
})
export class PaymentsService {

  #httpS = inject(HttpService);
  #accountS = inject(AccountService);

  getPayments$(searchOptions: ISearchPaymentsRequestDTO) {
    return this.#httpS.post<ISearchPaymentsResponseDTO>('Payments/Search', searchOptions);
  }

  addMoney$(addMoneyData: IAddMoneyRequestDTO) {
    return this.#httpS.post<IPaymentResponseDTO>('Payments/Add', addMoneyData)
      .pipe(
        tap((res) => this.#accountS.setMoney(addMoneyData.accountId, res.remainedMoney))
      );
  }

    spendMoney$(spendMoneyData: ISpendMoneyRequest) {
      return this.#httpS.post<IPaymentResponseDTO>('Payments/Spend', spendMoneyData)
        .pipe(
          tap((res) => this.#accountS.setMoney(spendMoneyData.accountId, res.remainedMoney))
        );
    }
}
