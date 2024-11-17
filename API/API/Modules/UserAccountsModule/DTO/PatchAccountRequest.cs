using API.Modules.AccountsModule.Share;

namespace API.Modules.UserAccountsModule.DTO;

public class PatchAccountRequest
{
    public required string? PhoneNumber { get; set; }
    public required string? Number { get; set; }
    public required float? Money { get; set; }
    public required AccountType? AccountType { get; set; }
}