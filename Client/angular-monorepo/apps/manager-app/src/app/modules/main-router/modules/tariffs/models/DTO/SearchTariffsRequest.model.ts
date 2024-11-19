import { SearchFloatQuery } from "../../../../../shared/models/SearchFloatQuery.model"
import { AccountType } from "../../../accounts/models/AccountType.enum"
import { ServiceType } from "../../../services/models/ServiceType.enum"
import { UnitType } from "../../../services/models/UnitType.enum"

export interface SearchTariffsRequest {
  code?: string
  name?: string
  description?: string
  accountType?: AccountType

  price?: SearchFloatQuery
  servicesAmounts?: SearchServiceQuery

  excludedTemplateIds?: string[]

  skip: number,
  take: number,
}

export interface SearchServiceQuery {
  serviceType?: ServiceType
  unitType?: UnitType
  Amount?: SearchFloatQuery
}
