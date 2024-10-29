
export interface IInvoiceDTO {
  id: string;
  toPay: number;
  createdAt: string;
  payedAt?: string;
}
