using API.Modules.AccountsModule.User.DTO;

namespace API.Modules.AccountsModule.User;

public static class UserMapper
{
    public static UserEntity Map(RegisterUserRequest source)
        => new()
        {
            Email = source.Email!,
            Accounts = new HashSet<AccountEntity>(),
        };
}

public static class AccountMapper
{
    public static AccountEntity Map(RegisterUserRequest source)
        => new()
        {
            Money = 0,
            Number = source.Number,
            PhoneNumber = source.PhoneNumber,
            AccountType = source.AccountType,
        };
}