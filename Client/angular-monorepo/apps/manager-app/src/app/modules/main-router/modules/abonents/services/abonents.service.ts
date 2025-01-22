import { HttpService } from "@angular-monorepo/infrastructure";
import { inject, Injectable } from "@angular/core";
import { get } from "lodash";
import { map } from "rxjs";
import { SearchResponse } from "../../../../shared/models/SearchResponse.model";
import { User } from "../models/User.model";

@Injectable({ providedIn: 'root' })
export class AbonentsService {
  httpS = inject(HttpService)

  search$ (skip = 0, take = 100, filters: any) {
    return this.httpS.post<SearchResponse<User & { phoneNumber?: string }>>('Users/Search', {
      ...filters,
      skip,
      take,
    }).pipe(
      map((result: any) => {
        result.items = result.items.map(abonent => {
          abonent.phoneNumber = get(abonent, ['accounts', 0, 'phoneNumber'], null)
          return abonent
        })
        return result
      })
    )
  }
}
