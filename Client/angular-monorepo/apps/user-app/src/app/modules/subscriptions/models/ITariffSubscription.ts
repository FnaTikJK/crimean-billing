import { IServiceAmountDTO } from '../submodules/service/DTO/iServiceAmountDTO';

export interface ITariffSubscription {
  templateID: string;
  name?: string;
  price?: string;
  services?: IServiceAmountDTO[];
}
