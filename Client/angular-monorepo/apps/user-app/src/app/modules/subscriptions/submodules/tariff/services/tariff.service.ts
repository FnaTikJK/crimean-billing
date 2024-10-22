import { inject, Injectable } from '@angular/core';
import { HttpService } from '@angular-monorepo/infrastructure';
import { ISearchTariffResponseDTO } from '../DTO/response/ISearchTariffResponseDTO';
import { map } from 'rxjs/operators';
import { ISearchTariffRequestDTO } from '../DTO/request/ISearchTariffRequestDTO';
import { ITariff } from '../models/ITariff';

@Injectable({
  providedIn: 'root'
})
export class TariffService {

  #httpS = inject(HttpService);

  getTariffs$(searchOptions: ISearchTariffRequestDTO) {
    return this.#httpS.post<ISearchTariffResponseDTO>('Tariffs/Search', searchOptions)
      .pipe(
        map(res => res.items as ITariff[])
      );
  }
  getTariffLength$() {
    return this.#httpS.post<ISearchTariffResponseDTO>('Tariffs/Search', { take: 0 })
      .pipe(
        map(res => res.totalCount)
      );
  }
}

