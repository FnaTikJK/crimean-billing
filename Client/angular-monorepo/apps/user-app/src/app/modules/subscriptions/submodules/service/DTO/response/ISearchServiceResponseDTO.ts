import { IServiceDTO } from '../IServiceDTO';


export interface ISearchServiceResponseDTO {
  totalCount: number;
  items: IServiceDTO[];
}
