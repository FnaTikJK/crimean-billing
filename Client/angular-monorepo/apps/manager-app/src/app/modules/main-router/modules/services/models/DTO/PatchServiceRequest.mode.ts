import { AccountType } from "../../../accounts/models/AccountType.enum";
import { ServiceType } from "../ServiceType.enum";
import { UnitType } from "../UnitType.enum";

export interface PatchServiceRequest {
    templateId: string
    name?: string
    code?: string
    description?: string
    accountType?: AccountType
    serviceType?: ServiceType,
    unitType?: UnitType,
    isTariffService?: boolean
    price?: number
    amount?: number
    needKillService?: boolean
}
