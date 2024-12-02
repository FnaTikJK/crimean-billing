import { OrderDirection } from '../../../../../shared/models/OrderDirection.enum';
import { SearchDateTimeQuery } from '../../../../../shared/models/SearchDateTimeQuery.model';
import { PaymentType } from '../../models/PaymentType.enum';

enum SearchPaymentsOrdering {
  Money,
  DateTime,
}

export interface ISearchPaymentsRequestDTO {
  skip?: number;
  take?: number;
  type?: PaymentType;
  accountId?: string;
  dateTime?: SearchDateTimeQuery;
  ordering?: SearchPaymentsOrdering;
  orderDirection?: OrderDirection;
}
