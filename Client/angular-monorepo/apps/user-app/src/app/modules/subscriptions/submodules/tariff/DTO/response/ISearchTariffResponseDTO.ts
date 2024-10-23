import { ITariffDTO } from '../ITariffDTO';

export interface ISearchTariffResponseDTO {
  totalCount: number;
  items: ITariffDTO[];
}
