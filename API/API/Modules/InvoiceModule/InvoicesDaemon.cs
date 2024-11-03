using API.DAL;
using API.Infrastructure;
using API.Modules.InvoiceModule.Model;
using API.Modules.SubscriptionsModule.Model;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.InvoiceModule;

public interface IInvoicesDaemon
{
    Task<Result<bool>> CreateInvoices();
}

public class InvoicesDaemon : IInvoicesDaemon
{
    private readonly DataContext db;
    private readonly ILog log;

    private readonly DbSet<SubscriptionEntity> subscriptions;
    private readonly DbSet<InvoiceEntity> invoices;

    public InvoicesDaemon(DataContext db, ILog log)
    {
        this.db = db;
        this.log = log;
        subscriptions = db.Subscriptions;
        invoices = db.Invoices;
    }

    public async Task<Result<bool>> CreateInvoices()
    {
        var paymentDate = DateTimeProvider.NowDate.AddDays(3);
        var query = subscriptions
            .Include(e => e.Account)
            .Include(e => e.Tariff)
            .Where(e => e.PaymentDate == paymentDate);
        foreach (var subscription in query)
        {
            var existedInvoice = await invoices.FirstOrDefaultAsync(e => e.Account.Id == subscription.AccountId 
                                                                         && e.PaymentId == null);
            if (existedInvoice != null)
                continue;

            var newInvoice = new InvoiceEntity
            {
                Account = subscription.Account,
                Tariff = subscription.Tariff,
                CreatedAt = DateTimeProvider.Now,
            };
            await invoices.AddAsync(newInvoice);
        }

        await db.SaveChangesAsync();
        return Result.Ok(true);
    }
}