using System.ComponentModel.DataAnnotations;
using API.DAL;

namespace API.Modules.AccountsModule.Manager;

public class ManagerEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string? Email { get; set; }
}