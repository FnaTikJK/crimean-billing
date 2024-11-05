import { IPaymentPayerOwnDTO } from '../IPaymentPayerOwnDTO';

export interface ISearchPaymentsResponseDTO {
  totalCount: number;
  items: IPaymentPayerOwnDTO[];
}
