import { PaymentType } from '../../models/PaymentType.enum';

export interface ISearchPaymentsRequestDTO {
  skip?: number;
  take?: number;
  type?: PaymentType;
  accountId?: string;
}
