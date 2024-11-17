import { HttpService } from '@angular-monorepo/infrastructure';
import { inject, Injectable, signal } from '@angular/core';
import { SearchServicesRequest } from '../models/DTO/SearchServicesRequest.model';
import { SearchServiceResponse } from '../models/DTO/SearchServiceResponse.model';
import { SearchServiceRequestOrderBy } from '../models/DTO/SearchServiceRequestOrderBy.model';

@Injectable({
  providedIn: 'root'
})
export class ServicesService {
  httpS = inject(HttpService)

  // create$() {

  // }

  // patch$() {
  //   {
  //     "templateId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  //       "name": "string",
  //         "code": "string",
  //           "description": "string",
  //             "accountType": 0,
  //               "serviceType": 0,
  //                 "unitType": 0,
  //                   "isTariffService": true,
  //                     "price": 0,
  //                       "amount": 0,
  //                         "needKillService": false
  //   }
  // }

  // delete$() {
  //   "needKillService": false
  // }

  search$(skip = 0, take = 100, filters: Omit<SearchServicesRequest, 'skip' | 'take'> = {}) {
    return this.httpS.post<SearchServiceResponse>('Services/Search', {
      skip,
      take,
      ...filters,
      orderBy: SearchServiceRequestOrderBy.Code
    })
  }
}
