using System.Linq.Expressions;
using API.Infrastructure.BaseApiDTOs;
using API.Modules.AccountsModule.Share;
using API.Modules.ServicesModule.Model;
using API.Modules.ServicesModule.Model.DTO;

namespace API.Modules.ServicesModule.DTO;

public class SearchServicesRequest : SearchRequest
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public AccountType? AccountType { get; set; }
    public ServiceType? ServiceType { get; set; }
    public UnitType? UnitType { get; set; }
    public bool? IsTariffService { get; set; }
    public SearchFloatQuery? Price { get; set; }
    public SearchFloatQuery? Amount { get; set; }
    public SearchDateTimeQuery? CreatedAt { get; set; }
    public SearchDateTimeQuery? UpdatedAt { get; set; }
    
    public SearchServiceRequestOrderBy? OrderBy { get; set; }
    public OrderDirection? OrderDirection { get; set; }
    
    public HashSet<Guid>? ExcludedTemplateIds { get; set; }
}

public enum SearchServiceRequestOrderBy
{
    Code,
    Price,
    Amount,
    CreatedAt,
    UpdatedAt,
}

public static class SearchServiceRequestOrderByExtensions
{
    public static Func<ServiceEntity, object> ToOrderFunc(this SearchServiceRequestOrderBy orderBy)
    {
        if (orderBy == SearchServiceRequestOrderBy.Code)
            return (e) => e.Template.Code;
        if (orderBy == SearchServiceRequestOrderBy.Price)
            return (e) => e.Price;
        if (orderBy == SearchServiceRequestOrderBy.Amount)
            return (e) => e.Amount;

        throw new NotImplementedException("Not implemented orderBy field");
    }
    
    public static Expression<Func<ServiceDTO, bool>> CreatedAtFit(this SearchServicesRequest request)
    {
        if (request.CreatedAt == null)
            throw new Exception("Exception in Expression parsing");
        return service => request.CreatedAt.From <= service.CreatedAt && service.CreatedAt <= request.CreatedAt.To;
    }
    
    public static Expression<Func<ServiceDTO, bool>> UpdatedAtFit(this SearchServicesRequest request)
    {
        if (request.UpdatedAt == null)
            throw new Exception("Exception in Expression parsing");
        return service => request.UpdatedAt.From <= service.UpdatedAt && service.UpdatedAt <= request.UpdatedAt.To;
    }
}