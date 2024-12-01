using API.DAL;
using API.Infrastructure;
using API.Infrastructure.BaseApiDTOs;
using API.Modules.InvoiceModule.DTO;
using API.Modules.InvoiceModule.Model;
using API.Modules.InvoiceModule.Model.DTO;
using API.Modules.PaymentsModule;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.InvoiceModule;

public interface IInvoicesService
{
    Result<SearchInvoicesResponse> Search(SearchInvoicesRequest request);
    Task<Result<InvoiceDTO>> PayInvoice(PayInvoiceRequest request);
}

public class InvoicesService : IInvoicesService
{
    private readonly DataContext db;
    private readonly ILog log;
    private readonly IPaymentsService paymentsService;

    private readonly DbSet<InvoiceEntity> invoices;

    public InvoicesService(DataContext db, ILog log, IPaymentsService paymentsService)
    {
        this.db = db;
        this.log = log;
        this.paymentsService = paymentsService;
        invoices = db.Invoices;
    }

    public Result<SearchInvoicesResponse> Search(SearchInvoicesRequest request)
    {
        var query = invoices
            .Include(e => e.Account)
            .Include(e => e.Tariff)
            .Include(e => e.Payment)
            .AsNoTracking();

        if (request.AccountId != null)
            query = query.Where(e => e.AccountId == request.AccountId);
        if (request.CreatedAt != null)
            query = query.Where(request.CreatedAtFit());
        if (request.PayedAt != null)
            query = query.Where(request.PayedAtFit());
        if (request.ToPay != null)
            query = query.Where(request.ToPayFit());
        if (request.Ordering != null)
            query = OrderSearch(query, request.Ordering.Value, request.Direction);

        var totalCount = query.Count();
        return Result.Ok(new SearchInvoicesResponse
        {
            Items = query.Skip(request.Skip).Take(request.Take).AsEnumerable().Select(InvoicesMapper.Map).ToList(),
            TotalCount = totalCount
        });
    }

    private IOrderedQueryable<InvoiceEntity> OrderSearch(IQueryable<InvoiceEntity> query,
        SearchInvoicesOrdering orderBy, OrderDirection? orderDirection)
    {
        if (orderBy == SearchInvoicesOrdering.CreatedAt)
            return orderDirection == OrderDirection.Asc
                ? query.OrderBy(e => e.CreatedAt)
                : query.OrderByDescending(e => e.CreatedAt);
        if (orderBy == SearchInvoicesOrdering.PayedAt)
            return orderDirection == OrderDirection.Asc
                ? query.OrderBy(orderBy.PayedAtOrdering())
                : query.OrderByDescending(orderBy.PayedAtOrdering());
        if (orderBy == SearchInvoicesOrdering.ToPay)
            return orderDirection == OrderDirection.Asc
                ? query.OrderBy(orderBy.ToPayOrdering())
                : query.OrderByDescending(orderBy.ToPayOrdering());

        throw new Exception("Not implemented ordering");
    }

    public async Task<Result<InvoiceDTO>> PayInvoice(PayInvoiceRequest request)
    {
        var invoice = await invoices
            .Include(e => e.Account)
            .Include(e => e.Tariff)
            .Include(e => e.Payment)
            .FirstOrDefaultAsync(e => e.Id == request.InvoiceId);
        if (invoice == null)
            return Result.BadRequest<InvoiceDTO>("Такого Invoice не существует");

        if (invoice.Payment != null)
            return Result.Ok(InvoicesMapper.Map(invoice));

        var payResponse = paymentsService.TryPayInvoice(invoice);
        if (!payResponse.IsSuccess)
            return Result.BadRequest<InvoiceDTO>(payResponse.Error!);

        await db.SaveChangesAsync();
        return Result.Ok(InvoicesMapper.Map(invoice));
    }
}