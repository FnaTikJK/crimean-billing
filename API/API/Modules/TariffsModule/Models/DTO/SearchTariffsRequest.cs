using API.Infrastructure.BaseApiDTOs;
using API.Modules.AccountsModule.Share;
using API.Modules.ServicesModule.Model;

namespace API.Modules.TariffsModule.Models.DTO;

public class SearchTariffsRequest : SearchRequest
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public AccountType? AccountType { get; set; }
    
    public SearchFloatQuery? Price { get; set; }
    public IEnumerable<SearchServiceQuery>? ServicesAmounts { get; set; }
}

public class SearchServiceQuery
{
    public ServiceType? ServiceType { get; set; }
    public UnitType? UnitType { get; set; }
    public SearchFloatQuery? Amount { get; set; }
}