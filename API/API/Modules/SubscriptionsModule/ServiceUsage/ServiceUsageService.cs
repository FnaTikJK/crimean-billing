using API.DAL;
using API.Infrastructure;
using API.Modules.ServicesModule.Model;
using API.Modules.SubscriptionsModule.Model;
using API.Modules.SubscriptionsModule.ServiceUsage.DTO;
using API.Modules.SubscriptionsModule.ServiceUsage.Model;
using API.Modules.SubscriptionsModule.ServiceUsage.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.SubscriptionsModule.ServiceUsage;

public interface IServiceUsageService
{
    Task<Result<ServiceUsageDTO>> AddService(AddServiceRequest request);
    Task<Result<ServiceUsageDTO>> SpendService(SpendServiceRequest request);
}

public class ServiceUsageService : IServiceUsageService
{
    private readonly ILog log;
    private readonly DataContext db;

    private DbSet<ServiceUsageEntity> serviceUsages => db.ServiceUsages;
    private DbSet<ServiceEntity> services => db.Services;
    private DbSet<SubscriptionEntity> subscriptions => db.Subscriptions;

    public ServiceUsageService(ILog log, DataContext db)
    {
        this.log = log;
        this.db = db;
    }

    public async Task<Result<ServiceUsageDTO>> AddService(AddServiceRequest request)
    {
        var service = await services
            .Include(e => e.Template)
            .FirstOrDefaultAsync(e => e.Id == request.ServiceId);
        if (service == null)
            return Result.NotFound<ServiceUsageDTO>("Такого Service не существует");
        if (service.Template.IsTariffService)
            return Result.BadRequest<ServiceUsageDTO>("Сервис только для Tariff");

        var subscription = await subscriptions
            .Include(e => e.ServiceUsages)
            .FirstOrDefaultAsync(e => e.AccountId == request.AccountId);
        if (subscription == null)
            return Result.NotFound<ServiceUsageDTO>("У данного Account нет Subscription");

        if (subscription.ServiceUsages?.Any(e => e.ServiceId == request.ServiceId) is true)
            return Result.BadRequest<ServiceUsageDTO>("Такой Service уже подключён в подписку");

        var newServiceUsage = new ServiceUsageEntity
        {
            Subscription = subscription,
            Service = service,
            Spent = 0,
            SubscribedAt = DateTimeProvider.Now,
        };
        await serviceUsages.AddAsync(newServiceUsage);
        await db.SaveChangesAsync();

        return Result.Ok(ServiceUsageMapper.Map(newServiceUsage));
    }

    public async Task<Result<ServiceUsageDTO>> SpendService(SpendServiceRequest request)
    {
        var subscription = await subscriptions
            .Include(e => e.ServiceUsages).ThenInclude(e => e.Service).ThenInclude(e => e.Template)
            .FirstOrDefaultAsync(e => e.AccountId == request.AccountId);
        if (subscription == null)
            return Result.NotFound<ServiceUsageDTO>("У данного Account нет Subscription");

        var serviceUsage = subscription.ServiceUsages?.FirstOrDefault(e => e.ServiceId == request.ServiceId);
        if (serviceUsage == null)
            return Result.NotFound<ServiceUsageDTO>("У Subscription нет такого Service");

        if (serviceUsage.Service.Amount != null && serviceUsage.Spent + request.ToSpend > serviceUsage.Service.Amount)
            return Result.BadRequest<ServiceUsageDTO>($"Вы пытаетесь потратить больше, чем можно. Уже потрачено: {serviceUsage.Spent}. Максимум: {serviceUsage.Service.Amount}.");

        serviceUsage.Spent += request.ToSpend;
        await db.SaveChangesAsync();

        return Result.Ok(ServiceUsageMapper.Map(serviceUsage));
    }
}