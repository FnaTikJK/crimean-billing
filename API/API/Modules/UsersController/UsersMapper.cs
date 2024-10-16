using API.Modules.AccountsModule.User;
using API.Modules.UsersController.DTO;

namespace API.Modules.UsersController;

public static class UsersMapper
{
    public static UserDTO Map(UserEntity user) => new()
    {
        UserId = user.Id,
        Email = user.Email,
        Fio = user.Fio,
        Accounts = user.Accounts.Select(Map).ToList(),
    };

    public static AccountDTO Map(AccountEntity account) => new()
    {
        Id = account.Id,
        PhoneNumber = account.PhoneNumber,
        Money = account.Money,
        Number = account.Number,
        AccountType = account.AccountType,
    };
}