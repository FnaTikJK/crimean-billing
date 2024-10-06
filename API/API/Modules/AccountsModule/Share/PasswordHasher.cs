namespace API.Modules.AccountsModule.Share;

public interface IPasswordHasher
{
    public string Hash(string password);
}

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return password; // TODO: Сделать хеширование
    }
}