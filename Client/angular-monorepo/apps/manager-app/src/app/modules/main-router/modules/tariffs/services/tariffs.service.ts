import { HttpService } from "@angular-monorepo/infrastructure";
import { inject, Injectable } from "@angular/core";
import { SearchTariffsResponse } from "../models/DTO/SearchTariffsResponse.model";

@Injectable({ providedIn: 'root' })
export class TariffsService {
  httpS = inject(HttpService)

  search$(skip = 0, take = 100, filters = {}) {
    return this.httpS.post<SearchTariffsResponse>('Tariffs/Search', {
      ...filters,
      skip,
      take,
    })
  }
}
