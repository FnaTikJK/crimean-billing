export interface IInvoiceDTO {
  id: string;
  toPay: number;
  createdAt: string;
  payedAt?: string;
  accountId: string;
}
