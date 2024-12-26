import { AccountType } from '../../../../profile/submodules/account/models/AccountType';
import { ServiceType } from '../models/ServiceType';
import { UnitType } from '../models/UnitType';

export interface IServiceDTO {
  id: string;
  templateId: string;
  code: string;
  name: string;
  isTariffService: boolean;
  description: string;
  createdAt: string;
  updatedAt: string;
  accountType: AccountType;
  serviceType: ServiceType;
  unitType: UnitType;
  price: number;
  amount: number;
}
