import { PaymentType } from './PaymentType';
import { IInvoice } from '../../invoices/models/Invoice';

export interface IPaymentPayerOwn {
  id: string;
  money: number;
  type: PaymentType;
  dateTime: string;
  invoice?: IInvoice;
}
