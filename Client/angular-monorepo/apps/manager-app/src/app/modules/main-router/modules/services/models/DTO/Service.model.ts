import { AccountType } from "../../../accounts/models/AccountType.enum"
import { ServiceType } from "../ServiceType.enum"
import { UnitType } from "../UnitType.enum"

export interface Service {
  id?: string
  templateId: string
  code: string
  name: string
  isTariffService: boolean
  description: string
  accountType: AccountType
  serviceType: ServiceType
  unitType: UnitType

  price?: number
  amount?: number
}
