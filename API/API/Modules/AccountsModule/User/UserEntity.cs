using System.ComponentModel.DataAnnotations;
using API.DAL;

namespace API.Modules.AccountsModule.User;

public class UserEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public HashSet<AccountEntity> Accounts { get; set; }
}