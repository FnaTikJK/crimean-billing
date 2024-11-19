import { OrderDirection } from "../../../../../shared/models/OrderDirection.enum"
import { SearchFloatQuery } from "../../../../../shared/models/SearchFloatQuery.model"
import { AccountType } from "../AccountType.enum"

export interface SearchAccountsRequest
{
    userId?: string
    phoneNumber?: string
    number?: string
    money?: SearchFloatQuery
    accountType?: AccountType

    skip: number,
    take: number,

    orderBy?: string,
    orderDir?: OrderDirection,
}
