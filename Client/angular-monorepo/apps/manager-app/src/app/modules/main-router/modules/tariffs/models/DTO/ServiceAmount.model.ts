import { ServiceType } from "../../../services/models/ServiceType.enum"
import { UnitType } from "../../../services/models/UnitType.enum"

export interface ServiceAmount {
     templateId: string
     name: string
     serviceType: ServiceType
     unitType: UnitType
     amount?: number
}
