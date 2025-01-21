import { inject, Injectable } from "@angular/core";
import { HttpService } from "@angular-monorepo/infrastructure";
import { RegisterManagerRequest } from "../models/RegisterManagerRequest.model";
import { SearchManagersResponse } from "../models/SearchManagersResponse.model";
import { SearchManagersRequest } from '../models/SearchManagersRequest.model';
import { RegisterManagerResponse } from "../models/RegisterManagerResponse.model";

@Injectable({ providedIn: 'root' })
export class ManagersService {
  httpS = inject(HttpService)

  search$(skip: number, take: number, filter: Omit<SearchManagersRequest, 'skip' | 'take'>) {
    return this.httpS.post<SearchManagersResponse>('Managers/Search', {
      ...filter,
      skip,
      take,
    })
  }

  register$({login, password, fio}: RegisterManagerRequest) {
    return this.httpS.post<RegisterManagerResponse>('Auth/Managers/Register', {
      login,
      password,
      fio
    })
  }
}
