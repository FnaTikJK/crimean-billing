using API.Modules.AccountsModule.User;
using API.Modules.UsersController.DTO;
using API.Modules.UsersController.Requests;
using Org.BouncyCastle.Ocsp;

namespace API.Modules.UsersController;

public static class UsersMapper
{
    public static UserDTO Map(UserEntity user) => new()
    {
        UserId = user.Id,
        Email = user.Email,
        TelegramId = user.TelegramId,
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

    public static void Patch(PatchUserRequest request, UserEntity target)
    {
        if (request.Fio != null)
            target.Fio = request.Fio;
        if (request.Email != null)
            target.Email = request.Email;
        if (request.TelegramId != null)
            target.TelegramId = request.TelegramId;
    }
}