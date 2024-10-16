using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.Share;

namespace API.Modules.AccountsModule.User;

public class AccountEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public UserEntity User { get; set; }
    public required string PhoneNumber { get; set; }
    public required int Number { get; set; }
    public required float Money { get; set; }
    public required AccountType AccountType { get; set; }
}