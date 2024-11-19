
export interface IInvoice {
  id: string;
  toPay: number;
  createdAt: string;
  payedAt?: string;
  accountId: string;
}
