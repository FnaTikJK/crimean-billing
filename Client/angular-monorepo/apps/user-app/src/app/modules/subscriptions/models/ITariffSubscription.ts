import { IServiceAmount } from '../submodules/service/models/IServiceAmount';

export interface ITariffSubscription {
  templateId: string;
  name?: string;
  price?: string;
  services?: IServiceAmount[];
}

