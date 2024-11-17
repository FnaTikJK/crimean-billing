using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule.User;
using API.Modules.UserAccountsModule.DTO;
using API.Modules.UsersController.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.UserAccountsModule;

public interface IAccountsService
{
    Result<SearchAccountsResponse> Search(SearchAccountsRequest request);
    Task<Result<AccountDTO>> PatchAccount(Guid accountId, PatchAccountRequest request);
}

public class AccountsService : IAccountsService
{
    private readonly ILog log;
    private readonly DataContext db;

    private DbSet<AccountEntity> accounts => db.Accounts;

    public AccountsService(ILog log, DataContext db)
    {
        this.log = log;
        this.db = db;
    }

    public Result<SearchAccountsResponse> Search(SearchAccountsRequest request)
    {
        var query = accounts
            .Include(e => e.User)
            .AsNoTracking();

        if (request.UserId != null)
            query = query.Where(e => e.User.Id == request.UserId);
        if (request.AccountType != null)
            query = query.Where(e => e.AccountType == request.AccountType);
        if (request.PhoneNumber != null)
            query = query.Where(e => e.PhoneNumber.Contains(request.PhoneNumber));
        if (request.Number != null)
            query = query.Where(e => e.Number.ToLower().Contains(request.Number.ToLower()));
        if (request.Money != null)
            query = query.Where(request.MoneyFit());

        var totalCount = query.Count();
        var items = query
            .Skip(request.Skip)
            .Take(request.Take)
            .AsEnumerable()
            .Select(AccountsMapper.MapWithUser)
            .ToList();
        return Result.Ok(new SearchAccountsResponse()
        {
            TotalCount = totalCount,
            Items = items,
        });
    }

    public async Task<Result<AccountDTO>> PatchAccount(Guid accountId, PatchAccountRequest request)
    {
        var account = await accounts.FirstOrDefaultAsync(e => e.Id == accountId);
        if (account == null)
            return Result.BadRequest<AccountDTO>("Такого ЛС не существует");
        
        AccountsMapper.Patch(request, account);
        await db.SaveChangesAsync();
        return Result.Ok(AccountsMapper.Map(account));
    }
}