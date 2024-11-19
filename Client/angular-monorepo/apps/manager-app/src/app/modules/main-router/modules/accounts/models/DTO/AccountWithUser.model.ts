import { AccountType } from "../AccountType.enum"
import { UserInAccount } from "./UserInAccount.model"

export interface AccountWithUser {
    id: string
    phoneNumber: string
    number: string
    money: number
    accountType: AccountType
    user: UserInAccount
}
