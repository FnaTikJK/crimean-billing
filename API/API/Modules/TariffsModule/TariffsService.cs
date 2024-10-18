using API.DAL;
using API.Infrastructure;
using API.Modules.ServicesModule.Model;
using API.Modules.TariffsModule.DTO;
using API.Modules.TariffsModule.Models;
using API.Modules.TariffsModule.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.TariffsModule;

public interface ITariffsService
{
    Task<Result<TariffDTO>> CreateTariff(CreateTariffRequest request);
}

public class TariffsService : ITariffsService
{
    private readonly DataContext db;
    private readonly ILog log;

    private readonly DbSet<TariffTemplateEntity> templates;
    private readonly DbSet<TariffEntity> tariffs;
    private readonly DbSet<TariffServiceAmountEntity> serviceAmounts;
    private readonly DbSet<ServiceTemplateEntity> services;

    public TariffsService(DataContext db, ILog log)
    {
        this.db = db;
        this.log = log;
        this.templates = db.TariffTemplates;
        this.tariffs = db.Tariffs;
        this.serviceAmounts = db.TariffServiceAmounts;
        this.services = db.ServiceTemplates;
    }

    public async Task<Result<TariffDTO>> CreateTariff(CreateTariffRequest request)
    {
        var withSameCode = await templates.AsNoTracking().FirstOrDefaultAsync(e => e.Code == request.Code);
        if (withSameCode != null)
            return Result.BadRequest<TariffDTO>("Тариф с таким кодом уже существует");

        var serviceTemplatesIds = request.ServicesAmounts.Select(e => e.ServiceTemplateId).ToHashSet();
        var serviceTemplates = services
            .Where(e => serviceTemplatesIds.Contains(e.Id))
            .ToDictionary(e => e.Id, e => e);
        if (serviceTemplates.Count < request.ServicesAmounts.Count())
            return Result.BadRequest<TariffDTO>("Некорректные ServiceTemplateIds");
        if (serviceTemplates.Any(e => e.Value.IsTariffService == false))
            return Result.BadRequest<TariffDTO>("Вы пытаетесь добавить добавить Service не предназначенный для Tariff (флаг IsTariffService)");

        var template = TariffsMapper.Map(request, serviceTemplates);
        await templates.AddAsync(template);
        await db.SaveChangesAsync();

        return Result.Ok(TariffsMapper.Map(template));
    }
}