import { inject, Injectable } from '@angular/core';
import { HttpService } from '@angular-monorepo/infrastructure';
import { ISearchTariffResponseDTO } from '../DTO/response/ISearchTariffResponseDTO';
import { map } from 'rxjs/operators';
import { ISearchTariffRequestDTO } from '../DTO/request/ISearchTariffRequestDTO';
import { ITariff } from '../models/ITariff';
import { ITariffDTO } from '../DTO/ITariffDTO';
import { catchError, of } from 'rxjs';

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

  getTariff$(tariffTemplateID: string) {
    return this.#httpS.get<ITariffDTO>(`Tariffs/${tariffTemplateID}`)
      .pipe(
        map(tariff => tariff as ITariff),
        catchError(() => of(null))
      );
  }
  getTariffLength$(tariffIDToExclude?: string) {
    const searchParams: ISearchTariffRequestDTO = { take: 0, excludedTemplateIds: tariffIDToExclude ? [tariffIDToExclude] : undefined };
    return this.#httpS.post<ISearchTariffResponseDTO>('Tariffs/Search', searchParams)
      .pipe(
        map(res => res.totalCount)
      );
  }
}

