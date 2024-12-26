import { ITariffSubscription } from './ITariffSubscription';
import { IServiceUsage } from '../submodules/service/models/IServiceUsage';

export interface ISubscription {
  id: string;
  paymentDate: string;
  tariff: ITariffSubscription;
  actualTariff?: ITariffSubscription;
  preferredTariff?: ITariffSubscription;
  serviceUsages: IServiceUsage[];
}


