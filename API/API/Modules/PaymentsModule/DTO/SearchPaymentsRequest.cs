using System.Linq.Expressions;
using API.Infrastructure.BaseApiDTOs;
using API.Modules.PaymentsModule.Model;

namespace API.Modules.PaymentsModule.DTO;

public class SearchPaymentsRequest : SearchRequestWithoutIds
{
    public Guid? AccountId { get; set; }
    public PaymentType? PaymentType { get; set; }
    public SearchFloatQuery? Money{ get; set; }
    public SearchDateTimeQuery? DateTime { get; set; }
    public SearchPaymentsOrdering? Ordering { get; set; }
    public OrderDirection OrderDirection { get; set; } = OrderDirection.Desc;
}

public enum SearchPaymentsOrdering
{
    Money,
    DateTime,
}

public static class SearchPaymentsRequestExpressions
{
    public static Expression<Func<PaymentEntity, bool>> MoneyFit(this SearchPaymentsRequest request)
    {
        return payment => request.Money == null
                          || request.Money.From <= payment.Money && payment.Money <= request.Money.To;
    }
    
    public static Expression<Func<PaymentEntity, bool>> DateTimeFit(this SearchPaymentsRequest request)
    {
        return payment => request.DateTime == null
                          || request.DateTime.From <= payment.DateTime && payment.DateTime <= request.DateTime.To;
    }
}