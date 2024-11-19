import { HttpService } from "@angular-monorepo/infrastructure";
import { inject, Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class AbonentsService {
  httpS = inject(HttpService)

  search$ (skip = 0, take = 100, filters: any) {
    return this.httpS.post('Abonent/Search', {
      ...filters,
      skip,
      take,
    })
  }
}
