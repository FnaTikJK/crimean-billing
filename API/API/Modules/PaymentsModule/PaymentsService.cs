using System.Linq.Expressions;
using API.DAL;
using API.Infrastructure;
using API.Infrastructure.BaseApiDTOs;
using API.Modules.AccountsModule.User;
using API.Modules.InvoiceModule;
using API.Modules.InvoiceModule.Model;
using API.Modules.PaymentsModule.DTO;
using API.Modules.PaymentsModule.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.PaymentsModule;

public interface IPaymentsService
{
    Result<SearchPaymentsResponse> Search(SearchPaymentsRequest request);
    Task<Result<PaymentsResponse>> AddMoney(AddMoneyRequest request);
    Task<Result<PaymentsResponse>> SpendMoney(SpendMoneyRequest request);
    Result<PaymentEntity> TryPayInvoice(InvoiceEntity invoice);
}

public class PaymentsService : IPaymentsService
{
    private readonly DataContext db;
    private readonly ILog log;
    
    private readonly DbSet<AccountEntity> accounts;
    private readonly DbSet<PaymentEntity> payments;

    public PaymentsService(DataContext db, ILog log)
    {
        this.db = db;
        this.log = log;
        this.accounts = db.Accounts;
        this.payments = db.Payments;
    }

    public Result<SearchPaymentsResponse> Search(SearchPaymentsRequest request)
    {
        var query = payments.AsNoTracking()
            .Include(e => e.Account)
            .Include(e => e.Invoice)
            .AsQueryable();

        if (request.AccountId != null) 
            query = query.Where(e => e.AccountId == request.AccountId);
        if (request.PaymentType != null)
            query = query.Where(e => e.Type == request.PaymentType);
        if (request.Money != null)
            query = query.Where(request.MoneyFit());
        if (request.DateTime != null)
            query = query.Where(request.DateTimeFit());
        if (request.Ordering != null)
        {
            Expression<Func<PaymentEntity, DateTime>> byDateTime = payment => payment.DateTime;
            Expression<Func<PaymentEntity, float>> byMoney = payment => payment.Money;
            query = request.OrderDirection == OrderDirection.Asc
                ? request.Ordering == SearchPaymentsOrdering.Money
                    ? query.OrderBy(byMoney)
                    : query.OrderBy(byDateTime)
                : request.Ordering == SearchPaymentsOrdering.Money
                    ? query.OrderByDescending(byMoney)
                    : query.OrderByDescending(byDateTime);
        }
        else
        {
            query = query.OrderByDescending(e => e.DateTime);
        }
        
        var totalCount = query.Count();
        return Result.Ok(new SearchPaymentsResponse
        {
            TotalCount = totalCount,
            Items = query.Skip(request.Skip).Take(request.Take).AsEnumerable().Select(PaymentsMapper.Map).ToList(),
        });
    }

    public async Task<Result<PaymentsResponse>> AddMoney(AddMoneyRequest request)
    {
        var account = await accounts.FirstOrDefaultAsync(e => e.Id == request.AccountId);
        if (account == null)
            return Result.BadRequest<PaymentsResponse>("Такого ЛС не существует");

        account.Money += request.ToAdd;
        var payment = new PaymentEntity
        {
            Account = account,
            Money = request.ToAdd,
            Type = PaymentType.Deposit,
            DateTime = DateTimeProvider.Now,
        };
        await payments.AddAsync(payment);
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
        var payment = new PaymentEntity
        {
            Account = account,
            Money = request.ToSpend,
            Type = PaymentType.Withdrawal,
            DateTime = DateTimeProvider.Now,
        };
        await db.Payments.AddAsync(payment);
        await db.SaveChangesAsync();
        
        log.Info($"Spent money: {request.ToSpend} to account: {request.AccountId}");
        return Result.Ok(new PaymentsResponse
        {
            RemainedMoney = account.Money,
        });
    }

    public Result<PaymentEntity> TryPayInvoice(InvoiceEntity invoice)
    {
        var account = invoice.Account;
        var invoicePrice = invoice.CalculateTotalPrice();
        if (account.Money < invoicePrice)
            return Result.BadRequest<PaymentEntity>("Недостаточно средств на счете");

        account.Money -= invoicePrice;
        var payment = new PaymentEntity
        {
            Account = account,
            Money = invoicePrice,
            Type = PaymentType.Withdrawal,
            DateTime = DateTimeProvider.Now,
            Invoice = invoice,
        };
        invoice.Payment = payment;
        db.Add(payment);
        return Result.Ok(payment);
    }
}