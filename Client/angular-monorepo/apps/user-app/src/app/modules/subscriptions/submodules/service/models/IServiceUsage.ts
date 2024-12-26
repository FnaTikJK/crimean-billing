import { UnitType } from './UnitType';

export interface IServiceUsage {
  id: string;
  subscribedAt: string;
  spent: number;
  amount: number;
  serviceId: string;
  serviceName: string;
  serviceUnitType: UnitType;
}
