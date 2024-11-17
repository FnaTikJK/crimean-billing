using System.Linq.Expressions;
using API.Infrastructure.BaseApiDTOs;
using API.Modules.AccountsModule.Share;
using API.Modules.AccountsModule.User;

namespace API.Modules.UserAccountsModule.DTO;

public class SearchAccountsRequest : SearchRequest
{
    public Guid? UserId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Number { get; set; }
    public SearchFloatQuery? Money { get; set; }
    public AccountType? AccountType { get; set; }
}

public static class SearchAccountRequestExpressions
{
    public static Expression<Func<AccountEntity, bool>> MoneyFit(this SearchAccountsRequest request)
    {
        return account => request.Money == null
                          || request.Money.From <= account.Money && account.Money <= request.Money.To;
    }
}