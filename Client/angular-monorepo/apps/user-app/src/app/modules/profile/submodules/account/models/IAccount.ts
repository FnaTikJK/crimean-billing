import { AccountType } from './AccountType';

export interface IAccount {
  id: string;
  phoneNumber?: string;
  number: number;
  money: number;
  accountType: AccountType;
}
