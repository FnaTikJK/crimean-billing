import { AccountType } from './AccountType';

export interface IAccount {
  id: string;
  phoneNumber?: string;
  number: string;
  money: number;
  accountType: AccountType;
}
