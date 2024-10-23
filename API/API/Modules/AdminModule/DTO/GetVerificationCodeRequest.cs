using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Modules.AdminModule.DTO;

public class GetVerificationCodeRequest
{
    [RegularExpression(@"8[0-9]{10}")]
    [DefaultValue("88005553535")]
    public string PhoneNumber { get; set; }
}