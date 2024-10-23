import { IServiceAmountDTO } from '../submodules/service/DTO/iServiceAmountDTO';

export interface ITariffSubscriptionDTO {
  templateId: string;
  name?: string;
  price?: string;
  services?: IServiceAmountDTO[];
}

