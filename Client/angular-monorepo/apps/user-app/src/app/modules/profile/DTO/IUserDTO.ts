import { IAccount } from '../submodules/account/models/IAccount';

export interface IUserDTO {
  userId: string;
  email: string;
  fio: string;
  telegramId: number;
  accounts: IAccount[];
}
