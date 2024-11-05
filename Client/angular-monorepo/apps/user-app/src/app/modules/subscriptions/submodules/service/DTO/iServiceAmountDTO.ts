import { ServiceType } from '../models/ServiceType';
import { UnitType } from '../models/UnitType';

export interface IServiceAmountDTO {
  templateId: string;
  name?: string;
  serviceType: ServiceType;
  unitType: UnitType;
  amount?: number;
  spent?: number;
}
