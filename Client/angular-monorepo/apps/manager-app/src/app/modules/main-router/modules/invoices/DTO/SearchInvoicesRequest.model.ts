import { OrderDirection } from "../../../../shared/models/OrderDirection.enum";
import { SearchDateTimeQuery } from "../../../../shared/models/SearchDateTimeQuery.model";
import { SearchFloatQuery } from "../../../../shared/models/SearchFloatQuery.model";

export interface SearchInvoicesRequest {
  accountId: string;
  toPay: SearchFloatQuery;
  createdAt: SearchDateTimeQuery;
  payedAt: SearchDateTimeQuery;
  ordering: SearchInvoicesOrdering;
  direction: OrderDirection;

  skip: number,
  take: number,

  orderBy?: string,
  orderDir?: OrderDirection,
}

export enum SearchInvoicesOrdering {
  ToPay,
  CreatedAt,
  PayedAt,
}
