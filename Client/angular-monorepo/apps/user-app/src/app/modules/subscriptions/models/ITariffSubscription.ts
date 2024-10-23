import { IServiceAmountDTO } from '../submodules/service/DTO/iServiceAmountDTO';

export interface ITariffSubscription {
  templateId: string;
  name?: string;
  price?: string;
  services?: IServiceAmountDTO[];
}

