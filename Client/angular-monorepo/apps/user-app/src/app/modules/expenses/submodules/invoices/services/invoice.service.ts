import { inject, Injectable } from '@angular/core';
import { HttpService } from '@angular-monorepo/infrastructure';
import { map } from 'rxjs/operators';
import { ISearchInvoicesRequestDTO } from '../DTO/request/SearchInvoicesRequestDTO';
import { ISearchInvoicesResponseDTO } from '../DTO/response/SearchInvoicesResponseDTO';
import { IInvoice } from '../models/Invoice';
import { IPayInvoiceRequestDTO } from '../DTO/request/PayInvoiceRequestDTO';
import { IInvoiceDTO } from '../DTO/InvoiceDTO';
import { AccountService } from '../../../../profile/submodules/account/services/account.service';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  #httpS = inject(HttpService);
  #accountS = inject(AccountService);

  getInvoices$(searchOptions: ISearchInvoicesRequestDTO) {
    return this.#httpS.post<ISearchInvoicesResponseDTO>('Invoices/Search', searchOptions)
      .pipe(
        map(res => res.items as IInvoice[])
      );
  }

  payForInvoice$(payInvoiceRequest: IPayInvoiceRequestDTO, accountId: string) {
    return this.#httpS.post<IInvoiceDTO>('Invoices/Pay', payInvoiceRequest)
      .pipe(
        tap((payedInvoice) => this.#accountS.spendMoney(accountId, payedInvoice.toPay)),
        map((payedInvoice) => payedInvoice as IInvoice)
      );
  }
}
