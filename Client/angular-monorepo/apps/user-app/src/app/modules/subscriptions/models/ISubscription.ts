import { ITariffSubscription } from './ITariffSubscription';

export interface ISubscription {
  id: string;
  paymentDate: string;
  tariff: ITariffSubscription;
  preferredTariff?: ITariffSubscription;
}

