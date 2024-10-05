using API.DAL;

namespace API.Modules.AccountsModule.User;

public class AccountEntity : IEntity
{
    public Guid Id { get; set; }
    public UserEntity User { get; set; }
    public int PhoneNumber { get; set; }
}