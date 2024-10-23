using API.DAL;
using API.Infrastructure;
using API.Modules.SubscriptionsModule.DTO;
using API.Modules.SubscriptionsModule.Model;
using API.Modules.SubscriptionsModule.Model.DTO;
using API.Modules.TariffsModule.Extensions;
using API.Modules.TariffsModule.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.SubscriptionsModule;

public interface ISubscriptionsService
{
    Task<Result<SubscriptionDTO>> Subscribe(SubscribeRequest request);
    Task<Result<SubscriptionDTO>> GetMySubscription(GetSubscriptionRequest request, Guid userId);
}

public class SubscriptionsService : ISubscriptionsService
{
    private readonly DataContext db;
    private readonly ILog log;

    private readonly DbSet<SubscriptionEntity> subscriptions;
    private readonly DbSet<SubscriptionsPreferredChangesEntity> subscriptionsPreferredChanges;
    private readonly DbSet<TariffEntity> tariffs;
    private readonly DbSet<TariffTemplateEntity> tariffTemplates;

    public SubscriptionsService(DataContext db, ILog log)
    {
        this.db = db;
        this.log = log;
        this.subscriptions = db.Subscriptions;
        this.subscriptionsPreferredChanges = db.SubscriptionsPreferredChanges;
        this.tariffs = db.Tariffs;
        this.tariffTemplates = db.TariffTemplates;
    }

    public async Task<Result<SubscriptionDTO>> Subscribe(SubscribeRequest request)
    {
        var tariffTemplate = await tariffTemplates
            .Include(e => e.Tariffs).ThenInclude(t => t.ServicesAmounts).ThenInclude(s => s.ServiceTemplate)
            .FirstOrDefaultAsync(e => e.Id == request.TariffTemplateId);
        if (tariffTemplate == null)
            return Result.BadRequest<SubscriptionDTO>("Такого TariffTemplate не существует");

        var subscription = await FindSubscription(request.AccountId, false);
        if (subscription == null)
        {
            var account = await db.Accounts.FirstOrDefaultAsync(e => e.Id == request.AccountId);
            if (account == null)
                return Result.BadRequest<SubscriptionDTO>("Такого Account не существует");
            
            subscription = new SubscriptionEntity
            {
                Account = account,
                Tariff = tariffTemplate.Tariffs.FindActual(),
                CreatedAt = DateTimeProvider.Now,
                PaymentDate = DateTimeProvider.NowDate.AddDays(30),
            };
            await subscriptions.AddAsync(subscription);
        }
        else
        {
            if (subscription.PreferredChange?.TariffTemplateId == request.TariffTemplateId)
                return Result.BadRequest<SubscriptionDTO>("Этот тариф уже выбран для смены");

            subscription.PreferredChange = subscription.Tariff.Template.Id == request.TariffTemplateId
                ? null
                : new SubscriptionsPreferredChangesEntity
                {
                    Subscription = subscription,
                    TariffTemplate = tariffTemplate,
                };
        }

        await db.SaveChangesAsync();
        return Result.Ok(SubscriptionsMapper.Map(subscription));
    }

    public async Task<Result<SubscriptionDTO>> GetMySubscription(GetSubscriptionRequest request, Guid userId)
    {
        var subscription = await FindSubscription(request.AccountId, true);
        if (subscription == null)
            return Result.NotFound<SubscriptionDTO>("Такого Subscription не существует");
        
        return Result.Ok(SubscriptionsMapper.Map(subscription));
    }

    private Task<SubscriptionEntity?> FindSubscription(Guid accountId, bool asNoTracking)
    {
        var query = subscriptions
            .Include(e => e.Tariff).ThenInclude(t => t.Template)
            .Include(e => e.Tariff).ThenInclude(t => t.ServicesAmounts).ThenInclude(s => s.ServiceTemplate)
            .Include(e => e.PreferredChange).ThenInclude(p => p.TariffTemplate).ThenInclude(tm => tm.Tariffs).ThenInclude(t => t.ServicesAmounts).ThenInclude(s => s.ServiceTemplate)
            .AsQueryable();
        if (asNoTracking)
            query = query.AsNoTracking();
        
        return query.FirstOrDefaultAsync(e => e.Account.Id == accountId);
    }
}