using API.Modules.AccountsModule.User;
using API.Modules.UserAccountsModule.DTO;
using API.Modules.UserAccountsModule.Model.DTO;
using API.Modules.UsersController.DTO;

namespace API.Modules.UserAccountsModule;

public static class AccountsMapper
{
    public static AccountWithUserDTO MapWithUser(AccountEntity account)
    {
        return new AccountWithUserDTO
        {
            Id = account.Id,
            Money = account.Money,
            Number = account.Number,
            PhoneNumber = account.PhoneNumber,
            AccountType = account.AccountType,
            User = Map(account.User),
        };
    }

    private static UserInAccountDTO Map(UserEntity user)
    {
        return new UserInAccountDTO
        {
            UserId = user.Id,
            Fio = user.Fio,
        };
    }

    public static AccountDTO Map(AccountEntity account)
    {
        return new AccountDTO()
        {
            Id = account.Id,
            Money = account.Money,
            Number = account.Number,
            AccountType = account.AccountType,
            PhoneNumber = account.PhoneNumber,
        };
    }

    public static void Patch(PatchAccountRequest source, AccountEntity target)
    {
        if (source.Money != null)
            target.Money = source.Money.Value;
        if (source.Number != null)
            target.Number = source.Number;
        if (source.PhoneNumber != null)
            target.PhoneNumber = source.PhoneNumber;
        if (source.AccountType != null)
            target.AccountType = source.AccountType.Value;
    }
}