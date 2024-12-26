import { ITariffSubscriptionDTO } from '../ITariffSubscriptionDTO';
import { IServiceUsageDTO } from '../../submodules/service/DTO/IServiceUsageDTO';

export interface ISubscriptionsResponseDTO {
  id: string;
  paymentDate: string;
  tariff: ITariffSubscriptionDTO;
  actualTariff?: ITariffSubscriptionDTO;
  preferredTariff?: ITariffSubscriptionDTO;
  serviceUsages: IServiceUsageDTO[];
}
