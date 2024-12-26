import { inject, Injectable } from '@angular/core';
import { HttpService } from '@angular-monorepo/infrastructure';
import { ISearchServiceRequestDTO } from '../DTO/request/ISearchServiceRequestDTO';
import { map } from 'rxjs';
import { ISearchServiceResponseDTO } from '../DTO/response/ISearchServiceResponseDTO';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {

  #httpS = inject(HttpService);

  getServices$(searchOptions: ISearchServiceRequestDTO) {
    return this.#httpS.post<ISearchServiceResponseDTO>('Services/Search', searchOptions);
  }
}

