import { OrderDirection } from "../../../../../shared/models/OrderDirection.enum"
import { AccountType } from "../../../accounts/models/AccountType.enum"
import { ServiceType } from "../ServiceType.enum"
import { UnitType } from "../UnitType.enum"
import { SearchServiceRequestOrderBy } from "./SearchServiceRequestOrderBy.model"
import { SearchFloatQuery } from "../../../../../shared/models/SearchFloatQuery.model"

export interface SearchServicesRequest {
    ids?: string[]
    skip: number
    take: number
    name?: string
    code?: string
    description?: string
    accountType?: AccountType
    serviceType?: ServiceType
    unitType?: UnitType
    isTariffService?: boolean
    price?: Partial<SearchFloatQuery>
    amount?: Partial<SearchFloatQuery>

    orderBy?: SearchServiceRequestOrderBy
    orderDirection?: OrderDirection

    excludedTemplateIds?: string[]
}
