import { AccountType } from "../AccountType.enum";

export interface RegisterAccountRequest {
  userId: string,
  phoneNumber: string,
  number: string,
  accountType: AccountType
}
