import { PaymentType } from '../../models/PaymentType';

export interface ISearchPaymentsRequestDTO {
  skip?: number;
  take?: number;
  type?: PaymentType;
  accountId: string;
}
