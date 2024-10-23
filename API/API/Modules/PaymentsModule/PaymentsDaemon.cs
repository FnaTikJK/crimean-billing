using API.DAL;
using API.Infrastructure;
using API.Modules.InvoiceModule.Model;
using API.Modules.TariffsModule;
using API.Modules.TariffsModule.Extensions;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.PaymentsModule;

public interface IPaymentsDaemon
{
    Task<Result<bool>> TryPayInvoices();
}

public class PaymentsDaemon : IPaymentsDaemon
{
    private readonly DataContext db;
    private readonly ILog log;
    private readonly IPaymentsService paymentsService;
    private readonly ITariffsService tariffsService;

    private readonly DbSet<InvoiceEntity> invoices;
    
    public PaymentsDaemon(DataContext db, ILog log, IPaymentsService paymentsService, ITariffsService tariffsService)
    {
        this.db = db;
        this.log = log;
        invoices = db.Invoices;
        this.paymentsService = paymentsService;
        this.tariffsService = tariffsService;
    }

    public async Task<Result<bool>> TryPayInvoices()
    {
        var nowDate = DateTimeProvider.NowDate;
        var notPayedInvoices = invoices
            .Include(e => e.Account).ThenInclude(a => a.Subscription).ThenInclude(s => s.Tariff)
            .Include(e => e.Account).ThenInclude(a => a.Subscription).ThenInclude(s => s.PreferredChange).ThenInclude(p => p.TariffTemplate).ThenInclude(t => t.Tariffs)
            .Where(e => e.Account.Subscription!.PaymentDate == nowDate)
            .Where(e => e.PayedAt == null);
        foreach (var invoice in notPayedInvoices)
        {
            var paymentResponse = paymentsService.TryPayInvoice(invoice);
            if (paymentResponse.IsSuccess)
            {
                var subscription = invoice.Account.Subscription!;
                subscription.PaymentDate = invoice.Account.Subscription!.PaymentDate.AddDays(30);
                invoice.PayedAt = DateTimeProvider.Now;
                if (subscription.PreferredChange != null)
                {
                    subscription.Tariff = subscription.PreferredChange.TariffTemplate.Tariffs.FindActual();
                    subscription.PreferredChange = null;
                }
                else if (subscription.Tariff.IsNotActual())
                {
                    var actualTariffResponse = await tariffsService.FindActual(subscription.Tariff.Id);
                    if (!actualTariffResponse.IsSuccess)
                        return Result.BadRequest<bool>(actualTariffResponse.Error!);
                    subscription.Tariff = actualTariffResponse.Value;
                }
            }
        }

        await db.SaveChangesAsync();
        return Result.Ok(true);
    }
}