using System.ComponentModel.DataAnnotations;

namespace API.Modules.PaymentsModule.DTO;

public class SpendMoneyRequest
{
    public required Guid AccountId { get; set; }
    [Range(0, float.MaxValue)]
    public required float ToSpend { get; set; } 
}