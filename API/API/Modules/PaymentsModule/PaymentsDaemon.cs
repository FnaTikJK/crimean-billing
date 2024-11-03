using API.DAL;
using API.Infrastructure;
using API.Modules.InvoiceModule.Model;
using API.Modules.NotificationModule;
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
    private readonly INotificationsService notificationsService;

    private readonly DbSet<InvoiceEntity> invoices;
    
    public PaymentsDaemon(DataContext db, ILog log, IPaymentsService paymentsService, ITariffsService tariffsService, INotificationsService notificationsService)
    {
        this.db = db;
        this.log = log;
        invoices = db.Invoices;
        this.paymentsService = paymentsService;
        this.tariffsService = tariffsService;
        this.notificationsService = notificationsService;
    }

    public async Task<Result<bool>> TryPayInvoices()
    {
        var nowDate = DateTimeProvider.NowDate;
        var notPayedInvoices = invoices
            .Include(e => e.Account).ThenInclude(a => a.Subscription).ThenInclude(s => s.Tariff)
            .Include(e => e.Account).ThenInclude(a => a.Subscription).ThenInclude(s => s.PreferredChange).ThenInclude(p => p.TariffTemplate).ThenInclude(t => t.Tariffs)
            .Include(e => e.Account).ThenInclude(a => a.User)
            .Where(e => e.Account.Subscription!.PaymentDate == nowDate)
            .Where(e => e.PaymentId == null);
        foreach (var invoice in notPayedInvoices)
        {
            var paymentResponse = paymentsService.TryPayInvoice(invoice);
            if (paymentResponse.IsSuccess)
            {
                var payment = paymentResponse.Value;
                var subscription = invoice.Account.Subscription!;
                subscription.PaymentDate = subscription.PaymentDate.AddDays(30);
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

                await notificationsService.SendInvoiceSuccessPaymentNotification(invoice,
                    DateOnly.FromDateTime(subscription.PaymentDate), payment.Money);
            }
            else
            {
                await notificationsService.SendInvoiceFailurePaymentNotification(invoice, paymentResponse.Error!);
            }
        }

        await db.SaveChangesAsync();
        return Result.Ok(true);
    }
}