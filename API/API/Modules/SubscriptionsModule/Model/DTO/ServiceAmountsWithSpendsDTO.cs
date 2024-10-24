using API.Modules.TariffsModule.Models.DTO;

namespace API.Modules.SubscriptionsModule.Model.DTO;

public class ServiceAmountsWithSpendsDTO : ServiceAmountDTO
{
    public required float Spent { get; set; }
}