using API.Modules.AccountsModule.User.DTO;

namespace API.Modules.AccountsModule.User;

public static class UsersMapper
{
    public static UserEntity Map(RegisterUserRequest source)
        => new()
        {
            Email = source.Email,
            Fio = source.Fio,
        };
}

public static class AccountMapper
{
    public static AccountEntity Map(RegisterAccountRequest source)
        => new()
        {
            Money = 0,
            Number = source.Number,
            PhoneNumber = source.PhoneNumber,
            AccountType = source.AccountType,
        };
}