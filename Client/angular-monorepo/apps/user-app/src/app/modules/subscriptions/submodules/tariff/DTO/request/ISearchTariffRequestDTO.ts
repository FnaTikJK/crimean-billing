import { AccountType } from '../../../../../profile/submodules/account/models/AccountType';

export interface ISearchTariffRequestDTO {
  ids?: string[];
  skip?: number;
  take?: number;
  code?: string;
  name?: string;
  description?: string;
  accountType?: AccountType;
  excludedTemplateIds?: string[];
}
