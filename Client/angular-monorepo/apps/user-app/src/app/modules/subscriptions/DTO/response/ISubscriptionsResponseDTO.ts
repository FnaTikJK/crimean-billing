import { ITariffSubscriptionDTO } from '../ITariffSubscriptionDTO';

export interface ISubscriptionsResponseDTO {
  id: string;
  paymentDate: string;
  tariff: ITariffSubscriptionDTO;
  preferredTariff?: ITariffSubscriptionDTO;
}
