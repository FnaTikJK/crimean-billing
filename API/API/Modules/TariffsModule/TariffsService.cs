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
    Task<Result<TariffDTO>> PatchTariff(PatchTariffRequest request);
    Result<SearchTariffResponse> SearchTariffs(SearchTariffsRequest request);
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

        var serviceTemplatesResult = FindServicesToBound(request.ServicesAmounts);
        if (!serviceTemplatesResult.IsSuccess)
            return Result.BadRequest<TariffDTO>(serviceTemplatesResult.Error!);

        var serviceTemplates = serviceTemplatesResult.Value;
        var template = TariffsMapper.Map(request, serviceTemplates);
        await templates.AddAsync(template);
        await db.SaveChangesAsync();

        return Result.Ok(TariffsMapper.Map(template));
    }

    public async Task<Result<TariffDTO>> PatchTariff(PatchTariffRequest request)
    {
        if (request.IsIncorrect(out var errorMessage))
            return Result.BadRequest<TariffDTO>(errorMessage);
        var withSameCode = await templates.AsNoTracking().FirstOrDefaultAsync(e => e.Code == request.Code);
        if (withSameCode != null)
            return Result.BadRequest<TariffDTO>("Tariff с таким кодом уже существует");
        var template = await templates
            .Include(e => e.Tariffs).ThenInclude(t => t.ServicesAmounts)
            .FirstOrDefaultAsync(e => e.Id == request.TemplateId);
        if (template == null)
            return Result.BadRequest<TariffDTO>("Такого Tariff не существует");
        
        var serviceTemplatesResult = FindServicesToBound(request.ServicesAmounts);
        if (!serviceTemplatesResult.IsSuccess)
            return Result.BadRequest<TariffDTO>(serviceTemplatesResult.Error!);

        TariffsMapper.Map(template, request, serviceTemplatesResult.Value);
        await db.SaveChangesAsync();

        return Result.Ok(TariffsMapper.Map(template));
    }

    public Result<SearchTariffResponse> SearchTariffs(SearchTariffsRequest request)
    {
        var query = templates.AsNoTracking()
            .Include(e => e.Tariffs).ThenInclude(t => t.ServicesAmounts).ThenInclude(s => s.ServiceTemplate)
            .Select(TariffsMapper.Map)
            .AsQueryable();

        
        if (request.Code != null)
            query = query.Where(e => e.Code.Contains(request.Code, StringComparison.OrdinalIgnoreCase)); 
        if (request.Name != null)
            query = query.Where(e => e.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
        if (request.Description != null)
            query = query.Where(e => e.Description.Contains(request.Description, StringComparison.OrdinalIgnoreCase));
        if (request.AccountType != null)
            query = query.Where(e => e.AccountType == request.AccountType);
        
        if (request.Price != null)
            query = query.Where(e => request.Price.Fit(e.Price));
        if (request.ServicesAmounts != null)
            query = query.Where(e => SearchServicesAmounts(e.Services, request.ServicesAmounts));

        if (request.ExcludedTemplateIds != null)
            query = query.Where(e => !request.ExcludedTemplateIds.Contains(e.TemplateId));
        
        var result = query.Skip(request.Skip).Take(request.Take);
        var totalCount = query.Count();
        return Result.Ok(new SearchTariffResponse()
        {
            TotalCount = totalCount,
            Items = result.ToList(),
        });
    }

    private bool SearchServicesAmounts(IEnumerable<ServiceAmountDTO> source, IEnumerable<SearchServiceQuery> toSearch)
    {
        return toSearch.All(
            s => source.Any(e => (s.UnitType == null || s.UnitType.Value == e.UnitType)
                                && (s.ServiceType == null || s.ServiceType.Value == e.ServiceType)
                                && (s.Amount == null || s.Amount.Fit(e.Amount))));
    }

    private Result<Dictionary<Guid, ServiceTemplateEntity>?> FindServicesToBound(
        IEnumerable<CreateTariffServiceAmountsRequest>? servicesAmounts)
    {
        if (servicesAmounts == null)
            return Result.Ok<Dictionary<Guid, ServiceTemplateEntity>?>(null);
        
        var serviceTemplatesIds = servicesAmounts.Select(e => e.ServiceTemplateId).ToHashSet();
        var serviceTemplates = services
            .Where(e => serviceTemplatesIds.Contains(e.Id))
            .ToDictionary(e => e.Id, e => e);
        if (serviceTemplates.Count < serviceTemplatesIds.Count())
            return Result.BadRequest<Dictionary<Guid, ServiceTemplateEntity>?>("Некорректные ServiceTemplateIds");
        if (serviceTemplates.Any(e => e.Value.IsTariffService == false))
            return Result.BadRequest<Dictionary<Guid, ServiceTemplateEntity>?>("Вы пытаетесь добавить добавить Service не предназначенный для Tariff (флаг IsTariffService)");

        return Result.Ok(serviceTemplates)!;
    }
}