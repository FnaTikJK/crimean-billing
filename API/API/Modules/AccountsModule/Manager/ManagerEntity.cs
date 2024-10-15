using System.ComponentModel.DataAnnotations;
using API.DAL;

namespace API.Modules.AccountsModule.Manager;

public class ManagerEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
    public required string Fio { get; set; }
}