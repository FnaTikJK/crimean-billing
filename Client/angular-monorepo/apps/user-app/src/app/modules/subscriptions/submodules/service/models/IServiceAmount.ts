import { ServiceType } from './ServiceType';
import { UnitType } from './UnitType';

export interface IServiceAmount {
  templateId: string;
  name?: string;
  serviceType: ServiceType;
  unitType: UnitType;
  amount?: number;
  spent?: number;
}
