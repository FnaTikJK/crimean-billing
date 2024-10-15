using System.ComponentModel.DataAnnotations;

namespace API.Modules.PaymentsModule.DTO;

public class AddMoneyRequest
{
    public required Guid AccountId { get; set; }
    [Range(0, float.MaxValue)]
    public required float ToAdd { get; set; } 
}