import { IPaymentPayerOwnDTO } from '../PaymentPayerOwnDTO';

export interface ISearchPaymentsResponseDTO {
  totalCount: number;
  items: IPaymentPayerOwnDTO[];
}
