using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using API.Modules.AccountsModule.Share;

namespace API.Modules.AccountsModule.User.DTO;

public class RegisterAccountRequest
{
    public required Guid UserId { get; set; }
    [RegularExpression(@"8[0-9]{10}")]
    public required string PhoneNumber { get; set; }
    public required int Number { get; set; }
    public required AccountType AccountType { get; set; }
}