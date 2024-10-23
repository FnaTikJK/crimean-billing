import { AccountType } from '../../../../profile/submodules/account/models/AccountType';
import { IServiceAmount } from '../../service/models/IServiceAmount';

export interface ITariff {
  id: string;
  templateId: string;
  code?: string;
  name?: string;
  description?: string;
  accountType: AccountType;
  price: number;
  services?: IServiceAmount[];
}
