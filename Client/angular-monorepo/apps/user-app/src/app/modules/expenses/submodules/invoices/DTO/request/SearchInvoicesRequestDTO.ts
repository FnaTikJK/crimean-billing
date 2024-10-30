
export interface ISearchInvoicesRequestDTO {
  accountID: string;
  isPayed?: boolean;
  skip?: number;
  take?: number;
}
