import { UnitType } from './UnitType';

export interface IServiceUsage {
  id: string;
  serviceId: string;
  serviceTemplateId: string;
  subscribedAt: string;
  spent: number;
  amount: number;
  serviceName: string;
  serviceUnitType: UnitType;
  price: number;
  serviceType: number;
}
