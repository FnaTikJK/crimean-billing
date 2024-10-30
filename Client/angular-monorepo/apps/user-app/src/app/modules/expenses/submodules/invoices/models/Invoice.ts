
export interface IInvoice {
  id: string;
  toPay: number;
  createdAt: string;
  payedAt?: string;
}
