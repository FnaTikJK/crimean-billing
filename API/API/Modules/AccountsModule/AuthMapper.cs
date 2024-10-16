using API.Modules.AccountsModule.Manager;
using API.Modules.AccountsModule.Manager.DTO;
using API.Modules.AccountsModule.User.DTO;

namespace API.Modules.AccountsModule.User;

public static class AuthMapper
{
    public static UserEntity Map(RegisterUserRequest source)
        => new()
        {
            Email = source.Email,
            Fio = source.Fio,
        };
    
    public static AccountEntity Map(RegisterAccountRequest source)
        => new()
        {
            Money = 0,
            Number = source.Number,
            PhoneNumber = source.PhoneNumber,
            AccountType = source.AccountType,
        };

    public static ManagerEntity Map(RegisterManagerRequest source, string hashedPassword)
        => new()
        {
            Login = source.Login,
            PasswordHash = hashedPassword,
            Fio = source.Fio,
        };
}