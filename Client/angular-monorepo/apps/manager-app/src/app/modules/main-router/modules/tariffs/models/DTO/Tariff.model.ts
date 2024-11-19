import { AccountType } from "../../../accounts/models/AccountType.enum"
import { ServiceAmount } from "./ServiceAmount.model"

export interface Tariff {
  id: string
  templateId: string
  code: string
  name: string
  description: string
  accountType: AccountType
  price: number
  services: ServiceAmount
}
