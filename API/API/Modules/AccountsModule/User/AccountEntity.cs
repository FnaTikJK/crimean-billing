using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.Share;

namespace API.Modules.AccountsModule.User;

public class AccountEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public UserEntity User { get; set; }
    public int PhoneNumber { get; set; }
    public int Number { get; set; }
    public AccountType AccountType { get; set; }
}