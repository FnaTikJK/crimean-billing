import { inject, Injectable } from "@angular/core";
import { SearchAccountsRequest } from "../models/DTO/SearchAccountsRequest.model";
import { HttpService } from "@angular-monorepo/infrastructure";
import { SearchAccountsResponse } from "../models/DTO/SearchAccountsResponse.model";

@Injectable({ providedIn: 'root' })
export class AccountsService {
  httpS = inject(HttpService)

  search$ (skip: number, take: number, filter: Omit<SearchAccountsRequest, 'skip' | 'take'>) {
    return this.httpS.post<SearchAccountsResponse>('Accounts/Search', {
      ...filter,
      skip,
      take,
    })
  }
}
