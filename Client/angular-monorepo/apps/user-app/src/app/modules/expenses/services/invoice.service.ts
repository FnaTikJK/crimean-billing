import { inject, Injectable } from '@angular/core';
import { HttpService } from '@angular-monorepo/infrastructure';
import { map } from 'rxjs/operators';
import { ISearchInvoicesRequestDTO } from '../submodules/invoices/DTO/request/SearchInvoicesRequestDTO';
import { ISearchInvoicesResponseDTO } from '../submodules/invoices/DTO/response/SearchInvoicesResponseDTO';
import { IInvoice } from '../submodules/invoices/models/Invoice';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  #httpS = inject(HttpService);

  getInvoices$(searchOptions: ISearchInvoicesRequestDTO) {
    return this.#httpS.post<ISearchInvoicesResponseDTO>('Invoices/Search', searchOptions)
      .pipe(
        map(res => res.items as IInvoice[])
      );
  }
}
