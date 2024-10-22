import { AccountType } from '../../../../profile/submodules/account/models/AccountType';
import { IServiceAmountDTO } from '../../service/DTO/iServiceAmountDTO';

export interface ITariffDTO {
  id: string;
  templateId: string;
  code?: string;
  name?: string;
  description?: string;
  accountType: AccountType;
  price: number;
  services?: IServiceAmountDTO[];
}
