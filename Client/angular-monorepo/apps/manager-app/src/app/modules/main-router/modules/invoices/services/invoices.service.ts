import { inject, Injectable } from "@angular/core";
import { SearchInvoicesRequest } from "../DTO/SearchInvoicesRequest.model";
import { HttpService } from "@angular-monorepo/infrastructure";
import { SearchInvoicesResponse } from "../DTO/SearchInvoicesResponse.model";

@Injectable({ providedIn: 'root' })
export class InvoicesService {
  httpS = inject(HttpService)

  search$(skip: number, take: number, filter: Omit<SearchInvoicesRequest, 'skip' | 'take'>) {
    return this.httpS.post<SearchInvoicesResponse>('Invoices/Search', {
      ...filter,
      skip,
      take,
    })
  }
}
