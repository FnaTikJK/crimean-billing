﻿using API.DAL;
using API.Infrastructure;
using API.Infrastructure.BaseApiDTOs;
using API.Modules.ServicesModule.DTO;
using API.Modules.ServicesModule.Model;
using API.Modules.ServicesModule.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace API.Modules.ServicesModule;

public interface IServicesService
{
    Task<Result<ServiceDTO>> CreateService(CreateServiceRequest request);
    Task<Result<ServiceDTO>> PatchService(PatchServiceRequest request);
    Task<Result<SearchServicesResponse>> SearchServices(SearchServicesRequest request);
    Task<Result<ServiceWithHistoryDTO>> GetServiceInfo(Guid templateId);
}

public class ServicesService : IServicesService
{
    private readonly DataContext db;
    private readonly ILog log;

    private readonly DbSet<ServiceTemplateEntity> templates;
    private readonly DbSet<ServiceEntity> services;

    public ServicesService(DataContext db, ILog log)
    {
        this.db = db;
        this.log = log;
        this.templates = db.ServiceTemplates;
        this.services = db.Services;
    }

    public async Task<Result<ServiceDTO>> CreateService(CreateServiceRequest request)
    {
        var withSameCode = await templates.AsNoTracking().FirstOrDefaultAsync(e => e.Code == request.Code);
        if (withSameCode != null)
            return Result.BadRequest<ServiceDTO>("Сервис с таким кодом уже существует");

        var template = ServicesMapper.MapTemplate(request);
        await templates.AddAsync(template);
        await db.SaveChangesAsync();
        return Result.Ok(ServicesMapper.Map(template, template.Services?.FirstOrDefault()));
    }

    public async Task<Result<ServiceDTO>> PatchService(PatchServiceRequest request)
    {
        var withSameCode = await templates.AsNoTracking().FirstOrDefaultAsync(e => e.Code == request.Code);
        if (withSameCode != null)
            return Result.BadRequest<ServiceDTO>("ServiceTemplate с таким кодом уже существует");
        var template = await templates.Include(e => e.Services).FirstOrDefaultAsync(e => e.Id == request.TemplateId);
        if (template == null)
            return Result.BadRequest<ServiceDTO>("ServiceTemplate не найден");
        
        var service = template.Services?.FirstOrDefault(e => e.DeletedAt == null);
        ServicesMapper.Patch(request, template);
        var actualService = await ActualizeService(request, template, service);
        if (!actualService.IsSuccess)
            return Result.BadRequest<ServiceDTO>(actualService.Error!);
        await db.SaveChangesAsync();

        return Result.Ok(ServicesMapper.Map(template, actualService.Value));
    }

    public async Task<Result<SearchServicesResponse>> SearchServices(SearchServicesRequest request)
    {
        var query = templates.AsNoTracking().Include(e => e.Services).Select(ServicesMapper.Map).AsQueryable();

        if (request.Ids != null)
            query = query.Where(e => e.Id != null && request.Ids.Contains(e.Id.Value));
        if (request.Code != null)
            query = query.Where(e => e.Code.Contains(request.Code, StringComparison.OrdinalIgnoreCase));
        if (request.Description != null)
            query = query.Where(e => e.Description.Contains(request.Description, StringComparison.OrdinalIgnoreCase));
        if (request.Name != null)
            query = query.Where(e => e.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
        if (request.ServiceType != null)
            query = query.Where(e => e.ServiceType == request.ServiceType);
        if (request.AccountType != null)
            query = query.Where(e => e.AccountType == request.AccountType);
        if (request.UnitType != null)
            query = query.Where(e => e.UnitType == request.UnitType);
        if (request.IsTariffService != null)
            query = query.Where(e => e.IsTariffService == request.IsTariffService);
        if (request.Price != null)
            query = query.Where(e => request.Price.Fit(e.Price));
        if (request.Amount != null)
            query = query.Where(e => request.Amount.Fit(e.Amount));
        if (request.CreatedAt != null)
            query = query.Where(request.CreatedAtFit());
        if (request.UpdatedAt != null)
            query = query.Where(request.UpdatedAtFit());

        if (request.OrderBy != null)
            query = OrderSearch(query, request.OrderBy.Value, request.OrderDirection);
        
        if (request.ExcludedTemplateIds != null)
            query = query.Where(e => !request.ExcludedTemplateIds.Contains(e.TemplateId));

        var result = query.Skip(request.Skip).Take(request.Take);
        var totalCount = query.Count();
        return Result.Ok(new SearchServicesResponse()
        {
            TotalCount = totalCount,
            Items = result.ToList(),
        });
    }

    public async Task<Result<ServiceWithHistoryDTO>> GetServiceInfo(Guid templateId)
    {
        var template = await templates.AsNoTracking().Include(e => e.Services).FirstOrDefaultAsync(e => e.Id == templateId);
        if (template == null)
            return Result.BadRequest<ServiceWithHistoryDTO>("Такого Template не существует");

        return Result.Ok(new ServiceWithHistoryDTO()
        {
            Current = ServicesMapper.Map(template),
            History = template.Services?
                .Where(e => e.DeletedAt != null).Select(ServicesMapper.Map)
                .OrderByDescending(e => e.CreatedAt)
                .ToList()
        });
    }

    private IOrderedQueryable<ServiceDTO> OrderSearch(IQueryable<ServiceDTO> query, SearchServiceRequestOrderBy orderBy, OrderDirection? orderDirection)
    {
        if (orderBy == SearchServiceRequestOrderBy.Code)
            return orderDirection is OrderDirection.Asc
                ? query.OrderBy(e => e.Code)
                : query.OrderByDescending(e => e.Code);
        if (orderBy == SearchServiceRequestOrderBy.Price)
            return orderDirection is OrderDirection.Asc
                ? query.OrderBy(e => e.Price)
                : query.OrderByDescending(e => e.Price);
        if (orderBy == SearchServiceRequestOrderBy.Amount)
            return orderDirection is OrderDirection.Asc
                ? query.OrderBy(e => e.Amount)
                : query.OrderByDescending(e => e.Amount);
        if (orderBy == SearchServiceRequestOrderBy.CreatedAt)
            return orderDirection is OrderDirection.Asc
                ? query.OrderBy(e => e.CreatedAt)
                : query.OrderByDescending(e => e.CreatedAt);
        if (orderBy == SearchServiceRequestOrderBy.UpdatedAt)
            return orderDirection is OrderDirection.Asc
                ? query.OrderBy(e => e.UpdatedAt)
                : query.OrderByDescending(e => e.UpdatedAt);

        throw new NotImplementedException("not implemented order by");
    }

    private async Task<Result<ServiceEntity?>> ActualizeService(
        PatchServiceRequest request,
        ServiceTemplateEntity template, 
        ServiceEntity? service)
    {
        if (request.Price == null && request.Amount == null)
            return Result.Ok(service);
        
        if (service != null)
            service.DeletedAt = DateTimeProvider.Now;
        if (request.NeedKillService is true)
        {
            if (request.Price != null || request.Amount != null)
                return Result.BadRequest<ServiceEntity?>("Вы пытаетесь удалить сервис и пропатичть его. (Флаг NeedKillService)");
            return Result.Ok<ServiceEntity?>(null);
        }

        var actualService = ServicesMapper.MapService(request, service);
        template.Services ??= new HashSet<ServiceEntity>();
        template.Services.Add(actualService);
        await services.AddAsync(actualService);
        return Result.Ok<ServiceEntity?>(actualService);
    }
}