using API.Infrastructure;
using API.Modules.AccountsModule.User;
using API.Modules.InvoiceModule;
using API.Modules.InvoiceModule.Model;
using API.Modules.TelegramModule;

namespace API.Modules.NotificationModule;

public interface INotificationsService
{
    Task<Result<bool>> SendInvoiceCreatedNotification(InvoiceEntity invoice);
    Task<Result<bool>> SendInvoiceSuccessPaymentNotification(InvoiceEntity invoice, DateOnly newPaymentDate, float payed);
    Task<Result<bool>> SendInvoiceFailurePaymentNotification(InvoiceEntity invoice, string error);

}

public class NotificationsService : INotificationsService
{
    private readonly IMailsService mailsService;
    private readonly ITelegramService telegramService;

    public NotificationsService(IMailsService mailsService, ITelegramService telegramService)
    {
        this.mailsService = mailsService;
        this.telegramService = telegramService;
    }

    public async Task<Result<bool>> SendInvoiceCreatedNotification(InvoiceEntity invoice)
    {
        var text = $"Вам выставлен счёт на Лицевой номер: {invoice.Account.Number}. Сумма: {invoice.CalculateTotalPrice()}.";
        
        var email = invoice.Account.User.Email;
        mailsService.SendEmail("Выставлен счёт", text, email);
        
        var telegramId = invoice.Account.User.TelegramId;
        if (telegramId != null)
            await telegramService.SendMessage(telegramId.Value, text);
        
        return Result.Ok(true);
    }

    public async Task<Result<bool>> SendInvoiceSuccessPaymentNotification(InvoiceEntity invoice, DateOnly newPaymentDate, float payed)
    {
        var text = $"Продлена подписка у Лицевого номера: {invoice.Account.Number} до {newPaymentDate}. Сумма: {invoice.CalculateTotalPrice()}.";
        
        var email = invoice.Account.User.Email;
        mailsService.SendEmail("Выставлен счёт", text, email);
        
        var telegramId = invoice.Account.User.TelegramId;
        if (telegramId != null)
            await telegramService.SendMessage(telegramId.Value, text);
        
        return Result.Ok(true);
    }

    public async Task<Result<bool>> SendInvoiceFailurePaymentNotification(InvoiceEntity invoice, string error)
    {
        var text = $"Не удалось продлить подписку у Лицевого номера: {invoice.Account.Number}. {error}.";
        
        var email = invoice.Account.User.Email;
        mailsService.SendEmail("Не удалось продлить подписку", text, email);
        
        var telegramId = invoice.Account.User.TelegramId;
        if (telegramId != null)
            await telegramService.SendMessage(telegramId.Value, text);
        
        return Result.Ok(true);
    }
}