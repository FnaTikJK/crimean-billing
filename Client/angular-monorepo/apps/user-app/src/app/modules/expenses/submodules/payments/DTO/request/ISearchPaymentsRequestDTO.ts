import { PaymentType } from '../../models/PaymentType';

export interface ISearchPaymentsRequestDTO {
  skip?: number;
  take?: number;
  paymentType?: PaymentType;
  accountId: string;
}
