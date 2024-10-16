using System.ComponentModel.DataAnnotations;

namespace API.Modules.AdminModule.DTO;

public class GetVerificationCodeRequest
{
    [RegularExpression(@"8[0-9]{10}")]
    public string PhoneNumber { get; set; }
}