import { PaymentType } from '../models/PaymentType';
import { IInvoiceDTO } from '../../invoices/DTO/InvoiceDTO';

export interface IPaymentPayerOwnDTO {
  id: string;
  money: number;
  type: PaymentType;
  dateTime: string;
  invoice?: IInvoiceDTO;
}
