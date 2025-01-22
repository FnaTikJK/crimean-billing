import { SearchAccountsResponse } from "../../accounts/models/DTO/SearchAccountsResponse.model";

export interface User {
    userId: string;
    email: string;
    telegramId?: string;
    fio: string;
    accounts: SearchAccountsResponse['items'];
}
