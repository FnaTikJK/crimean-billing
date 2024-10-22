import { IServiceAmountDTO } from '../submodules/service/DTO/iServiceAmountDTO';

export interface ITariffSubscriptionDTO {
  templateID: string;
  name?: string;
  price?: string;
  services?: IServiceAmountDTO[];
}
