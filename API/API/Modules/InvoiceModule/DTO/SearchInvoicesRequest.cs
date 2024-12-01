using System.ComponentModel;
using System.Linq.Expressions;
using API.Infrastructure.BaseApiDTOs;
using API.Modules.InvoiceModule.Model;

namespace API.Modules.InvoiceModule.DTO;

public class SearchInvoicesRequest : SearchRequestWithoutIds
{
    public Guid? AccountId { get; set; }
    public SearchFloatQuery? ToPay { get; set; }
    public SearchDateTimeQuery? CreatedAt { get; set; }
    public SearchDateTimeQuery? PayedAt { get; set; }
    public SearchInvoicesOrdering? Ordering { get; set; }
    public OrderDirection? Direction { get; set; }
}

public enum SearchInvoicesOrdering
{
    ToPay,
    CreatedAt,
    PayedAt,
}

public static class SearchServiceRequestOrderByExtensions
{
    public static Expression<Func<InvoiceEntity, bool>> ToPayFit(this SearchInvoicesRequest request)
    {
        if (request.ToPay == null)
            throw new Exception("Exception in Expression parsing");
        return invoice => request.ToPay.From <= invoice.Tariff.Price 
                          && invoice.Tariff.Price <= request.ToPay.To;
    }
    
    public static Expression<Func<InvoiceEntity, bool>> CreatedAtFit(this SearchInvoicesRequest request)
    {
        if (request.CreatedAt == null)
            throw new Exception("Exception in Expression parsing");
        return invoice => request.CreatedAt.From <= invoice.CreatedAt && invoice.CreatedAt <= request.CreatedAt.To;
    }
    
    public static Expression<Func<InvoiceEntity, bool>> PayedAtFit(this SearchInvoicesRequest request)
    {
        if (request.PayedAt == null)
            throw new Exception("Exception in Expression parsing");
        return invoice => invoice.Payment != null 
                          && request.PayedAt.From <= invoice.Payment.DateTime 
                          && invoice.Payment.DateTime <= request.PayedAt.To;
    }
    
    public static Expression<Func<InvoiceEntity, DateTime?>> PayedAtOrdering(this SearchInvoicesOrdering ordering)
    {
        return invoice => invoice.Payment == null ? null : invoice.Payment.DateTime;
    }
    
    public static Expression<Func<InvoiceEntity, float?>> ToPayOrdering(this SearchInvoicesOrdering ordering)
    {
        return invoice => invoice.Tariff.Price;
    }
}