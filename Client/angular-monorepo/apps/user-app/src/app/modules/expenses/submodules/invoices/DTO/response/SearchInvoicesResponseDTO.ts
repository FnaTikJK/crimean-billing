import { IInvoiceDTO } from '../InvoiceDTO';

export interface ISearchInvoicesResponseDTO {
  totalCount: number;
  items: IInvoiceDTO[];
}
