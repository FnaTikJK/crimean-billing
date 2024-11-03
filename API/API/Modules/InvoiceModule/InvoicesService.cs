using API.DAL;
using API.Infrastructure;
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
        var query = invoices.AsNoTracking()
            .Include(e => e.Account)
            .Include(e => e.Tariff)
            .Include(e => e.Payment)
            .Where(e => e.Account.Id == request.AccountId);
        
        if (request.IsPayed is true)
            query = query.Where(e => e.Payment != null);
        else if (request.IsPayed is false)
            query = query.Where(e => e.Payment == null);

        var totalCount = query.Count();
        return Result.Ok(new SearchInvoicesResponse
        {
            Items = query.Skip(request.Skip).Take(request.Take).AsEnumerable().Select(InvoicesMapper.Map).ToList(),
            TotalCount = totalCount
        });
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