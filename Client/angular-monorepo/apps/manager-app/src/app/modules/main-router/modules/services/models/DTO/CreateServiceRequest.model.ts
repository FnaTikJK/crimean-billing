import { AccountType } from "../../../accounts/models/AccountType.enum"
import { ServiceType } from "../ServiceType.enum"
import { UnitType } from "../UnitType.enum"

export interface CreateServiceRequest {
name:	string | null
code:	string | null
description: string | null
accountType: AccountType,
serviceType: ServiceType
unitType: UnitType
isTariffService: boolean
price: number | null
amount: number | null
}

export const DEFAULT_CREATE_SERVICE_REQUEST: CreateServiceRequest = {
  name: null,
  code: null,
  description: null,
  accountType: AccountType.Sim,
  serviceType: ServiceType.Calls,
  unitType: UnitType.Gb,
  isTariffService: false,
  price: null,
  amount: null,
}
