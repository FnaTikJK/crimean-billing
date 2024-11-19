import { HttpService } from '@angular-monorepo/infrastructure';
import { inject, Injectable, signal } from '@angular/core';
import { ISearchPaymentsRequestDTO } from '../DTO/request/SearchPaymentsRequestDTO';
import { ISearchPaymentsResponseDTO } from '../DTO/response/SearchPaymentsResponseDTO';

@Injectable({
  providedIn: 'root'
})
export class PaymentsService {
  httpS = inject(HttpService)


  search$(skip = 0, take = 100, filters: Omit<ISearchPaymentsRequestDTO, 'skip' | 'take'> = {}) {
    return this.httpS.post<ISearchPaymentsResponseDTO>('Payments/Search', {
      skip,
      take,
      ...filters,
    })
  }
}
