using API.Modules.AccountsModule.Share;

namespace API.Modules.UsersController.DTO;

public class AccountDTO
{
    public required Guid Id { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Number { get; set; }
    public required float Money { get; set; }
    public required AccountType AccountType { get; set; }
}