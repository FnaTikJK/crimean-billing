import { IAccount } from '../submodules/account/models/IAccount';

export interface IProfile {
  userId: string;
  email: string;
  fio: string;
  telegramId: number;
  accounts: IAccount[];
}
