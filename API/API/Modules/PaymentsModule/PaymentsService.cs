using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule.User;
using API.Modules.PaymentsModule.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.PaymentsModule;

public interface IPaymentsService
{
    Task<Result<PaymentsResponse>> AddMoney(AddMoneyRequest request);
    Task<Result<PaymentsResponse>> SpendMoney(SpendMoneyRequest request);
}

public class PaymentsService : IPaymentsService
{
    private readonly DataContext db;
    private readonly ILog log;
    
    private readonly DbSet<AccountEntity> accounts;

    public PaymentsService(DataContext db, ILog log)
    {
        this.db = db;
        this.log = log;
        this.accounts = db.Accounts;
    }

    public async Task<Result<PaymentsResponse>> AddMoney(AddMoneyRequest request)
    {
        var account = await accounts.FirstOrDefaultAsync(e => e.Id == request.AccountId);
        if (account == null)
            return Result.BadRequest<PaymentsResponse>("Такого ЛС не существует");

        account.Money += request.ToAdd;
        await db.SaveChangesAsync();
        
        log.Info($"Added money: {request.ToAdd}, to account: {request.AccountId}");
        return Result.Ok(new PaymentsResponse
        {
            RemainedMoney = account.Money,
        });
    }

    public async Task<Result<PaymentsResponse>> SpendMoney(SpendMoneyRequest request)
    {
        var account = await accounts.FirstOrDefaultAsync(e => e.Id == request.AccountId);
        if (account == null)
            return Result.BadRequest<PaymentsResponse>("Такого ЛС не существует");

        if (account.Money < request.ToSpend)
            return Result.BadRequest<PaymentsResponse>("Недостаточно денег на ЛС");
        
        account.Money -= request.ToSpend;
        await db.SaveChangesAsync();
        
        log.Info($"Spent money: {request.ToSpend} to account: {request.AccountId}");
        return Result.Ok(new PaymentsResponse
        {
            RemainedMoney = account.Money,
        });
    }
}