import { UnitType } from '../models/UnitType';

export interface IServiceUsageDTO {
  id: string;
  subscribedAt: string;
  spent: number;
  amount: number;
  serviceId: string;
  serviceName: string;
  serviceUnitType: UnitType;
}
